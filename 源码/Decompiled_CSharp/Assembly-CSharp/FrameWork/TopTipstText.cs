using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "TopTipstText")]
[UiMode(Mode.Normal, true)]
public class TopTipstText : UiActor
{
	public RectTransform RectTransformTopTipstText;

	public CanvasGroup CanvasGroupTopTipstText;

	public AddScripts AddScriptsTopTipstText;

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
		RectTransformTopTipstText = GetGameObject().transform.GetComponent<RectTransform>();
		CanvasGroupTopTipstText = GetGameObject().transform.GetComponent<CanvasGroup>();
		AddScriptsTopTipstText = GetGameObject().transform.GetComponent<AddScripts>();
		RectTransformView = GetGameObject().transform.Find("View/").GetComponent<RectTransform>();
		CanvasRendererView = GetGameObject().transform.Find("View/").GetComponent<CanvasRenderer>();
		ImageView = GetGameObject().transform.Find("View/").GetComponent<Image>();
		AddScriptsView = GetGameObject().transform.Find("View/").GetComponent<AddScripts>();
		RectTransformTitle = GetGameObject().transform.Find("View/Title/").GetComponent<RectTransform>();
		CanvasRendererTitle = GetGameObject().transform.Find("View/Title/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITitle = GetGameObject().transform.Find("View/Title/").GetComponent<TextMeshProUGUI>();
		AddScriptsTitle = GetGameObject().transform.Find("View/Title/").GetComponent<AddScripts>();
	}

	public TopTipstText(Transform trans)
		: base(trans)
	{
	}

	public TopTipstText()
	{
	}
}
