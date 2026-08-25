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
[ActorInfo("", "ShopGetWindows")]
public class ShopGetWindows : UiActor
{
	private float _scale;

	private float _height;

	private Xlsx_Item _xlsxItem;

	private int _count;

	public RectTransform RectTransformBg;

	public CanvasRenderer CanvasRendererBg;

	public Image ImageBg;

	public BT BTBg;

	public AddScripts AddScriptsBg;

	public RectTransform RectTransformView;

	public AddScripts AddScriptsView;

	public RectTransform RectTransformBgView;

	public CanvasRenderer CanvasRendererBgView;

	public Image ImageBgView;

	public AddScripts AddScriptsBgView;

	public RectTransform RectTransformIcon;

	public CanvasRenderer CanvasRendererIcon;

	public Image ImageIcon;

	public AddScripts AddScriptsIcon;

	public RectTransform RectTransformIconName;

	public CanvasRenderer CanvasRendererIconName;

	public TextMeshProUGUI TextMeshProUGUIIconName;

	public AddScripts AddScriptsIconName;

	public RectTransform RectTransformGetCount;

	public CanvasRenderer CanvasRendererGetCount;

	public TextMeshProUGUI TextMeshProUGUIGetCount;

	public AddScripts AddScriptsGetCount;

	public RectTransform RectTransformLoseBtn;

	public CanvasRenderer CanvasRendererLoseBtn;

	public Image ImageLoseBtn;

	public BT BTLoseBtn;

	public AddScripts AddScriptsLoseBtn;

	public RectTransform RectTransformCount;

	public CanvasRenderer CanvasRendererCount;

	public TextMeshProUGUI TextMeshProUGUICount;

	public AddScripts AddScriptsCount;

	public RectTransform RectTransformCoin;

	public CanvasRenderer CanvasRendererCoin;

	public TextMeshProUGUI TextMeshProUGUICoin;

	public AddScripts AddScriptsCoin;

	public RectTransform RectTransformAddBtn;

	public CanvasRenderer CanvasRendererAddBtn;

	public Image ImageAddBtn;

	public AddScripts AddScriptsAddBtn;

	public BT BTAddBtn;

	public RectTransform RectTransformGet;

	public CanvasRenderer CanvasRendererGet;

	public Image ImageGet;

	public BT BTGet;

	public AddScripts AddScriptsGet;

	public RectTransform RectTransformCancle;

	public CanvasRenderer CanvasRendererCancle;

	public Image ImageCancle;

	public BT BTCancle;

