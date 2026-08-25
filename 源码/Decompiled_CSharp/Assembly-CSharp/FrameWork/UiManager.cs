using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Script.Mrg;
using TMPro;
using UnityEngine;

namespace FrameWork;

public static class UiManager
{
	private static int _index;

	private static Dictionary<Type, UiActor> _uiDic;

	private static Dictionary<int, Type> _typesDic;

	private static List<int> _uiList;

	private static Actor _uiRoot;

	private static Camera _camera;

	private static List<Transform> _transforms;

	private static Dictionary<string, Transform> _layerDic;

	private static ObjectPool<TopTipstText> _topTips;

	private static ObjectPool<TipstText> _pool;

	private static InfoTipstText _infoTipstText;

	private static InfoRightTipstText _infoRightTipstText;

	private static ObjectPool<TipsShow> _tipsShowPool;

	public static float ScaleFactor => _uiRoot.GetGameObject().GetComponent<Canvas>().scaleFactor;

	public static void Init()
	{
		_index = 0;
	}

	static UiManager()
	{
		_transforms = new List<Transform>();
		_layerDic = new Dictionary<string, Transform>();
		_topTips = new ObjectPool<TopTipstText>();
		_pool = new ObjectPool<TipstText>();
		_tipsShowPool = new ObjectPool<TipsShow>();
		_uiDic = new Dictionary<Type, UiActor>();
		_typesDic = new Dictionary<int, Type>();
		_uiList = new List<int>();
		_uiRoot = new UiRoot();
		UnityEngine.Object.DontDestroyOnLoad(_uiRoot.GetGameObject());
	}

	public static Camera GetCamera()
	{
		if (!_camera)
		{
			_camera = _uiRoot.transform.Find("Camera").GetComponent<Camera>();
		}
		return _camera;
	}

	public static int ShowCount()
	{
		int num = 0;
		Transform transform = GetTransform(new UiModeAttribute(Mode.Normal));
		for (int i = 0; i < transform.childCount; i++)
		{
			if (transform.GetChild(i).gameObject.activeSelf)
			{
				num++;
			}
		}
		return num;
	}

