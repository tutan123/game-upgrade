using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace RenderHeads.Media.AVProVideo.Demos;

public class LookAround360 : MonoBehaviour
{
	[SerializeField]
	private bool _lockPitch;

	[SerializeField]
	private float _maxSpinSpeed = 40f;

	[SerializeField]
	[Range(1f, 10f)]
	private float _spinDamping = 5f;

	private float _spinX;

	private float _spinY;

	private static bool IsVrPresent()
	{
		bool result = false;
		List<XRDisplaySubsystem> list = new List<XRDisplaySubsystem>();
		SubsystemManager.GetSubsystems(list);
		foreach (XRDisplaySubsystem item in list)
		{
			if (item.running)
			{
				result = true;
				break;
			}
		}
		return result;
	}

	private void Start()
	{
		if (IsVrPresent())
		{
			base.enabled = false;
		}
		else if (SystemInfo.supportsGyroscope)
		{
			Input.gyro.enabled = true;
		}
	}

	private void Update()
	{
		if (SystemInfo.supportsGyroscope && Input.gyro.enabled)
		{
			RotateFromGyro();
		}
		else
		{
			RotateFromMouseOrTouch();
		}
	}

	private void OnDestroy()
	{
		if (SystemInfo.supportsGyroscope)
		{
			Input.gyro.enabled = false;
		}
	}

	private void RotateFromGyro()
	{
		base.transform.localRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, 0f - Input.gyro.attitude.z, 0f - Input.gyro.attitude.w);
	}

	private void RotateFromMouseOrTouch()
	{
		if (Input.GetMouseButton(0))
		{
			float value = _maxSpinSpeed * (0f - Input.GetAxis("Mouse X")) * Time.deltaTime;
			float value2 = 0f;
			if (!_lockPitch)
			{
				value2 = _maxSpinSpeed * Input.GetAxis("Mouse Y") * Time.deltaTime;
			}
			value = Mathf.Clamp(value, -0.5f, 0.5f);
			value2 = Mathf.Clamp(value2, -0.5f, 0.5f);
			_spinX += value;
			_spinY += value2;
		}
		if (!Mathf.Approximately(_spinX, 0f) || !Mathf.Approximately(_spinY, 0f))
		{
			base.transform.Rotate(Vector3.up, _spinX);
			base.transform.Rotate(Vector3.right, _spinY);
			_spinX = Mathf.MoveTowards(_spinX, 0f, _spinDamping * Time.deltaTime);
			_spinY = Mathf.MoveTowards(_spinY, 0f, _spinDamping * Time.deltaTime);
		}
	}
}
