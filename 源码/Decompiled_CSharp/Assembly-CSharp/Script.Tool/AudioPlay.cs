using FrameWork;
using Script.Mrg;
using UnityEngine;

namespace Script.Tool;

public class AudioPlay : MonoBehaviour
{
	public void Play(AudioClip clip)
	{
		SingletonAsMono<AudioMrg>.Instance.Play(clip);
	}
}
