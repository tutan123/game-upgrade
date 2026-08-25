using UnityEngine;

public class TextFlash : MonoBehaviour
{
	private Material _material;

	public Renderer objToFlash;

	[Header("Emisson Color")]
	public Color baseColor = Color.white;

	[Header("Emisson Strength")]
	public float minEmisson = 0.1f;

	public float maxEmisson = 1.1f;

	[Header("Time")]
	public float freqrency = 0.5f;

	private void Start()
	{
		Random.seed = Random.Range(0, int.MaxValue);
		if (objToFlash == null)
		{
			objToFlash = base.gameObject.GetComponentInChildren<Renderer>();
		}
		_material = objToFlash.material;
	}

	private void Update()
	{
		float value = Mathf.PingPong(Time.time * freqrency, Random.Range(minEmisson, maxEmisson));
		Color value2 = baseColor * Mathf.LinearToGammaSpace(value);
		_material.SetColor("_EmissionColor", value2);
	}
}
