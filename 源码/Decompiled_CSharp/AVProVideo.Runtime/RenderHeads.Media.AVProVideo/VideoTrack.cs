namespace RenderHeads.Media.AVProVideo;

public class VideoTrack : TrackBase
{
	public int Bitrate { get; set; }

	private VideoTrack()
	{
	}

	internal VideoTrack(int uid, string name, string language, bool isDefault)
		: base(TrackType.Video, uid, name, language, isDefault)
	{
	}
}
