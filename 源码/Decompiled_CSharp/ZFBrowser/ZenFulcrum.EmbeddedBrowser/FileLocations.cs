using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser;

public static class FileLocations
{
	public class CEFDirs
	{
		public string resourcesPath;

		public string binariesPath;

		public string localesPath;

		public string subprocessFile;

		public string logFile;

		public bool logFileIsUnityLog = true;
	}

	public const string SlaveExecutable = "ZFGameBrowser";

	private static CEFDirs _dirs;

	public static CEFDirs Dirs => _dirs ?? (_dirs = GetCEFDirs());

	private static string GetFolderName(string name)
	{
		return new char[13]
		{
			'~', '$', '%', '&', '*', '\\', '|', ':', '"', '/',
			'<', '>', '?'
		}.Aggregate(name, (string current, char ch) => current.Replace(ch, '_'));
	}

	private static CEFDirs GetCEFDirs()
	{
		string text = Application.dataPath + "/Plugins";
		bool logFileIsUnityLog = true;
		string logFile = Application.dataPath + "/output_log.txt";
		string text2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "Low/" + GetFolderName(Application.companyName) + "/" + GetFolderName(Application.productName);
		if (Directory.Exists(text2))
		{
			logFile = text2 + "/Player-browser.log";
			logFileIsUnityLog = false;
		}
		return new CEFDirs
		{
			resourcesPath = text,
			binariesPath = text,
			localesPath = text + "/locales",
			subprocessFile = text + "/ZFGameBrowser.exe",
			logFile = logFile,
			logFileIsUnityLog = logFileIsUnityLog
		};
	}
}
