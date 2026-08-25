using System.Collections.Generic;
using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.UiTool;

public class TaskItem : MonoBehaviour
{
	public TMP_Text taskName;

	public TMP_Text taskState;

	public TaskInfo taskInfo;

	public Image selectImg;

	private Xlsx_Task _xlsxTask;

	public void Init(Xlsx_Task xlsxTask, TaskType type, bool isShowSelect = false)
	{
		_xlsxTask = xlsxTask;
		taskName.text = LanguageMrg.GetText(xlsxTask.Title);
		if (xlsxTask.IsShowSy == 1 && xlsxTask.Round - SingletonAsMono<GameDataMrg>.Instance.CurRound <= 3 && xlsxTask.Round >= SingletonAsMono<GameDataMrg>.Instance.CurRound && type == TaskType.Open)
		{
			taskName.text += "  <sprite name=Time>";
		}
		switch (type)
		{
		case TaskType.Open:
			taskState.text = string.Format(LanguageMrg.GetText("A525"), xlsxTask.Round - SingletonAsMono<GameDataMrg>.Instance.CurRound);
			if (xlsxTask.Range < SingletonAsMono<GameDataMrg>.Instance.CurRound)
			{
				taskState.text = string.Format(LanguageMrg.GetText("A532"));
			}
			break;
		case TaskType.Suc:
			taskState.text = string.Format(LanguageMrg.GetText("A531"));
			break;
		case TaskType.Lose:
			taskState.text = string.Format(LanguageMrg.GetText("A532"));
			break;
		}
		selectImg.SetActive(isShowSelect);
		if (isShowSelect)
		{
			taskInfo.Init(_xlsxTask);
		}
	}

	public void ShowSelect(string key)
	{
		selectImg.SetActive(key == _xlsxTask.Key);
		if (key == _xlsxTask.Key)
		{
			taskInfo.Init(_xlsxTask);
		}
	}

	public bool IsHas(Xlsx_Task xlsxTask)
	{
		return _xlsxTask.Key == xlsxTask.Key;
	}

	private void OnEnable()
	{
		EventManager.AddListener(MessageType.Game, GameMessageType.ClickTaskItem, CheckSelect);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.ClickTaskItem, CheckSelect);
	}

	private void CheckSelect(List<object> objects)
	{
		string text = objects[0].ToString();
		selectImg.SetActive(_xlsxTask.Key == text);
	}

	public void OnClick()
	{
		List<object> eventMsg = EventManager.GetEventMsg();
		eventMsg.Add(_xlsxTask.Key);
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.ClickTaskItem, eventMsg);
		taskInfo.Init(_xlsxTask);
	}
}
