using System;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

[Serializable]
public struct VideoResolveOptions
{
	public enum AspectRatio
	{
		NoScaling,
		FitVertically,
		FitHorizontally,
		FitInside,
		FitOutside,
		Stretch
	}

	[SerializeField]
	public bool applyHSBC;

	[SerializeField]
	[Range(0f, 1f)]
	public float hue;

	[Range(0f, 1f)]
	[SerializeField]
	public float saturation;

	[SerializeField]
	[Range(0f, 1f)]
	public float brightness;

	[Range(0f, 1f)]
	[SerializeField]
	public float contrast;

	[SerializeField]
	[Range(0.0001f, 10f)]
	public float gamma;

	[SerializeField]
	public Color tint;

	[SerializeField]
	public bool generateMipmaps;

	[SerializeField]
	public AspectRatio aspectRatio;

	public bool IsColourAdjust()
	{
		if (applyHSBC)
		{
			if (hue == 0f && saturation == 0.5f && brightness == 0.5f && contrast == 0.5f)
			{
				return gamma != 1f;
			}
			return true;
		}
		return false;
	}

	internal void ResetColourAdjust()
	{
		hue = 0f;
		saturation = 0.5f;
		brightness = 0.5f;
		contrast = 0.5f;
		gamma = 1f;
	}

	public static VideoResolveOptions Create()
	{
		VideoResolveOptions videoResolveOptions = default(VideoResolveOptions);
		videoResolveOptions.tint = Color.white;
		videoResolveOptions.aspectRatio = AspectRatio.Stretch;
		VideoResolveOptions result = videoResolveOptions;
		result.ResetColourAdjust();
		return result;
	}
}
