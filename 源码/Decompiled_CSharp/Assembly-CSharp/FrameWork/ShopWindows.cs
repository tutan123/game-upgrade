using System.Collections.Generic;
using System.Linq;
using Script.Mrg;
using Script.Tool;
using Script.UiTool;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "ShopWindows")]
public class ShopWindows : UiActor
{
	private string _type;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformContent;

	public ContentSizeFitter ContentSizeFitterContent;

	public VerticalLayoutGroup VerticalLayoutGroupContent;

	public AddScripts AddScriptsContent;

	public override void Awake()
	{
		base.Awake();
		RectTransformClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<AddScripts>();
		RectTransformContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<RectTransform>();
		ContentSizeFitterContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<ContentSizeFitter>();
		VerticalLayoutGroupContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<VerticalLayoutGroup>();
		AddScriptsContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<AddScripts>();
	}

	public ShopWindows(Transform trans)
		: base(trans)
	{
	}

	public ShopWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(Continue);
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
	}

	public void Init()
	{
		Init(_type);
	}

	public void Init(string type)
	{
		_type = type;
		int t = int.Parse(type);
		List<Xlsx_Item> data = Xlsx_Item_Query.data.Where((Xlsx_Item item) => item.GetType == t).ToList();
		data = data.Where(delegate(Xlsx_Item item)
		{
			if (item.GetMaxCount == -1)
			{
				return true;
			}
			int getMaxCount = item.GetMaxCount;
			return SingletonAsMono<GameDataMrg>.Instance.GetProperty(item.Key, "GetCount", 0L) < getMaxCount;
		}).ToList();
		data.Sort((Xlsx_Item item, Xlsx_Item xlsxItem) => -item.Range.CompareTo(xlsxItem.Range));
		RectTransformContent.TranFor(data.Count, RectTransformContent.GetChild(0), delegate(int i, GameObject o)
		{
			o.GetComponent<ShopItem>().Init(data[i]);
		});
	}

	private void Continue()
	{
		UiManager.HideUi<ShopGetWindows>();
		CloseUi();
	}

	public override void OnDisable()
	{
		base.OnDisable();
		UiManager.HideUi<ShopGetWindows>();
	}
}
