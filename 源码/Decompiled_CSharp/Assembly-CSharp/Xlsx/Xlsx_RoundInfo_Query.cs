using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Xlsx;

public static class Xlsx_RoundInfo_Query
{
	public static List<Xlsx_RoundInfo> data;

	public static XlsxData<string, Xlsx_RoundInfo> XlsxDataAsOneKey;

	public static XlsxData<string, string, Xlsx_RoundInfo> XlsxDataAsTowKey;

	static Xlsx_RoundInfo_Query()
	{
		data = new List<Xlsx_RoundInfo>();
		string[] array = ABMrg.Load<TextAsset>("Xlsx_RoundInfo").text.Split('\n');
		string[] array2 = array[2].Split('|');
		string[] array3 = array[1].Split('|');
		for (int i = 3; i < array.Length; i++)
		{
			if (!string.IsNullOrEmpty(array[i]))
			{
				string[] array4 = array[i].Split('|');
				Xlsx_RoundInfo xlsx_RoundInfo = new Xlsx_RoundInfo();
				Type type = xlsx_RoundInfo.GetType();
				for (int j = 0; j < array4.Length; j++)
				{
					type.GetField(array2[j]).SetValue(xlsx_RoundInfo, Tool.ConversionType(array3[j], array4[j]));
				}
				data.Add(xlsx_RoundInfo);
			}
		}
		XlsxDataAsOneKey = new XlsxData<string, Xlsx_RoundInfo>("Key", data);
	}
}
