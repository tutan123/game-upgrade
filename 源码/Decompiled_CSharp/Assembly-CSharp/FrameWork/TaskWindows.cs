using System.Collections.Generic;
using System.Linq;
using Script.Mrg;
using Script.Tool;
using Script.UiTool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace FrameWork;

[ActorInfo("", "TaskWindows")]
[UiMode(Mode.Normal, true)]
public class TaskWindows : UiActor
{
	private Toggle _lastToggle;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformUnSuc;

	public CanvasRenderer CanvasRendererUnSuc;

	public Image ImageUnSuc;

	public Toggle ToggleUnSuc;

	public AddScripts AddScriptsUnSuc;

	public RectTransform RectTransformSuc;

	public CanvasRenderer CanvasRendererSuc;

	public Image ImageSuc;

	public Toggle ToggleSuc;

	public AddScripts AddScriptsSuc;

	public RectTransform RectTransformOutOf;

	public CanvasRenderer CanvasRendererOutOf;

	public Image ImageOutOf;

	public Toggle ToggleOutOf;

	public AddScripts AddScriptsOutOf;

	public RectTransform RectTransformContent;

	public ContentSizeFitter ContentSizeFitterContent;

	public VerticalLayoutGroup VerticalLayoutGroupContent;

	public AddScripts AddScriptsContent;

	public RectTransform RectTransformInfoBg;

	public CanvasRenderer CanvasRendererInfoBg;

	public Image ImageInfoBg;

	public TaskInfo TaskInfoInfoBg;

	public AddScripts AddScriptsInfoBg;

	public RectTransform RectTransformHaoGanDu;

	public CanvasRenderer CanvasRendererHaoGanDu;

	public TextMeshProUGUI TextMeshProUGUIHaoGanDu;

	public AddScripts AddScriptsHaoGanDu;

	public override void Awake()
	{
		base.Awake();
		RectTransformClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<AddScripts>();
		RectTransformUnSuc = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/UnSuc/").GetComponent<RectTransform>();
		CanvasRendererUnSuc = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/UnSuc/").GetComponent<CanvasRenderer>();
		ImageUnSuc = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/UnSuc/").GetComponent<Image>();
		ToggleUnSuc = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/UnSuc/").GetComponent<Toggle>();
		AddScriptsUnSuc = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/UnSuc/").GetComponent<AddScripts>();
		RectTransformSuc = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/Suc/").GetComponent<RectTransform>();
		CanvasRendererSuc = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/Suc/").GetComponent<CanvasRenderer>();
		ImageSuc = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/Suc/").GetComponent<Image>();
		ToggleSuc = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/Suc/").GetComponent<Toggle>();
		AddScriptsSuc = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/Suc/").GetComponent<AddScripts>();
		RectTransformOutOf = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/OutOf/").GetComponent<RectTransform>();
		CanvasRendererOutOf = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/OutOf/").GetComponent<CanvasRenderer>();
		ImageOutOf = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/OutOf/").GetComponent<Image>();
		ToggleOutOf = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/OutOf/").GetComponent<Toggle>();
		AddScriptsOutOf = GetGameObject().transform.Find("View/Bg/SeleectBtnGroup/OutOf/").GetComponent<AddScripts>();
		RectTransformContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<RectTransform>();
		ContentSizeFitterContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<ContentSizeFitter>();
		VerticalLayoutGroupContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<VerticalLayoutGroup>();
		AddScriptsContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/").GetComponent<AddScripts>();
		RectTransformInfoBg = GetGameObject().transform.Find("View/Bg/InfoBg/").GetComponent<RectTransform>();
		CanvasRendererInfoBg = GetGameObject().transform.Find("View/Bg/InfoBg/").GetComponent<CanvasRenderer>();
		ImageInfoBg = GetGameObject().transform.Find("View/Bg/InfoBg/").GetComponent<Image>();
		TaskInfoInfoBg = GetGameObject().transform.Find("View/Bg/InfoBg/").GetComponent<TaskInfo>();
		AddScriptsInfoBg = GetGameObject().transform.Find("View/Bg/InfoBg/").GetComponent<AddScripts>();
		RectTransformHaoGanDu = GetGameObject().transform.Find("View/Bg/InfoBg/HaoGanDu/").GetComponent<RectTransform>();
		CanvasRendererHaoGanDu = GetGameObject().transform.Find("View/Bg/InfoBg/HaoGanDu/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIHaoGanDu = GetGameObject().transform.Find("View/Bg/InfoBg/HaoGanDu/").GetComponent<TextMeshProUGUI>();
		AddScriptsHaoGanDu = GetGameObject().transform.Find("View/Bg/InfoBg/HaoGanDu/").GetComponent<AddScripts>();
	}

	public TaskWindows(Transform trans)
		: base(trans)
	{
	}

	public TaskWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(CloseUi);
		ToggleUnSuc.onValueChanged.AddListener(delegate
		{
			OnValueChange(ToggleUnSuc);
		});
		ToggleSuc.onValueChanged.AddListener(delegate
		{
			OnValueChange(ToggleSuc);
		});
		ToggleOutOf.onValueChanged.AddListener(delegate
		{
			OnValueChange(ToggleOutOf);
		});
		Tool.HideAllChild(RectTransformContent);
		ToggleUnSuc.isOn = false;
		ToggleUnSuc.isOn = true;
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		OnValueChange(null);
		base.transform.SetParent(UiManager.GetTransform(new UiModeAttribute(Mode.Normal)));
	}