	public AddScripts AddScriptsCancle;

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
		AddScriptsView = GetGameObject().transform.Find("View/").GetComponent<AddScripts>();
		RectTransformBgView = GetGameObject().transform.Find("View/BgView/").GetComponent<RectTransform>();
		CanvasRendererBgView = GetGameObject().transform.Find("View/BgView/").GetComponent<CanvasRenderer>();
		ImageBgView = GetGameObject().transform.Find("View/BgView/").GetComponent<Image>();
		AddScriptsBgView = GetGameObject().transform.Find("View/BgView/").GetComponent<AddScripts>();
		RectTransformIcon = GetGameObject().transform.Find("View/BgView/IconBg/Icon/").GetComponent<RectTransform>();
		CanvasRendererIcon = GetGameObject().transform.Find("View/BgView/IconBg/Icon/").GetComponent<CanvasRenderer>();
		ImageIcon = GetGameObject().transform.Find("View/BgView/IconBg/Icon/").GetComponent<Image>();
		AddScriptsIcon = GetGameObject().transform.Find("View/BgView/IconBg/Icon/").GetComponent<AddScripts>();
		RectTransformIconName = GetGameObject().transform.Find("View/BgView/IconName/").GetComponent<RectTransform>();
		CanvasRendererIconName = GetGameObject().transform.Find("View/BgView/IconName/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIIconName = GetGameObject().transform.Find("View/BgView/IconName/").GetComponent<TextMeshProUGUI>();
		AddScriptsIconName = GetGameObject().transform.Find("View/BgView/IconName/").GetComponent<AddScripts>();
		RectTransformGetCount = GetGameObject().transform.Find("View/BgView/GetCount/").GetComponent<RectTransform>();
		CanvasRendererGetCount = GetGameObject().transform.Find("View/BgView/GetCount/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIGetCount = GetGameObject().transform.Find("View/BgView/GetCount/").GetComponent<TextMeshProUGUI>();
		AddScriptsGetCount = GetGameObject().transform.Find("View/BgView/GetCount/").GetComponent<AddScripts>();
		RectTransformLoseBtn = GetGameObject().transform.Find("View/BgView/BtGroup/Add/LoseAn/LoseBtn/").GetComponent<RectTransform>();
		CanvasRendererLoseBtn = GetGameObject().transform.Find("View/BgView/BtGroup/Add/LoseAn/LoseBtn/").GetComponent<CanvasRenderer>();
		ImageLoseBtn = GetGameObject().transform.Find("View/BgView/BtGroup/Add/LoseAn/LoseBtn/").GetComponent<Image>();
		BTLoseBtn = GetGameObject().transform.Find("View/BgView/BtGroup/Add/LoseAn/LoseBtn/").GetComponent<BT>();
		AddScriptsLoseBtn = GetGameObject().transform.Find("View/BgView/BtGroup/Add/LoseAn/LoseBtn/").GetComponent<AddScripts>();
		RectTransformCount = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddBg/Count/").GetComponent<RectTransform>();
		CanvasRendererCount = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddBg/Count/").GetComponent<CanvasRenderer>();
		TextMeshProUGUICount = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddBg/Count/").GetComponent<TextMeshProUGUI>();
		AddScriptsCount = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddBg/Count/").GetComponent<AddScripts>();
		RectTransformCoin = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddBg/Coin/").GetComponent<RectTransform>();
		CanvasRendererCoin = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddBg/Coin/").GetComponent<CanvasRenderer>();
		TextMeshProUGUICoin = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddBg/Coin/").GetComponent<TextMeshProUGUI>();
		AddScriptsCoin = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddBg/Coin/").GetComponent<AddScripts>();
		RectTransformAddBtn = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddAn/AddBtn/").GetComponent<RectTransform>();
		CanvasRendererAddBtn = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddAn/AddBtn/").GetComponent<CanvasRenderer>();
		ImageAddBtn = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddAn/AddBtn/").GetComponent<Image>();
		AddScriptsAddBtn = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddAn/AddBtn/").GetComponent<AddScripts>();
		BTAddBtn = GetGameObject().transform.Find("View/BgView/BtGroup/Add/AddAn/AddBtn/").GetComponent<BT>();
		RectTransformGet = GetGameObject().transform.Find("View/BgView/BtGroup/Btn/Get/").GetComponent<RectTransform>();
		CanvasRendererGet = GetGameObject().transform.Find("View/BgView/BtGroup/Btn/Get/").GetComponent<CanvasRenderer>();
		ImageGet = GetGameObject().transform.Find("View/BgView/BtGroup/Btn/Get/").GetComponent<Image>();
		BTGet = GetGameObject().transform.Find("View/BgView/BtGroup/Btn/Get/").GetComponent<BT>();
		AddScriptsGet = GetGameObject().transform.Find("View/BgView/BtGroup/Btn/Get/").GetComponent<AddScripts>();
		RectTransformCancle = GetGameObject().transform.Find("View/BgView/BtGroup/Btn/Cancle/").GetComponent<RectTransform>();
		CanvasRendererCancle = GetGameObject().transform.Find("View/BgView/BtGroup/Btn/Cancle/").GetComponent<CanvasRenderer>();
		ImageCancle = GetGameObject().transform.Find("View/BgView/BtGroup/Btn/Cancle/").GetComponent<Image>();
		BTCancle = GetGameObject().transform.Find("View/BgView/BtGroup/Btn/Cancle/").GetComponent<BT>();
		AddScriptsCancle = GetGameObject().transform.Find("View/BgView/BtGroup/Btn/Cancle/").GetComponent<AddScripts>();
		RectTransformInfo = GetGameObject().transform.Find("View/BgView/Text/Info/").GetComponent<RectTransform>();
		CanvasRendererInfo = GetGameObject().transform.Find("View/BgView/Text/Info/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIInfo = GetGameObject().transform.Find("View/BgView/Text/Info/").GetComponent<TextMeshProUGUI>();
		AddScriptsInfo = GetGameObject().transform.Find("View/BgView/Text/Info/").GetComponent<AddScripts>();
		CheckTextWidthInfo = GetGameObject().transform.Find("View/BgView/Text/Info/").GetComponent<CheckTextWidth>();
		RectTransformAddInfo = GetGameObject().transform.Find("View/BgView/Text/AddInfo/").GetComponent<RectTransform>();
		CanvasRendererAddInfo = GetGameObject().transform.Find("View/BgView/Text/AddInfo/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIAddInfo = GetGameObject().transform.Find("View/BgView/Text/AddInfo/").GetComponent<TextMeshProUGUI>();
		AddScriptsAddInfo = GetGameObject().transform.Find("View/BgView/Text/AddInfo/").GetComponent<AddScripts>();
		CheckTextWidthAddInfo = GetGameObject().transform.Find("View/BgView/Text/AddInfo/").GetComponent<CheckTextWidth>();
	}

	public ShopGetWindows(Transform trans)
		: base(trans)
	{
	}

	public ShopGetWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTCancle.onClick.AddListener(CloseUi);
		BTLoseBtn.onClick.AddListener(OnClickLose);
		BTAddBtn.onClick.AddListener(OnClickAdd);
		BTGet.onClick.AddListener(OnClickGet);
		BTBg.onClick.AddListener(CloseUi);
	}

	public override void Open(object[] objects)
	{
	}

	public override void OnEnable()
	{
		base.OnEnable();
		_scale = UiManager.ScaleFactor;
		_height = RectTransformBgView.rect.height * UiManager.ScaleFactor;
	}

	public void Init(Xlsx_Item xlsxItem)
	{
		_count = 1;
		_xlsxItem = xlsxItem;
		ImageIcon.SetActive(active: true);
		if (!string.IsNullOrEmpty(xlsxItem.Icon))
		{
			ImageIcon.sprite = ABMrg.Load<Sprite>(xlsxItem.Icon);
		}
		else
		{
			ImageIcon.SetActive(active: false);
		}
		TextMeshProUGUIIconName.text = LanguageMrg.GetText(xlsxItem.Name);
		TextMeshProUGUIInfo.text = LanguageMrg.GetText(xlsxItem.Info);
		TextMeshProUGUIAddInfo.text = LanguageMrg.GetText(xlsxItem.AddInfo);
		InitCoin();
	}

	private void InitCoin()
	{
		int getMaxCount = _xlsxItem.GetMaxCount;
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty(_xlsxItem.Key, "GetCount", 0L);
		TextMeshProUGUIGetCount.SetActive(getMaxCount > 0);
		TextMeshProUGUIGetCount.text = getMaxCount - property + "/" + getMaxCount;
		int property2 = SingletonAsMono<GameDataMrg>.Instance.GetProperty("Money", "Property", 0L);
		TextMeshProUGUICoin.text = "<sprite name=Qian>" + _count * _xlsxItem.Price;
		TextMeshProUGUICount.text = _count.ToString() ?? "";
		if (_count <= 1)
		{
			RectTransformLoseBtn.SetActive(active: false);
		}
		else
		{
			RectTransformLoseBtn.SetActive(active: true);
		}
		if (property2 >= _count * _xlsxItem.Price)
		{
			RectTransformAddBtn.SetActive(active: true);
		}
		else
		{
			RectTransformAddBtn.SetActive(active: false);
		}
	}

	private void OnClickAdd()
	{
		int getMaxCount = _xlsxItem.GetMaxCount;
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty(_xlsxItem.Key, "GetCount", 0L);
		if (_count + property >= getMaxCount)
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1328"));
			return;
		}
		_count++;
		InitCoin();
	}

	private void OnClickLose()
	{
		_count--;
		InitCoin();
	}

	private void OnClickGet()
	{
		int getMaxCount = _xlsxItem.GetMaxCount;
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty(_xlsxItem.Key, "GetCount", 0L);
		if (_count + property > getMaxCount)
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1328"));
		}
		else if (SingletonAsMono<GameDataMrg>.Instance.GetProperty("Money", "Property", 0L) >= _count * _xlsxItem.Price)
		{
			SingletonAsMono<GameDataMrg>.Instance.AddProperty("Money", -_count * _xlsxItem.Price, "Property");
			SingletonAsMono<GameDataMrg>.Instance.AddProperty(_xlsxItem.Key, _count, "GetCount");
			if (Enum.TryParse<ItemType>(_xlsxItem.Key, out var result))
			{
				new PropertyData
				{
					PropertyType = PropertyType.Item,
					itemType = result,
					PropertyValue = _count
				}.AddTypeValueAsShowTips(isCheck: false);
			}
			CloseUi();
			UiManager.GetUi<ShopWindows>().Init();
		}
		else
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1230"));
		}
	}

	public override void CloseUi()
	{
		UiManager.RemoveUi(GetIndex());
	}

	public override bool IsCanJoinPause()
	{
		return false;
	}
}
