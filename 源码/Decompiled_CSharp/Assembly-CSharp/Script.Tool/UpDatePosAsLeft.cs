using TMPro;
using UnityEngine;

namespace Script.Tool;

public class UpDatePosAsLeft : MonoBehaviour
{
	public TMP_Text text;

	private RectTransform _rect;

	public Vector2 offset;

	private string _str;

	private void Awake()
	{
		_rect = GetComponent<RectTransform>();
	}

	private void Update()
	{
		_rect.anchoredPosition = offset;
	}
}
