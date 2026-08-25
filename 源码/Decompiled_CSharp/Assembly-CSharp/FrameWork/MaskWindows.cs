using UnityEngine;

namespace FrameWork;

[UiMode(Mode.Popup, true)]
[ActorInfo("", "MaskWindows")]
public class MaskWindows : UiActor
{
	public override void Awake()
	{
		base.Awake();
	}

	public MaskWindows(Transform trans)
		: base(trans)
	{
	}

	public MaskWindows()
	{
	}

	protected override void Play()
	{
	}

	protected override void Pause()
	{
	}
}
