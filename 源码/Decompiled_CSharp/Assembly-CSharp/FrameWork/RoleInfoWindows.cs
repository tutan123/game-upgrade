using System.Collections.Generic;
using System.Text.RegularExpressions;
using Script.Mrg;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "RoleInfoWindows")]
[UiMode(Mode.Normal, true)]
public class RoleInfoWindows : UiActor
{
	public class TypeData
	{
		public TypeTypeValue TypeTypeValue;

		public PropertyTypeValue PropertyTypeValue;

		public string name;

		public string Value;
	}

	public enum TypeTypeValue
	{
		Nor,
		Property
	}

	public RectTransform RectTransformContent;

	public VerticalLayoutGroup VerticalLayoutGroupContent;

	public ContentSizeFitter ContentSizeFitterContent;

	public AddScripts AddScriptsContent;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public AddScripts AddScriptsClose;

	public BT BTClose;

	public override void Awake()
	{
		base.Awake();
		RectTransformContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<RectTransform>();
		VerticalLayoutGroupContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<VerticalLayoutGroup>();
		ContentSizeFitterContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<ContentSizeFitter>();
		AddScriptsContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<AddScripts>();
		RectTransformClose = GetGameObject().transform.Find("View/Bg/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("View/Bg/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("View/Bg/Close/").GetComponent<Image>();
		AddScriptsClose = GetGameObject().transform.Find("View/Bg/Close/").GetComponent<AddScripts>();
		BTClose = GetGameObject().transform.Find("View/Bg/Close/").GetComponent<BT>();
	}

	public RoleInfoWindows(Transform trans)
		: base(trans)
	{
	}

	public RoleInfoWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(CloseUi);
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		List<TypeData> list = new List<TypeData>();
		for (PropertyTypeValue propertyTypeValue = PropertyTypeValue.Stamina; propertyTypeValue <= PropertyTypeValue.XinDonLiShangXian; propertyTypeValue++)
		{
			list.Add(new TypeData
			{
				TypeTypeValue = TypeTypeValue.Property,
				PropertyTypeValue = propertyTypeValue
			});
		}
		foreach (KeyValuePair<string, string> datum in SingletonAsMono<GameDataMrg>.Instance.GetData().Data)
		{
			list.Add(new TypeData
			{
				TypeTypeValue = TypeTypeValue.Nor,
				Value = Regex.Unescape(datum.Value),
				name = datum.Key
			});
		}
		RectTransformContent.TranFor(list.Count, RectTransformContent.GetChild(0), delegate(int i, GameObject o)
		{
			if (list[i].TypeTypeValue == TypeTypeValue.Nor)
			{
				o.transform.GetChild(0).GetComponent<TMP_Text>().text = list[i].name;
				o.transform.GetChild(1).GetComponent<TMP_Text>().text = list[i].Value;
			}
			else
			{
				o.transform.GetChild(0).GetComponent<TMP_Text>().text = list[i].PropertyTypeValue.GetName();
				o.transform.GetChild(1).GetComponent<TMP_Text>().text = SingletonAsMono<GameDataMrg>.Instance.GetProperty(list[i].PropertyTypeValue.ToString(), "Property", 0L).ToString() ?? "";
			}
		});
	}
}
