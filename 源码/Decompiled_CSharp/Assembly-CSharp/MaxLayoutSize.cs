using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class MaxLayoutSize : MonoBehaviour, ILayoutSelfController, ILayoutController
{
	public float maxWidth = -1f;

	public float maxHeight = -1f;

	private RectTransform rectTransform;

	private void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void SetLayoutHorizontal()
	{
		if (maxWidth > 0f && rectTransform.sizeDelta.x > maxWidth)
		{
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maxWidth);
		}
	}

	public void SetLayoutVertical()
	{
		if (maxHeight > 0f && rectTransform.sizeDelta.y > maxHeight)
		{
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, maxHeight);
		}
	}
}
