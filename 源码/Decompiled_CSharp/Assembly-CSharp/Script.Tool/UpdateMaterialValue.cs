using UnityEngine;

namespace Script.Tool;

[ExecuteAlways]
public class UpdateMaterialValue : MonoBehaviour
{
	public string key;

	public float value;

	public Material material;

	private void Update()
	{
		if (!(material == null))
		{
			material.SetFloat(key, value);
		}
	}
}
