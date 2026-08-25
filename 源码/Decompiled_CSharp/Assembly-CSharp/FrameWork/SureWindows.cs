using System;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Popup, true)]
[ActorInfo("", "SureWindows")]
public class SureWindows : UiActor
{
	private Action _action;

	private bool _isCanClick;

	public RectTransform RectTransformTitle;

	public CanvasRenderer CanvasRendererTitle;

	public TextMeshProUGUI TextMeshProUGUITitle;

	public AddScripts AddScriptsTitle;

	public RectTransform RectTransformOkBtn;

	public CanvasRenderer CanvasRendererOkBtn;

	public Image ImageOkBtn;

	public AddScripts AddScriptsOkBtn;

	public BT BTOkBtn;

	public RectTransform RectTransformCancelBtn;

	public CanvasRenderer CanvasRendererCancelBtn;

	public Image ImageCancelBtn;

	public AddScripts AddScriptsCancelBtn;

	public BT BTCancelBtn;

	public override void Awake()
	{
		base.Awake();
		RectTransformTitle = GetGameObject().transform.Find("View/Bg/Title/").GetComponent<RectTransform>();
		CanvasRendererTitle = GetGameObject().transform.Find("View/Bg/Title/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITitle = GetGameObject().transform.Find("View/Bg/Title/").GetComponent<TextMeshProUGUI>();
		AddScriptsTitle = GetGameObject().transform.Find("View/Bg/Title/").GetComponent<AddScripts>();
		RectTransformOkBtn = GetGameObject().transform.Find("View/Bg/ButGroup/OkBtn/").GetComponent<RectTransform>();
		CanvasRendererOkBtn = GetGameObject().transform.Find("View/Bg/ButGroup/OkBtn/").GetComponent<CanvasRenderer>();
		ImageOkBtn = GetGameObject().transform.Find("View/Bg/ButGroup/OkBtn/").GetComponent<Image>();
		AddScriptsOkBtn = GetGameObject().transform.Find("View/Bg/ButGroup/OkBtn/").GetComponent<AddScripts>();
		BTOkBtn = GetGameObject().transform.Find("View/Bg/ButGroup/OkBtn/").GetComponent<BT>();
		RectTransformCancelBtn = GetGameObject().transform.Find("View/Bg/ButGroup/CancelBtn/").GetComponent<RectTransform>();
		CanvasRendererCancelBtn = GetGameObject().transform.Find("View/Bg/ButGroup/CancelBtn/").GetComponent<CanvasRenderer>();
		ImageCancelBtn = GetGameObject().transform.Find("View/Bg/ButGroup/CancelBtn/").GetComponent<Image>();
		AddScriptsCancelBtn = GetGameObject().transform.Find("View/Bg/ButGroup/CancelBtn/").GetComponent<AddScripts>();
		BTCancelBtn = GetGameObject().transform.Find("View/Bg/ButGroup/CancelBtn/").GetComponent<BT>();
	}

	public SureWindows(Transform trans)
		: base(trans)
	{
	}

	public SureWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTCancelBtn.onClick.AddListener(CloseUi);
		BTOkBtn.onClick.AddListener(delegate
		{
			if (_isCanClick)
			{
				_action?.Invoke();
				CloseUi();
				_isCanClick = false;
			}
		});
	}

	public void Init(string title, Action action)
	{
		_action = action;
		TextMeshProUGUITitle.text = title;
		_isCanClick = true;
	}
}
