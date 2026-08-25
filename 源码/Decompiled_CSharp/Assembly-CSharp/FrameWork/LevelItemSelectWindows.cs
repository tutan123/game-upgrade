using System.Collections.Generic;
using Script.Mrg;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "LevelItemSelectWindows")]
public class LevelItemSelectWindows : UiActor
{
	private int _addZhiLi;

	public RectTransform RectTransformStart;

	public CanvasRenderer CanvasRendererStart;

	public Image ImageStart;

	public BT BTStart;

	public AddScripts AddScriptsStart;

	public RectTransform RectTransformZhiLi;

	public CanvasRenderer CanvasRendererZhiLi;

	public TextMeshProUGUI TextMeshProUGUIZhiLi;

	public AddScripts AddScriptsZhiLi;

	public RectTransform RectTransformShuDuGroup;

	public HorizontalLayoutGroup HorizontalLayoutGroupShuDuGroup;

	public AddScripts AddScriptsShuDuGroup;

	public RectTransform RectTransformDaXiaoGroup;

	public HorizontalLayoutGroup HorizontalLayoutGroupDaXiaoGroup;

	public AddScripts AddScriptsDaXiaoGroup;

	public RectTransform RectTransformJianLiGroup;

	public HorizontalLayoutGroup HorizontalLayoutGroupJianLiGroup;

	public AddScripts AddScriptsJianLiGroup;

	public override void Awake()
	{
		base.Awake();
		RectTransformStart = GetGameObject().transform.Find("Bg/Start/").GetComponent<RectTransform>();
		CanvasRendererStart = GetGameObject().transform.Find("Bg/Start/").GetComponent<CanvasRenderer>();
		ImageStart = GetGameObject().transform.Find("Bg/Start/").GetComponent<Image>();
		BTStart = GetGameObject().transform.Find("Bg/Start/").GetComponent<BT>();
		AddScriptsStart = GetGameObject().transform.Find("Bg/Start/").GetComponent<AddScripts>();
		RectTransformZhiLi = GetGameObject().transform.Find("Bg/ZhiLiBg/ZhiLi/").GetComponent<RectTransform>();
		CanvasRendererZhiLi = GetGameObject().transform.Find("Bg/ZhiLiBg/ZhiLi/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIZhiLi = GetGameObject().transform.Find("Bg/ZhiLiBg/ZhiLi/").GetComponent<TextMeshProUGUI>();
		AddScriptsZhiLi = GetGameObject().transform.Find("Bg/ZhiLiBg/ZhiLi/").GetComponent<AddScripts>();
		RectTransformShuDuGroup = GetGameObject().transform.Find("View/ShuXin/ShuDu/ShuDuGroup/").GetComponent<RectTransform>();
		HorizontalLayoutGroupShuDuGroup = GetGameObject().transform.Find("View/ShuXin/ShuDu/ShuDuGroup/").GetComponent<HorizontalLayoutGroup>();
		AddScriptsShuDuGroup = GetGameObject().transform.Find("View/ShuXin/ShuDu/ShuDuGroup/").GetComponent<AddScripts>();
		RectTransformDaXiaoGroup = GetGameObject().transform.Find("View/ShuXin/DaXiao/DaXiaoGroup/").GetComponent<RectTransform>();
		HorizontalLayoutGroupDaXiaoGroup = GetGameObject().transform.Find("View/ShuXin/DaXiao/DaXiaoGroup/").GetComponent<HorizontalLayoutGroup>();
		AddScriptsDaXiaoGroup = GetGameObject().transform.Find("View/ShuXin/DaXiao/DaXiaoGroup/").GetComponent<AddScripts>();
		RectTransformJianLiGroup = GetGameObject().transform.Find("View/ShuXin/JianLi/JianLiGroup/").GetComponent<RectTransform>();
		HorizontalLayoutGroupJianLiGroup = GetGameObject().transform.Find("View/ShuXin/JianLi/JianLiGroup/").GetComponent<HorizontalLayoutGroup>();
		AddScriptsJianLiGroup = GetGameObject().transform.Find("View/ShuXin/JianLi/JianLiGroup/").GetComponent<AddScripts>();
	}

	public LevelItemSelectWindows(Transform trans)
		: base(trans)
	{
	}

	public LevelItemSelectWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTStart.onClick.AddListener(StartGame);
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		_addZhiLi = 0;
		InitShuXin();
		Tool.ShowTutorial("GonZuo", new Sprite[1] { ABMrg.Load<Sprite>("GonZuo") });
	}

	public void AddZhiLi(int value)
	{
		_addZhiLi += value;
	}

	public bool IsHasTiLi(int value)
	{
		return _addZhiLi + value <= SingletonAsMono<GameDataMrg>.Instance.GetProperty("Wisdom", "Property", 0L);
	}

	private void StartGame()
	{
		Xlsx_MiNiGameLevel xlsx_MiNiGameLevel = Xlsx_MiNiGameLevel_Query.XlsxDataAsOneKey.ByKeyGetValue(SingletonAsMono<GameDataMrg>.Instance.xlsxMiNiGameLevelKey);
		CloseUi();
		LoadMrg.Load(xlsx_MiNiGameLevel.Scene);
	}

	public override void OnEnable()
	{
		base.OnEnable();
		EventManager.AddListener(MessageType.Game, GameMessageType.UpdateProperty, UpdateProperty);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		EventManager.RemoveListener(MessageType.Game, GameMessageType.UpdateProperty, UpdateProperty);
	}

	private void UpdateProperty(List<object> parma)
	{
		InitZhiLi();
	}

	public void InitZhiLi()
	{
		TextMeshProUGUIZhiLi.text = (SingletonAsMono<GameDataMrg>.Instance.GetProperty("Wisdom", "Property", 0L) - _addZhiLi).ToString() ?? "";
	}

	private void InitShuXin()
	{
		InitZhiLi();
		List<Xlsx_MiNiGameItem> data = Xlsx_MiNiGameItem_Query.data;
		for (int i = 0; i < data.Count; i++)
		{
			SingletonAsMono<GameDataMrg>.Instance.SetProperty(data[i].Key, 0);
			SingletonAsMono<GameDataMrg>.Instance.SetProperty(data[i].Key + "_Get", 0);
		}
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.ResetMiniGameItem);
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}
}
