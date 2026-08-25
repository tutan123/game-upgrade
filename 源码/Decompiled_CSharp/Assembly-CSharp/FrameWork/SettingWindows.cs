using Script.Tool;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "SettingWindows")]
[UiMode(Mode.Normal, true)]
public class SettingWindows : UiActor
{
	public RectTransform RectTransformBack;

	public CanvasRenderer CanvasRendererBack;

	public Image ImageBack;

	public BT BTBack;

	public AddScripts AddScriptsBack;

	public override void Awake()
	{
		base.Awake();
		RectTransformBack = GetGameObject().transform.Find("Bg/Back/").GetComponent<RectTransform>();
		CanvasRendererBack = GetGameObject().transform.Find("Bg/Back/").GetComponent<CanvasRenderer>();
		ImageBack = GetGameObject().transform.Find("Bg/Back/").GetComponent<Image>();
		BTBack = GetGameObject().transform.Find("Bg/Back/").GetComponent<BT>();
		AddScriptsBack = GetGameObject().transform.Find("Bg/Back/").GetComponent<AddScripts>();
	}

	public SettingWindows(Transform trans)
		: base(trans)
	{
	}

	public SettingWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTBack.onClick.AddListener(CloseUi);
	}
}
