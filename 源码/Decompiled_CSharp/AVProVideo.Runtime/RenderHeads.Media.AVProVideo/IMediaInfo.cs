namespace RenderHeads.Media.AVProVideo;

public interface IMediaInfo
{
	double GetDuration();

	int GetDurationFrames(float overrideFrameRate = 0f);

	int GetMaxFrameNumber(float overrideFrameRate = 0f);

	int GetVideoWidth();

	int GetVideoHeight();

	float GetVideoFrameRate();

	float GetVideoDisplayRate();

	bool HasVideo();

	bool HasAudio();

	string GetPlayerDescription();

	bool PlayerSupportsLinearColorSpace();

	bool IsPlaybackStalled();

	float[] GetTextureTransform();

	long GetEstimatedTotalBandwidthUsed();

	bool IsExternalPlaybackSupported();

	bool GetDecoderPerformance(ref int activeDecodeThreadCount, ref int decodedFrameCount, ref int droppedFrameCount);

	PlaybackQualityStats GetPlaybackQualityStats();
}
