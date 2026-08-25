using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomPeople : MonoBehaviour
{
	[Serializable]
	public class CustomBodyElement
	{
		public string name;

		public Transform bodyElement;
	}

	public AvatarMask mask;

	public GameObject slider;

	public Transform sliderParent;

	public List<Slider> sliders = new List<Slider>(0);

	public List<CustomBodyElement> customBodyElement = new List<CustomBodyElement>();

	private void Start()
	{
		for (int i = 0; i < mask.transformCount; i++)
		{
			if (mask.GetTransformActive(i))
			{
				CustomBodyElement customBodyElement = new CustomBodyElement();
				customBodyElement.name = mask.GetTransformPath(i);
				customBodyElement.bodyElement = base.transform.Find(mask.GetTransformPath(i));
				if (!customBodyElement.bodyElement.GetComponent<SkinnedMeshRenderer>())
				{
					this.customBodyElement.Add(customBodyElement);
				}
			}
		}
		for (int j = 0; j < this.customBodyElement.Count; j++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(slider, sliderParent);
			sliders.Add(gameObject.GetComponent<Slider>());
		}
	}

	private void Update()
	{
		for (int i = 0; i < customBodyElement.Count; i++)
		{
			if (customBodyElement[i].bodyElement != null)
			{
				customBodyElement[i].bodyElement.transform.localScale = Vector3.one * (sliders[i].value + 1f);
			}
		}
	}
}
