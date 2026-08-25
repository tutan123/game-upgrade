using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LitJson;
using Script.Mrg;
using UnityEngine;
using UnityEngine.Rendering;
using Xlsx;

namespace FrameWork.Data;

public static class GameData
{
	private static Dictionary<string, string> _dataDic;

	private static string _path;

	private static string _fileName;

	public static bool IsLoad;

	public static Xlsx_Language_Type Language
	{
		get
		{
			if (!_dataDic.ContainsKey("Language"))
			{
				SystemLanguage systemLanguage = Application.systemLanguage;
				if (systemLanguage == SystemLanguage.Chinese || systemLanguage == SystemLanguage.ChineseSimplified || systemLanguage == SystemLanguage.ChineseTraditional)
				{
					return Xlsx_Language_Type.Chinese;
				}
				return Xlsx_Language_Type.English;
			}
			return Enum.Parse<Xlsx_Language_Type>(_dataDic["Language"]);
		}
		set
		{
			_dataDic["Language"] = value.ToString();
			Save();
		}
	}

	public static string CurKey
	{
		get
		{
			return GetValue("CurKey", "");
		}
		set
		{
			SetString("CurKey", value);
		}
	}

	public static bool IsFistJoinGame
	{
		get
		{
			return GetValue("IsFistJoinGame", true.ToString()).ToBool();
		}
		set
		{
			SetString("IsFistJoinGame", value.ToString());
		}
	}

	public static string CurDataKey => CurKey + "_Data";

	public static void Init()
	{
	}

	static GameData()
	{
		_dataDic = new Dictionary<string, string>();
		_fileName = "/data.dat";
		_path = Application.persistentDataPath + _fileName;
		if (!File.Exists(_path))
		{
			File.Create(_path).Close();
			return;
		}
		byte[] array = File.ReadAllBytes(_path);
		if (array.Length == 0)
		{
			return;
		}
		byte[] bytes = Tool.Decrypt(array, Tool.Key);
		string @string = Encoding.UTF8.GetString(bytes);
		try
		{
			_dataDic = JsonMapper.ToObject<Dictionary<string, string>>(@string);
		}
		catch (Exception ex)
		{
			MyLog.LogError(ex.Message);
		}
	}

	public static void SetString(string key, string value)
	{
		_dataDic[key] = value;
		Save();
	}

	public static void SetStringNotSave(string key, string value)
	{
		_dataDic[key] = value;
	}

	public static string GetValue(string key, string defaultValue)
	{
		if (!_dataDic.ContainsKey(key))
		{
			_dataDic.Add(key, defaultValue);
		}
		return _dataDic[key];
	}

	public static void RemoveValue(string key)
	{
		_dataDic.Remove(key);
	}

	public static string GetDataKey(string key)
	{
		return key + "_Data";
	}

	public static SavedData GetCurSavedData()
	{
		return JsonMapper.ToObject<SavedData>(GetValue(CurDataKey, "{}"));
	}

	public static void Save()
	{
		Debug.Log("保存存档");
		try
		{
			string s = JsonMapper.ToJson(_dataDic);
			byte[] bytes = Tool.Encrypt(Encoding.UTF8.GetBytes(s), Tool.Key);
			File.WriteAllBytes(_path, bytes);
		}
		catch (Exception ex)
		{
			MyLog.LogError(ex.Message);
		}
	}

	public static bool IsOpen(string key, bool def = true)
	{
		return GetValue("IsOpen_" + key, def.ToString()).ToBool();
	}

	public static void SetOpen(string key, bool open)
	{
		SetString("IsOpen_" + key, open.ToString());
	}

	public static void SetOpenAsNum(string key, float value)
	{
		SetString("OpenAsNum_" + key, value.ToString() ?? "");
	}

	public static float GetOpenAsNum(string key, float def = 1f)
	{
		if (key == "Sound")
		{
			def = 0.3f;
		}
		return GetValue("OpenAsNum_" + key, def.ToString() ?? "").ToFloat();
	}

