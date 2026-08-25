namespace RenderHeads.Media.AVProVideo;

public interface IMediaSubtitles
{
	bool LoadSubtitlesSRT(string data);

	int GetSubtitleIndex();

	string GetSubtitleText();
}
