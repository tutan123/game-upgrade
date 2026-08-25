using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_Task_Query
{
	public static List<Xlsx_Task> data;

	public static XlsxData<string, Xlsx_Task> XlsxDataAsOneKey;

	public static XlsxData<string, string, Xlsx_Task> XlsxDataAsTowKey;

	static Xlsx_Task_Query()
	{
		data = new List<Xlsx_Task>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_Task").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_Task xlsx_Task = new Xlsx_Task();
				Type type = xlsx_Task.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_Task, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_Task);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_Task>("Key", data);
	}
}
