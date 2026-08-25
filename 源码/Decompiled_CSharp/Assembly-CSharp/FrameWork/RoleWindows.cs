using System.Collections.Generic;
using System.Linq;
using Script.Mrg;
using Script.Tool;
using Script.UiTool;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace FrameWork;

[ActorInfo("", "RoleWindows")]
[UiMode(Mode.Normal, true)]
public class RoleWindows : UiActor
{
	public RectTransform RectTransformBlack;

	public CanvasRenderer CanvasRendererBlack;

	public Image ImageBlack;

	public AddScripts AddScriptsBlack;

	public BT BTBlack;

	public RectTransform RectTransformTou;

	public CanvasRenderer CanvasRendererTou;

	public Image ImageTou;

	public AddScripts AddScriptsTou;

	public BT BTTou;

	public ZbItem ZbItemTou;

	public RectTransform RectTransformXion;

	public CanvasRenderer CanvasRendererXion;

	public Image ImageXion;

	public AddScripts AddScriptsXion;

	public BT BTXion;

	public ZbItem ZbItemXion;

	public RectTransform RectTransformFuBu;

	public CanvasRenderer CanvasRendererFuBu;

	public Image ImageFuBu;

	public AddScripts AddScriptsFuBu;

	public BT BTFuBu;

	public ZbItem ZbItemFuBu;

	public RectTransform RectTransformZuoShou;

	public CanvasRenderer CanvasRendererZuoShou;

	public Image ImageZuoShou;

	public AddScripts AddScriptsZuoShou;

	public BT BTZuoShou;

	public ZbItem ZbItemZuoShou;

	public RectTransform RectTransformYouShou;

	public CanvasRenderer CanvasRendererYouShou;

	public Image ImageYouShou;

	public AddScripts AddScriptsYouShou;

	public BT BTYouShou;

	public RectTransform RectTransformJiaoBu;

	public CanvasRenderer CanvasRendererJiaoBu;

	public Image ImageJiaoBu;

	public AddScripts AddScriptsJiaoBu;

	public BT BTJiaoBu;

	public ZbItem ZbItemJiaoBu;

	public RectTransform RectTransformZaiJuZb;

	public CanvasRenderer CanvasRendererZaiJuZb;

	public Image ImageZaiJuZb;

	public AddScripts AddScriptsZaiJuZb;

	public BT BTZaiJuZb;

	public ZbItem ZbItemZaiJuZb;

	public RectTransform RectTransformContent;

	public ContentSizeFitter ContentSizeFitterContent;

	public GridLayoutGroup GridLayoutGroupContent;

	public AddScripts AddScriptsContent;

	public RectTransform RectTransformQuanBu;

	public CanvasRenderer CanvasRendererQuanBu;

	public Image ImageQuanBu;

	public Toggle ToggleQuanBu;

	public AddScripts AddScriptsQuanBu;

	public RectTransform RectTransformTouBu;

	public CanvasRenderer CanvasRendererTouBu;

	public Image ImageTouBu;

	public Toggle ToggleTouBu;

	public AddScripts AddScriptsTouBu;

	public RectTransform RectTransformYiFu;

	public CanvasRenderer CanvasRendererYiFu;

	public Image ImageYiFu;

	public Toggle ToggleYiFu;

	public AddScripts AddScriptsYiFu;

	public RectTransform RectTransformPeiShi;

	public CanvasRenderer CanvasRendererPeiShi;

	public Image ImagePeiShi;

	public Toggle TogglePeiShi;

	public AddScripts AddScriptsPeiShi;

	public RectTransform RectTransformXieZi;

	public CanvasRenderer CanvasRendererXieZi;

	public Image ImageXieZi;

	public Toggle ToggleXieZi;

	public AddScripts AddScriptsXieZi;

	public RectTransform RectTransformZaiJu;

	public CanvasRenderer CanvasRendererZaiJu;

	public Image ImageZaiJu;

	public Toggle ToggleZaiJu;

	public AddScripts AddScriptsZaiJu;

	public RectTransform RectTransformXiaoHaoPin;

	public CanvasRenderer CanvasRendererXiaoHaoPin;

	public Image ImageXiaoHaoPin;

	public Toggle ToggleXiaoHaoPin;

	public AddScripts AddScriptsXiaoHaoPin;

	public RectTransform RectTransformQiTa;

	public CanvasRenderer CanvasRendererQiTa;

	public Image ImageQiTa;

	public Toggle ToggleQiTa;

	public AddScripts AddScriptsQiTa;

