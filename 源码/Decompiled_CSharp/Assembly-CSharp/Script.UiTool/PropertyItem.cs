using System.Collections.Generic;
using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UiTool;

public class PropertyItem : MonoBehaviour
{
	public PropertyTypeValue propertyTypeValue;

	public TMP_Text title;

	public TMP_Text count;

	public Image icon;

	public GameObject point;

	public ShowTipsPos showTipsPos;

	public bool isUiPos = true;

	public Camera uiCamera;

	private Color _startCountColor;

	private TipsShow _tipsShow;

	private void Awake()
	{
		if ((bool)count)
		{
			_startCountColor = count.color;
		}
	}

	private void OnEnable()
	{
		EventManager.AddListener(MessageType.Game, GameMessageType.UpdateProperty, Init);
		Init();
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.UpdateProperty, Init);
		if (_tipsShow != null)
		{
			UiManager.HideTipsShow(_tipsShow);
			_tipsShow = null;
		}
	}

	private void Init(List<object> objects)
	{
		Init();
	}

	public void Init()
	{
		if ((bool)title)
		{
			title.text = propertyTypeValue.GetName();
		}
		switch (propertyTypeValue)
		{
		case PropertyTypeValue.Execution:
			count.text = SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyTypeValue.ToString(), "Property", 0L) + "/" + SingletonAsMono<GameDataMrg>.Instance.GetProperty("XinDonLiShangXian", "Property", 0L);
			break;
		case PropertyTypeValue.Pressure:
		{
			bool flag = SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyTypeValue.ToString(), "Property", 0L) >= SingletonAsMono<GameDataMrg>.Instance.GetProperty("YaLiShangXian", "Property", 0L);
			count.text = SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyTypeValue.ToString(), "Property", 0L) + "/" + SingletonAsMono<GameDataMrg>.Instance.GetProperty("YaLiShangXian", "Property", 0L);
			count.color = (flag ? Color.red : _startCountColor);
			if ((bool)icon)
			{
				icon.color = (flag ? Color.red : Color.white);
			}
			break;
		}
		default:
			count.text = SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyTypeValue.ToString(), "Property", 0L).ToString().FormatStringNumber();
			break;
		}
	}

	public void ShowTips()
	{
		if ((bool)point)
		{
			string proTips = FrameWork.Tool.GetProTips(propertyTypeValue);
			if (!string.IsNullOrEmpty(proTips))
			{
				_tipsShow = UiManager.ShowTipsShow(proTips, point.transform.position, showTipsPos);
			}
		}
	}

	public void HideTips()
	{
		if (_tipsShow != null)
		{
			UiManager.HideTipsShow(_tipsShow);
			_tipsShow = null;
		}
	}

	public void ShowXdlTips()
	{
		if ((bool)point)
		{
			RectTransform component = point.GetComponent<RectTransform>();
			Vector2 position = component.position;
			if (!isUiPos)
			{
				position = RectTransformUtility.WorldToScreenPoint(uiCamera, component.position);
				position = UiManager.GetCamera().ScreenToWorldPoint(position);
			}
			string text = LanguageMrg.GetText("A5498") + "+" + SingletonAsMono<GameDataMrg>.Instance.GetProperty("XinDonLiHuiFu", "Property", 0L);
			_tipsShow = UiManager.ShowTipsShow(text, position, showTipsPos);
		}
	}
}
