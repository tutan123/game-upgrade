using Script.Audio;
using Script.Mrg;
using UnityEngine;

namespace FrameWork;

[ActorInfo("", "BatVicWindwos")]
[UiMode(Mode.Normal, true)]
public class BatVicWindwos : UiActor
{
	public override void Awake()
	{
		base.Awake();
	}

	public BatVicWindwos(Transform trans)
		: base(trans)
	{
	}

	public BatVicWindwos()
	{
	}

	public override void Open(object[] objects)
	{
		Timer.DelayCall(3.5f, CloseUi);
		SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.combatSuc);
	}

	public override void CloseUi()
	{
		UiManager.RemoveUi(GetIndex());
	}
}
