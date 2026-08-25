using System;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[RequireComponent(typeof(ScrollRect))]
public class ScrollRectTool : MonoBehaviour
{
	public GameObject prefab;

	public int horCount = 1;

	public float space = 5f;

	private RectTransform _prefabRect;

	private ScrollRect _scrollRect;

	private RectTransform _scrollRectTransform;

	private Action<int, GameObject> _scrollItemCallback;

	private int _count;

	public bool isInit;

	private int _spawnCount;

	private RectTransform _content;

	private Vector2 _lastLoc;

	private int _fistIndex;

	private int _lastIndex;

	private void Awake()
	{
		_scrollRect = GetComponent<ScrollRect>();
		if (prefab == null && _scrollRect.content.childCount > 0)
		{
			prefab = _scrollRect.content.GetChild(0).gameObject;
		}
		_prefabRect = prefab.GetComponent<RectTransform>();
		_prefabRect.pivot = new Vector2(0.5f, 1f);
		_scrollRectTransform = _scrollRect.GetComponent<RectTransform>();
		_scrollRect.horizontal = false;
		_scrollRect.vertical = true;
		_scrollRect.content.pivot = new Vector2(0.5f, 1f);
		_scrollRect.onValueChanged.AddListener(OnValueChanged);
	}

	public void MoveTo(int index)
	{
		int num = index;
		int num2 = _count - (_spawnCount - 1);
		int num3 = _spawnCount - 1;
		if (num <= num2 && num >= num3)
		{
			num = index - Mathf.FloorToInt((float)(_spawnCount - 1) / 2f);
		}
		else if (num >= _count - _spawnCount)
		{
			num = _count - _spawnCount;
		}
		else if (num < _spawnCount - 1)
		{
			num = 0;
		}
		_lastLoc = -GetItemPos(num);
		_fistIndex = num;
		_lastIndex = num + _spawnCount - 1;
		_scrollRect.content.anchoredPosition = new Vector2(0f, _lastLoc.y);
		if (index > _count - (_spawnCount - 1))
		{
			_scrollRect.verticalNormalizedPosition = 0f;
		}
		for (int i = 0; i < _spawnCount; i++)
		{
			int num4 = num + i;
			Transform transform = null;
			transform = ((i >= _content.childCount) ? UnityEngine.Object.Instantiate(prefab, _content).transform : _content.GetChild(i));
			transform.localPosition = GetItemPos(num4);
			_scrollItemCallback?.Invoke(num4, transform.gameObject);
			transform.SetActive(active: true);
		}
	}

	public void Init(int count, Action<int, GameObject> callback)
	{
		_count = count;
		_content = _scrollRect.content;
		_scrollItemCallback = callback;
		float num = _count;
		_content.pivot = new Vector2(0.5f, 1f);
		float x = (_prefabRect.rect.width + space) * (float)horCount;
		float y = (_prefabRect.rect.height + space) * (num / (float)horCount);
		_content.sizeDelta = new Vector2(x, y);
		int a = (int)(_scrollRectTransform.rect.height / _prefabRect.rect.height + 1f) * horCount;
		a = (_spawnCount = Mathf.Min(a, count));
		Tool.HideAllChild(_content);
		for (int i = 0; i < a; i++)
		{
			Transform transform = null;
			transform = ((i >= _content.childCount) ? UnityEngine.Object.Instantiate(prefab, _content).transform : _content.GetChild(i));
			transform.localPosition = GetItemPos(i);
			_scrollItemCallback?.Invoke(i, transform.gameObject);
			transform.SetActive(active: true);
		}
		_content.offsetMin = new Vector2(0f, _content.offsetMin.y);
		_content.offsetMax = new Vector2(0f, _content.offsetMax.y);
		_content.anchoredPosition = Vector2.zero;
		_lastLoc = Vector2.zero;
		_fistIndex = 0;
		_lastIndex = a - 1;
		isInit = true;
	}

	public void OnValueChanged(Vector2 pos)
	{
		bool flag = _scrollRect.content.localPosition.y > _lastLoc.y;
		for (int i = 0; i < _scrollRect.content.childCount; i++)
		{
			Transform child = _scrollRect.content.GetChild(i);
			Vector3 vector = _scrollRect.viewport.InverseTransformPoint(child.position);
			if (flag)
			{
				if (_lastIndex >= _count - 1)
				{
					return;
				}
				if (vector.y > _prefabRect.rect.height)
				{
					_fistIndex++;
					_lastIndex++;
					child.GetComponent<RectTransform>().anchoredPosition = GetItemPos(_lastIndex);
					_scrollItemCallback?.Invoke(_lastIndex, child.gameObject);
				}
			}
			else
			{
				if (_fistIndex <= 0)
				{
					return;
				}
				if (Mathf.Abs(vector.y) > _scrollRectTransform.rect.height)
				{
					_fistIndex--;
					_lastIndex--;
					child.GetComponent<RectTransform>().anchoredPosition = GetItemPos(_fistIndex);
					_scrollItemCallback?.Invoke(_fistIndex, child.gameObject);
				}
			}
		}
		_lastLoc = _scrollRect.content.localPosition;
	}

	public Vector2 GetItemPos(int index)
	{
		float num = index;
		float num2 = (float)(int)(num / (float)horCount) * (_prefabRect.rect.height + space);
		return new Vector2(0f - (float)(horCount - 1) / 2f * _prefabRect.rect.width + num % (float)horCount * (_prefabRect.rect.width + space), 0f - num2);
	}
}
