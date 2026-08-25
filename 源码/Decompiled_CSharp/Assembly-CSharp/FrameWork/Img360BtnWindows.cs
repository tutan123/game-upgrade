using System;
using System.Collections.Generic;
using Script.Mrg;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "Img360BtnWindows")]
[UiMode(Mode.Control, true)]
public class Img360BtnWindows : UiActor
{
	private float _scrollSpeed = 50000f;

	private float _smoothTime = 0.1f;

	private Vector2 _targetPos;

	private Vector2 _currentVel;

	public RectTransform RectTransformScrollView;

	public CanvasRenderer CanvasRendererScrollView;

	public Image ImageScrollView;

	public ScrollRect ScrollRectScrollView;

	public AddScripts AddScriptsScrollView;

	public RectTransform RectTransformBgImg;

	public CanvasRenderer CanvasRendererBgImg;

	public Image ImageBgImg;

	public AddScripts AddScriptsBgImg;

	public RectTransform RectTransformInfoBg;

	public CanvasRenderer CanvasRendererInfoBg;

	public CanvasGroup CanvasGroupInfoBg;

	public CanvasGroupTool CanvasGroupToolInfoBg;

	public Image ImageInfoBg;

	public AddScripts AddScriptsInfoBg;

	public RectTransform RectTransformInfoText;

	public CanvasRenderer CanvasRendererInfoText;

	public TextMeshProUGUI TextMeshProUGUIInfoText;

	public AddScripts AddScriptsInfoText;

	public RectTransform RectTransformTimeOutText;

	public CanvasRenderer CanvasRendererTimeOutText;

	public TextMeshProUGUI TextMeshProUGUITimeOutText;

	public AddScripts AddScriptsTimeOutText;

	public RectTransform RectTransformUseText;

	public CanvasRenderer CanvasRendererUseText;

	public TextMeshProUGUI TextMeshProUGUIUseText;

	public AddScripts AddScriptsUseText;

