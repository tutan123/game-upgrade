using DG.Tweening;
using Script.MinigameWebView;
using Script.Mrg;
using Script.Tool;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "GameWindows")]
public class GameWindows : UiActor
{
	public RectTransform RectTransformGameView;

	public AddScripts AddScriptsGameView;

	public MinigameWebViewLauncher MinigameWebViewLauncherGameView;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public override void Awake()
	{
		base.Awake();
		RectTransformGameView = GetGameObject().transform.Find("View/GameView/").GetComponent<RectTransform>();
		AddScriptsGameView = GetGameObject().transform.Find("View/GameView/").GetComponent<AddScripts>();
		MinigameWebViewLauncherGameView = GetGameObject().transform.Find("View/GameView/").GetComponent<MinigameWebViewLauncher>();
		RectTransformClose = GetGameObject().transform.Find("View/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("View/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("View/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("View/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("View/Close/").GetComponent<AddScripts>();
	}

	public GameWindows(Transform trans)
		: base(trans)
	{
	}

	public GameWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(Close);
	}

	public override void OnEnable()
	{
		base.OnEnable();
	}

	private void Close()
	{
		UiManager.OpenUi<SureWindows>().Init(LanguageMrg.GetText("A4720"), CloseUi);
	}

	public override void Open(object[] objects)
	{
		Transform transform = base.transform.Find("View");
		if ((bool)transform)
		{
			DoScale(transform.GetComponent<RectTransform>(), new Vector3(0.6f, 0.6f, 0.6f), Vector3.one);
			CanvasGroup component = transform.GetComponent<CanvasGroup>();
			if ((bool)component)
			{
				component.DOFade(1f, 0.3f);
			}
		}
		MinigameWebViewLauncherGameView.OpenPinball();
	}
}
