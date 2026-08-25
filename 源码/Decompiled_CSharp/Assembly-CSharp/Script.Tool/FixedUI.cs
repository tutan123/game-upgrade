using UnityEngine;

namespace Script.Tool;

public class FixedUI : MonoBehaviour
{
	public Transform target;

	public Vector3 offset = new Vector3(0.5f, 1f, 0f);

	private void LateUpdate()
	{
		if (!(target == null))
		{
			base.transform.position = target.position + offset;
		}
	}
}
