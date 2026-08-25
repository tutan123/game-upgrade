using Script.Tool;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "ImgShowWindows")]
public class ImgShowWindows : UiActor
{
	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformImg;

	public CanvasRenderer CanvasRendererImg;

	public Image ImageImg;

	public AddScripts AddScriptsImg;

	public override void Awake()
	{
		base.Awake();
		RectTransformClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<AddScripts>();
		RectTransformImg = GetGameObject().transform.Find("View/Img/").GetComponent<RectTransform>();
		CanvasRendererImg = GetGameObject().transform.Find("View/Img/").GetComponent<CanvasRenderer>();
		ImageImg = GetGameObject().transform.Find("View/Img/").GetComponent<Image>();
		AddScriptsImg = GetGameObject().transform.Find("View/Img/").GetComponent<AddScripts>();
	}

	public ImgShowWindows(Transform trans)
		: base(trans)
	{
	}

	public ImgShowWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(CloseUi);
	}

	public void Init(Sprite sprite)
	{
		ImageImg.sprite = sprite;
	}
}
