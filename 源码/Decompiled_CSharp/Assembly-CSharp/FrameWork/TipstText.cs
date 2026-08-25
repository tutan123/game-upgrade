using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Popup, true)]
[ActorInfo("", "TipstText")]
public class TipstText : UiActor
{
	public RectTransform RectTransformTipstText;

	public CanvasGroup CanvasGroupTipstText;

	public AddScripts AddScriptsTipstText;

	public RectTransform RectTransformView;

	public CanvasRenderer CanvasRendererView;

	public Image ImageView;

	public AddScripts AddScriptsView;

	public RectTransform RectTransformTitle;

	public CanvasRenderer CanvasRendererTitle;

	public TextMeshProUGUI TextMeshProUGUITitle;

	public AddScripts AddScriptsTitle;

	public override void Awake()
	{
		base.Awake();
		RectTransformTipstText = GetGameObject().transform.GetComponent<RectTransform>();
		CanvasGroupTipstText = GetGameObject().transform.GetComponent<CanvasGroup>();
		AddScriptsTipstText = GetGameObject().transform.GetComponent<AddScripts>();
		RectTransformView = GetGameObject().transform.Find("View/").GetComponent<RectTransform>();
		CanvasRendererView = GetGameObject().transform.Find("View/").GetComponent<CanvasRenderer>();
		ImageView = GetGameObject().transform.Find("View/").GetComponent<Image>();
		AddScriptsView = GetGameObject().transform.Find("View/").GetComponent<AddScripts>();
		RectTransformTitle = GetGameObject().transform.Find("View/Title/").GetComponent<RectTransform>();
		CanvasRendererTitle = GetGameObject().transform.Find("View/Title/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITitle = GetGameObject().transform.Find("View/Title/").GetComponent<TextMeshProUGUI>();
		AddScriptsTitle = GetGameObject().transform.Find("View/Title/").GetComponent<AddScripts>();
	}

	public TipstText(Transform trans)
		: base(trans)
	{
	}

	public TipstText()
	{
	}
}
