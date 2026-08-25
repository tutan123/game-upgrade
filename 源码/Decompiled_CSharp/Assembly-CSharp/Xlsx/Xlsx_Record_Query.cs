using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_Record_Query
{
	public static List<Xlsx_Record> data;

	public static XlsxData<string, Xlsx_Record> XlsxDataAsOneKey;

	public static XlsxData<string, int, Xlsx_Record> XlsxDataAsTowKey;

	static Xlsx_Record_Query()
	{
		data = new List<Xlsx_Record>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_Record").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_Record xlsx_Record = new Xlsx_Record();
				Type type = xlsx_Record.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_Record, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_Record);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_Record>("Key", data);
	}
}
