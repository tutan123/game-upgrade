using System;
using System.Collections.Generic;
using System.Linq;
using Script.Mrg;
using Script.Scene;
using Script.Tool;
using Script.UiTool;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace FrameWork;

[ActorInfo("", "MainWindows")]
[UiMode(Mode.Normal, true)]
public class MainWindows : UiActor
{
	private int[] _weekEnd = new int[4] { 4, 5, 11, 12 };

	private UiActor _curOpenUi;

	private bool _isCloseTips;

	private Transform _lastSelectBt;

	private Action _closeAction;

	public RectTransform RectTransformMask;

	public CanvasRenderer CanvasRendererMask;

	public Image ImageMask;

	public AddScripts AddScriptsMask;

	public RectTransform RectTransformMap;

	public CanvasRenderer CanvasRendererMap;

	public Image ImageMap;

	public BT BTMap;

	public AddScripts AddScriptsMap;

	public RectTransform RectTransformEquip;

	public CanvasRenderer CanvasRendererEquip;

	public Image ImageEquip;

	public BT BTEquip;

	public AddScripts AddScriptsEquip;

	public RectTransform RectTransformXinYuan;

	public CanvasRenderer CanvasRendererXinYuan;

	public Image ImageXinYuan;

	public BT BTXinYuan;

	public AddScripts AddScriptsXinYuan;

	public RectTransform RectTransformRole;

	public CanvasRenderer CanvasRendererRole;

	public Image ImageRole;

	public BT BTRole;

	public AddScripts AddScriptsRole;

	public RectTransform RectTransformMessage;

	public CanvasRenderer CanvasRendererMessage;

	public Image ImageMessage;

	public BT BTMessage;

	public AddScripts AddScriptsMessage;

	public RectTransform RectTransformSave;

	public CanvasRenderer CanvasRendererSave;

	public Image ImageSave;

	public BT BTSave;

	public AddScripts AddScriptsSave;

	public RectTransform RectTransformSetting;

	public CanvasRenderer CanvasRendererSetting;

	public Image ImageSetting;

	public BT BTSetting;

	public AddScripts AddScriptsSetting;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformMapGroup;

	public AddScripts AddScriptsMapGroup;

	public RectTransform RectTransformToNight;

	public CanvasRenderer CanvasRendererToNight;

	public Image ImageToNight;

	public BT BTToNight;

	public AddScripts AddScriptsToNight;

	public RectTransform RectTransformRoleGo;

	public AddScripts AddScriptsRoleGo;

	public RectTransform RectTransformTips;

	public TipsTool TipsToolTips;

	public AddScripts AddScriptsTips;

	public RectTransform RectTransformCloseTips;

	public CanvasRenderer CanvasRendererCloseTips;

	public Image ImageCloseTips;

	public BT BTCloseTips;

	public AddScripts AddScriptsCloseTips;

	public RectTransform RectTransformTask;

	public AddScripts AddScriptsTask;

	public CanvasRenderer CanvasRendererTask;

	public Image ImageTask;

	public RectTransform RectTransformTaskList;

	public VerticalLayoutGroup VerticalLayoutGroupTaskList;

	public AddScripts AddScriptsTaskList;

