using System;
using UnityEngine;
using Xlsx;

[Serializable]
public class AchievementData
{
	public Xlsx_Achievement_Key achievement;

	public float exTime;

	[HideInInspector]
	public bool IsEx;
}
