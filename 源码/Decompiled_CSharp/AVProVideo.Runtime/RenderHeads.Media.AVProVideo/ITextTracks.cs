namespace RenderHeads.Media.AVProVideo;

public interface ITextTracks
{
	TextTracks GetTextTracks();

	TextTrack GetActiveTextTrack();

	void SetActiveTextTrack(TextTrack track);

	TextCue GetCurrentTextCue();

	int GetTextTrackArrayIndexFromUid(int Uid);
}
