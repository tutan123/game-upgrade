using System.Collections.Generic;
using FrameWork.Data;
using Script.Mrg;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace FrameWork;

[ActorInfo("", "DebugWindows")]
[UiMode(Mode.Normal, true)]
public class DebugWindows : UiActor
{
	public RectTransform RectTransformBg;

	public CanvasRenderer CanvasRendererBg;

	public Image ImageBg;

	public BT BTBg;

	public AddScripts AddScriptsBg;

	public RectTransform RectTransformDropdown;

	public CanvasRenderer CanvasRendererDropdown;

	public Image ImageDropdown;

	public TMP_Dropdown TMP_DropdownDropdown;

	public AddScripts AddScriptsDropdown;

	public RectTransform RectTransformInputField;

	public CanvasRenderer CanvasRendererInputField;

	public Image ImageInputField;

	public TMP_InputField TMP_InputFieldInputField;

	public AddScripts AddScriptsInputField;

	public RectTransform RectTransformButton;

	public CanvasRenderer CanvasRendererButton;

	public Image ImageButton;

	public AddScripts AddScriptsButton;

	public BT BTButton;

	public RectTransform RectTransformDropdownItem;

	public CanvasRenderer CanvasRendererDropdownItem;

	public Image ImageDropdownItem;

	public TMP_Dropdown TMP_DropdownDropdownItem;

	public AddScripts AddScriptsDropdownItem;

	public RectTransform RectTransformButtonItem;

	public CanvasRenderer CanvasRendererButtonItem;

	public Image ImageButtonItem;

	public AddScripts AddScriptsButtonItem;

	public BT BTButtonItem;

	public RectTransform RectTransformSaveDatta;

	public CanvasRenderer CanvasRendererSaveDatta;

	public Image ImageSaveDatta;

	public AddScripts AddScriptsSaveDatta;

	public BT BTSaveDatta;

	public RectTransform RectTransformRoleInfoBtn;

	public CanvasRenderer CanvasRendererRoleInfoBtn;

	public Image ImageRoleInfoBtn;

	public AddScripts AddScriptsRoleInfoBtn;

	public BT BTRoleInfoBtn;

	public override void Awake()
	{
		base.Awake();
		RectTransformBg = GetGameObject().transform.Find("Bg/").GetComponent<RectTransform>();
		CanvasRendererBg = GetGameObject().transform.Find("Bg/").GetComponent<CanvasRenderer>();
		ImageBg = GetGameObject().transform.Find("Bg/").GetComponent<Image>();
		BTBg = GetGameObject().transform.Find("Bg/").GetComponent<BT>();
		AddScriptsBg = GetGameObject().transform.Find("Bg/").GetComponent<AddScripts>();
		RectTransformDropdown = GetGameObject().transform.Find("View/Dropdown/").GetComponent<RectTransform>();
		CanvasRendererDropdown = GetGameObject().transform.Find("View/Dropdown/").GetComponent<CanvasRenderer>();
		ImageDropdown = GetGameObject().transform.Find("View/Dropdown/").GetComponent<Image>();
		TMP_DropdownDropdown = GetGameObject().transform.Find("View/Dropdown/").GetComponent<TMP_Dropdown>();
		AddScriptsDropdown = GetGameObject().transform.Find("View/Dropdown/").GetComponent<AddScripts>();
		RectTransformInputField = GetGameObject().transform.Find("View/InputField/").GetComponent<RectTransform>();
		CanvasRendererInputField = GetGameObject().transform.Find("View/InputField/").GetComponent<CanvasRenderer>();
		ImageInputField = GetGameObject().transform.Find("View/InputField/").GetComponent<Image>();
		TMP_InputFieldInputField = GetGameObject().transform.Find("View/InputField/").GetComponent<TMP_InputField>();
		AddScriptsInputField = GetGameObject().transform.Find("View/InputField/").GetComponent<AddScripts>();
		RectTransformButton = GetGameObject().transform.Find("View/Button/").GetComponent<RectTransform>();
		CanvasRendererButton = GetGameObject().transform.Find("View/Button/").GetComponent<CanvasRenderer>();
		ImageButton = GetGameObject().transform.Find("View/Button/").GetComponent<Image>();
		AddScriptsButton = GetGameObject().transform.Find("View/Button/").GetComponent<AddScripts>();
		BTButton = GetGameObject().transform.Find("View/Button/").GetComponent<BT>();
		RectTransformDropdownItem = GetGameObject().transform.Find("View/DropdownItem/").GetComponent<RectTransform>();
		CanvasRendererDropdownItem = GetGameObject().transform.Find("View/DropdownItem/").GetComponent<CanvasRenderer>();
		ImageDropdownItem = GetGameObject().transform.Find("View/DropdownItem/").GetComponent<Image>();
		TMP_DropdownDropdownItem = GetGameObject().transform.Find("View/DropdownItem/").GetComponent<TMP_Dropdown>();
		AddScriptsDropdownItem = GetGameObject().transform.Find("View/DropdownItem/").GetComponent<AddScripts>();
		RectTransformButtonItem = GetGameObject().transform.Find("View/ButtonItem/").GetComponent<RectTransform>();
		CanvasRendererButtonItem = GetGameObject().transform.Find("View/ButtonItem/").GetComponent<CanvasRenderer>();
		ImageButtonItem = GetGameObject().transform.Find("View/ButtonItem/").GetComponent<Image>();
		AddScriptsButtonItem = GetGameObject().transform.Find("View/ButtonItem/").GetComponent<AddScripts>();
		BTButtonItem = GetGameObject().transform.Find("View/ButtonItem/").GetComponent<BT>();
		RectTransformSaveDatta = GetGameObject().transform.Find("View/SaveDatta/").GetComponent<RectTransform>();
		CanvasRendererSaveDatta = GetGameObject().transform.Find("View/SaveDatta/").GetComponent<CanvasRenderer>();
		ImageSaveDatta = GetGameObject().transform.Find("View/SaveDatta/").GetComponent<Image>();
		AddScriptsSaveDatta = GetGameObject().transform.Find("View/SaveDatta/").GetComponent<AddScripts>();
		BTSaveDatta = GetGameObject().transform.Find("View/SaveDatta/").GetComponent<BT>();
		RectTransformRoleInfoBtn = GetGameObject().transform.Find("View/RoleInfoBtn/").GetComponent<RectTransform>();
		CanvasRendererRoleInfoBtn = GetGameObject().transform.Find("View/RoleInfoBtn/").GetComponent<CanvasRenderer>();
		ImageRoleInfoBtn = GetGameObject().transform.Find("View/RoleInfoBtn/").GetComponent<Image>();
		AddScriptsRoleInfoBtn = GetGameObject().transform.Find("View/RoleInfoBtn/").GetComponent<AddScripts>();
		BTRoleInfoBtn = GetGameObject().transform.Find("View/RoleInfoBtn/").GetComponent<BT>();
	}

