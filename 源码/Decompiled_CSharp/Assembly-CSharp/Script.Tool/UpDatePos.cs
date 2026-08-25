using TMPro;
using UnityEngine;

namespace Script.Tool;

public class UpDatePos : MonoBehaviour
{
	public TMP_Text text;

	private RectTransform _rect;

	private string _str;

	public void Init(string str)
	{
		text.text = str;
	}

	private void Awake()
	{
		_rect = GetComponent<RectTransform>();
	}

	private void Update()
	{
		_rect.anchoredPosition = new Vector2(text.rectTransform.sizeDelta.x / 2f, 0f);
	}
}
