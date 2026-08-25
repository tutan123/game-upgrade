using UnityEngine;

namespace Script.Mrg;

public class CameraShakeMrg : MonoBehaviour
{
	public float shakeDuration = 0.3f;

	public float shakeMagnitude = 0.2f;

	public float dampingSpeed = 2f;

	private Vector3 originalPos;

	private float currentShakeTime;

	public static CameraShakeMrg Instance;

	private void Awake()
	{
		Instance = this;
		originalPos = base.transform.localPosition;
	}

	private void Update()
	{
		if (currentShakeTime > 0f)
		{
			base.transform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
			currentShakeTime -= Time.deltaTime * dampingSpeed;
		}
		else
		{
			currentShakeTime = 0f;
			base.transform.localPosition = originalPos;
		}
	}

	public void TriggerShake()
	{
		currentShakeTime = shakeDuration;
	}

	public void TriggerShake(float duration, float magnitude, float shakeDampingSpeed)
	{
		currentShakeTime = duration;
		shakeMagnitude = magnitude;
		dampingSpeed = shakeDampingSpeed;
	}
}
