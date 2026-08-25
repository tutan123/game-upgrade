using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Script.Tool;
using Script.UiTool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "VideoListWindows")]
[UiMode(Mode.Normal, true)]
public class VideoListWindows : UiActor
{
	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformTitle;

	public CanvasRenderer CanvasRendererTitle;

	public TextMeshProUGUI TextMeshProUGUITitle;

	public AddScripts AddScriptsTitle;

	public RectTransform RectTransformScrollView;

	public CanvasRenderer CanvasRendererScrollView;

	public Image ImageScrollView;

	public ScrollRect ScrollRectScrollView;

	public AddScripts AddScriptsScrollView;

	public RectTransform RectTransformContent;

	public ContentSizeFitter ContentSizeFitterContent;

	public VerticalLayoutGroup VerticalLayoutGroupContent;

	public AddScripts AddScriptsContent;

	public RectTransform RectTransformInfoBg;

	public CanvasRenderer CanvasRendererInfoBg;

	public Image ImageInfoBg;

	public AddScripts AddScriptsInfoBg;

	public VideoInfo VideoInfoInfoBg;

	public RectTransform RectTransformHaoGanDu;

	public CanvasRenderer CanvasRendererHaoGanDu;

	public TextMeshProUGUI TextMeshProUGUIHaoGanDu;

	public AddScripts AddScriptsHaoGanDu;

	public override void Awake()
	{
		base.Awake();
		RectTransformClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<AddScripts>();
		RectTransformTitle = GetGameObject().transform.Find("Bg/Title/").GetComponent<RectTransform>();
		CanvasRendererTitle = GetGameObject().transform.Find("Bg/Title/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITitle = GetGameObject().transform.Find("Bg/Title/").GetComponent<TextMeshProUGUI>();
		AddScriptsTitle = GetGameObject().transform.Find("Bg/Title/").GetComponent<AddScripts>();
		RectTransformScrollView = GetGameObject().transform.Find("View/Bg/Scroll View/").GetComponent<RectTransform>();
		CanvasRendererScrollView = GetGameObject().transform.Find("View/Bg/Scroll View/").GetComponent<CanvasRenderer>();
		ImageScrollView = GetGameObject().transform.Find("View/Bg/Scroll View/").GetComponent<Image>();
		ScrollRectScrollView = GetGameObject().transform.Find("View/Bg/Scroll View/").GetComponent<ScrollRect>();
		AddScriptsScrollView = GetGameObject().transform.Find("View/Bg/Scroll View/").GetComponent<AddScripts>();
		RectTransformContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<RectTransform>();
		ContentSizeFitterContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<ContentSizeFitter>();
		VerticalLayoutGroupContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<VerticalLayoutGroup>();
		AddScriptsContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<AddScripts>();
		RectTransformInfoBg = GetGameObject().transform.Find("View/Bg/InfoBg/").GetComponent<RectTransform>();
		CanvasRendererInfoBg = GetGameObject().transform.Find("View/Bg/InfoBg/").GetComponent<CanvasRenderer>();
		ImageInfoBg = GetGameObject().transform.Find("View/Bg/InfoBg/").GetComponent<Image>();
		AddScriptsInfoBg = GetGameObject().transform.Find("View/Bg/InfoBg/").GetComponent<AddScripts>();
		VideoInfoInfoBg = GetGameObject().transform.Find("View/Bg/InfoBg/").GetComponent<VideoInfo>();
		RectTransformHaoGanDu = GetGameObject().transform.Find("View/Bg/InfoBg/HaoGanDu/").GetComponent<RectTransform>();
		CanvasRendererHaoGanDu = GetGameObject().transform.Find("View/Bg/InfoBg/HaoGanDu/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIHaoGanDu = GetGameObject().transform.Find("View/Bg/InfoBg/HaoGanDu/").GetComponent<TextMeshProUGUI>();
		AddScriptsHaoGanDu = GetGameObject().transform.Find("View/Bg/InfoBg/HaoGanDu/").GetComponent<AddScripts>();
	}

	public VideoListWindows(Transform trans)
		: base(trans)
	{
	}

	public VideoListWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(CloseUi);
	}

	public override void Open(object[] objects)
	{
		Transform transform = base.transform.Find("Bg");
		if ((bool)transform)
		{
			CanvasGroup component = transform.GetComponent<CanvasGroup>();
			if ((bool)component)
			{
				component.DOFade(1f, 0.3f);
			}
		}
		Transform transform2 = base.transform.Find("View");
		if ((bool)transform2)
		{
			CanvasGroup component2 = transform2.GetComponent<CanvasGroup>();
			if ((bool)component2)
			{
				component2.DOFade(1f, 0.3f);
			}
		}
		EventManager.DispatchEvent(MessageType.UiMessage, UiMessageType.Open);
		Pause();
		VideoInfoInfoBg.SetActive(active: false);
	}

	public void Init(string n, List<FolderData> folderDatas)
	{
		List<FolderData> list = folderDatas.Where(Tool.IsLockFolder).ToList();
		for (int i = 0; i < list.Count; i++)
		{
			FolderDataItem folderDataItem = SingletonAsMono<FolderDataMrg>.Instance.Dequeue();
			folderDataItem.transform.SetParent(RectTransformContent);
			folderDataItem.transform.localScale = Vector3.one;
			folderDataItem.Init(ScrollRectScrollView, RectTransformContent, null, list[i], null);
		}
		TextMeshProUGUITitle.text = n;
	}

	public override void OnClose()
	{
		base.OnClose();
		for (int num = RectTransformContent.childCount - 1; num >= 0; num--)
		{
			SingletonAsMono<FolderDataMrg>.Instance.Enqueue(RectTransformContent.GetChild(num).GetComponent<FolderDataItem>());
		}
	}

	public void UpdateContent()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransformContent);
	}

	public void InitInfo(FileData fileData)
	{
		VideoInfoInfoBg.SetActive(active: true);
		VideoInfoInfoBg.Init(fileData);
	}

	public override void CloseUi()
	{
		Transform transform = base.transform.Find("Bg");
		if ((bool)transform)
		{
			CanvasGroup component = transform.GetComponent<CanvasGroup>();
			if ((bool)component)
			{
				component.DOFade(0f, 0.3f);
			}
		}
		Transform transform2 = base.transform.Find("View");
		if ((bool)transform2)
		{
			DoScale(transform2.GetComponent<RectTransform>(), Vector3.one, Vector3.one, delegate
			{
				UiManager.RemoveUi(GetIndex());
			});
			CanvasGroup component2 = transform2.GetComponent<CanvasGroup>();
			if ((bool)component2)
			{
				component2.DOFade(0f, 0.3f);
			}
		}
		else
		{
			UiManager.RemoveUi(GetIndex());
		}
	}
}
