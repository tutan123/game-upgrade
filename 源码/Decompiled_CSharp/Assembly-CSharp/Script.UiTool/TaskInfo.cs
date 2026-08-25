using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.UiTool;

public class TaskInfo : MonoBehaviour
{
	public TMP_Text eventName;

	public TMP_Text taskName;

	public TMP_Text taskDescription;

	public TMP_Text haoGaNdu;

	private Xlsx_Task _xlsxTask;

	public void Init(Xlsx_Task xlsxTask)
	{
		_xlsxTask = xlsxTask;
		eventName.text = LanguageMrg.GetText(xlsxTask.Title);
		taskName.text = LanguageMrg.GetText(xlsxTask.Title) + "     ";
		TaskType taskState = SingletonAsMono<GameDataMrg>.Instance.GetTaskState(xlsxTask.Key);
		if (xlsxTask.IsShowSy == 1 && xlsxTask.Round > 0 && xlsxTask.Round > SingletonAsMono<GameDataMrg>.Instance.CurRound && taskState == TaskType.Open)
		{
			taskName.text += string.Format(LanguageMrg.GetText("A525"), xlsxTask.Round - SingletonAsMono<GameDataMrg>.Instance.CurRound);
		}
		taskDescription.text = LanguageMrg.GetText(xlsxTask.Info);
		haoGaNdu.SetActive(active: false);
		haoGaNdu.SetActive(xlsxTask.PropertyTypeShow.Length != 0);
		haoGaNdu.text = "";
		for (int i = 0; i < xlsxTask.PropertyTypeShow.Length; i++)
		{
			float num = xlsxTask.PropertyTypeValueShow[i];
			float num2 = SingletonAsMono<GameDataMrg>.Instance.GetProperty(xlsxTask.PropertyTypeShow[i], "Property", 0L);
			if (num > 0f)
			{
				TMP_Text tMP_Text = haoGaNdu;
				tMP_Text.text = tMP_Text.text + xlsxTask.PropertyTypeShow[i].ToEnum<PropertyTypeValue>().GetName() + ":" + (Mathf.Min(num2 / num, 1f) * 100f).ToInt() + "% ";
			}
			else
			{
				TMP_Text tMP_Text2 = haoGaNdu;
				tMP_Text2.text = tMP_Text2.text + xlsxTask.PropertyTypeShow[i].ToEnum<PropertyTypeValue>().GetName() + ":" + num2;
			}
		}
	}

	public void InitNone()
	{
		eventName.text = "";
		taskName.text = "";
		taskDescription.text = "";
		haoGaNdu.text = "";
	}
}
