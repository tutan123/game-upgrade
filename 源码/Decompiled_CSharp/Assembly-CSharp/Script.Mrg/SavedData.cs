using System;
using System.Collections.Generic;

namespace Script.Mrg;

[Serializable]
public class SavedData
{
	public string Time;

	public string Key;

	public int Round = 1;

	public string SavedName = "存档";

	public bool isNew = true;

	public bool isStart;

	public bool is360 = true;

	public string event360Key;

	public Dictionary<string, string> Data = new Dictionary<string, string>();
}
