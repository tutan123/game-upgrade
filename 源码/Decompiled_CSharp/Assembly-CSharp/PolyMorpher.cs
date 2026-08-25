using System;
using System.Collections.Generic;
using UnityEngine;

public class PolyMorpher : MonoBehaviour
{
	[Serializable]
	public class CustomBodyElement
	{
		public string name;

		public Transform bodyElement;

		[HideInInspector]
		public Vector3 startScale;
	}

	public AvatarMask customPartMask;

	public Vector2 minMax = new Vector2(0.9f, 1.1f);

	public bool MorphOnStart = true;

	private List<CustomBodyElement> customBodyElement = new List<CustomBodyElement>();

	[ContextMenu("Get Body Parts")]
	private void GetBodyParts()
	{
		this.customBodyElement.Clear();
		for (int i = 0; i < customPartMask.transformCount; i++)
		{
			if (customPartMask.GetTransformActive(i))
			{
				CustomBodyElement customBodyElement = new CustomBodyElement();
				customBodyElement.name = customPartMask.GetTransformPath(i);
				customBodyElement.bodyElement = base.transform.Find(customPartMask.GetTransformPath(i));
				customBodyElement.startScale = Vector3.one;
				if (!customBodyElement.bodyElement.GetComponent<SkinnedMeshRenderer>())
				{
					this.customBodyElement.Add(customBodyElement);
				}
			}
		}
	}

	[ContextMenu("Random Size")]
	private void RandomSize()
	{
		for (int i = 0; i < customBodyElement.Count; i++)
		{
			if (customBodyElement[i].bodyElement != null)
			{
				float num = UnityEngine.Random.Range(minMax.x, minMax.y);
				customBodyElement[i].bodyElement.localScale = Vector3.one * num;
			}
		}
	}

	[ContextMenu("Reset Size")]
	private void ResetSize()
	{
		for (int i = 0; i < customBodyElement.Count; i++)
		{
			if (customBodyElement[i].bodyElement != null)
			{
				customBodyElement[i].bodyElement.localScale = customBodyElement[i].startScale;
			}
		}
	}

	private void Start()
	{
		if (MorphOnStart)
		{
			GetBodyParts();
			if (customBodyElement.Count > 0)
			{
				RandomSize();
			}
		}
	}
}