	public static bool IsEscWindowsLast()
	{
		if (IsShowUi<EscWindows>())
		{
			_transforms.Clear();
			Transform transform = GetTransform(new UiModeAttribute(Mode.Normal));
			for (int i = 0; i < transform.childCount; i++)
			{
				if (!_transforms.Contains(transform.GetChild(i)))
				{
					_transforms.Add(transform.GetChild(i));
				}
			}
			int num = -1;
			for (int j = 0; j < _transforms.Count; j++)
			{
				if (_transforms[j].GetComponent<ActorMono>().GetActor().GetType() == typeof(EscWindows))
				{
					num = j;
					break;
				}
			}
			for (int k = num; k < _transforms.Count; k++)
			{
				if (_transforms[k].GetComponent<ActorMono>().GetActor().GetType() != typeof(EscWindows) && _transforms[k].gameObject.activeSelf)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsEscWindowsFistHasUi()
	{
		if (IsShowUi<EscWindows>())
		{
			Transform transform = GetTransform(new UiModeAttribute(Mode.Normal));
			int num = -1;
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).GetComponent<ActorMono>().GetActor()
					.GetType() == typeof(EscWindows))
				{
					num = i;
					break;
				}
			}
			for (int j = 0; j < num; j++)
			{
				if (transform.GetChild(j).GetComponent<ActorMono>().GetActor()
					.GetType() != typeof(EscWindows) && transform.GetChild(j).gameObject.activeSelf)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static T GetUi<T>() where T : UiActor
	{
		if (_uiDic.ContainsKey(typeof(T)))
		{
			return (T)_uiDic[typeof(T)];
		}
		return null;
	}

	public static bool IsHasShowUi()
	{
		foreach (UiActor value in _uiDic.Values)
		{
			if (value.GetGameObject().activeSelf)
			{
				return true;
			}
		}
		return false;
	}

	public static void OpenUi(int index, object[] objs = null)
	{
		if (_typesDic.ContainsKey(index))
		{
			OpenUi(_typesDic[index]);
		}
	}

	public static T OpenUi<T>(object[] objs = null) where T : UiActor
	{
		return (T)OpenUi(typeof(T), objs);
	}

	public static UiActor OpenUi(Type type, object[] objects = null)
	{
		if (_uiRoot.GetGameObject() == null)
		{
			MyLog.LogError("场景中没有Canvas组件,无法显示Ui物体");
			return null;
		}
		if (_uiDic.ContainsKey(type))
		{
			_uiDic[type].SetActive(active: true);
			_uiDic[type].Open(objects);
			_uiDic[type].transform.SetAsLastSibling();
			if (!_uiList.Contains(_uiDic[type].GetIndex()))
			{
				_uiList.Add(_uiDic[type].GetIndex());
			}
			return _uiDic[type];
		}
		UiModeAttribute customAttribute = type.GetCustomAttribute<UiModeAttribute>();
		if (customAttribute == null)
		{
			MyLog.LogError("类不具备UiModeAttribute");
			return null;
		}
		Transform transform = GetTransform(customAttribute);
		object[] args = new object[1] { transform };
		UiActor uiActor;
		try
		{
			uiActor = (UiActor)Activator.CreateInstance(type, args);
		}
		catch (Exception ex)
		{
			MyLog.LogError(ex.Message);
			return null;
		}
		if (uiActor == null)
		{
			MyLog.LogError("生成ui失败");
			return null;
		}
		uiActor.Open(objects);
		uiActor.SetIndex(_index);
		_uiDic.Add(type, uiActor);
		_uiList.Add(uiActor.GetIndex());
		_typesDic.Add(uiActor.GetIndex(), type);
		_index++;
		return uiActor;
	}

	public static bool IsOpenUi<T>()
	{
		if (_uiDic.ContainsKey(typeof(T)))
		{
			return true;
		}
		return false;
	}

	public static bool IsShowUi<T>()
	{
		if (_uiDic.ContainsKey(typeof(T)))
		{
			return _uiDic[typeof(T)].GetGameObject().activeSelf;
		}
		return false;
	}

	public static T HideUi<T>() where T : UiActor
	{
		return (T)HideUi(typeof(T));
	}

	public static UiActor HideUi(Type type)
	{
		if (_uiDic.ContainsKey(type))
		{
			UiActor uiActor = _uiDic[type];
			uiActor.OnClose();
			uiActor.SetActive(active: false);
			_uiDic[type].SetActive(active: false);
			_uiList.Remove(uiActor.GetIndex());
			return uiActor;
		}
		return null;
	}

	public static void HideUi(int index)
	{
		if (_typesDic.ContainsKey(index))
		{
			HideUi(_typesDic[index]);
		}
	}

	public static void HideAllUi(string type = "nor")
	{
		List<object> eventMsg = EventManager.GetEventMsg();
		eventMsg.Add(-1);
		eventMsg.Add(type);
		EventManager.DispatchEvent(MessageType.UiMessage, UiMessageType.Remove, eventMsg);
	}

	public static void RemoveUi<T>() where T : UiActor
	{
		RemoveUi(typeof(T));
	}

	public static void RemoveUi(Type type)
	{
		if (_uiDic.ContainsKey(type))
		{
			UiActor uiActor = _uiDic[type];
			uiActor.OnClose();
			_typesDic.Remove(uiActor.GetIndex());
			_uiList.Remove(uiActor.GetIndex());
			_uiDic.Remove(type);
			uiActor.GetGameObject().Destroy();
		}
	}

	public static void RemoveUi(int index)
	{
		if (_typesDic.ContainsKey(index))
		{
			RemoveUi(_typesDic[index]);
		}
	}

	public static void Back()
	{
		if (_uiList.Count > 0)
		{
			HideUi(_uiList.Last());
		}
	}

	public static Transform GetTransform(UiModeAttribute uiModeAttribute)
	{
		if (_layerDic.ContainsKey(uiModeAttribute.Mode.ToString()))
		{
			return _layerDic[uiModeAttribute.Mode.ToString()];
		}
		Transform transform = _uiRoot.GetGameObject().transform.Find(uiModeAttribute.Mode.ToString());
		if (transform == null)
		{
			Transform transform2 = UnityEngine.Object.Instantiate(_uiRoot.GetGameObject().transform.GetChild(0), _uiRoot.GetGameObject().transform);
			transform2.name = uiModeAttribute.Mode.ToString();
			transform = transform2;
			_layerDic.Add(uiModeAttribute.Mode.ToString(), transform);
		}
		else if (!_layerDic.ContainsKey(uiModeAttribute.Mode.ToString()))
		{
			_layerDic.Add(uiModeAttribute.Mode.ToString(), transform);
		}
		return transform;
	}

	public static void ShowTopTips(string text)
	{
		TopTipstText tips = _topTips.DeQueue();
		tips.transform.SetParent(_uiRoot.transform.Find("Popup"));
		tips.transform.SetAsLastSibling();
		tips.GetGameObject().SetActiveAsCheck(active: true);
		tips.transform.GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, -100f);
		tips.transform.localScale = Vector3.one * 0.6f;
		tips.CanvasGroupTopTipstText.alpha = 1f;
		tips.TextMeshProUGUITitle.text = text;
		Sequence sequence = DOTween.Sequence();
		sequence.Append(tips.transform.DOScale(Vector3.one, 0.5f));
		sequence.AppendInterval(0.5f);
		TweenerCore<float, float, FloatOptions> tweenerCore = tips.CanvasGroupTopTipstText.DOFade(0f, 0.5f).SetDelay(1.5f);
		tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)delegate
		{
			_topTips.EnQueue(tips);
			tips.GetGameObject().SetActiveAsCheck(active: false);
		});
		sequence.Play();
	}

