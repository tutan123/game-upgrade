using System.Collections.Generic;
using Script.MiNiGame;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "LevelSelectWindows")]
public class LevelSelectWindows : UiActor
{
	public RectTransform RectTransformGroup;

	public GridLayoutGroup GridLayoutGroupGroup;

	public AddScripts AddScriptsGroup;

	public override void Awake()
	{
		base.Awake();
		RectTransformGroup = GetGameObject().transform.Find("View/Group/").GetComponent<RectTransform>();
		GridLayoutGroupGroup = GetGameObject().transform.Find("View/Group/").GetComponent<GridLayoutGroup>();
		AddScriptsGroup = GetGameObject().transform.Find("View/Group/").GetComponent<AddScripts>();
	}

	public LevelSelectWindows(Transform trans)
		: base(trans)
	{
	}

	public LevelSelectWindows()
	{
	}

	public override void Start()
	{
		base.Start();
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		List<Xlsx_MiNiGameLevel> data = Xlsx_MiNiGameLevel_Query.data;
		RectTransformGroup.TranFor(data.Count, RectTransformGroup.GetChild(0), delegate(int i, GameObject o)
		{
			o.GetComponent<LevelSelectItem>().Init(data[i].Key);
		});
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}
}
