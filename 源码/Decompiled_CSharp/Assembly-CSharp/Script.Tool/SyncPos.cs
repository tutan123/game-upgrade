using UnityEngine;

namespace Script.Tool;

public class SyncPos : MonoBehaviour
{
	public Transform target;

	public Vector3 offset;

	private void Update()
	{
		base.transform.position = new Vector3(target.position.x, base.transform.position.y, target.position.z) + offset;
	}
}
