using System.Collections.Generic;
using Script.Mrg;
using Script.UiTool;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "InfoRightTipstText")]
[UiMode(Mode.Popup, false)]
public class InfoRightTipstText : UiActor
{
	private Queue<TaskTipsData> _queue = new Queue<TaskTipsData>();

	public RectTransform RectTransformInfoRightTipstText;

	public AddScripts AddScriptsInfoRightTipstText;

	public RectTransform RectTransformView;

	public CanvasRenderer CanvasRendererView;

	public Image ImageView;

	public AddScripts AddScriptsView;

	public VerticalLayoutGroup VerticalLayoutGroupView;

	public override void Awake()
	{
		base.Awake();
		RectTransformInfoRightTipstText = GetGameObject().transform.GetComponent<RectTransform>();
		AddScriptsInfoRightTipstText = GetGameObject().transform.GetComponent<AddScripts>();
		RectTransformView = GetGameObject().transform.Find("View/").GetComponent<RectTransform>();
		CanvasRendererView = GetGameObject().transform.Find("View/").GetComponent<CanvasRenderer>();
		ImageView = GetGameObject().transform.Find("View/").GetComponent<Image>();
		AddScriptsView = GetGameObject().transform.Find("View/").GetComponent<AddScripts>();
		VerticalLayoutGroupView = GetGameObject().transform.Find("View/").GetComponent<VerticalLayoutGroup>();
	}

	public InfoRightTipstText(Transform trans)
		: base(trans)
	{
	}

	public InfoRightTipstText()
	{
	}

	public override void Start()
	{
		base.Start();
		Tool.HideAllChild(RectTransformView);
	}

	public void AddTips(TaskTipsData message)
	{
		_queue.Enqueue(message);
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		if (_queue.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < RectTransformView.childCount; i++)
		{
			if (!RectTransformView.GetChild(i).gameObject.activeSelf)
			{
				ShowTips(RectTransformView.GetChild(i), _queue.Dequeue());
				break;
			}
		}
	}

	private void ShowTips(Transform tips, TaskTipsData data)
	{
		tips.SetActive(active: true);
		tips.GetComponent<InfoRightItem>().Init(data);
	}

	protected override void RemoveUi(List<object> parma)
	{
	}
}