	public static void SetDisPlay()
	{
		if (IsOpen("DisplayMode"))
		{
			ApplyScreenSettings(fullScreen: true);
		}
		else
		{
			ApplyScreenSettings(fullScreen: false);
		}
	}

	private static void ApplyScreenSettings(bool fullScreen)
	{
		float num = 1.7777778f;
		Resolution currentResolution = Screen.currentResolution;
		if (fullScreen)
		{
			int num2 = currentResolution.width;
			int num3 = Mathf.RoundToInt((float)num2 / num);
			if (num3 > currentResolution.height)
			{
				num3 = currentResolution.height;
				num2 = Mathf.RoundToInt((float)num3 * num);
			}
			Screen.SetResolution(num2, num3, FullScreenMode.FullScreenWindow);
			MyLog.Log($"全屏设置为: {num2}x{num3}");
			return;
		}
		float num4 = 0.75f;
		int num5 = Mathf.RoundToInt((float)currentResolution.height * num4);
		int num6 = Mathf.RoundToInt((float)num5 * num);
		if ((float)num6 > (float)currentResolution.width * num4)
		{
			num6 = Mathf.RoundToInt((float)currentResolution.width * num4);
			num5 = Mathf.RoundToInt((float)num6 / num);
		}
		Screen.SetResolution(num6, num5, FullScreenMode.Windowed);
		MyLog.Log($"窗口化设置为: {num6}x{num5}");
	}

	public static string GetQualityText()
	{
		return (int)GetOpenAsNum("Quality", 3f) switch
		{
			1 => LanguageMrg.GetText("A1701"), 
			2 => LanguageMrg.GetText("A1700"), 
			3 => LanguageMrg.GetText("A1699"), 
			_ => LanguageMrg.GetText("A1699"), 
		};
	}

	public static void SetQualityApply()
	{
		switch ((int)GetOpenAsNum("Quality", 3f))
		{
		case 1:
			SetVeryLow();
			break;
		case 2:
			SetMedium();
			break;
		case 3:
			SetUltra();
			break;
		default:
			SetUltra();
			break;
		}
		SetDisPlay();
	}

	public static void SetVeryLow()
	{
		QualitySettings.vSyncCount = 1;
		QualitySettings.SetQualityLevel(0, applyExpensiveChanges: false);
		QualitySettings.shadows = ShadowQuality.Disable;
		QualitySettings.shadowCascades = 0;
		QualitySettings.shadowDistance = 0f;
		QualitySettings.pixelLightCount = 0;
		QualitySettings.antiAliasing = 0;
		QualitySettings.lodBias = 100f;
		QualitySettings.maximumLODLevel = 0;
		QualitySettings.softParticles = false;
		QualitySettings.realtimeReflectionProbes = false;
		QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
		RenderSettings.fog = false;
		RenderSettings.ambientMode = AmbientMode.Flat;
		RenderSettings.reflectionIntensity = 0f;
	}

	public static void SetMedium()
	{
		QualitySettings.vSyncCount = 1;
		QualitySettings.SetQualityLevel(2, applyExpensiveChanges: false);
		QualitySettings.shadows = ShadowQuality.HardOnly;
		QualitySettings.shadowCascades = 1;
		QualitySettings.shadowDistance = 60f;
		QualitySettings.pixelLightCount = 1;
		QualitySettings.antiAliasing = 2;
		QualitySettings.lodBias = 100f;
		QualitySettings.maximumLODLevel = 0;
		QualitySettings.softParticles = false;
		QualitySettings.realtimeReflectionProbes = false;
		QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
	}

	public static void SetUltra()
	{
		QualitySettings.vSyncCount = 1;
		QualitySettings.SetQualityLevel(5, applyExpensiveChanges: false);
		QualitySettings.shadows = ShadowQuality.All;
		QualitySettings.shadowCascades = 2;
		QualitySettings.shadowDistance = 150f;
		QualitySettings.pixelLightCount = 4;
		QualitySettings.antiAliasing = 4;
		QualitySettings.lodBias = 100f;
		QualitySettings.maximumLODLevel = 0;
		QualitySettings.softParticles = true;
		QualitySettings.realtimeReflectionProbes = true;
		QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
	}
}