	public void InitNotBack()
	{
		base.transform.SetParent(UiManager.GetTransform(new UiModeAttribute(Mode.MainItem)));
		BTClose.SetActive(active: false);
	}

	private void OnValueChange(Toggle toggle)
	{
		if (_lastToggle == toggle)
		{
			return;
		}
		_lastToggle = toggle;
		TaskInfoInfoBg.InitNone();
		List<Xlsx_Task> source = Xlsx_Task_Query.data.Where((Xlsx_Task task) => task.NotShowTask != 1).ToList();
		if (ToggleUnSuc.isOn)
		{
			List<Xlsx_Task> xlsxTasks = (from task in source
				where SingletonAsMono<GameDataMrg>.Instance.GetTaskState(task.Key) == TaskType.Open && SingletonAsMono<GameDataMrg>.Instance.CurRound <= task.Round
				where !Tool.IsSucProperty(task.PropertyType, task.PropertyTypeValue)
				select task).ToList();
			Init(xlsxTasks, TaskType.Open);
		}
		else if (ToggleSuc.isOn)
		{
			List<Xlsx_Task> xlsxTasks2 = source.Where((Xlsx_Task task) => SingletonAsMono<GameDataMrg>.Instance.GetTaskState(task.Key) == TaskType.Suc || Tool.IsSucProperty(task.PropertyType, task.PropertyTypeValue)).ToList();
			Init(xlsxTasks2, TaskType.Suc);
		}
		else if (ToggleOutOf.isOn)
		{
			List<Xlsx_Task> xlsxTasks3 = (from task in source
				where SingletonAsMono<GameDataMrg>.Instance.GetTaskState(task.Key) == TaskType.Lose || (SingletonAsMono<GameDataMrg>.Instance.GetTaskState(task.Key) == TaskType.Open && SingletonAsMono<GameDataMrg>.Instance.CurRound > task.Round)
				where !Tool.IsSucProperty(task.PropertyType, task.PropertyTypeValue)
				select task).ToList();
			Init(xlsxTasks3, TaskType.Suc);
		}
	}

	public void Init(List<Xlsx_Task> xlsxTasks, TaskType taskType)
	{
		xlsxTasks.Sort((Xlsx_Task task, Xlsx_Task xlsxTask) => -task.Range.CompareTo(xlsxTask.Range));
		RectTransformContent.TranFor(xlsxTasks.Count, RectTransformContent.GetChild(0), delegate(int i, GameObject o)
		{
			o.GetComponent<TaskItem>().Init(xlsxTasks[i], taskType);
		});
	}

	public void InitTask(Xlsx_Task t, TaskType type = TaskType.Open)
	{
		SingletonAsMono<Mono>.Instance.Frame(delegate
		{
			switch (type)
			{
			case TaskType.Open:
				ToggleUnSuc.isOn = true;
				break;
			case TaskType.Suc:
				ToggleSuc.isOn = true;
				break;
			case TaskType.Lose:
				ToggleOutOf.isOn = true;
				break;
			}
			for (int i = 0; i < RectTransformContent.childCount; i++)
			{
				RectTransformContent.GetChild(i).GetComponent<TaskItem>().ShowSelect(t.Key);
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
