using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_Language_Query
{
	public static List<Xlsx_Language> data;

	public static XlsxData<string, Xlsx_Language> XlsxDataAsOneKey;

	public static XlsxData<string, string, Xlsx_Language> XlsxDataAsTowKey;

	static Xlsx_Language_Query()
	{
		data = new List<Xlsx_Language>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_Language").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_Language xlsx_Language = new Xlsx_Language();
				Type type = xlsx_Language.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_Language, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_Language);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_Language>("Key", data);
	}
}
