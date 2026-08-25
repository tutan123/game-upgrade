using FrameWork;
using UnityEngine;
using UnityEngine.Events;

namespace Script.Mrg;

public class UpdateMrg : SingletonAsMono<UpdateMrg>
{
	public UnityEvent<float> OnUpdate = new UnityEvent<float>();

	private void Update()
	{
		OnUpdate?.Invoke(Time.deltaTime);
	}
}
