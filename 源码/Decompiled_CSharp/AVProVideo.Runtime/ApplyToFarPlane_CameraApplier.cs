using UnityEngine;

public class ApplyToFarPlane_CameraApplier : MonoBehaviour
{
	[SerializeField]
	private Material _material;

	public Material Material
	{
		get
		{
			return _material;
		}
		set
		{
			_material = value;
		}
	}

	private void OnWillRenderObject()
	{
		if ((bool)_material)
		{
			_material.SetFloat("_CurrentCamID", Camera.current.GetInstanceID());
		}
	}
}
