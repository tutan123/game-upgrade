using System;
using System.Collections.Generic;
using Script.MiNiGame;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "LevelFailWindows")]
public class LevelFailWindows : UiActor
{
	private Action _action;

	public RectTransform RectTransformNextBtn;

	public CanvasRenderer CanvasRendererNextBtn;

	public Image ImageNextBtn;

	public BT BTNextBtn;

	public AddScripts AddScriptsNextBtn;

	public RectTransform RectTransformWuPinText;

	public CanvasRenderer CanvasRendererWuPinText;

	public TextMeshProUGUI TextMeshProUGUIWuPinText;

	public AddScripts AddScriptsWuPinText;

	public RectTransform RectTransformScoreText;

	public CanvasRenderer CanvasRendererScoreText;

	public TextMeshProUGUI TextMeshProUGUIScoreText;

	public AddScripts AddScriptsScoreText;

	public RectTransform RectTransformKpi;

	public CanvasRenderer CanvasRendererKpi;

	public KpiInfo KpiInfoKpi;

	public AddScripts AddScriptsKpi;

	public VerticalLayoutGroup VerticalLayoutGroupKpi;

	public override void Awake()
	{
		base.Awake();
		RectTransformNextBtn = GetGameObject().transform.Find("Bg/NextBtn/").GetComponent<RectTransform>();
		CanvasRendererNextBtn = GetGameObject().transform.Find("Bg/NextBtn/").GetComponent<CanvasRenderer>();
		ImageNextBtn = GetGameObject().transform.Find("Bg/NextBtn/").GetComponent<Image>();
		BTNextBtn = GetGameObject().transform.Find("Bg/NextBtn/").GetComponent<BT>();
		AddScriptsNextBtn = GetGameObject().transform.Find("Bg/NextBtn/").GetComponent<AddScripts>();
		RectTransformWuPinText = GetGameObject().transform.Find("View/Bg/WuPinText/").GetComponent<RectTransform>();
		CanvasRendererWuPinText = GetGameObject().transform.Find("View/Bg/WuPinText/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIWuPinText = GetGameObject().transform.Find("View/Bg/WuPinText/").GetComponent<TextMeshProUGUI>();
		AddScriptsWuPinText = GetGameObject().transform.Find("View/Bg/WuPinText/").GetComponent<AddScripts>();
		RectTransformScoreText = GetGameObject().transform.Find("View/Bg/ScoreText/").GetComponent<RectTransform>();
		CanvasRendererScoreText = GetGameObject().transform.Find("View/Bg/ScoreText/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIScoreText = GetGameObject().transform.Find("View/Bg/ScoreText/").GetComponent<TextMeshProUGUI>();
		AddScriptsScoreText = GetGameObject().transform.Find("View/Bg/ScoreText/").GetComponent<AddScripts>();
		RectTransformKpi = GetGameObject().transform.Find("View/Bg/MiNiGameData/Kpi/").GetComponent<RectTransform>();
		CanvasRendererKpi = GetGameObject().transform.Find("View/Bg/MiNiGameData/Kpi/").GetComponent<CanvasRenderer>();
		KpiInfoKpi = GetGameObject().transform.Find("View/Bg/MiNiGameData/Kpi/").GetComponent<KpiInfo>();
		AddScriptsKpi = GetGameObject().transform.Find("View/Bg/MiNiGameData/Kpi/").GetComponent<AddScripts>();
		VerticalLayoutGroupKpi = GetGameObject().transform.Find("View/Bg/MiNiGameData/Kpi/").GetComponent<VerticalLayoutGroup>();
	}

	public LevelFailWindows(Transform trans)
		: base(trans)
	{
	}

	public LevelFailWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTNextBtn.onClick.AddListener(Close);
	}

	public void Init(string text, Action action)
	{
		_action = action;
		KpiInfoKpi.Init(text);
	}

	private void Close()
	{
		_action?.Invoke();
		CloseUi();
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}
}
