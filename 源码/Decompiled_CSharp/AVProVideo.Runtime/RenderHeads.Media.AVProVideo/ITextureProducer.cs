using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public interface ITextureProducer
{
	int GetTextureCount();

	Texture GetTexture(int index = 0);

	int GetTextureFrameCount();

	bool SupportsTextureFrameCount();

	long GetTextureTimeStamp();

	float GetTexturePixelAspectRatio();

	bool RequiresVerticalFlip();

	StereoPacking GetTextureStereoPacking();

	TransparencyMode GetTextureTransparency();

	AlphaPacking GetTextureAlphaPacking();

	Matrix4x4 GetYpCbCrTransform();

	float[] GetAffineTransform();

	Matrix4x4 GetTextureMatrix();

	RenderTextureFormat GetCompatibleRenderTextureFormat(GetCompatibleRenderTextureFormatOptions options = GetCompatibleRenderTextureFormatOptions.Default, int plane = 0);
}
