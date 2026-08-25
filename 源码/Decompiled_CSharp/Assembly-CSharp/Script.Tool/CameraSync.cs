using UnityEngine;

namespace Script.Tool;

public class CameraSync : MonoBehaviour
{
	public Transform target;

	public Vector2 maxPos;

	public Vector2 minPos;

	private void Update()
	{
		Vector3 position = target.position;
		position.z = -100f;
		if (position.x > maxPos.x)
		{
			position.x = maxPos.x;
		}
		if (position.y > maxPos.y)
		{
			position.y = maxPos.y;
		}
		if (position.x < minPos.x)
		{
			position.x = minPos.x;
		}
		if (position.y < minPos.y)
		{
			position.y = minPos.y;
		}
		base.transform.position = position;
	}
}
