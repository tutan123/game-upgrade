using UnityEngine;

namespace Script.Tool;

public class ForwardToCamera : MonoBehaviour
{
	public Camera camera;

	public Vector3 offset;

	private void LateUpdate()
	{
		base.transform.forward = camera.transform.forward + offset;
	}
}