	public DebugWindows(Transform trans)
		: base(trans)
	{
	}

	public DebugWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTBg.onClick.AddListener(CloseUi);
		BTButton.onClick.AddListener(AddValue);
		BTButtonItem.onClick.AddListener(AddItem);
		BTSaveDatta.onClick.AddListener(SaveData);
		BTRoleInfoBtn.onClick.AddListener(delegate
		{
			UiManager.OpenUi<RoleInfoWindows>();
		});
		List<TMP_Dropdown.OptionData> list = new List<TMP_Dropdown.OptionData>();
		for (PropertyTypeValue propertyTypeValue = PropertyTypeValue.Stamina; propertyTypeValue <= PropertyTypeValue.TmpJiNengDian; propertyTypeValue++)
		{
			list.Add(new TMP_Dropdown.OptionData(propertyTypeValue.GetName() + "_" + propertyTypeValue));
		}
		List<TMP_Dropdown.OptionData> list2 = new List<TMP_Dropdown.OptionData>();
		for (int i = 0; i < Xlsx_Item_Query.data.Count; i++)
		{
			list2.Add(new TMP_Dropdown.OptionData(LanguageMrg.GetText(Xlsx_Item_Query.data[i].Name) + "_" + Xlsx_Item_Query.data[i].Key));
		}
		TMP_DropdownDropdownItem.options = list2;
		TMP_DropdownDropdown.options = list;
	}

	private void SaveData()
	{
		string text = TMP_InputFieldInputField.text;
		if (!string.IsNullOrEmpty(text))
		{
			GameData.SetString(GameData.CurDataKey, text);
		}
	}

	public void AddValue()
	{
		string text = TMP_InputFieldInputField.text;
		if (!string.IsNullOrEmpty(text))
		{
			int num = int.Parse(text);
			string text2 = TMP_DropdownDropdown.options[TMP_DropdownDropdown.value].text.Split("_")[1];
			SingletonAsMono<GameDataMrg>.Instance.AddProperty(text2.ToString(), num, "Property");
		}
	}

	public void AddItem()
	{
		string text = TMP_InputFieldInputField.text;
		if (!string.IsNullOrEmpty(text))
		{
			int num = int.Parse(text);
			string text2 = TMP_DropdownDropdownItem.options[TMP_DropdownDropdownItem.value].text.Split("_")[1];
			SingletonAsMono<GameDataMrg>.Instance.AddProperty(text2.ToString(), num, "Item");
		}
	}
}
