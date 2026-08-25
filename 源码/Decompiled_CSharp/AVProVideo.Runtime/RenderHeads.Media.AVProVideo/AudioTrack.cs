namespace RenderHeads.Media.AVProVideo;

public class AudioTrack : TrackBase
{
	public int Bitrate { get; private set; }

	public int ChannelCount { get; private set; }

	private AudioTrack()
	{
	}

	internal AudioTrack(int uid, string name, string language, bool isDefault)
		: base(TrackType.Audio, uid, name, language, isDefault)
	{
	}
}
