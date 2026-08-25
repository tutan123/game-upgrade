using TMPro;
using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(TMP_Text))]
public class TMPMarquee : MonoBehaviour
{
	[Header("滚动速度")]
	public float scrollSpeed = 45f;

	[Header("首尾衔接留白")]
	public float gap = 10f;

	private TMP_Text _tmp;

	private RectTransform _textRt;

	private float _textRealWidth;

	private float _maskViewWidth;

	private bool _needMarquee;

	private string _lastText;

	private bool _isCanMove = true;

	private void Awake()
	{
		_tmp = GetComponent<TMP_Text>();
		_textRt = GetComponent<RectTransform>();
	}

	private void Start()
	{
		RefreshMarquee();
	}

	private void OnEnable()
	{
		_isCanMove = false;
	}

	public void Enter()
	{
		_isCanMove = true;
		_textRt.anchoredPosition = Vector2.zero;
	}

	public void Exit()
	{
		_isCanMove = false;
		_textRt.anchoredPosition = Vector2.zero;
	}

	private void Update()
	{
		if (_tmp == null || _textRt == null)
		{
			return;
		}
		if (_lastText != _tmp.text)
		{
			_lastText = _tmp.text;
			RefreshMarquee();
		}
		if (_needMarquee && _isCanMove)
		{
			_textRt.anchoredPosition += Vector2.left * scrollSpeed * Time.deltaTime;
			if (_textRt.anchoredPosition.x < 0f - _textRealWidth)
			{
				_textRt.anchoredPosition = new Vector2(_maskViewWidth + gap, _textRt.anchoredPosition.y);
			}
		}
	}

	public void RefreshMarquee()
	{
		if (!(_tmp == null) && !(_textRt == null) && !(base.transform.parent == null))
		{
			_tmp.ForceMeshUpdate();
			_maskViewWidth = base.transform.parent.GetComponent<RectTransform>().rect.width;
			_textRealWidth = _tmp.preferredWidth;
			_needMarquee = _textRealWidth > _maskViewWidth;
			if (!_needMarquee)
			{
				_textRt.anchoredPosition = Vector2.zero;
			}
			else
			{
				_textRt.anchoredPosition = new Vector2(0f, _textRt.anchoredPosition.y);
			}
		}
	}
}
