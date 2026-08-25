namespace RenderHeads.Media.AVProVideo;

public interface IAudioTracks
{
	AudioTracks GetAudioTracks();

	AudioTrack GetActiveAudioTrack();

	void SetActiveAudioTrack(AudioTrack track);
}
