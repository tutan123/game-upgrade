using System;
using Script.Mrg;
using Script.Tool;
using Script.UiTool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "ItemInfoWindows")]
public class ItemInfoWindows : UiActor
{
	private Xlsx_Item _xlsxItem;

	private ZbType _zbType;

	private float _height;

	public RectTransform RectTransformBg;

	public CanvasRenderer CanvasRendererBg;

	public Image ImageBg;

	public BT BTBg;

	public AddScripts AddScriptsBg;

	public RectTransform RectTransformView;

	public CanvasGroup CanvasGroupView;

	public AddScripts AddScriptsView;

	public RectTransform RectTransformIcon;

	public CanvasRenderer CanvasRendererIcon;

	public Image ImageIcon;

	public AddScripts AddScriptsIcon;

	public RectTransform RectTransformItemName;

	public CanvasRenderer CanvasRendererItemName;

	public TextMeshProUGUI TextMeshProUGUIItemName;

	public AddScripts AddScriptsItemName;

	public RectTransform RectTransformZbBtn;

	public CanvasRenderer CanvasRendererZbBtn;

	public Image ImageZbBtn;

	public BT BTZbBtn;

	public AddScripts AddScriptsZbBtn;

	public RectTransform RectTransformXieXiaBtn;

	public CanvasRenderer CanvasRendererXieXiaBtn;

	public Image ImageXieXiaBtn;

	public BT BTXieXiaBtn;

	public AddScripts AddScriptsXieXiaBtn;

	public RectTransform RectTransformInfo;

	public CanvasRenderer CanvasRendererInfo;

	public TextMeshProUGUI TextMeshProUGUIInfo;

	public AddScripts AddScriptsInfo;

	public CheckTextWidth CheckTextWidthInfo;

	public RectTransform RectTransformAddInfo;

	public CanvasRenderer CanvasRendererAddInfo;

	public TextMeshProUGUI TextMeshProUGUIAddInfo;

	public AddScripts AddScriptsAddInfo;

	public CheckTextWidth CheckTextWidthAddInfo;

