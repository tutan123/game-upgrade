using UnityEngine;

namespace PolyPerfect;

public class Common_SurfaceRotation : MonoBehaviour
{
	private string terrainLayer = "Terrain";

	private int layer;

	private bool rotate = true;

	private Quaternion targetRotation;

	private float rotationSpeed = 2f;

	private void Awake()
	{
		layer = LayerMask.GetMask(terrainLayer);
	}

	private void Start()
	{
		Vector3 direction = base.transform.parent.TransformDirection(Vector3.down);
		if (Physics.Raycast(base.transform.parent.position, direction, out var hitInfo, 50f, layer))
		{
			_ = hitInfo.distance;
			Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
			base.transform.rotation = quaternion * base.transform.parent.rotation;
		}
	}

	private void Update()
	{
		if (rotate)
		{
			Vector3 direction = base.transform.parent.TransformDirection(Vector3.down);
			if (Physics.Raycast(base.transform.parent.position, direction, out var hitInfo, 50f, layer))
			{
				_ = hitInfo.distance;
				Quaternion quaternion = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
				targetRotation = quaternion * base.transform.parent.rotation;
			}
			base.transform.rotation = Quaternion.Lerp(base.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
		}
	}

	public void SetRotationSpeed(float speed)
	{
		if (speed > 0f)
		{
			rotationSpeed = speed;
		}
	}

	private void OnBecameVisible()
	{
		rotate = true;
	}

	private void OnBecameInvisible()
	{
		rotate = false;
	}
}
