using UnityEngine;

public class CameraControl : MonoBehaviour
{
	[Range(-25f, 25f)]
	public float cameraMoveSpeed_x;

	[Range(-25f, 25f)]
	public float cameraMoveSpeed_y;

	[Range(-25f, 25f)]
	public float cameraMoveSpeed_z;

	private void Update()
	{
		base.gameObject.transform.Translate(cameraMoveSpeed_x * Time.deltaTime, cameraMoveSpeed_y * Time.deltaTime, cameraMoveSpeed_z * Time.deltaTime);
	}
}
