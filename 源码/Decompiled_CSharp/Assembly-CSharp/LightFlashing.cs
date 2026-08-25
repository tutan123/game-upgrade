using UnityEngine;

public class LightFlashing : MonoBehaviour
{
	public float time = 0.2f;

	public float min = 0.5f;

	public float max = 5f;

	public bool useSmooth;

	public float smoothTime = 10f;

	public GameObject mGlow;

	private Color mColor;

	private Light mLight;

	private Material mMaterial;

	private void Start()
	{
		mLight = GetComponent<Light>();
		mMaterial = mGlow.GetComponent<Renderer>().material;
		if (!useSmooth && mLight != null)
		{
			InvokeRepeating("OneLightChange", time, time);
			mColor = mMaterial.GetColor("_TintColor");
		}
	}

	private void OneLightChange()
	{
		mLight.intensity = Random.Range(min, max);
	}

	private void Update()
	{
		if (useSmooth && mLight != null)
		{
			mLight.intensity = Mathf.Lerp(mLight.intensity, Random.Range(min, max), Time.deltaTime * smoothTime);
		}
		mColor.a = (mLight.intensity - min) / (max - min) * 0.5f + 0.5f;
		mMaterial.SetColor("_TintColor", mColor);
	}
}