	public override void Awake()
	{
		base.Awake();
		RectTransformBlack = GetGameObject().transform.Find("Bg/Black/").GetComponent<RectTransform>();
		CanvasRendererBlack = GetGameObject().transform.Find("Bg/Black/").GetComponent<CanvasRenderer>();
		ImageBlack = GetGameObject().transform.Find("Bg/Black/").GetComponent<Image>();
		AddScriptsBlack = GetGameObject().transform.Find("Bg/Black/").GetComponent<AddScripts>();
		BTBlack = GetGameObject().transform.Find("Bg/Black/").GetComponent<BT>();
		RectTransformTou = GetGameObject().transform.Find("View/Role/Tou/").GetComponent<RectTransform>();
		CanvasRendererTou = GetGameObject().transform.Find("View/Role/Tou/").GetComponent<CanvasRenderer>();
		ImageTou = GetGameObject().transform.Find("View/Role/Tou/").GetComponent<Image>();
		AddScriptsTou = GetGameObject().transform.Find("View/Role/Tou/").GetComponent<AddScripts>();
		BTTou = GetGameObject().transform.Find("View/Role/Tou/").GetComponent<BT>();
		ZbItemTou = GetGameObject().transform.Find("View/Role/Tou/").GetComponent<ZbItem>();
		RectTransformXion = GetGameObject().transform.Find("View/Role/Xion/").GetComponent<RectTransform>();
		CanvasRendererXion = GetGameObject().transform.Find("View/Role/Xion/").GetComponent<CanvasRenderer>();
		ImageXion = GetGameObject().transform.Find("View/Role/Xion/").GetComponent<Image>();
		AddScriptsXion = GetGameObject().transform.Find("View/Role/Xion/").GetComponent<AddScripts>();
		BTXion = GetGameObject().transform.Find("View/Role/Xion/").GetComponent<BT>();
		ZbItemXion = GetGameObject().transform.Find("View/Role/Xion/").GetComponent<ZbItem>();
		RectTransformFuBu = GetGameObject().transform.Find("View/Role/FuBu/").GetComponent<RectTransform>();
		CanvasRendererFuBu = GetGameObject().transform.Find("View/Role/FuBu/").GetComponent<CanvasRenderer>();
		ImageFuBu = GetGameObject().transform.Find("View/Role/FuBu/").GetComponent<Image>();
		AddScriptsFuBu = GetGameObject().transform.Find("View/Role/FuBu/").GetComponent<AddScripts>();
		BTFuBu = GetGameObject().transform.Find("View/Role/FuBu/").GetComponent<BT>();
		ZbItemFuBu = GetGameObject().transform.Find("View/Role/FuBu/").GetComponent<ZbItem>();
		RectTransformZuoShou = GetGameObject().transform.Find("View/Role/ZuoShou/").GetComponent<RectTransform>();
		CanvasRendererZuoShou = GetGameObject().transform.Find("View/Role/ZuoShou/").GetComponent<CanvasRenderer>();
		ImageZuoShou = GetGameObject().transform.Find("View/Role/ZuoShou/").GetComponent<Image>();
		AddScriptsZuoShou = GetGameObject().transform.Find("View/Role/ZuoShou/").GetComponent<AddScripts>();
		BTZuoShou = GetGameObject().transform.Find("View/Role/ZuoShou/").GetComponent<BT>();
		ZbItemZuoShou = GetGameObject().transform.Find("View/Role/ZuoShou/").GetComponent<ZbItem>();
		RectTransformYouShou = GetGameObject().transform.Find("View/Role/YouShou/").GetComponent<RectTransform>();
		CanvasRendererYouShou = GetGameObject().transform.Find("View/Role/YouShou/").GetComponent<CanvasRenderer>();
		ImageYouShou = GetGameObject().transform.Find("View/Role/YouShou/").GetComponent<Image>();
		AddScriptsYouShou = GetGameObject().transform.Find("View/Role/YouShou/").GetComponent<AddScripts>();
		BTYouShou = GetGameObject().transform.Find("View/Role/YouShou/").GetComponent<BT>();
		RectTransformJiaoBu = GetGameObject().transform.Find("View/Role/JiaoBu/").GetComponent<RectTransform>();
		CanvasRendererJiaoBu = GetGameObject().transform.Find("View/Role/JiaoBu/").GetComponent<CanvasRenderer>();
		ImageJiaoBu = GetGameObject().transform.Find("View/Role/JiaoBu/").GetComponent<Image>();
		AddScriptsJiaoBu = GetGameObject().transform.Find("View/Role/JiaoBu/").GetComponent<AddScripts>();
		BTJiaoBu = GetGameObject().transform.Find("View/Role/JiaoBu/").GetComponent<BT>();
		ZbItemJiaoBu = GetGameObject().transform.Find("View/Role/JiaoBu/").GetComponent<ZbItem>();
		RectTransformZaiJuZb = GetGameObject().transform.Find("View/Role/ZaiJuZb/").GetComponent<RectTransform>();
		CanvasRendererZaiJuZb = GetGameObject().transform.Find("View/Role/ZaiJuZb/").GetComponent<CanvasRenderer>();
		ImageZaiJuZb = GetGameObject().transform.Find("View/Role/ZaiJuZb/").GetComponent<Image>();
		AddScriptsZaiJuZb = GetGameObject().transform.Find("View/Role/ZaiJuZb/").GetComponent<AddScripts>();
		BTZaiJuZb = GetGameObject().transform.Find("View/Role/ZaiJuZb/").GetComponent<BT>();
		ZbItemZaiJuZb = GetGameObject().transform.Find("View/Role/ZaiJuZb/").GetComponent<ZbItem>();
		RectTransformContent = GetGameObject().transform.Find("View/ZhuanBei/Scroll View/Viewport/Content/").GetComponent<RectTransform>();
		ContentSizeFitterContent = GetGameObject().transform.Find("View/ZhuanBei/Scroll View/Viewport/Content/").GetComponent<ContentSizeFitter>();
		GridLayoutGroupContent = GetGameObject().transform.Find("View/ZhuanBei/Scroll View/Viewport/Content/").GetComponent<GridLayoutGroup>();
		AddScriptsContent = GetGameObject().transform.Find("View/ZhuanBei/Scroll View/Viewport/Content/").GetComponent<AddScripts>();
		RectTransformQuanBu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/QuanBu/").GetComponent<RectTransform>();
		CanvasRendererQuanBu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/QuanBu/").GetComponent<CanvasRenderer>();
		ImageQuanBu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/QuanBu/").GetComponent<Image>();
		ToggleQuanBu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/QuanBu/").GetComponent<Toggle>();
		AddScriptsQuanBu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/QuanBu/").GetComponent<AddScripts>();
		RectTransformTouBu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/TouBu/").GetComponent<RectTransform>();
		CanvasRendererTouBu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/TouBu/").GetComponent<CanvasRenderer>();
		ImageTouBu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/TouBu/").GetComponent<Image>();
		ToggleTouBu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/TouBu/").GetComponent<Toggle>();
		AddScriptsTouBu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/TouBu/").GetComponent<AddScripts>();
		RectTransformYiFu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/YiFu/").GetComponent<RectTransform>();
		CanvasRendererYiFu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/YiFu/").GetComponent<CanvasRenderer>();
		ImageYiFu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/YiFu/").GetComponent<Image>();
		ToggleYiFu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/YiFu/").GetComponent<Toggle>();
		AddScriptsYiFu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/YiFu/").GetComponent<AddScripts>();
		RectTransformPeiShi = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/PeiShi/").GetComponent<RectTransform>();
		CanvasRendererPeiShi = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/PeiShi/").GetComponent<CanvasRenderer>();
		ImagePeiShi = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/PeiShi/").GetComponent<Image>();
		TogglePeiShi = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/PeiShi/").GetComponent<Toggle>();
		AddScriptsPeiShi = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/PeiShi/").GetComponent<AddScripts>();
		RectTransformXieZi = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/XieZi/").GetComponent<RectTransform>();
		CanvasRendererXieZi = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/XieZi/").GetComponent<CanvasRenderer>();
		ImageXieZi = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/XieZi/").GetComponent<Image>();
		ToggleXieZi = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/XieZi/").GetComponent<Toggle>();
		AddScriptsXieZi = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/XieZi/").GetComponent<AddScripts>();
		RectTransformZaiJu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/ZaiJu/").GetComponent<RectTransform>();
		CanvasRendererZaiJu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/ZaiJu/").GetComponent<CanvasRenderer>();
		ImageZaiJu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/ZaiJu/").GetComponent<Image>();
		ToggleZaiJu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/ZaiJu/").GetComponent<Toggle>();
		AddScriptsZaiJu = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/ZaiJu/").GetComponent<AddScripts>();
		RectTransformXiaoHaoPin = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/XiaoHaoPin/").GetComponent<RectTransform>();
		CanvasRendererXiaoHaoPin = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/XiaoHaoPin/").GetComponent<CanvasRenderer>();
		ImageXiaoHaoPin = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/XiaoHaoPin/").GetComponent<Image>();
		ToggleXiaoHaoPin = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/XiaoHaoPin/").GetComponent<Toggle>();
		AddScriptsXiaoHaoPin = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/XiaoHaoPin/").GetComponent<AddScripts>();
		RectTransformQiTa = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/QiTa/").GetComponent<RectTransform>();
		CanvasRendererQiTa = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/QiTa/").GetComponent<CanvasRenderer>();
		ImageQiTa = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/QiTa/").GetComponent<Image>();
		ToggleQiTa = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/QiTa/").GetComponent<Toggle>();
		AddScriptsQiTa = GetGameObject().transform.Find("View/ZhuanBei/ToggleGroup/QiTa/").GetComponent<AddScripts>();
	}

