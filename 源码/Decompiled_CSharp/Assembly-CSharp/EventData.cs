using System;
using FrameWork;
using UnityEngine;

[Serializable]
public class EventData
{
	public float evenTime;

	public VideoMessageType eventName;

	public string eventValue;

	[HideInInspector]
	public bool isEx;

	public bool isReturn;
}