	public override void Awake()
	{
		base.Awake();
		RectTransformMask = GetGameObject().transform.Find("Vieww/Mask/").GetComponent<RectTransform>();
		CanvasRendererMask = GetGameObject().transform.Find("Vieww/Mask/").GetComponent<CanvasRenderer>();
		ImageMask = GetGameObject().transform.Find("Vieww/Mask/").GetComponent<Image>();
		AddScriptsMask = GetGameObject().transform.Find("Vieww/Mask/").GetComponent<AddScripts>();
		RectTransformMap = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Map/").GetComponent<RectTransform>();
		CanvasRendererMap = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Map/").GetComponent<CanvasRenderer>();
		ImageMap = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Map/").GetComponent<Image>();
		BTMap = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Map/").GetComponent<BT>();
		AddScriptsMap = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Map/").GetComponent<AddScripts>();
		RectTransformEquip = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Equip/").GetComponent<RectTransform>();
		CanvasRendererEquip = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Equip/").GetComponent<CanvasRenderer>();
		ImageEquip = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Equip/").GetComponent<Image>();
		BTEquip = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Equip/").GetComponent<BT>();
		AddScriptsEquip = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Equip/").GetComponent<AddScripts>();
		RectTransformXinYuan = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/XinYuan/").GetComponent<RectTransform>();
		CanvasRendererXinYuan = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/XinYuan/").GetComponent<CanvasRenderer>();
		ImageXinYuan = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/XinYuan/").GetComponent<Image>();
		BTXinYuan = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/XinYuan/").GetComponent<BT>();
		AddScriptsXinYuan = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/XinYuan/").GetComponent<AddScripts>();
		RectTransformRole = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Role/").GetComponent<RectTransform>();
		CanvasRendererRole = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Role/").GetComponent<CanvasRenderer>();
		ImageRole = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Role/").GetComponent<Image>();
		BTRole = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Role/").GetComponent<BT>();
		AddScriptsRole = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Role/").GetComponent<AddScripts>();
		RectTransformMessage = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Message/").GetComponent<RectTransform>();
		CanvasRendererMessage = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Message/").GetComponent<CanvasRenderer>();
		ImageMessage = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Message/").GetComponent<Image>();
		BTMessage = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Message/").GetComponent<BT>();
		AddScriptsMessage = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Message/").GetComponent<AddScripts>();
		RectTransformSave = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Save/").GetComponent<RectTransform>();
		CanvasRendererSave = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Save/").GetComponent<CanvasRenderer>();
		ImageSave = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Save/").GetComponent<Image>();
		BTSave = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Save/").GetComponent<BT>();
		AddScriptsSave = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Save/").GetComponent<AddScripts>();
		RectTransformSetting = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Setting/").GetComponent<RectTransform>();
		CanvasRendererSetting = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Setting/").GetComponent<CanvasRenderer>();
		ImageSetting = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Setting/").GetComponent<Image>();
		BTSetting = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Setting/").GetComponent<BT>();
		AddScriptsSetting = GetGameObject().transform.Find("Vieww/BtGroup/BtnGroup/Setting/").GetComponent<AddScripts>();
		RectTransformClose = GetGameObject().transform.Find("Vieww/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("Vieww/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("Vieww/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("Vieww/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("Vieww/Close/").GetComponent<AddScripts>();
		RectTransformMapGroup = GetGameObject().transform.Find("Vieww/MapGroup/").GetComponent<RectTransform>();
		AddScriptsMapGroup = GetGameObject().transform.Find("Vieww/MapGroup/").GetComponent<AddScripts>();
		RectTransformToNight = GetGameObject().transform.Find("Vieww/MapGroup/ToNight/").GetComponent<RectTransform>();
		CanvasRendererToNight = GetGameObject().transform.Find("Vieww/MapGroup/ToNight/").GetComponent<CanvasRenderer>();
		ImageToNight = GetGameObject().transform.Find("Vieww/MapGroup/ToNight/").GetComponent<Image>();
		BTToNight = GetGameObject().transform.Find("Vieww/MapGroup/ToNight/").GetComponent<BT>();
		AddScriptsToNight = GetGameObject().transform.Find("Vieww/MapGroup/ToNight/").GetComponent<AddScripts>();
		RectTransformRoleGo = GetGameObject().transform.Find("Vieww/MapGroup/RoleGo/").GetComponent<RectTransform>();
		AddScriptsRoleGo = GetGameObject().transform.Find("Vieww/MapGroup/RoleGo/").GetComponent<AddScripts>();
		RectTransformTips = GetGameObject().transform.Find("Vieww/MapGroup/RoleGo/TipsGo/Tips/").GetComponent<RectTransform>();
		TipsToolTips = GetGameObject().transform.Find("Vieww/MapGroup/RoleGo/TipsGo/Tips/").GetComponent<TipsTool>();
		AddScriptsTips = GetGameObject().transform.Find("Vieww/MapGroup/RoleGo/TipsGo/Tips/").GetComponent<AddScripts>();
		RectTransformCloseTips = GetGameObject().transform.Find("Vieww/MapGroup/RoleGo/TipsGo/Tips/Sure/CloseTips/").GetComponent<RectTransform>();
		CanvasRendererCloseTips = GetGameObject().transform.Find("Vieww/MapGroup/RoleGo/TipsGo/Tips/Sure/CloseTips/").GetComponent<CanvasRenderer>();
		ImageCloseTips = GetGameObject().transform.Find("Vieww/MapGroup/RoleGo/TipsGo/Tips/Sure/CloseTips/").GetComponent<Image>();
		BTCloseTips = GetGameObject().transform.Find("Vieww/MapGroup/RoleGo/TipsGo/Tips/Sure/CloseTips/").GetComponent<BT>();
		AddScriptsCloseTips = GetGameObject().transform.Find("Vieww/MapGroup/RoleGo/TipsGo/Tips/Sure/CloseTips/").GetComponent<AddScripts>();
		RectTransformTask = GetGameObject().transform.Find("Vieww/MapGroup/Task/").GetComponent<RectTransform>();
		AddScriptsTask = GetGameObject().transform.Find("Vieww/MapGroup/Task/").GetComponent<AddScripts>();
		CanvasRendererTask = GetGameObject().transform.Find("Vieww/MapGroup/Task/").GetComponent<CanvasRenderer>();
		ImageTask = GetGameObject().transform.Find("Vieww/MapGroup/Task/").GetComponent<Image>();
		RectTransformTaskList = GetGameObject().transform.Find("Vieww/MapGroup/Task/TaskList/").GetComponent<RectTransform>();
		VerticalLayoutGroupTaskList = GetGameObject().transform.Find("Vieww/MapGroup/Task/TaskList/").GetComponent<VerticalLayoutGroup>();
		AddScriptsTaskList = GetGameObject().transform.Find("Vieww/MapGroup/Task/TaskList/").GetComponent<AddScripts>();
	}

	public MainWindows(Transform trans)
		: base(trans)
	{
	}

	public MainWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTMap.onClick.AddListener(OpenMap);
		BTEquip.onClick.AddListener(OpenEquip);
		BTMessage.onClick.AddListener(delegate
		{
			OpenMessage();
		});
		BTRole.onClick.AddListener(OpenRole);
		BTSave.onClick.AddListener(OpenSave);
		BTXinYuan.onClick.AddListener(delegate
		{
			OpenTask();
		});
		BTCloseTips.onClick.AddListener(CloseTips);
		BTClose.onClick.AddListener(Close);
		BTToNight.onClick.AddListener(SetYe);
		BTSetting.onClick.AddListener(OpenSettings);
		HideSelect(BTMap.transform);
		HideSelect(BTEquip.transform);
		HideSelect(BTMessage.transform);
		HideSelect(BTRole.transform);
		HideSelect(BTSave.transform);
		HideSelect(BTXinYuan.transform);
		HideSelect(BTSetting.transform);
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		_isCloseTips = false;
		if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.Map)
		{
			OpenMap();
			RectTransformMapGroup.SetActive(active: true);
			BTToNight.SetActive((SingletonAsMono<GameDataMrg>.Instance.IsMorning && _weekEnd.Contains(SingletonAsMono<GameDataMrg>.Instance.CurRound)) || (SingletonAsMono<GameDataMrg>.Instance.IsMorning && SingletonAsMono<GameDataMrg>.Instance.CurRound >= 14));
			BTSave.interactable = true;
			BTMap.interactable = true;
			SetMask(RectTransformSave, isShow: false);
			SetMask(RectTransformMap, isShow: false);
			BTClose.SetActive(active: false);
		}
		else if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.Video360Scene)
		{
			CloseCurUi();
			CloseTipsAsNotAdd();
			BTSave.interactable = true;
			BTMap.interactable = false;
			SetMask(RectTransformSave, isShow: false);
			SetMask(RectTransformMap, isShow: true);
			BTClose.SetActive(active: true);
			BTToNight.SetActive(active: false);
			RectTransformMapGroup.SetActive(active: false);
		}
		else if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.GameScene)
		{
			CloseCurUi();
			CloseTipsAsNotAdd();
			BTSave.interactable = false;
			BTMap.interactable = false;
			SetMask(RectTransformSave, isShow: true);
			SetMask(RectTransformMap, isShow: true);
			BTClose.SetActive(active: true);
			BTToNight.SetActive(active: false);
			RectTransformMapGroup.SetActive(active: false);
		}
	}

	private void SetMask(RectTransform rectTransform, bool isShow)
	{
		Transform transform = rectTransform.Find("Mask");
		if (transform != null)
		{
			transform.SetActive(isShow);
		}
	}

	public void Init(Action action)
	{
		_closeAction = action;
	}

	public void SetYe()
	{
		UiManager.OpenUi<SureWindows>().Init(LanguageMrg.GetText("A1596"), delegate
		{
			if ((bool)Map3DScene.Instance)
			{
				SingletonAsMono<GameDataMrg>.Instance.MapLoc = Map3DScene.Instance.player.transform.position;
			}
			SingletonAsMono<GameDataMrg>.Instance.IsMorning = false;
			EventManager.DispatchEvent(MessageType.Video, VideoMessageType.UpdateTime);
			LoadMrg.Load(Scenes.Map3D);
		});
	}

	public void OpenMap()
	{
		if (UiManager.IsOpenUi<PhoneWindows>() && !UiManager.GetUi<PhoneWindows>().IsCanClose())
		{
			UiManager.ShowTips(LanguageMrg.GetText("A831"));
			return;
		}
		if (SingletonAsMono<GameDataMrg>.Instance.Is2D)
		{
			if (UiManager.IsOpenUi<MapRiWindows>() || UiManager.IsOpenUi<MapYeWindows>())
			{
				return;
			}
			CloseCurUi();
			BTToNight.SetActive((SingletonAsMono<GameDataMrg>.Instance.IsMorning && _weekEnd.Contains(SingletonAsMono<GameDataMrg>.Instance.CurRound)) || (SingletonAsMono<GameDataMrg>.Instance.IsMorning && SingletonAsMono<GameDataMrg>.Instance.CurRound >= 14));
			RectTransformMapGroup.SetActive(active: true);
			if (!_isCloseTips)
			{
				ShowTips();
			}
			if (SingletonAsMono<GameDataMrg>.Instance.IsMorning)
			{
				_curOpenUi = UiManager.OpenUi<MapRiWindows>();
				UiManager.GetUi<MapRiWindows>().transform.SetTagetTop(base.transform);
			}
			else
			{
				_curOpenUi = UiManager.OpenUi<MapYeWindows>();
				UiManager.GetUi<MapYeWindows>().transform.SetTagetTop(base.transform);
			}
			RectTransformMask.SetActive(active: true);
		}
		else
		{
			CloseCurUi();
			if (!_isCloseTips)
			{
				ShowTips();
			}
			BTToNight.SetActive((SingletonAsMono<GameDataMrg>.Instance.IsMorning && _weekEnd.Contains(SingletonAsMono<GameDataMrg>.Instance.CurRound)) || (SingletonAsMono<GameDataMrg>.Instance.IsMorning && SingletonAsMono<GameDataMrg>.Instance.CurRound >= 14));
			RectTransformMapGroup.SetActive(active: true);
			RectTransformMask.SetActive(active: false);
		}
		if (!SingletonAsMono<GameDataMrg>.Instance.IsMorning)
		{
			Tool.ShowTutorial("ChiFan", new Sprite[1] { ABMrg.Load<Sprite>("ChiFan") });
		}
		if (SingletonAsMono<GameDataMrg>.Instance.CurRound == 3)
		{
			Tool.ShowTutorial("Round3Map3D", new Sprite[1] { ABMrg.Load<Sprite>("Round3Map3D") });
		}
		if (SingletonAsMono<GlobalMrg>.Instance.videoState != VideoState.Video360Scene && !Tool.ShowTutorial("FistLoadingToMap", new Sprite[1] { ABMrg.Load<Sprite>("FistToMap") }) && SingletonAsMono<GameDataMrg>.Instance.IsMorning && SingletonAsMono<GameDataMrg>.Instance.CurRound == 4)
		{
			Tool.ShowTutorial("RiToYe", new Sprite[1] { ABMrg.Load<Sprite>("RiToYe") });
		}
		_lastSelectBt = BTMap.transform;
		ShowCurSelect();
		InitTask();
	}

	private void InitTask()
	{
		List<Xlsx_Task> list = (from task in Xlsx_Task_Query.data
			where SingletonAsMono<GameDataMrg>.Instance.GetTaskState(task.Key) == TaskType.Open && SingletonAsMono<GameDataMrg>.Instance.CurRound <= task.Round
			where !Tool.IsSucProperty(task.PropertyType, task.PropertyTypeValue)
			where task.IsMain == 1
			select task).ToList();
		int count = Mathf.Min(1, list.Count);
		list.Sort((Xlsx_Task task, Xlsx_Task xlsxTask) => -task.Range.CompareTo(xlsxTask.Range));
		RectTransformTask.SetActive(list.Count > 0);
		RectTransformTaskList.TranFor(count, RectTransformTaskList.GetChild(0), delegate(int i, GameObject o)
		{
			o.GetComponent<MapTaskItem>().Init(list[i]);
		});
	}

	private void ShowTips()
	{
		Xlsx_RoundInfo xlsx_RoundInfo = Xlsx_RoundInfo_Query.XlsxDataAsOneKey.ByKeyGetValue("A" + SingletonAsMono<GameDataMrg>.Instance.CurRound);
		if (xlsx_RoundInfo != null)
		{
			if (SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty("IsShowTips_" + xlsx_RoundInfo.Key + "_" + SingletonAsMono<GameDataMrg>.Instance.IsMorning, "Tips") > 0)
			{
				RectTransformRoleGo.SetActive(active: false);
				return;
			}
			string text = "";
			text = ((!SingletonAsMono<GameDataMrg>.Instance.IsMorning) ? xlsx_RoundInfo.Night : xlsx_RoundInfo.Morning);
			if (!string.IsNullOrEmpty(text))
			{
				TipsToolTips.ShowTips(LanguageMrg.GetText(text));
				RectTransformRoleGo.SetActive(active: true);
			}
			else
			{
				RectTransformRoleGo.SetActive(active: false);
			}
		}
		else
		{
			RectTransformRoleGo.SetActive(active: false);
		}
	}

	private void CloseTips()
	{
		_isCloseTips = true;
		RectTransformRoleGo.SetActive(active: false);
		SingletonAsMono<GameDataMrg>.Instance.AddRoundProperty("IsShowTips_A" + SingletonAsMono<GameDataMrg>.Instance.CurRound + "_" + SingletonAsMono<GameDataMrg>.Instance.IsMorning, 1, "Tips");
	}

	private void CloseTipsAsNotAdd()
	{
		_isCloseTips = true;
		RectTransformRoleGo.SetActive(active: false);
	}

	public void OpenRole()
	{
		if (UiManager.IsOpenUi<PhoneWindows>() && !UiManager.GetUi<PhoneWindows>().IsCanClose())
		{
			UiManager.ShowTips(LanguageMrg.GetText("A831"));
		}
		else if (!UiManager.IsOpenUi<ScoreWindows>())
		{
			CloseCurUi();
			_curOpenUi = UiManager.OpenUi<ScoreWindows>();
			UiManager.GetUi<ScoreWindows>().Init();
			UiManager.GetUi<ScoreWindows>().transform.SetTagetTop(base.transform);
			_lastSelectBt = BTRole.transform;
			ShowCurSelect();
		}
	}

	public void OpenEquip()
	{
		if (UiManager.IsOpenUi<PhoneWindows>() && !UiManager.GetUi<PhoneWindows>().IsCanClose())
		{
			UiManager.ShowTips(LanguageMrg.GetText("A831"));
		}
		else if (!UiManager.IsOpenUi<RoleWindows>())
		{
			CloseCurUi();
			_curOpenUi = UiManager.OpenUi<RoleWindows>();
			UiManager.GetUi<RoleWindows>().transform.SetTagetTop(base.transform);
			_lastSelectBt = BTEquip.transform;
			ShowCurSelect();
		}
	}

	public TaskWindows OpenTask()
	{
		if (UiManager.IsOpenUi<PhoneWindows>() && !UiManager.GetUi<PhoneWindows>().IsCanClose())
		{
			UiManager.ShowTips(LanguageMrg.GetText("A831"));
			return null;
		}
		if (UiManager.IsOpenUi<TaskWindows>())
		{
			return UiManager.GetUi<TaskWindows>();
		}
		CloseCurUi();
		_curOpenUi = UiManager.OpenUi<TaskWindows>();
		UiManager.GetUi<TaskWindows>().transform.SetTagetTop(base.transform);
		_lastSelectBt = BTXinYuan.transform;
		ShowCurSelect();
		return UiManager.GetUi<TaskWindows>();
	}

	public PhoneWindows OpenMessage()
	{
		if (UiManager.IsOpenUi<PhoneWindows>())
		{
			return UiManager.GetUi<PhoneWindows>();
		}
		CloseCurUi();
		_curOpenUi = UiManager.OpenUi<PhoneWindows>();
		UiManager.GetUi<PhoneWindows>().transform.SetTagetTop(base.transform);
		_lastSelectBt = BTMessage.transform;
		ShowCurSelect();
		return UiManager.GetUi<PhoneWindows>();
	}

	public void OpenSave()
	{
		if (UiManager.IsOpenUi<PhoneWindows>() && !UiManager.GetUi<PhoneWindows>().IsCanClose())
		{
			UiManager.ShowTips(LanguageMrg.GetText("A831"));
		}
		else if (!UiManager.IsOpenUi<SaveWindows>())
		{
			CloseCurUi();
			_curOpenUi = UiManager.OpenUi<SaveWindows>();
			UiManager.GetUi<SaveWindows>().InitType(isLoad: false);
			UiManager.GetUi<SaveWindows>().transform.SetTagetTop(base.transform);
			_lastSelectBt = BTSave.transform;
			ShowCurSelect();
		}
	}

	public void OpenSettings()
	{
		if (UiManager.IsOpenUi<PhoneWindows>() && !UiManager.GetUi<PhoneWindows>().IsCanClose())
		{
			UiManager.ShowTips(LanguageMrg.GetText("A831"));
		}
		else
		{
			UiManager.OpenUi<EscWindows>();
		}
	}

	private void CloseCurUi()
	{
		if (_curOpenUi != null && _curOpenUi.GetGameObject() != null)
		{
			if (_curOpenUi.GetUiName() == "Map")
			{
				BTToNight.SetActive(active: false);
				RectTransformMapGroup.SetActive(active: false);
			}
			UiManager.RemoveUi(_curOpenUi.GetIndex());
			_curOpenUi = null;
		}
		else
		{
			RectTransformMapGroup.SetActive(active: false);
		}
		if ((bool)_lastSelectBt)
		{
			HideSelect(_lastSelectBt);
		}
	}

	private void CloseCurUiAsCloseSelf()
	{
		if (_curOpenUi != null && _curOpenUi.GetGameObject() != null)
		{
			if (_curOpenUi.GetUiName() == "Map")
			{
				BTToNight.SetActive(active: false);
			}
			_curOpenUi.CloseUi();
			_curOpenUi = null;
		}
		if ((bool)_lastSelectBt)
		{
			HideSelect(_lastSelectBt);
		}
	}

	private void HideSelect(Transform tran)
	{
		tran.Find("Select")?.gameObject.SetActive(value: false);
	}

	private void ShowCurSelect()
	{
		if ((bool)_lastSelectBt)
		{
			ShowSelect(_lastSelectBt);
		}
	}

	private void ShowSelect(Transform tran)
	{
		tran.Find("Select")?.gameObject.SetActive(value: true);
	}

	public override void OnClose()
	{
		CloseCurUiAsCloseSelf();
		_closeAction?.Invoke();
		base.OnClose();
	}

	public override bool IsCanJoinPause()
	{
		if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.Map)
		{
			return false;
		}
		return true;
	}

	private void Close()
	{
		if (UiManager.IsOpenUi<PhoneWindows>() && !UiManager.GetUi<PhoneWindows>().IsCanClose())
		{
			UiManager.ShowTips(LanguageMrg.GetText("A831"));
		}
		else
		{
			CloseUi();
		}
	}

	public void ChangeMap()
	{
		OpenMap();
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
		else if (SingletonAsMono<GlobalMrg>.Instance.videoState != VideoState.Map && (!UiManager.IsOpenUi<PhoneWindows>() || UiManager.GetUi<PhoneWindows>().IsCanClose()))
		{
			CloseUi();
		}
	}
}
