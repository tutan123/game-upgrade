using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace RenderHeads.Media.AVProVideo;

[HelpURL("https://www.renderheads.com/products/avpro-video/")]
[AddComponentMenu("AVPro Video/Update Multi-Pass Stereo", 320)]
public class UpdateMultiPassStereo : MonoBehaviour
{
	[Header("Stereo camera")]
	[SerializeField]
	private Camera _camera;

	private static readonly LazyShaderProperty PropWorldCameraPosition = new LazyShaderProperty("_WorldCameraPosition");

	private static readonly LazyShaderProperty PropWorldCameraRight = new LazyShaderProperty("_WorldCameraRight");

	private Camera _foundCamera;

	public Camera Camera
	{
		get
		{
			return _camera;
		}
		set
		{
			_camera = value;
		}
	}

	private void Awake()
	{
		if (_camera == null)
		{
			Debug.LogWarning("[AVProVideo] No camera set for UpdateMultiPassStereo component. If you are rendering in multi-pass stereo then it is recommended to set this.");
		}
	}

	private void Start()
	{
		LogXRDeviceDetails();
	}

	private void LogXRDeviceDetails()
	{
		string text = "[AVProVideo] XR Device details: UnityEngine.XR.XRSettings.loadedDeviceName = " + XRSettings.loadedDeviceName + " | supportedDevices = ";
		string[] supportedDevices = XRSettings.supportedDevices;
		int num = supportedDevices.Length;
		for (int i = 0; i < num; i++)
		{
			text += supportedDevices[i];
			if (i < num - 1)
			{
				text += ", ";
			}
		}
		List<InputDevice> list = new List<InputDevice>();
		InputDevices.GetDevices(list);
		int count = list.Count;
		if (count > 0)
		{
			text += " | XR Devices = ";
			for (int j = 0; j < count; j++)
			{
				text += list[j].name;
				if (j < count - 1)
				{
					text += ", ";
				}
			}
		}
		InputDevice deviceAtXRNode = InputDevices.GetDeviceAtXRNode(XRNode.Head);
		text = text + " | headDevice name = " + deviceAtXRNode.name + ", manufacturer = " + deviceAtXRNode.manufacturer;
		Debug.Log(text);
	}

	private static bool IsMultiPassVrEnabled()
	{
		if (!XRSettings.enabled)
		{
			return false;
		}
		if (XRSettings.stereoRenderingMode != 0)
		{
			return false;
		}
		return true;
	}

	private void LateUpdate()
	{
		if (!IsMultiPassVrEnabled())
		{
			return;
		}
		if (_camera != null && _foundCamera != _camera)
		{
			_foundCamera = _camera;
		}
		if (_foundCamera == null)
		{
			_foundCamera = Camera.main;
			if (_foundCamera == null)
			{
				Debug.LogWarning("[AVProVideo] Cannot find main camera for UpdateMultiPassStereo, this can lead to eyes flickering");
				if (Camera.allCameras.Length != 0)
				{
					_foundCamera = Camera.allCameras[0];
					Debug.LogWarning("[AVProVideo] UpdateMultiPassStereo using camera " + _foundCamera.name);
				}
			}
		}
		if (_foundCamera != null)
		{
			Shader.DisableKeyword("USING_URP");
			Shader.SetGlobalVector(PropWorldCameraPosition.Id, _foundCamera.transform.position);
			Shader.SetGlobalVector(PropWorldCameraRight.Id, _foundCamera.transform.right);
		}
	}
}
