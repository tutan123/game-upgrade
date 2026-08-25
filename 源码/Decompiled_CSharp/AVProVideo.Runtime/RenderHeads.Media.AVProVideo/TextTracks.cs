namespace RenderHeads.Media.AVProVideo;

public class TextTracks : TrackCollection<TextTrack>
{
	public override TrackType TrackType => TrackType.Text;
}
