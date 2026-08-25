using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_WineList_Query
{
	public static List<Xlsx_WineList> data;

	public static XlsxData<string, Xlsx_WineList> XlsxDataAsOneKey;

	public static XlsxData<string, string, Xlsx_WineList> XlsxDataAsTowKey;

	static Xlsx_WineList_Query()
	{
		data = new List<Xlsx_WineList>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_WineList").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_WineList xlsx_WineList = new Xlsx_WineList();
				Type type = xlsx_WineList.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_WineList, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_WineList);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_WineList>("Key", data);
	}
}
