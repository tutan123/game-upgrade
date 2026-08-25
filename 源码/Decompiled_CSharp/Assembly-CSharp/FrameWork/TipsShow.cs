using TMPro;
using UnityEngine;

namespace FrameWork;

[ActorInfo("", "TipsShow")]
[UiMode(Mode.Normal, true)]
public class TipsShow : UiActor
{
	private float _width;

	private float _scaleFactor;

	public RectTransform RectTransformTipsShow;

	public Animator AnimatorTipsShow;

	public AddScripts AddScriptsTipsShow;

	public RectTransform RectTransformText;

	public CanvasRenderer CanvasRendererText;

	public TextMeshProUGUI TextMeshProUGUIText;

	public AddScripts AddScriptsText;

	public CanvasGroup CanvasGroupText;

	public override void Awake()
	{
		base.Awake();
		RectTransformTipsShow = GetGameObject().transform.GetComponent<RectTransform>();
		AnimatorTipsShow = GetGameObject().transform.GetComponent<Animator>();
		AddScriptsTipsShow = GetGameObject().transform.GetComponent<AddScripts>();
		RectTransformText = GetGameObject().transform.Find("Text/").GetComponent<RectTransform>();
		CanvasRendererText = GetGameObject().transform.Find("Text/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIText = GetGameObject().transform.Find("Text/").GetComponent<TextMeshProUGUI>();
		AddScriptsText = GetGameObject().transform.Find("Text/").GetComponent<AddScripts>();
		CanvasGroupText = GetGameObject().transform.Find("Text/").GetComponent<CanvasGroup>();
	}

	public TipsShow(Transform trans)
		: base(trans)
	{
	}

	public TipsShow()
	{
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
	}

	public override void Start()
	{
		base.Start();
	}

	public void Init(Vector2 pos, bool isCustom = true)
	{
		pos = Tool.GetTargetLocalLoc(RectTransformTipsShow, pos);
		RectTransformText.anchoredPosition = pos;
		if (isCustom)
		{
			if (pos.x > _width)
			{
				RectTransformText.pivot = new Vector2(1f, 1f);
			}
			else
			{
				RectTransformText.pivot = new Vector2(0f, 0f);
			}
		}
	}

	public override void OnEnable()
	{
		base.OnEnable();
		_width = RectTransformText.rect.width * UiManager.ScaleFactor;
	}

	private void UpdatePosition()
	{
		RectTransformText.anchoredPosition = Input.mousePosition / UiManager.ScaleFactor;
		if (Input.mousePosition.x > _width)
		{
			RectTransformText.pivot = new Vector2(1f, 1f);
		}
		else
		{
			RectTransformText.pivot = new Vector2(0f, 0f);
		}
	}
}
