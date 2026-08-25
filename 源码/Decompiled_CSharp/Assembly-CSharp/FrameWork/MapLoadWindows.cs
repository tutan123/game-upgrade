using System.Collections.Generic;
using Script.Mrg;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Load, false)]
[ActorInfo("", "MapLoadWindows")]
public class MapLoadWindows : UiActor
{
	private float _loadProgress;

	private float _switchTime = 3f;

	public RectTransform RectTransformMapLoadWindows;

	public MapLoadData MapLoadDataMapLoadWindows;

	public AddScripts AddScriptsMapLoadWindows;

	public RectTransform RectTransformBgImage;

	public CanvasRenderer CanvasRendererBgImage;

	public Image ImageBgImage;

	public AddScripts AddScriptsBgImage;

	public RectTransform RectTransformSlider;

	public Slider SliderSlider;

	public AddScripts AddScriptsSlider;

	public RectTransform RectTransformLoadText;

	public CanvasRenderer CanvasRendererLoadText;

	public TextMeshProUGUI TextMeshProUGUILoadText;

	public AddScripts AddScriptsLoadText;

	public RectTransform RectTransformTips;

	public CanvasRenderer CanvasRendererTips;

	public TextMeshProUGUI TextMeshProUGUITips;

	public AddScripts AddScriptsTips;

	public override void Awake()
	{
		base.Awake();
		RectTransformMapLoadWindows = GetGameObject().transform.GetComponent<RectTransform>();
		MapLoadDataMapLoadWindows = GetGameObject().transform.GetComponent<MapLoadData>();
		AddScriptsMapLoadWindows = GetGameObject().transform.GetComponent<AddScripts>();
		RectTransformBgImage = GetGameObject().transform.Find("Bg/BgImage/").GetComponent<RectTransform>();
		CanvasRendererBgImage = GetGameObject().transform.Find("Bg/BgImage/").GetComponent<CanvasRenderer>();
		ImageBgImage = GetGameObject().transform.Find("Bg/BgImage/").GetComponent<Image>();
		AddScriptsBgImage = GetGameObject().transform.Find("Bg/BgImage/").GetComponent<AddScripts>();
		RectTransformSlider = GetGameObject().transform.Find("View/Slider/").GetComponent<RectTransform>();
		SliderSlider = GetGameObject().transform.Find("View/Slider/").GetComponent<Slider>();
		AddScriptsSlider = GetGameObject().transform.Find("View/Slider/").GetComponent<AddScripts>();
		RectTransformLoadText = GetGameObject().transform.Find("View/Slider/LoadText/").GetComponent<RectTransform>();
		CanvasRendererLoadText = GetGameObject().transform.Find("View/Slider/LoadText/").GetComponent<CanvasRenderer>();
		TextMeshProUGUILoadText = GetGameObject().transform.Find("View/Slider/LoadText/").GetComponent<TextMeshProUGUI>();
		AddScriptsLoadText = GetGameObject().transform.Find("View/Slider/LoadText/").GetComponent<AddScripts>();
		RectTransformTips = GetGameObject().transform.Find("View/Tips/").GetComponent<RectTransform>();
		CanvasRendererTips = GetGameObject().transform.Find("View/Tips/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITips = GetGameObject().transform.Find("View/Tips/").GetComponent<TextMeshProUGUI>();
		AddScriptsTips = GetGameObject().transform.Find("View/Tips/").GetComponent<AddScripts>();
	}

	public MapLoadWindows(Transform trans)
		: base(trans)
	{
	}

	public MapLoadWindows()
	{
	}

	public override void Open(object[] objects)
	{
		_loadProgress = 0f;
		SliderSlider.value = 0f;
		SetText(LanguageMrg.GetText("A1684"));
		TextMeshProUGUITips.text = Tool.GetLoadTips();
		ImageBgImage.sprite = MapLoadDataMapLoadWindows.GetRandomSprite();
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		if (SliderSlider.value < _loadProgress)
		{
			SliderSlider.value += 0.2f * deltaTime;
		}
	}

	public void SetProgress(float progress)
	{
		_loadProgress = progress;
	}

	public void SetText(string text)
	{
		TextMeshProUGUILoadText.text = text;
	}

	public void SetCom()
	{
		SetText(LanguageMrg.GetText("A1688"));
		SetProgress(1f);
		SliderSlider.value = 1f;
	}

	public override void OnClose()
	{
	}

	protected override void RemoveUi(List<object> parma)
	{
	}
}
