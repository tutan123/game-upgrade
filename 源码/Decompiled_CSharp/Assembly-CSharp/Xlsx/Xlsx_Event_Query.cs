using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_Event_Query
{
	public static List<Xlsx_Event> data;

	public static XlsxData<string, Xlsx_Event> XlsxDataAsOneKey;

	public static XlsxData<string, int, Xlsx_Event> XlsxDataAsTowKey;

	static Xlsx_Event_Query()
	{
		data = new List<Xlsx_Event>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_Event").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_Event xlsx_Event = new Xlsx_Event();
				Type type = xlsx_Event.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_Event, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_Event);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_Event>("Key", data);
	}
}
