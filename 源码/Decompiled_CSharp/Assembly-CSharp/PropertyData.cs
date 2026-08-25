using System;
using UnityEngine;

[Serializable]
public class PropertyData
{
	public VideoIfType videoIfType;

	public PropertyType PropertyType;

	public string TypeName;

	public ItemType itemType;

	public PropertyTypeValue propertyTypeValue;

	public OtherType otherType;

	public int PropertyValue;

	public float addPropertyTime;

	public bool isShowTips = true;

	[HideInInspector]
	public bool isEx;

	public bool isRound;

	public bool isAlwaysEx = true;
}
