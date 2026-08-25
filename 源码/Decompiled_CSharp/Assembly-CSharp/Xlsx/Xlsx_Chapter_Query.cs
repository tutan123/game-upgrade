using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_Chapter_Query
{
	public static List<Xlsx_Chapter> data;

	public static XlsxData<string, Xlsx_Chapter> XlsxDataAsOneKey;

	public static XlsxData<string, int, Xlsx_Chapter> XlsxDataAsTowKey;

	static Xlsx_Chapter_Query()
	{
		data = new List<Xlsx_Chapter>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_Chapter").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_Chapter xlsx_Chapter = new Xlsx_Chapter();
				Type type = xlsx_Chapter.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_Chapter, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_Chapter);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_Chapter>("Key", data);
	}
}
