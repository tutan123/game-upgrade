using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_Item_Query
{
	public static List<Xlsx_Item> data;

	public static XlsxData<string, Xlsx_Item> XlsxDataAsOneKey;

	public static XlsxData<string, string, Xlsx_Item> XlsxDataAsTowKey;

	static Xlsx_Item_Query()
	{
		data = new List<Xlsx_Item>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_Item").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_Item xlsx_Item = new Xlsx_Item();
				Type type = xlsx_Item.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_Item, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_Item);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_Item>("Key", data);
	}
}
