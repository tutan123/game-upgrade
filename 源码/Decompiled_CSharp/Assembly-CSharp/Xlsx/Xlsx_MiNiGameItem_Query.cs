using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_MiNiGameItem_Query
{
	public static List<Xlsx_MiNiGameItem> data;

	public static XlsxData<string, Xlsx_MiNiGameItem> XlsxDataAsOneKey;

	public static XlsxData<string, int, Xlsx_MiNiGameItem> XlsxDataAsTowKey;

	static Xlsx_MiNiGameItem_Query()
	{
		data = new List<Xlsx_MiNiGameItem>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_MiNiGameItem").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_MiNiGameItem xlsx_MiNiGameItem = new Xlsx_MiNiGameItem();
				Type type = xlsx_MiNiGameItem.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_MiNiGameItem, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_MiNiGameItem);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_MiNiGameItem>("Key", data);
	}
}
