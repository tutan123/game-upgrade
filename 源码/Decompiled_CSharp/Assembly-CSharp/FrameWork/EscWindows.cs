using System.Collections.Generic;
using Script.Mrg;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "EscWindows")]
[UiMode(Mode.Normal, false)]
public class EscWindows : UiActor
{
	private string[] _tutorialIcon = new string[8] { "360", "ComBat", "SaveIcon", "ZbIcon", "ChiFan", "GonZuo", "HeJiu", "Map" };

	public RectTransform RectTransformContinueTheGame;

	public CanvasRenderer CanvasRendererContinueTheGame;

	public TextMeshProUGUI TextMeshProUGUIContinueTheGame;

	public BT BTContinueTheGame;

	public LanguageComponent LanguageComponentContinueTheGame;

	public AddScripts AddScriptsContinueTheGame;

	public RectTransform RectTransformPhone;

	public CanvasRenderer CanvasRendererPhone;

	public TextMeshProUGUI TextMeshProUGUIPhone;

	public BT BTPhone;

	public LanguageComponent LanguageComponentPhone;

	public AddScripts AddScriptsPhone;

	public RectTransform RectTransformLoadSave;

	public CanvasRenderer CanvasRendererLoadSave;

	public TextMeshProUGUI TextMeshProUGUILoadSave;

	public BT BTLoadSave;

	public LanguageComponent LanguageComponentLoadSave;

	public AddScripts AddScriptsLoadSave;

	public RectTransform RectTransformRole;

	public CanvasRenderer CanvasRendererRole;

	public TextMeshProUGUI TextMeshProUGUIRole;

	public BT BTRole;

	public LanguageComponent LanguageComponentRole;

	public AddScripts AddScriptsRole;

	public RectTransform RectTransformTask;

	public CanvasRenderer CanvasRendererTask;

	public TextMeshProUGUI TextMeshProUGUITask;

	public BT BTTask;

	public LanguageComponent LanguageComponentTask;

	public AddScripts AddScriptsTask;

	public RectTransform RectTransformArchivesCenter;

	public CanvasRenderer CanvasRendererArchivesCenter;

	public TextMeshProUGUI TextMeshProUGUIArchivesCenter;

	public BT BTArchivesCenter;

	public LanguageComponent LanguageComponentArchivesCenter;

	public AddScripts AddScriptsArchivesCenter;

	public RectTransform RectTransformTutorial;

	public CanvasRenderer CanvasRendererTutorial;

	public TextMeshProUGUI TextMeshProUGUITutorial;

	public BT BTTutorial;

	public LanguageComponent LanguageComponentTutorial;

	public AddScripts AddScriptsTutorial;

	public RectTransform RectTransformSetting;

	public CanvasRenderer CanvasRendererSetting;

	public TextMeshProUGUI TextMeshProUGUISetting;

	public BT BTSetting;

	public LanguageComponent LanguageComponentSetting;

	public AddScripts AddScriptsSetting;

	public RectTransform RectTransformExit;

	public CanvasRenderer CanvasRendererExit;

	public TextMeshProUGUI TextMeshProUGUIExit;

	public BT BTExit;

	public LanguageComponent LanguageComponentExit;

	public AddScripts AddScriptsExit;

	public RectTransform RectTransformDebug;

	public CanvasRenderer CanvasRendererDebug;

	public Image ImageDebug;

	public BT BTDebug;

	public AddScripts AddScriptsDebug;

