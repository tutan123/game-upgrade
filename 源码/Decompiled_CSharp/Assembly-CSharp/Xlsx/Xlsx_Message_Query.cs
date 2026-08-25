using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_Message_Query
{
	public static List<Xlsx_Message> data;

	public static XlsxData<string, Xlsx_Message> XlsxDataAsOneKey;

	public static XlsxData<string, int, Xlsx_Message> XlsxDataAsTowKey;

	static Xlsx_Message_Query()
	{
		data = new List<Xlsx_Message>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_Message").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_Message xlsx_Message = new Xlsx_Message();
				Type type = xlsx_Message.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_Message, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_Message);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_Message>("Key", data);
	}
}
