using FrameWork;

namespace Script.Audio;

public static class AudioDataClip
{
	public static AudioData AudioData;

	static AudioDataClip()
	{
		AudioData = ABMrg.Load<AudioData>("AudioData");
	}
}
