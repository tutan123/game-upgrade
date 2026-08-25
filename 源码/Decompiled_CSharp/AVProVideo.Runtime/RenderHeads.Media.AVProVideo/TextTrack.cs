namespace RenderHeads.Media.AVProVideo;

public class TextTrack : TrackBase
{
	private TextTrack()
	{
	}

	internal TextTrack(int uid, string name, string language, bool isDefault)
		: base(TrackType.Text, uid, name, language, isDefault)
	{
	}
}
