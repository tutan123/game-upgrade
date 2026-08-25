using System.Collections.Generic;
using Script.UiTool;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;
using Xlsx;

[CreateNodeMenu("Video/ButtonNode")]
[NodeWidth(300)]
public class ButtonNode : Node
{
	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public ButtonNode button;

	public string buttonName;

	public Xlsx_Language_Key btnName;

	public Vector3 buttonPosition;

	public float buttonSize = 80f;

	public bool isJoinCombat;

	public bool isShowResetXdl;

	public bool isClickSureTips;

	public Xlsx_Language_Key btnSureTips;

	public bool isOpenUiBtn;

	public string uiName;

	public bool isClickExEvent;

	public List<EventData> eventDatas = new List<EventData>();

	public bool isHasUseProperty;

	public List<PropertyData> usePropertyDatas = new List<PropertyData>();

	public Xlsx_Language_Key usePropertyName;

	public bool isHasUsePropertyOr;

	[FormerlySerializedAs("usePropertyDatas")]
	public List<PropertyData> usePropertyDatasOr = new List<PropertyData>();

	public Xlsx_Language_Key usePropertyNameOr;

	public bool isSetProperty;

	public List<PropertyData> propertyDatas = new List<PropertyData>();

	public bool isCheckVideoExists;

	public List<VideoUnlockData> videoUnlockDatas;

	public bool btnIsNor;

	public Sprite btnSprite;

	public PropertyTypeValue btnType;

	protected override void Init()
	{
		base.Init();
	}

	public override object GetValue(NodePort port)
	{
		if (port.fieldName == "button")
		{
			return this;
		}
		return null;
	}
}