	public override void Awake()
	{
		base.Awake();
		RectTransformScrollView = GetGameObject().transform.Find("View/Scroll View/").GetComponent<RectTransform>();
		CanvasRendererScrollView = GetGameObject().transform.Find("View/Scroll View/").GetComponent<CanvasRenderer>();
		ImageScrollView = GetGameObject().transform.Find("View/Scroll View/").GetComponent<Image>();
		ScrollRectScrollView = GetGameObject().transform.Find("View/Scroll View/").GetComponent<ScrollRect>();
		AddScriptsScrollView = GetGameObject().transform.Find("View/Scroll View/").GetComponent<AddScripts>();
		RectTransformBgImg = GetGameObject().transform.Find("View/Scroll View/Viewport/Content/BgImg/").GetComponent<RectTransform>();
		CanvasRendererBgImg = GetGameObject().transform.Find("View/Scroll View/Viewport/Content/BgImg/").GetComponent<CanvasRenderer>();
		ImageBgImg = GetGameObject().transform.Find("View/Scroll View/Viewport/Content/BgImg/").GetComponent<Image>();
		AddScriptsBgImg = GetGameObject().transform.Find("View/Scroll View/Viewport/Content/BgImg/").GetComponent<AddScripts>();
		RectTransformInfoBg = GetGameObject().transform.Find("View/InfoBg/").GetComponent<RectTransform>();
		CanvasRendererInfoBg = GetGameObject().transform.Find("View/InfoBg/").GetComponent<CanvasRenderer>();
		CanvasGroupInfoBg = GetGameObject().transform.Find("View/InfoBg/").GetComponent<CanvasGroup>();
		CanvasGroupToolInfoBg = GetGameObject().transform.Find("View/InfoBg/").GetComponent<CanvasGroupTool>();
		ImageInfoBg = GetGameObject().transform.Find("View/InfoBg/").GetComponent<Image>();
		AddScriptsInfoBg = GetGameObject().transform.Find("View/InfoBg/").GetComponent<AddScripts>();
		RectTransformInfoText = GetGameObject().transform.Find("View/InfoBg/InfoText/").GetComponent<RectTransform>();
		CanvasRendererInfoText = GetGameObject().transform.Find("View/InfoBg/InfoText/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIInfoText = GetGameObject().transform.Find("View/InfoBg/InfoText/").GetComponent<TextMeshProUGUI>();
		AddScriptsInfoText = GetGameObject().transform.Find("View/InfoBg/InfoText/").GetComponent<AddScripts>();
		RectTransformTimeOutText = GetGameObject().transform.Find("View/InfoBg/Info/TimeOutText/").GetComponent<RectTransform>();
		CanvasRendererTimeOutText = GetGameObject().transform.Find("View/InfoBg/Info/TimeOutText/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITimeOutText = GetGameObject().transform.Find("View/InfoBg/Info/TimeOutText/").GetComponent<TextMeshProUGUI>();
		AddScriptsTimeOutText = GetGameObject().transform.Find("View/InfoBg/Info/TimeOutText/").GetComponent<AddScripts>();
		RectTransformUseText = GetGameObject().transform.Find("View/InfoBg/Info/UseText/").GetComponent<RectTransform>();
		CanvasRendererUseText = GetGameObject().transform.Find("View/InfoBg/Info/UseText/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIUseText = GetGameObject().transform.Find("View/InfoBg/Info/UseText/").GetComponent<TextMeshProUGUI>();
		AddScriptsUseText = GetGameObject().transform.Find("View/InfoBg/Info/UseText/").GetComponent<AddScripts>();
	}

	public Img360BtnWindows(Transform trans)
		: base(trans)
	{
	}

	public Img360BtnWindows()
	{
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Open(object[] objects)
	{
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		Move();
	}

	private void Move()
	{
		if (!Input.GetMouseButton(0))
		{
			_targetPos = ScrollRectScrollView.content.anchoredPosition;
			float x = 0f - Input.GetAxisRaw("Horizontal");
			float y = 0f - Input.GetAxisRaw("Vertical");
			Vector2 vector = new Vector2(x, y) * _scrollSpeed * Time.deltaTime;
			_targetPos += vector;
			ScrollRectScrollView.content.anchoredPosition = Vector2.SmoothDamp(ScrollRectScrollView.content.anchoredPosition, _targetPos, ref _currentVel, _smoothTime);
		}
	}

	private void ClampTargetPosition()
	{
		Vector2 size = ScrollRectScrollView.content.rect.size;
		Vector2 size2 = ScrollRectScrollView.viewport.rect.size;
		float min = 0f;
		float max = size.x - size2.x;
		float min2 = 0f - (size.y - size2.y);
		float max2 = 0f;
		_targetPos.x = Mathf.Clamp(_targetPos.x, min, max);
		_targetPos.y = Mathf.Clamp(_targetPos.y, min2, max2);
	}

	public void Init(Sprite sprite, VideoNode videoNode, List<ButtonNode> nodes)
	{
		ImageBgImg.sprite = sprite;
		double num = (double)sprite.texture.width / (double)sprite.texture.height;
		int num2 = 1920;
		int num3 = (int)Math.Round((double)num2 / num);
		if (num3 < 1080)
		{
			num3 = 1080;
			num2 = (int)Math.Round((double)num3 * num);
		}
		ImageBgImg.rectTransform.sizeDelta = new Vector2(num2, num3);
		RectTransformBgImg.transform.TranFor(nodes.Count, RectTransformBgImg.transform.GetChild(0), delegate(int i, GameObject o)
		{
			o.transform.localPosition = nodes[i].buttonPosition;
			o.GetComponent<VideoButton>().Init(LanguageMrg.GetText(nodes[i].btnName), nodes[i].buttonSize, videoNode, nodes[i].GetOutVideo(), nodes[i]);
		});
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}
}
