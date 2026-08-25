using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("fadd7b29307c3cdc0103ad6a6b4a78ca", "UiRoot")]
public class UiRoot : UiActor
{
	public RectTransform RectTransformUiRoot;

	public Canvas CanvasUiRoot;

	public CanvasScaler CanvasScalerUiRoot;

	public GraphicRaycaster GraphicRaycasterUiRoot;

	public RectTransform RectTransformBackground;

	public RectTransform RectTransformNormal;

	public RectTransform RectTransformControl;

	public RectTransform RectTransformPopup;

	public override void Awake()
	{
		base.Awake();
		RectTransformUiRoot = GetGameObject().transform.GetComponent<RectTransform>();
		CanvasUiRoot = GetGameObject().transform.GetComponent<Canvas>();
		CanvasScalerUiRoot = GetGameObject().transform.GetComponent<CanvasScaler>();
		GraphicRaycasterUiRoot = GetGameObject().transform.GetComponent<GraphicRaycaster>();
		RectTransformBackground = GetGameObject().transform.Find("Background/").GetComponent<RectTransform>();
		RectTransformNormal = GetGameObject().transform.Find("Normal/").GetComponent<RectTransform>();
		RectTransformControl = GetGameObject().transform.Find("Control/").GetComponent<RectTransform>();
		RectTransformPopup = GetGameObject().transform.Find("Popup/").GetComponent<RectTransform>();
	}

	public UiRoot(Transform trans)
		: base(trans)
	{
	}

	public UiRoot()
	{
	}

	protected override void RemoveUi(List<object> parma)
	{
	}
}
