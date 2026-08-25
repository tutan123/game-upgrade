using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_Achievement_Query
{
	public static List<Xlsx_Achievement> data;

	public static XlsxData<string, Xlsx_Achievement> XlsxDataAsOneKey;

	public static XlsxData<string, string, Xlsx_Achievement> XlsxDataAsTowKey;

	static Xlsx_Achievement_Query()
	{
		data = new List<Xlsx_Achievement>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_Achievement").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_Achievement xlsx_Achievement = new Xlsx_Achievement();
				Type type = xlsx_Achievement.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_Achievement, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_Achievement);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_Achievement>("Key", data);
	}
}
