using System;
using System.Collections.Generic;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "InputWindows")]
public class InputWindows : UiActor
{
	private Action<string> _sucAction;

	public RectTransform RectTransformInputField;

	public CanvasRenderer CanvasRendererInputField;

	public Image ImageInputField;

	public TMP_InputField TMP_InputFieldInputField;

	public AddScripts AddScriptsInputField;

	public RectTransform RectTransformOkBtn;

	public CanvasRenderer CanvasRendererOkBtn;

	public Image ImageOkBtn;

	public AddScripts AddScriptsOkBtn;

	public BT BTOkBtn;

	public RectTransform RectTransformTitle;

	public CanvasRenderer CanvasRendererTitle;

	public TextMeshProUGUI TextMeshProUGUITitle;

	public AddScripts AddScriptsTitle;

	public RectTransform RectTransformCloseBtn;

	public CanvasRenderer CanvasRendererCloseBtn;

	public Image ImageCloseBtn;

	public AddScripts AddScriptsCloseBtn;

	public BT BTCloseBtn;

	public override void Awake()
	{
		base.Awake();
		RectTransformInputField = GetGameObject().transform.Find("View/Bg/InputField/").GetComponent<RectTransform>();
		CanvasRendererInputField = GetGameObject().transform.Find("View/Bg/InputField/").GetComponent<CanvasRenderer>();
		ImageInputField = GetGameObject().transform.Find("View/Bg/InputField/").GetComponent<Image>();
		TMP_InputFieldInputField = GetGameObject().transform.Find("View/Bg/InputField/").GetComponent<TMP_InputField>();
		AddScriptsInputField = GetGameObject().transform.Find("View/Bg/InputField/").GetComponent<AddScripts>();
		RectTransformOkBtn = GetGameObject().transform.Find("View/Bg/OkBtn/").GetComponent<RectTransform>();
		CanvasRendererOkBtn = GetGameObject().transform.Find("View/Bg/OkBtn/").GetComponent<CanvasRenderer>();
		ImageOkBtn = GetGameObject().transform.Find("View/Bg/OkBtn/").GetComponent<Image>();
		AddScriptsOkBtn = GetGameObject().transform.Find("View/Bg/OkBtn/").GetComponent<AddScripts>();
		BTOkBtn = GetGameObject().transform.Find("View/Bg/OkBtn/").GetComponent<BT>();
		RectTransformTitle = GetGameObject().transform.Find("View/Bg/Title/").GetComponent<RectTransform>();
		CanvasRendererTitle = GetGameObject().transform.Find("View/Bg/Title/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITitle = GetGameObject().transform.Find("View/Bg/Title/").GetComponent<TextMeshProUGUI>();
		AddScriptsTitle = GetGameObject().transform.Find("View/Bg/Title/").GetComponent<AddScripts>();
		RectTransformCloseBtn = GetGameObject().transform.Find("View/Bg/CloseBtn/").GetComponent<RectTransform>();
		CanvasRendererCloseBtn = GetGameObject().transform.Find("View/Bg/CloseBtn/").GetComponent<CanvasRenderer>();
		ImageCloseBtn = GetGameObject().transform.Find("View/Bg/CloseBtn/").GetComponent<Image>();
		AddScriptsCloseBtn = GetGameObject().transform.Find("View/Bg/CloseBtn/").GetComponent<AddScripts>();
		BTCloseBtn = GetGameObject().transform.Find("View/Bg/CloseBtn/").GetComponent<BT>();
	}

	public InputWindows(Transform trans)
		: base(trans)
	{
	}

	public InputWindows()
	{
	}

	public void Init(string title, Action<string> action, bool isCanClose = true, string norText = "")
	{
		TMP_InputFieldInputField.text = norText;
		TextMeshProUGUITitle.text = title;
		_sucAction = action;
		BTCloseBtn.SetActive(isCanClose);
	}

	public override void Start()
	{
		base.Start();
		BTOkBtn.onClick.AddListener(Click);
		BTCloseBtn.onClick.AddListener(CloseUi);
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		OnValueChange();
	}

	private void OnValueChange()
	{
		TMP_InputFieldInputField.textComponent.transform.localPosition = Vector3.zero;
		if (TMP_InputFieldInputField.text.Length > 12)
		{
			TMP_InputField tMP_InputFieldInputField = TMP_InputFieldInputField;
			string text = TMP_InputFieldInputField.text;
			int length = 12 - 0;
			tMP_InputFieldInputField.text = text.Substring(0, length);
		}
	}

	private void Click()
	{
		_sucAction?.Invoke(TMP_InputFieldInputField.text);
		CloseUi();
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (BTCloseBtn.gameObject.activeSelf)
		{
			RemoveUi(GetIndex());
		}
		else if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}
}
