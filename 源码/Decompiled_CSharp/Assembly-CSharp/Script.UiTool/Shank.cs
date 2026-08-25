using UnityEngine;

namespace Script.UiTool;

public class Shank : MonoBehaviour
{
	public float speed = 5f;

	public float amount = 10f;

	private bool _isShank;

	private RectTransform rectTransform;

	private Vector2 initialPosition;

	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void InitPos(Vector2 position)
	{
		_isShank = true;
		initialPosition = position;
	}

	private void OnDisable()
	{
		_isShank = false;
	}

	private void Update()
	{
		if (_isShank)
		{
			float x = Mathf.Sin(Time.time * speed) * amount;
			float y = Mathf.Cos(Time.time * speed * 0.7f) * amount;
			rectTransform.anchoredPosition = initialPosition + new Vector2(x, y);
		}
	}
}
