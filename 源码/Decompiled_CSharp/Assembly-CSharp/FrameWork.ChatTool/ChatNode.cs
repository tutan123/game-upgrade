using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XNode;
using Xlsx;

namespace FrameWork.ChatTool;

[NodeWidth(300)]
[CreateNodeMenu("Chat/ChatNode")]
public class ChatNode : Node
{
	public string uniqueID;

	[Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
	public ChatNode input;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public ChatNode outPut;

	public MType mType;

	public Sprite icon;

	public bool isSelfMsg;

	public string msgTip = "备注";

	public Xlsx_Language_Key msg;

	public Sprite img;

	public Sprite fenMian;

	public string videoClipPath;

	public AudioPathData audioPath;

	[HideInInspector]
	public ChatNode selectNode;

	public bool isHasBtn;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public ChatNode selectBtnNode;

	public bool isBtnInfo;

	public string selectNameTip = "选项备注";

	public Xlsx_Language_Key selectName;

	public List<PropertyData> fistPropertyDatas = new List<PropertyData>();

	public Xlsx_Language_Key languageKey;

	public List<PropertyData> propertyDatas = new List<PropertyData>();

	public float delay = 1f;

	public bool isEnd;

	public List<TaskData> taskData;

	protected override void Init()
	{
		base.Init();
		UpdateIDInGraph();
	}

	private void UpdateIDInGraph()
	{
		bool flag = string.IsNullOrEmpty(uniqueID);
		if (!flag)
		{
			flag = graph.nodes.Any((Node n) => n != null && n != this && n is BaseNode baseNode && baseNode.uniqueID == uniqueID);
		}
		if (flag)
		{
			uniqueID = Guid.NewGuid().ToString();
		}
	}

	private void VideoChange()
	{
	}
}
