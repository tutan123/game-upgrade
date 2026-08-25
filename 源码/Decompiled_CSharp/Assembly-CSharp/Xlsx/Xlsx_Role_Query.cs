using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_Role_Query
{
	public static List<Xlsx_Role> data;

	public static XlsxData<string, Xlsx_Role> XlsxDataAsOneKey;

	public static XlsxData<string, string, Xlsx_Role> XlsxDataAsTowKey;

	static Xlsx_Role_Query()
	{
		data = new List<Xlsx_Role>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_Role").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_Role xlsx_Role = new Xlsx_Role();
				Type type = xlsx_Role.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_Role, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_Role);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_Role>("Key", data);
	}
}
