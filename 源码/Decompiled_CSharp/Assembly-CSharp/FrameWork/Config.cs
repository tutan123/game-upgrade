using System;
using UnityEngine;

namespace FrameWork;

public static class Config
{
	private static ConfigData _configData;

	public static string DataPath;

	public static string DllPath;

	public static string DllCopyPath;

	public static string ServerIp => _configData.serverIp;

	public static int ServerPort => _configData.serverPort;

	public static bool IsAb => _configData.isAb;

	public static string ConfigName => _configData.configName;

	public static string Key => _configData.key;

	public static string[] Dlls => _configData.dlls;

	public static string DownLoadUrl => _configData.downLoadUrl;

	static Config()
	{
		DataPath = Application.dataPath.Replace("Assets", "");
		DllPath = DataPath + "\\HybridCLRData\\HotUpdateDlls\\";
		DllCopyPath = DataPath + "\\Assets\\FrameWork\\Asset\\Dll\\";
		_configData = Resources.Load<ConfigData>("ConfigData");
	}

	public static string GetAbPath()
	{
		string text = "";
		switch (Application.platform)
		{
		case RuntimePlatform.WindowsPlayer:
		case RuntimePlatform.WindowsEditor:
			text = "/StandaloneWindows64/";
			break;
		case RuntimePlatform.Android:
			text = "/Android/";
			break;
		case RuntimePlatform.IPhonePlayer:
			text = "/Ios/";
			break;
		case RuntimePlatform.WebGLPlayer:
			text = "/WebGl/";
			break;
		}
		return text + _configData.versions + "/";
	}

	public static long ConvertDateTimep(DateTime time)
	{
		return (time.ToUniversalTime().Ticks - 621355968000000000L) / 10000000;
	}
}
