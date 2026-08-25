using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_Pos_Query
{
	public static List<Xlsx_Pos> data;

	public static XlsxData<int, Xlsx_Pos> XlsxDataAsOneKey;

	public static XlsxData<int, int, Xlsx_Pos> XlsxDataAsTowKey;

	static Xlsx_Pos_Query()
	{
		data = new List<Xlsx_Pos>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_Pos").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_Pos xlsx_Pos = new Xlsx_Pos();
				Type type = xlsx_Pos.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_Pos, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_Pos);
			}
		}
		XlsxDataAsOneKey = new XlsxData<int, Xlsx_Pos>("Key", data);
	}
}
