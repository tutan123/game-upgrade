using System.Collections.Generic;
using Script.Mrg;
using Script.Scene;
using UnityEngine;

namespace FrameWork;

[ActorInfo("", "BgmWindows")]
[UiMode(Mode.Background, true)]
public class BgmWindows : UiActor
{
	private AudioSource _audioSource;

	public override void Awake()
	{
		base.Awake();
	}

	public BgmWindows(Transform trans)
		: base(trans)
	{
	}

	public BgmWindows()
	{
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		VideoNode node = GameScene.Instance.videoItem.GetNode();
		if ((bool)node.bgmWinClip)
		{
			_audioSource = SingletonAsMono<AudioMrg>.Instance.Play(node.bgmWinClip, node.isLoopBgm);
		}
	}

	public override void CloseUi()
	{
		if ((bool)_audioSource)
		{
			SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
			_audioSource = null;
		}
		base.CloseUi();
	}

	public override void OnDisable()
	{
		base.OnDisable();
		if ((bool)_audioSource)
		{
			SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
			_audioSource = null;
		}
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			if ((bool)_audioSource)
			{
				SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
				_audioSource = null;
			}
			RemoveUi(GetIndex());
		}
	}

	protected override void Pause()
	{
	}

	protected override void Play()
	{
	}
}
