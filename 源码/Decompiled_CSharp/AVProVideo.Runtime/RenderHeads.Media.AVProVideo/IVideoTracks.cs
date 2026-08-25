namespace RenderHeads.Media.AVProVideo;

public interface IVideoTracks
{
	VideoTracks GetVideoTracks();

	VideoTrack GetActiveVideoTrack();

	void SetActiveVideoTrack(VideoTrack track);
}
