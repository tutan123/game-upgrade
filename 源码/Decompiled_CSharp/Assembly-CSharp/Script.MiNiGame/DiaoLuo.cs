using FrameWork;
using UnityEngine;

namespace Script.MiNiGame;

public class DiaoLuo : MonoBehaviour
{
	public float speed = 10f;

	public float showTime = 5f;

	private TimeData _timeData;

	private void Update()
	{
		base.transform.Translate(-Vector2.up * speed * Time.deltaTime, Space.World);
	}

	private void OnEnable()
	{
		_timeData = Timer.DelayCall(showTime, delegate
		{
			if (base.gameObject != null)
			{
				Object.Destroy(base.gameObject);
			}
		});
	}

	private void OnDisable()
	{
		Timer.DestroyTimer(_timeData);
	}
}
