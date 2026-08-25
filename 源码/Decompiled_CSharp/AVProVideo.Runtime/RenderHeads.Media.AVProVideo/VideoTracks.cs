namespace RenderHeads.Media.AVProVideo;

public class VideoTracks : TrackCollection<VideoTrack>
{
	public override TrackType TrackType => TrackType.Video;
}