	public RoleWindows(Transform trans)
		: base(trans)
	{
	}

	public RoleWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		TogglePeiShi.onValueChanged.AddListener(OnValueChange);
		ToggleTouBu.onValueChanged.AddListener(OnValueChange);
		ToggleQiTa.onValueChanged.AddListener(OnValueChange);
		ToggleQuanBu.onValueChanged.AddListener(OnValueChange);
		ToggleZaiJu.onValueChanged.AddListener(OnValueChange);
		ToggleXiaoHaoPin.onValueChanged.AddListener(OnValueChange);
		ToggleXieZi.onValueChanged.AddListener(OnValueChange);
		ToggleYiFu.onValueChanged.AddListener(OnValueChange);
		ToggleQuanBu.isOn = false;
		ToggleQuanBu.isOn = true;
		BTBlack.onClick.AddListener(Close);
	}

	private void Close()
	{
		CloseUi();
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		OnValueChange(value: false);
		Sprite sprite = ABMrg.Load<Sprite>("ZbIcon");
		Tool.ShowTutorial("RoleWindows", new Sprite[1] { sprite });
	}

	public override void OnEnable()
	{
		base.OnEnable();
		EventManager.AddListener(MessageType.Game, GameMessageType.ZbUpdate, UpdateZb);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		EventManager.RemoveListener(MessageType.Game, GameMessageType.ZbUpdate, UpdateZb);
	}

	private void UpdateZb(List<object> objects)
	{
		OnValueChange(value: false);
	}

	private void OnValueChange(bool value)
	{
		int type = -1;
		if (ToggleTouBu.isOn)
		{
			type = 1;
		}
		else if (ToggleYiFu.isOn)
		{
			type = 2;
		}
		else if (TogglePeiShi.isOn)
		{
			type = 3;
		}
		else if (ToggleXieZi.isOn)
		{
			type = 4;
		}
		else if (ToggleZaiJu.isOn)
		{
			type = 5;
		}
		else if (ToggleXiaoHaoPin.isOn)
		{
			type = 6;
		}
		else if (ToggleQiTa.isOn)
		{
			type = 7;
		}
		if (type != -1)
		{
			List<Xlsx_Item> list = Xlsx_Item_Query.data.Where((Xlsx_Item item) => item.ItemType == type && SingletonAsMono<GameDataMrg>.Instance.GetProperty(item.Key, "Item", 0L) > 0).ToList();
			list.Sort((Xlsx_Item item, Xlsx_Item xlsxItem) => -SingletonAsMono<GameDataMrg>.Instance.GetItemGetTime(item.Key).CompareTo(SingletonAsMono<GameDataMrg>.Instance.GetItemGetTime(xlsxItem.Key)));
			InitContent(list);
		}
		else
		{
			List<Xlsx_Item> list2 = Xlsx_Item_Query.data.Where((Xlsx_Item item) => SingletonAsMono<GameDataMrg>.Instance.GetProperty(item.Key, "Item", 0L) > 0).ToList();
			list2.Sort((Xlsx_Item item, Xlsx_Item xlsxItem) => -SingletonAsMono<GameDataMrg>.Instance.GetItemGetTime(item.Key).CompareTo(SingletonAsMono<GameDataMrg>.Instance.GetItemGetTime(xlsxItem.Key)));
			InitContent(list2);
		}
	}

	private void InitContent(List<Xlsx_Item> xlsxItems)
	{
		int count = Mathf.Max(50, xlsxItems.Count);
		RectTransformContent.TranFor(count, RectTransformContent.GetChild(0), delegate(int i, GameObject o)
		{
			if (i >= xlsxItems.Count)
			{
				o.GetComponent<RoleItem>().Init(null);
			}
			else
			{
				o.GetComponent<RoleItem>().Init(xlsxItems[i]);
			}
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
