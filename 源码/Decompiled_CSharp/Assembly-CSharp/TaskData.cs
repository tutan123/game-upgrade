using System;
using Script.Mrg;
using UnityEngine;
using Xlsx;

[Serializable]
public class TaskData
{
	public Xlsx_Task_Key xlsxTaskKey;

	public TaskType taskType;

	public float exTime;

	[HideInInspector]
	public bool isEx;
}
