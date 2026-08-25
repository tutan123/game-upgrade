using System.Collections.Generic;
using UnityEngine;

namespace FrameWork;

[ActorInfo("", "WineListWindows")]
[UiMode(Mode.Normal, true)]
public class WineListWindows : UiActor
{
	public override void Awake()
	{
		base.Awake();
	}

	public WineListWindows(Transform trans)
		: base(trans)
	{
	}

	public WineListWindows()
	{
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}
}
