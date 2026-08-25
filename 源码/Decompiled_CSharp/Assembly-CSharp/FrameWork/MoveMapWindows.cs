using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "MoveMapWindows")]
[UiMode(Mode.Normal, true)]
public class MoveMapWindows : UiActor
{
	public RectTransform RectTransformRawImage;

	public CanvasRenderer CanvasRendererRawImage;

	public RawImage RawImageRawImage;

	public AddScripts AddScriptsRawImage;

	public override void Awake()
	{
		base.Awake();
		RectTransformRawImage = GetGameObject().transform.Find("View/RawImage/").GetComponent<RectTransform>();
		CanvasRendererRawImage = GetGameObject().transform.Find("View/RawImage/").GetComponent<CanvasRenderer>();
		RawImageRawImage = GetGameObject().transform.Find("View/RawImage/").GetComponent<RawImage>();
		AddScriptsRawImage = GetGameObject().transform.Find("View/RawImage/").GetComponent<AddScripts>();
	}

	public MoveMapWindows(Transform trans)
		: base(trans)
	{
	}

	public MoveMapWindows()
	{
	}
}