	public static void ShowTips(string text)
	{
		TipstText tips = _pool.DeQueue();
		tips.transform.SetParent(_uiRoot.transform.Find("Popup"));
		tips.transform.SetAsLastSibling();
		tips.GetGameObject().SetActiveAsCheck(active: true);
		tips.transform.localPosition = Vector3.zero;
		tips.transform.localScale = Vector3.one * 0.6f;
		tips.CanvasGroupTipstText.alpha = 1f;
		tips.TextMeshProUGUITitle.text = text;
		Sequence sequence = DOTween.Sequence();
		sequence.Append(tips.transform.DOScale(Vector3.one, 0.5f));
		sequence.AppendInterval(0.5f);
		sequence.Append(tips.transform.DOLocalMoveY(300f, 1f));
		TweenerCore<float, float, FloatOptions> tweenerCore = tips.CanvasGroupTipstText.DOFade(0f, 1f).SetDelay(2f);
		tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)delegate
		{
			_pool.EnQueue(tips);
			tips.GetGameObject().SetActiveAsCheck(active: false);
		});
		sequence.Play();
	}

	public static bool IsCanPlay()
	{
		Transform transform = GetTransform(new UiModeAttribute(Mode.Normal));
		int num = 0;
		for (int i = 0; i < transform.childCount; i++)
		{
			GameObject gameObject = transform.GetChild(i).gameObject;
			if (gameObject.TryGetComponent<ActorMono>(out var component) && gameObject.activeSelf && component.GetActor().IsCanJoinPause())
			{
				num++;
			}
		}
		return num < 1;
	}

	public static void ShowInfoTips(string text, bool isAdd, string type)
	{
		if (_infoTipstText == null || (_infoTipstText != null && _infoTipstText.GetGameObject() == null))
		{
			_infoTipstText = new InfoTipstText();
			_infoTipstText.transform.SetParent(_uiRoot.transform.Find("Popup"));
			_infoTipstText.transform.GetComponent<RectTransform>().anchorMin = Vector2.zero;
			_infoTipstText.transform.GetComponent<RectTransform>().anchorMax = Vector2.one;
			_infoTipstText.transform.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			_infoTipstText.transform.GetComponent<RectTransform>().offsetMax = Vector2.zero;
			_infoTipstText.transform.localScale = Vector3.one;
		}
		_infoTipstText.AddTips(text, isAdd, type);
	}

	public static void ShowInfoTipsRight(TaskTipsData tipsData)
	{
		if (_infoRightTipstText == null || (_infoRightTipstText != null && _infoRightTipstText.GetGameObject() == null))
		{
			_infoRightTipstText = new InfoRightTipstText();
			_infoRightTipstText.transform.SetParent(_uiRoot.transform.Find("Popup"));
			_infoRightTipstText.transform.GetComponent<RectTransform>().anchorMin = Vector2.zero;
			_infoRightTipstText.transform.GetComponent<RectTransform>().anchorMax = Vector2.one;
			_infoRightTipstText.transform.GetComponent<RectTransform>().offsetMin = Vector2.zero;
			_infoRightTipstText.transform.GetComponent<RectTransform>().offsetMax = Vector2.zero;
			_infoRightTipstText.transform.localScale = Vector3.one;
		}
		_infoRightTipstText.AddTips(tipsData);
	}

	public static TipsShow ShowTipsShow(string text, Vector2 position, ShowTipsPos showTipsPos = ShowTipsPos.None)
	{
		TipsShow tipsShow = _tipsShowPool.DeQueue();
		tipsShow.transform.SetParent(_uiRoot.transform.Find("Popup"));
		tipsShow.transform.SetAsFirstSibling();
		tipsShow.GetGameObject().SetActiveAsCheck(active: true);
		tipsShow.transform.localScale = Vector3.one;
		RectTransform component = tipsShow.GetGameObject().GetComponent<RectTransform>();
		component.anchorMin = Vector2.zero;
		component.anchorMax = Vector2.one;
		component.offsetMin = Vector2.zero;
		component.offsetMax = Vector2.zero;
		component.pivot = new Vector2(0.5f, 0.5f);
		switch (showTipsPos)
		{
		case ShowTipsPos.Left:
			tipsShow.TextMeshProUGUIText.GetComponent<RectTransform>().pivot = new Vector2(0f, 0.5f);
			tipsShow.TextMeshProUGUIText.alignment = TextAlignmentOptions.Left;
			break;
		case ShowTipsPos.Right:
			tipsShow.TextMeshProUGUIText.GetComponent<RectTransform>().pivot = new Vector2(1f, 0.5f);
			tipsShow.TextMeshProUGUIText.alignment = TextAlignmentOptions.Right;
			break;
		default:
			tipsShow.TextMeshProUGUIText.alignment = TextAlignmentOptions.Left;
			break;
		}
		tipsShow.TextMeshProUGUIText.text = text;
		tipsShow.Init(position, showTipsPos == ShowTipsPos.None);
		return tipsShow;
	}

	public static void HideTipsShow(TipsShow tipsShow)
	{
		tipsShow.GetGameObject().SetActiveAsCheck(active: false);
		_tipsShowPool.EnQueue(tipsShow);
	}
}
