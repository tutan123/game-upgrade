using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_MiNiGameLevel_Query
{
	public static List<Xlsx_MiNiGameLevel> data;

	public static XlsxData<string, Xlsx_MiNiGameLevel> XlsxDataAsOneKey;

	public static XlsxData<string, string, Xlsx_MiNiGameLevel> XlsxDataAsTowKey;

	static Xlsx_MiNiGameLevel_Query()
	{
		data = new List<Xlsx_MiNiGameLevel>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_MiNiGameLevel").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_MiNiGameLevel xlsx_MiNiGameLevel = new Xlsx_MiNiGameLevel();
				Type type = xlsx_MiNiGameLevel.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_MiNiGameLevel, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_MiNiGameLevel);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_MiNiGameLevel>("Key", data);
	}
}
