using UnityEngine;

namespace Script.Tool;

public class DelayDestroy : MonoBehaviour
{
	public float time;

	private void Start()
	{
		Object.Destroy(base.gameObject, time);
	}
}
