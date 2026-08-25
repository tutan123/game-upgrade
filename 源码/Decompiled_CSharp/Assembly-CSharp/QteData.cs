using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QteData
{
	public QteType qteType;

	public float qteStartTime;

	public float qteEndTime;

	public int clickCount;

	[HideInInspector]
	public bool isSpawnEx;

	public Vector2 qteLoc;

	public string qteName;

	public List<PropertyData> sucPropertyData = new List<PropertyData>();

	public List<PropertyData> losePropertyData = new List<PropertyData>();

	[HideInInspector]
	public List<PropertyData> sumProperty = new List<PropertyData>();

	public bool isExAdd = true;

	public bool isExCheck;

	[HideInInspector]
	public bool isSucEx;
}