	public override void Awake()
	{
		base.Awake();
		RectTransformContinueTheGame = GetGameObject().transform.Find("View/BtnGroup/ContinueTheGameBtn/ContinueTheGame/").GetComponent<RectTransform>();
		CanvasRendererContinueTheGame = GetGameObject().transform.Find("View/BtnGroup/ContinueTheGameBtn/ContinueTheGame/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIContinueTheGame = GetGameObject().transform.Find("View/BtnGroup/ContinueTheGameBtn/ContinueTheGame/").GetComponent<TextMeshProUGUI>();
		BTContinueTheGame = GetGameObject().transform.Find("View/BtnGroup/ContinueTheGameBtn/ContinueTheGame/").GetComponent<BT>();
		LanguageComponentContinueTheGame = GetGameObject().transform.Find("View/BtnGroup/ContinueTheGameBtn/ContinueTheGame/").GetComponent<LanguageComponent>();
		AddScriptsContinueTheGame = GetGameObject().transform.Find("View/BtnGroup/ContinueTheGameBtn/ContinueTheGame/").GetComponent<AddScripts>();
		RectTransformPhone = GetGameObject().transform.Find("View/BtnGroup/PhoneBtn/Phone/").GetComponent<RectTransform>();
		CanvasRendererPhone = GetGameObject().transform.Find("View/BtnGroup/PhoneBtn/Phone/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIPhone = GetGameObject().transform.Find("View/BtnGroup/PhoneBtn/Phone/").GetComponent<TextMeshProUGUI>();
		BTPhone = GetGameObject().transform.Find("View/BtnGroup/PhoneBtn/Phone/").GetComponent<BT>();
		LanguageComponentPhone = GetGameObject().transform.Find("View/BtnGroup/PhoneBtn/Phone/").GetComponent<LanguageComponent>();
		AddScriptsPhone = GetGameObject().transform.Find("View/BtnGroup/PhoneBtn/Phone/").GetComponent<AddScripts>();
		RectTransformLoadSave = GetGameObject().transform.Find("View/BtnGroup/LoadSaveBtn/LoadSave/").GetComponent<RectTransform>();
		CanvasRendererLoadSave = GetGameObject().transform.Find("View/BtnGroup/LoadSaveBtn/LoadSave/").GetComponent<CanvasRenderer>();
		TextMeshProUGUILoadSave = GetGameObject().transform.Find("View/BtnGroup/LoadSaveBtn/LoadSave/").GetComponent<TextMeshProUGUI>();
		BTLoadSave = GetGameObject().transform.Find("View/BtnGroup/LoadSaveBtn/LoadSave/").GetComponent<BT>();
		LanguageComponentLoadSave = GetGameObject().transform.Find("View/BtnGroup/LoadSaveBtn/LoadSave/").GetComponent<LanguageComponent>();
		AddScriptsLoadSave = GetGameObject().transform.Find("View/BtnGroup/LoadSaveBtn/LoadSave/").GetComponent<AddScripts>();
		RectTransformRole = GetGameObject().transform.Find("View/BtnGroup/RoleBtn/Role/").GetComponent<RectTransform>();
		CanvasRendererRole = GetGameObject().transform.Find("View/BtnGroup/RoleBtn/Role/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIRole = GetGameObject().transform.Find("View/BtnGroup/RoleBtn/Role/").GetComponent<TextMeshProUGUI>();
		BTRole = GetGameObject().transform.Find("View/BtnGroup/RoleBtn/Role/").GetComponent<BT>();
		LanguageComponentRole = GetGameObject().transform.Find("View/BtnGroup/RoleBtn/Role/").GetComponent<LanguageComponent>();
		AddScriptsRole = GetGameObject().transform.Find("View/BtnGroup/RoleBtn/Role/").GetComponent<AddScripts>();
		RectTransformTask = GetGameObject().transform.Find("View/BtnGroup/TaskBtn/Task/").GetComponent<RectTransform>();
		CanvasRendererTask = GetGameObject().transform.Find("View/BtnGroup/TaskBtn/Task/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITask = GetGameObject().transform.Find("View/BtnGroup/TaskBtn/Task/").GetComponent<TextMeshProUGUI>();
		BTTask = GetGameObject().transform.Find("View/BtnGroup/TaskBtn/Task/").GetComponent<BT>();
		LanguageComponentTask = GetGameObject().transform.Find("View/BtnGroup/TaskBtn/Task/").GetComponent<LanguageComponent>();
		AddScriptsTask = GetGameObject().transform.Find("View/BtnGroup/TaskBtn/Task/").GetComponent<AddScripts>();
		RectTransformArchivesCenter = GetGameObject().transform.Find("View/BtnGroup/Archives CenterBtn/Archives Center/").GetComponent<RectTransform>();
		CanvasRendererArchivesCenter = GetGameObject().transform.Find("View/BtnGroup/Archives CenterBtn/Archives Center/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIArchivesCenter = GetGameObject().transform.Find("View/BtnGroup/Archives CenterBtn/Archives Center/").GetComponent<TextMeshProUGUI>();
		BTArchivesCenter = GetGameObject().transform.Find("View/BtnGroup/Archives CenterBtn/Archives Center/").GetComponent<BT>();
		LanguageComponentArchivesCenter = GetGameObject().transform.Find("View/BtnGroup/Archives CenterBtn/Archives Center/").GetComponent<LanguageComponent>();
		AddScriptsArchivesCenter = GetGameObject().transform.Find("View/BtnGroup/Archives CenterBtn/Archives Center/").GetComponent<AddScripts>();
		RectTransformTutorial = GetGameObject().transform.Find("View/BtnGroup/Tutorial/Tutorial/").GetComponent<RectTransform>();
		CanvasRendererTutorial = GetGameObject().transform.Find("View/BtnGroup/Tutorial/Tutorial/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITutorial = GetGameObject().transform.Find("View/BtnGroup/Tutorial/Tutorial/").GetComponent<TextMeshProUGUI>();
		BTTutorial = GetGameObject().transform.Find("View/BtnGroup/Tutorial/Tutorial/").GetComponent<BT>();
		LanguageComponentTutorial = GetGameObject().transform.Find("View/BtnGroup/Tutorial/Tutorial/").GetComponent<LanguageComponent>();
		AddScriptsTutorial = GetGameObject().transform.Find("View/BtnGroup/Tutorial/Tutorial/").GetComponent<AddScripts>();
		RectTransformSetting = GetGameObject().transform.Find("View/BtnGroup/Setting/Setting/").GetComponent<RectTransform>();
		CanvasRendererSetting = GetGameObject().transform.Find("View/BtnGroup/Setting/Setting/").GetComponent<CanvasRenderer>();
		TextMeshProUGUISetting = GetGameObject().transform.Find("View/BtnGroup/Setting/Setting/").GetComponent<TextMeshProUGUI>();
		BTSetting = GetGameObject().transform.Find("View/BtnGroup/Setting/Setting/").GetComponent<BT>();
		LanguageComponentSetting = GetGameObject().transform.Find("View/BtnGroup/Setting/Setting/").GetComponent<LanguageComponent>();
		AddScriptsSetting = GetGameObject().transform.Find("View/BtnGroup/Setting/Setting/").GetComponent<AddScripts>();
		RectTransformExit = GetGameObject().transform.Find("View/BtnGroup/ExitBtn/Exit/").GetComponent<RectTransform>();
		CanvasRendererExit = GetGameObject().transform.Find("View/BtnGroup/ExitBtn/Exit/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIExit = GetGameObject().transform.Find("View/BtnGroup/ExitBtn/Exit/").GetComponent<TextMeshProUGUI>();
		BTExit = GetGameObject().transform.Find("View/BtnGroup/ExitBtn/Exit/").GetComponent<BT>();
		LanguageComponentExit = GetGameObject().transform.Find("View/BtnGroup/ExitBtn/Exit/").GetComponent<LanguageComponent>();
		AddScriptsExit = GetGameObject().transform.Find("View/BtnGroup/ExitBtn/Exit/").GetComponent<AddScripts>();
		RectTransformDebug = GetGameObject().transform.Find("View/Debug/").GetComponent<RectTransform>();
		CanvasRendererDebug = GetGameObject().transform.Find("View/Debug/").GetComponent<CanvasRenderer>();
		ImageDebug = GetGameObject().transform.Find("View/Debug/").GetComponent<Image>();
		BTDebug = GetGameObject().transform.Find("View/Debug/").GetComponent<BT>();
		AddScriptsDebug = GetGameObject().transform.Find("View/Debug/").GetComponent<AddScripts>();
	}

	public EscWindows(Transform trans)
		: base(trans)
	{
	}

	public EscWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTContinueTheGame.onClick.AddListener(Continue);
		BTArchivesCenter.onClick.AddListener(DanAn);
		BTLoadSave.onClick.AddListener(OpenSaveWindows);
		BTExit.onClick.AddListener(ToStart);
		BTRole.onClick.AddListener(OpenRoleInfo);
		BTPhone.onClick.AddListener(OpenPhone);
		BTTask.onClick.AddListener(OpenXinYuan);
		BTDebug.onClick.AddListener(delegate
		{
			UiManager.OpenUi<DebugWindows>();
		});
		BTArchivesCenter.onClick.AddListener(OpenRecord);
		BTTutorial.onClick.AddListener(OpenTutorial);
		BTSetting.onClick.AddListener(OpenSetting);
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		BTDebug.SetActive(SingletonAsMono<GameDataMrg>.Instance.isHasDebug);
	}

	private void OpenTutorial()
	{
		List<Sprite> list = new List<Sprite>();
		for (int i = 0; i < _tutorialIcon.Length; i++)
		{
			list.Add(ABMrg.Load<Sprite>(_tutorialIcon[i]));
		}
		UiManager.OpenUi<TutorialWindows>().Init(list.ToArray());
	}

	public void OpenSetting()
	{
		UiManager.OpenUi<SettingWindows>();
	}

	public void OpenRecord()
	{
		UiManager.OpenUi<RecordWindows>();
	}

	private void OpenRoleInfo()
	{
		UiManager.OpenUi<MainWindows>().OpenEquip();
		if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.Map)
		{
			CloseUi();
		}
	}

	private void OpenXinYuan()
	{
		UiManager.OpenUi<MainWindows>().OpenTask();
	}

	private void ToStart()
	{
		UiManager.OpenUi<SureWindows>().Init(LanguageMrg.GetText("A1208"), delegate
		{
			LoadMrg.Load(Scenes.Start);
			CloseUi();
		});
	}

	private void DanAn()
	{
	}

	private void OpenSaveWindows()
	{
		UiManager.OpenUi<LoadSaveWindows>().InitType(isLoad: true, CloseUi);
	}

	private void OpenPhone()
	{
		UiManager.OpenUi<MainWindows>().OpenMessage();
	}

	public void Continue()
	{
		CloseUi();
	}
}
