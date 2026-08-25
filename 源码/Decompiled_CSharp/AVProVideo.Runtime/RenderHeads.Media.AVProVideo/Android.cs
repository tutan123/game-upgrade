namespace RenderHeads.Media.AVProVideo;

public static class Android
{
	public enum VideoApi
	{
		MediaPlayer = 1,
		ExoPlayer
	}

	public enum VideoOutputMode
	{
		Texture
	}

	public enum AudioOutput
	{
		System,
		Unity,
		FacebookAudio360
	}

	public enum TextureFiltering
	{
		Point,
		Bilinear,
		Trilinear
	}

	public const int Default_MinBufferTimeMs = 10000;

	public const int Default_MaxBufferTimeMs = 50000;

	public const int Default_BufferForPlaybackMs = 1000;

	public const int Default_BufferForPlaybackAfterRebufferMs = 2000;
}
