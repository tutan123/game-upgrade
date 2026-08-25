using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_Chat_Query
{
	public static List<Xlsx_Chat> data;

	public static XlsxData<string, Xlsx_Chat> XlsxDataAsOneKey;

	public static XlsxData<string, string, Xlsx_Chat> XlsxDataAsTowKey;

	static Xlsx_Chat_Query()
	{
		data = new List<Xlsx_Chat>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_Chat").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_Chat xlsx_Chat = new Xlsx_Chat();
				Type type = xlsx_Chat.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_Chat, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_Chat);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_Chat>("Key", data);
	}
}
