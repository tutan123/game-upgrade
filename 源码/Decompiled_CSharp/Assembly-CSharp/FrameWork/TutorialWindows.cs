using System;
using System.Collections.Generic;
using DG.Tweening;
using Script.Mrg;
using Script.Tool;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "TutorialWindows")]
[UiMode(Mode.Normal, true)]
public class TutorialWindows : UiActor
{
	private Sprite[] _sprites;

	private int _index;

	public RectTransform RectTransformBg;

	public CanvasRenderer CanvasRendererBg;

	public Image ImageBg;

	public BT BTBg;

	public AddScripts AddScriptsBg;

	public RectTransform RectTransformIcon;

	public CanvasRenderer CanvasRendererIcon;

	public Image ImageIcon;

	public AddScripts AddScriptsIcon;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public AddScripts AddScriptsClose;

	public BT BTClose;

	public RectTransform RectTransformNext;

	public CanvasRenderer CanvasRendererNext;

	public Image ImageNext;

	public AddScripts AddScriptsNext;

	public BT BTNext;

	public RectTransform RectTransformLast;

	public CanvasRenderer CanvasRendererLast;

	public Image ImageLast;

	public AddScripts AddScriptsLast;

	public BT BTLast;

	public override void Awake()
	{
		base.Awake();
		RectTransformBg = GetGameObject().transform.Find("Bg/").GetComponent<RectTransform>();
		CanvasRendererBg = GetGameObject().transform.Find("Bg/").GetComponent<CanvasRenderer>();
		ImageBg = GetGameObject().transform.Find("Bg/").GetComponent<Image>();
		BTBg = GetGameObject().transform.Find("Bg/").GetComponent<BT>();
		AddScriptsBg = GetGameObject().transform.Find("Bg/").GetComponent<AddScripts>();
		RectTransformIcon = GetGameObject().transform.Find("View/Icon/").GetComponent<RectTransform>();
		CanvasRendererIcon = GetGameObject().transform.Find("View/Icon/").GetComponent<CanvasRenderer>();
		ImageIcon = GetGameObject().transform.Find("View/Icon/").GetComponent<Image>();
		AddScriptsIcon = GetGameObject().transform.Find("View/Icon/").GetComponent<AddScripts>();
		RectTransformClose = GetGameObject().transform.Find("View/Icon/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("View/Icon/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("View/Icon/Close/").GetComponent<Image>();
		AddScriptsClose = GetGameObject().transform.Find("View/Icon/Close/").GetComponent<AddScripts>();
		BTClose = GetGameObject().transform.Find("View/Icon/Close/").GetComponent<BT>();
		RectTransformNext = GetGameObject().transform.Find("View/Icon/Next/").GetComponent<RectTransform>();
		CanvasRendererNext = GetGameObject().transform.Find("View/Icon/Next/").GetComponent<CanvasRenderer>();
		ImageNext = GetGameObject().transform.Find("View/Icon/Next/").GetComponent<Image>();
		AddScriptsNext = GetGameObject().transform.Find("View/Icon/Next/").GetComponent<AddScripts>();
		BTNext = GetGameObject().transform.Find("View/Icon/Next/").GetComponent<BT>();
		RectTransformLast = GetGameObject().transform.Find("View/Icon/Last/").GetComponent<RectTransform>();
		CanvasRendererLast = GetGameObject().transform.Find("View/Icon/Last/").GetComponent<CanvasRenderer>();
		ImageLast = GetGameObject().transform.Find("View/Icon/Last/").GetComponent<Image>();
		AddScriptsLast = GetGameObject().transform.Find("View/Icon/Last/").GetComponent<AddScripts>();
		BTLast = GetGameObject().transform.Find("View/Icon/Last/").GetComponent<BT>();
	}

	public TutorialWindows(Transform trans)
		: base(trans)
	{
	}

	public TutorialWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(CloseUi);
		BTNext.onClick.AddListener(Next);
		BTLast.onClick.AddListener(Last);
		BTBg.onClick.AddListener(CheckClose);
	}

	public override void Open(object[] objects)
	{
		Transform transform = base.transform.Find("Bg");
		if ((bool)transform)
		{
			CanvasGroup component = transform.GetComponent<CanvasGroup>();
			if ((bool)component)
			{
				component.DOFade(1f, 0.3f);
			}
		}
		Transform transform2 = base.transform.Find("View");
		if ((bool)transform2)
		{
			CanvasGroup component2 = transform2.GetComponent<CanvasGroup>();
			if ((bool)component2)
			{
				component2.DOFade(1f, 0.3f);
			}
		}
		EventManager.DispatchEvent(MessageType.UiMessage, UiMessageType.Open);
		Pause();
	}

	public void Init(Sprite[] sprite)
	{
		_index = 0;
		_sprites = sprite;
		ImageIcon.sprite = sprite[_index];
		InitBtn();
	}

	private void CheckClose()
	{
		UiManager.OpenUi<SureWindows>().Init(LanguageMrg.GetText("A6360"), CloseUi);
	}

	private void InitBtn()
	{
		BTNext.SetActive(_sprites.Length > _index + 1);
		BTLast.SetActive(_index > 0);
	}

	private void Next()
	{
		if (_sprites.Length > _index + 1)
		{
			_index++;
			ImageIcon.sprite = _sprites[_index];
			InitBtn();
		}
	}

	private void Last()
	{
		if (_index > 0)
		{
			_index--;
			ImageIcon.sprite = _sprites[_index];
			InitBtn();
		}
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}

	public override void CloseUi()
	{
		Transform transform = base.transform.Find("Bg");
		if ((bool)transform)
		{
			CanvasGroup component = transform.GetComponent<CanvasGroup>();
			if ((bool)component)
			{
				_bgTweener = component.DOFade(0f, 0.3f);
			}
		}
		Transform transform2 = base.transform.Find("View");
		if ((bool)transform2)
		{
			CanvasGroup component2 = transform2.GetComponent<CanvasGroup>();
			if ((bool)component2)
			{
				_viewBgTweener = component2.DOFade(0f, 0.3f);
				Tweener viewBgTweener = _viewBgTweener;
				viewBgTweener.onComplete = (TweenCallback)Delegate.Combine(viewBgTweener.onComplete, (TweenCallback)delegate
				{
					UiManager.RemoveUi(GetIndex());
				});
			}
			else
			{
				UiManager.RemoveUi(GetIndex());
			}
		}
		else
		{
			UiManager.RemoveUi(GetIndex());
		}
	}
}
