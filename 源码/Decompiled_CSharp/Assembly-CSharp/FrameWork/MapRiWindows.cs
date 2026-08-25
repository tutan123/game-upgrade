using System.Collections.Generic;
using Script.UiTool;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "MapRiWindows")]
public class MapRiWindows : UiActor
{
	private float _scrollSpeed = 20000f;

	private float _smoothTime = 0.1f;

	private Vector2 _targetPos;

	private Vector2 _currentVel;

	public RectTransform RectTransformView;

	public MapTool MapToolView;

	public AddScripts AddScriptsView;

	public CanvasGroup CanvasGroupView;

	public RectTransform RectTransformScrollView;

	public CanvasRenderer CanvasRendererScrollView;

	public Image ImageScrollView;

	public ScrollRect ScrollRectScrollView;

	public AddScripts AddScriptsScrollView;

	public RectTransform RectTransformPointGroup;

	public AddScripts AddScriptsPointGroup;

	public override void Awake()
	{
		base.Awake();
		RectTransformView = GetGameObject().transform.Find("View/").GetComponent<RectTransform>();
		MapToolView = GetGameObject().transform.Find("View/").GetComponent<MapTool>();
		AddScriptsView = GetGameObject().transform.Find("View/").GetComponent<AddScripts>();
		CanvasGroupView = GetGameObject().transform.Find("View/").GetComponent<CanvasGroup>();
		RectTransformScrollView = GetGameObject().transform.Find("View/Scroll View/").GetComponent<RectTransform>();
		CanvasRendererScrollView = GetGameObject().transform.Find("View/Scroll View/").GetComponent<CanvasRenderer>();
		ImageScrollView = GetGameObject().transform.Find("View/Scroll View/").GetComponent<Image>();
		ScrollRectScrollView = GetGameObject().transform.Find("View/Scroll View/").GetComponent<ScrollRect>();
		AddScriptsScrollView = GetGameObject().transform.Find("View/Scroll View/").GetComponent<AddScripts>();
		RectTransformPointGroup = GetGameObject().transform.Find("View/Scroll View/Viewport/Content/Map/PointGroup/").GetComponent<RectTransform>();
		AddScriptsPointGroup = GetGameObject().transform.Find("View/Scroll View/Viewport/Content/Map/PointGroup/").GetComponent<AddScripts>();
	}

	public MapRiWindows(Transform trans)
		: base(trans)
	{
	}

	public MapRiWindows()
	{
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		MapToolView.Init();
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
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

	public override string GetUiName()
	{
		return "Map";
	}
}
