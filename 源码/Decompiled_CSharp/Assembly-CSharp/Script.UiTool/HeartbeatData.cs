using System;
using UnityEngine;

namespace Script.UiTool;

[Serializable]
public class HeartbeatData
{
	public RuntimeAnimatorController animatorController;

	public HeartbeatType heartbeatType;

	public string videoPath;

	private void VideoChange()
	{
	}
}
