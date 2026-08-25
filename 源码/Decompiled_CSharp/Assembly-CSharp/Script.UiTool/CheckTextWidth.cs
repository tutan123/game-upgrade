using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Script.UiTool;

public class CheckTextWidth : MonoBehaviour
{
	public float maxWidth;

	public Vector2 offset = new Vector2(20f, 20f);

	public List<RectTransform> rects;

	public TMP_Text text;

	public RectTransform rect;

	private void Awake()
	{
		if (!text)
		{
			text = GetComponent<TMP_Text>();
		}
		if (!rect)
		{
			rect = GetComponent<RectTransform>();
		}
	}

	private void OnEnable()
	{
		UpdateTextSize();
	}

	private void Update()
	{
		UpdateTextSize();
	}

	public void UpdateTextSize()
	{
		Vector2 preferredValues = text.GetPreferredValues(maxWidth, float.PositiveInfinity);
		preferredValues.x = Mathf.Min(maxWidth, preferredValues.x);
		rect.sizeDelta = preferredValues;
		for (int i = 0; i < rects.Count; i++)
		{
			rects[i].sizeDelta = preferredValues + offset;
		}
	}
}
