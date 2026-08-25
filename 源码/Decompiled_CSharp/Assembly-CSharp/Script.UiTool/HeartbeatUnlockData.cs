using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.UiTool;

[Serializable]
public class HeartbeatUnlockData
{
	public List<VideoUnlockData> videoDataOne;

	public List<VideoUnlockData> videoDataTwo;

	public GameObject showObj;
}
