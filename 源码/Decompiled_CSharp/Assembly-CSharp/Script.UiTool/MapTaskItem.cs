using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.UiTool;

public class MapTaskItem : MonoBehaviour
{
	public TMP_Text taskName;

	public TMP_Text taskInfo;

	private Xlsx_Task _xlsxTask;

	public void Init(Xlsx_Task xlsxTask)
	{
		_xlsxTask = xlsxTask;
		string text = "";
		for (int i = 0; i < xlsxTask.PropertyTypeValueShow.Length; i++)
		{
			float num = xlsxTask.PropertyTypeValueShow[i];
			float num2 = SingletonAsMono<GameDataMrg>.Instance.GetProperty(xlsxTask.PropertyTypeShow[i], "Property", 0L);
			text = ((!(num > 0f)) ? (text + xlsxTask.PropertyTypeShow[i].ToEnum<PropertyTypeValue>().GetName() + ":" + num2) : (text + xlsxTask.PropertyTypeShow[i].ToEnum<PropertyTypeValue>().GetName() + ":" + (Mathf.Min(num2 / num, 1f) * 100f).ToInt() + "% "));
		}
		taskName.text = LanguageMrg.GetText(xlsxTask.Title);
		if (xlsxTask.IsShowSy == 1 && SingletonAsMono<GameDataMrg>.Instance.GetTaskState(xlsxTask.Key) == TaskType.Open)
		{
			taskName.text += string.Format(LanguageMrg.GetText("A6355"), xlsxTask.Round - SingletonAsMono<GameDataMrg>.Instance.CurRound);
		}
		taskInfo.SetActive(!string.IsNullOrEmpty(text));
		taskInfo.text = text;
	}

	public void ClickTask()
	{
		UiManager.OpenUi<MainWindows>().OpenTask().InitTask(_xlsxTask);
	}
}
