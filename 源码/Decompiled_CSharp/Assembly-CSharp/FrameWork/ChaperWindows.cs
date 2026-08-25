using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "ChaperWindows")]
public class ChaperWindows : UiActor
{
	public RectTransform RectTransformTitleText;

	public CanvasRenderer CanvasRendererTitleText;

	public TextMeshProUGUI TextMeshProUGUITitleText;

	public AddScripts AddScriptsTitleText;

	public RectTransform RectTransformBlack;

	public CanvasRenderer CanvasRendererBlack;

	public Image ImageBlack;

	public AddScripts AddScriptsBlack;

	public BT BTBlack;

	public RectTransform RectTransformSlider;

	public Slider SliderSlider;

	public AddScripts AddScriptsSlider;

	public RectTransform RectTransformContent;

	public VerticalLayoutGroup VerticalLayoutGroupContent;

	public ContentSizeFitter ContentSizeFitterContent;

	public AddScripts AddScriptsContent;

	public RectTransform RectTransformChaperIcon;

	public CanvasRenderer CanvasRendererChaperIcon;

	public Image ImageChaperIcon;

	public AddScripts AddScriptsChaperIcon;

	public RectTransform RectTransformRoleInfo;

	public AddScripts AddScriptsRoleInfo;

	public override void Awake()
	{
		base.Awake();
		RectTransformTitleText = GetGameObject().transform.Find("Bg/TitleText/").GetComponent<RectTransform>();
		CanvasRendererTitleText = GetGameObject().transform.Find("Bg/TitleText/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITitleText = GetGameObject().transform.Find("Bg/TitleText/").GetComponent<TextMeshProUGUI>();
		AddScriptsTitleText = GetGameObject().transform.Find("Bg/TitleText/").GetComponent<AddScripts>();
		RectTransformBlack = GetGameObject().transform.Find("Bg/Black/").GetComponent<RectTransform>();
		CanvasRendererBlack = GetGameObject().transform.Find("Bg/Black/").GetComponent<CanvasRenderer>();
		ImageBlack = GetGameObject().transform.Find("Bg/Black/").GetComponent<Image>();
		AddScriptsBlack = GetGameObject().transform.Find("Bg/Black/").GetComponent<AddScripts>();
		BTBlack = GetGameObject().transform.Find("Bg/Black/").GetComponent<BT>();
		RectTransformSlider = GetGameObject().transform.Find("Bg/ChaperProgress/Slider/").GetComponent<RectTransform>();
		SliderSlider = GetGameObject().transform.Find("Bg/ChaperProgress/Slider/").GetComponent<Slider>();
		AddScriptsSlider = GetGameObject().transform.Find("Bg/ChaperProgress/Slider/").GetComponent<AddScripts>();
		RectTransformContent = GetGameObject().transform.Find("View/Scroll View/Viewport/Content/").GetComponent<RectTransform>();
		VerticalLayoutGroupContent = GetGameObject().transform.Find("View/Scroll View/Viewport/Content/").GetComponent<VerticalLayoutGroup>();
		ContentSizeFitterContent = GetGameObject().transform.Find("View/Scroll View/Viewport/Content/").GetComponent<ContentSizeFitter>();
		AddScriptsContent = GetGameObject().transform.Find("View/Scroll View/Viewport/Content/").GetComponent<AddScripts>();
		RectTransformChaperIcon = GetGameObject().transform.Find("View/Info/ChaperIcon/").GetComponent<RectTransform>();
		CanvasRendererChaperIcon = GetGameObject().transform.Find("View/Info/ChaperIcon/").GetComponent<CanvasRenderer>();
		ImageChaperIcon = GetGameObject().transform.Find("View/Info/ChaperIcon/").GetComponent<Image>();
		AddScriptsChaperIcon = GetGameObject().transform.Find("View/Info/ChaperIcon/").GetComponent<AddScripts>();
		RectTransformRoleInfo = GetGameObject().transform.Find("View/Info/RoleInfo/").GetComponent<RectTransform>();
		AddScriptsRoleInfo = GetGameObject().transform.Find("View/Info/RoleInfo/").GetComponent<AddScripts>();
	}

	public ChaperWindows(Transform trans)
		: base(trans)
	{
	}

	public ChaperWindows()
	{
	}
}
