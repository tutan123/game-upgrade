using FrameWork.Data;
using Xlsx;

namespace Script.Mrg;

public static class LanguageMrg
{
	public static string GetText(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return "";
		}
		switch (GameData.Language)
		{
		case Xlsx_Language_Type.Chinese:
			if (Xlsx_Language_Query.XlsxDataAsOneKey.ByKeyGetValue(key) != null)
			{
				return Xlsx_Language_Query.XlsxDataAsOneKey.ByKeyGetValue(key).Chinese.Replace("\\n", "\n").Replace("\\t", "\t");
			}
			return "";
		case Xlsx_Language_Type.English:
			if (Xlsx_Language_Query.XlsxDataAsOneKey.ByKeyGetValue(key) != null)
			{
				return Xlsx_Language_Query.XlsxDataAsOneKey.ByKeyGetValue(key).English.Replace("\\n", "\n").Replace("\\t", "\t");
			}
			return "";
		default:
			return Xlsx_Language_Query.XlsxDataAsOneKey.ByKeyGetValue(key).Chinese;
		}
	}

	public static string GetText(Xlsx_Language_Key key)
	{
		return GetText(key.ToString());
	}
}