	public override void Awake()
	{
		base.Awake();
		RectTransformBg = GetGameObject().transform.Find("Bg/").GetComponent<RectTransform>();
		CanvasRendererBg = GetGameObject().transform.Find("Bg/").GetComponent<CanvasRenderer>();
		ImageBg = GetGameObject().transform.Find("Bg/").GetComponent<Image>();
		BTBg = GetGameObject().transform.Find("Bg/").GetComponent<BT>();
		AddScriptsBg = GetGameObject().transform.Find("Bg/").GetComponent<AddScripts>();
		RectTransformView = GetGameObject().transform.Find("View/").GetComponent<RectTransform>();
		CanvasGroupView = GetGameObject().transform.Find("View/").GetComponent<CanvasGroup>();
		AddScriptsView = GetGameObject().transform.Find("View/").GetComponent<AddScripts>();
		RectTransformIcon = GetGameObject().transform.Find("View/Bg/Icon/").GetComponent<RectTransform>();
		CanvasRendererIcon = GetGameObject().transform.Find("View/Bg/Icon/").GetComponent<CanvasRenderer>();
		ImageIcon = GetGameObject().transform.Find("View/Bg/Icon/").GetComponent<Image>();
		AddScriptsIcon = GetGameObject().transform.Find("View/Bg/Icon/").GetComponent<AddScripts>();
		RectTransformItemName = GetGameObject().transform.Find("View/Bg/ItemName/").GetComponent<RectTransform>();
		CanvasRendererItemName = GetGameObject().transform.Find("View/Bg/ItemName/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIItemName = GetGameObject().transform.Find("View/Bg/ItemName/").GetComponent<TextMeshProUGUI>();
		AddScriptsItemName = GetGameObject().transform.Find("View/Bg/ItemName/").GetComponent<AddScripts>();
		RectTransformZbBtn = GetGameObject().transform.Find("View/Bg/ZbBtn/").GetComponent<RectTransform>();
		CanvasRendererZbBtn = GetGameObject().transform.Find("View/Bg/ZbBtn/").GetComponent<CanvasRenderer>();
		ImageZbBtn = GetGameObject().transform.Find("View/Bg/ZbBtn/").GetComponent<Image>();
		BTZbBtn = GetGameObject().transform.Find("View/Bg/ZbBtn/").GetComponent<BT>();
		AddScriptsZbBtn = GetGameObject().transform.Find("View/Bg/ZbBtn/").GetComponent<AddScripts>();
		RectTransformXieXiaBtn = GetGameObject().transform.Find("View/Bg/XieXiaBtn/").GetComponent<RectTransform>();
		CanvasRendererXieXiaBtn = GetGameObject().transform.Find("View/Bg/XieXiaBtn/").GetComponent<CanvasRenderer>();
		ImageXieXiaBtn = GetGameObject().transform.Find("View/Bg/XieXiaBtn/").GetComponent<Image>();
		BTXieXiaBtn = GetGameObject().transform.Find("View/Bg/XieXiaBtn/").GetComponent<BT>();
		AddScriptsXieXiaBtn = GetGameObject().transform.Find("View/Bg/XieXiaBtn/").GetComponent<AddScripts>();
		RectTransformInfo = GetGameObject().transform.Find("View/Bg/Text/Info/").GetComponent<RectTransform>();
		CanvasRendererInfo = GetGameObject().transform.Find("View/Bg/Text/Info/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIInfo = GetGameObject().transform.Find("View/Bg/Text/Info/").GetComponent<TextMeshProUGUI>();
		AddScriptsInfo = GetGameObject().transform.Find("View/Bg/Text/Info/").GetComponent<AddScripts>();
		CheckTextWidthInfo = GetGameObject().transform.Find("View/Bg/Text/Info/").GetComponent<CheckTextWidth>();
		RectTransformAddInfo = GetGameObject().transform.Find("View/Bg/Text/AddInfo/").GetComponent<RectTransform>();
		CanvasRendererAddInfo = GetGameObject().transform.Find("View/Bg/Text/AddInfo/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIAddInfo = GetGameObject().transform.Find("View/Bg/Text/AddInfo/").GetComponent<TextMeshProUGUI>();
		AddScriptsAddInfo = GetGameObject().transform.Find("View/Bg/Text/AddInfo/").GetComponent<AddScripts>();
		CheckTextWidthAddInfo = GetGameObject().transform.Find("View/Bg/Text/AddInfo/").GetComponent<CheckTextWidth>();
	}

	public ItemInfoWindows(Transform trans)
		: base(trans)
	{
	}

	public ItemInfoWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTBg.onClick.AddListener(CloseUi);
		BTZbBtn.onClick.AddListener(ClickZb);
		BTXieXiaBtn.onClick.AddListener(ClickXieXia);
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		_height = (float)Screen.height / UiManager.ScaleFactor;
	}

	public void Init(Xlsx_Item xlsxItem, Vector2 pos, bool isCd = true, ZbType zbType = ZbType.QiTa)
	{
		_xlsxItem = xlsxItem;
		TextMeshProUGUIItemName.text = LanguageMrg.GetText(_xlsxItem.Name);
		TextMeshProUGUIInfo.text = LanguageMrg.GetText(_xlsxItem.Info);
		TextMeshProUGUIAddInfo.text = LanguageMrg.GetText(_xlsxItem.AddInfo);
		BTXieXiaBtn.SetActive(!isCd);
		_zbType = zbType;
		Vector2 targetLocalLoc = Tool.GetTargetLocalLoc(base.transform.GetComponent<RectTransform>(), pos);
		if (_zbType == ZbType.QiTa && !string.IsNullOrEmpty(_xlsxItem.Img))
		{
			try
			{
				Sprite sprite = ABMrg.Load<Sprite>(_xlsxItem.Img);
				UiManager.OpenUi<ImgShowWindows>().Init(sprite);
			}
			catch (Exception ex)
			{
				MyLog.LogWarning(ex.Message);
			}
		}
		if (0f > targetLocalLoc.y && Mathf.Abs(targetLocalLoc.y) + RectTransformView.sizeDelta.y > _height / 2f)
		{
			RectTransformView.pivot = new Vector2(0.5f, 0f);
		}
		else
		{
			RectTransformView.pivot = new Vector2(0.5f, 1f);
		}
		RectTransformView.localPosition = targetLocalLoc;
		ZbType itemType = (ZbType)_xlsxItem.ItemType;
		BTZbBtn.SetActive(itemType != ZbType.QiTa && isCd);
		if (!string.IsNullOrEmpty(_xlsxItem.Icon))
		{
			ImageIcon.sprite = ABMrg.Load<Sprite>(_xlsxItem.Icon);
		}
	}

	private void ClickXieXia()
	{
		SingletonAsMono<GameDataMrg>.Instance.SetCurZbKey(_zbType, "");
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.ZbUpdate);
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.UpdateProperty);
		CloseUi();
	}

	private void ClickZb()
	{
		Tool.ClickZb(_xlsxItem);
		CloseUi();
	}
}
