using UnityEngine;

namespace FrameWork;

[CreateAssetMenu(fileName = "ConfigData", menuName = "FrameWork/CreateConfigData")]
public class ConfigData : ScriptableObject
{
	public string versions = "1.0.0";

	public bool isAb = true;

	public string abClassPath = "Assets/FrameWork/Scripts/AssetAb";

	public string abEndName = "info";

	public string abAssetPath = "Assets/FrameWork/Asset";

	public string configName = "ABConfig.txt";

	public string key = "kljsdkkdlo4454GG00155sajuklmbkdl";

	public string[] dlls = new string[1] { "HotUpdate.dll" };

	public string downLoadUrl = "http://127.0.0.1:3000";

	public string[] spawnScriptUse = new string[3] { "System.Collections.Generic", "UnityEngine", "UnityEngine.UI" };

	public string spawnScriptPath = "Assets/FrameWork/Scripts/PrefabScript";

	public string resourcesPath = "Assets/Resources/Asset";

	public string serverIp = "127.0.0.1";

	public int serverPort = 8888;
}
