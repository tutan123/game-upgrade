using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public class MediaCachingOptions
{
	public double minimumRequiredBitRate;

	public Vector2 minimumRequiredResolution;

	public double maximumRequiredBitRate;

	public Vector2 maximumRequiredResolution;

	public string title;

	public byte[] artwork;
}
