using Script.Mrg;
using Script.Tool;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "RecordWindows")]
public class RecordWindows : UiActor
{
	private FolderDataGroup _folderDataGroup;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformZhuXian;

	public CanvasRenderer CanvasRendererZhuXian;

	public Image ImageZhuXian;

	public BT BTZhuXian;

	public AddScripts AddScriptsZhuXian;

	public RectTransform RectTransformXinDonGuShiBt;

	public CanvasRenderer CanvasRendererXinDonGuShiBt;

	public Image ImageXinDonGuShiBt;

	public BT BTXinDonGuShiBt;

	public AddScripts AddScriptsXinDonGuShiBt;

	public RectTransform RectTransformQiTaGuShiBt;

	public CanvasRenderer CanvasRendererQiTaGuShiBt;

	public Image ImageQiTaGuShiBt;

	public BT BTQiTaGuShiBt;

	public AddScripts AddScriptsQiTaGuShiBt;

	public override void Awake()
	{
		base.Awake();
		RectTransformClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<AddScripts>();
		RectTransformZhuXian = GetGameObject().transform.Find("View/Group/ZhuXian/ZhuXian/").GetComponent<RectTransform>();
		CanvasRendererZhuXian = GetGameObject().transform.Find("View/Group/ZhuXian/ZhuXian/").GetComponent<CanvasRenderer>();
		ImageZhuXian = GetGameObject().transform.Find("View/Group/ZhuXian/ZhuXian/").GetComponent<Image>();
		BTZhuXian = GetGameObject().transform.Find("View/Group/ZhuXian/ZhuXian/").GetComponent<BT>();
		AddScriptsZhuXian = GetGameObject().transform.Find("View/Group/ZhuXian/ZhuXian/").GetComponent<AddScripts>();
		RectTransformXinDonGuShiBt = GetGameObject().transform.Find("View/Group/XinDonGuShi/XinDonGuShiBt/").GetComponent<RectTransform>();
		CanvasRendererXinDonGuShiBt = GetGameObject().transform.Find("View/Group/XinDonGuShi/XinDonGuShiBt/").GetComponent<CanvasRenderer>();
		ImageXinDonGuShiBt = GetGameObject().transform.Find("View/Group/XinDonGuShi/XinDonGuShiBt/").GetComponent<Image>();
		BTXinDonGuShiBt = GetGameObject().transform.Find("View/Group/XinDonGuShi/XinDonGuShiBt/").GetComponent<BT>();
		AddScriptsXinDonGuShiBt = GetGameObject().transform.Find("View/Group/XinDonGuShi/XinDonGuShiBt/").GetComponent<AddScripts>();
		RectTransformQiTaGuShiBt = GetGameObject().transform.Find("View/Group/QiTaGuShi/QiTaGuShiBt/").GetComponent<RectTransform>();
		CanvasRendererQiTaGuShiBt = GetGameObject().transform.Find("View/Group/QiTaGuShi/QiTaGuShiBt/").GetComponent<CanvasRenderer>();
		ImageQiTaGuShiBt = GetGameObject().transform.Find("View/Group/QiTaGuShi/QiTaGuShiBt/").GetComponent<Image>();
		BTQiTaGuShiBt = GetGameObject().transform.Find("View/Group/QiTaGuShi/QiTaGuShiBt/").GetComponent<BT>();
		AddScriptsQiTaGuShiBt = GetGameObject().transform.Find("View/Group/QiTaGuShi/QiTaGuShiBt/").GetComponent<AddScripts>();
	}

	public RecordWindows(Transform trans)
		: base(trans)
	{
	}

	public RecordWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		_folderDataGroup = ABMrg.Load<FolderDataGroup>("VideoData");
		BTClose.onClick.AddListener(CloseUi);
		BTZhuXian.onClick.AddListener(ZhuXian);
		BTXinDonGuShiBt.onClick.AddListener(XinDon);
		BTQiTaGuShiBt.onClick.AddListener(QiTa);
	}

	private void ZhuXian()
	{
		UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.Main.FolderName), _folderDataGroup.Main.FolderList);
	}

	private void XinDon()
	{
		UiManager.OpenUi<HeartbeatWindows>();
	}

	private void QiTa()
	{
		UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.Other.FolderName), _folderDataGroup.Other.FolderList);
	}
}
