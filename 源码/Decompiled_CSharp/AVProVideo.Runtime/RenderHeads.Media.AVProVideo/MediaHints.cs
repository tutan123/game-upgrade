using System;

namespace RenderHeads.Media.AVProVideo;

[Serializable]
public struct MediaHints
{
	public TransparencyMode transparency;

	public AlphaPacking alphaPacking;

	public StereoPacking stereoPacking;

	private static MediaHints defaultHints;

	public static MediaHints Default => defaultHints;
}
