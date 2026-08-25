using System;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public class VideoRender
{
	[Flags]
	public enum ResolveFlags
	{
		Mipmaps = 1,
		PackedAlpha = 2,
		StereoLeft = 4,
		StereoRight = 8,
		ColorspaceSRGB = 0x10
	}

	public const string Shader_IMGUI = "AVProVideo/Internal/IMGUI/Texture Transparent";

	public const string Shader_Resolve = "AVProVideo/Internal/Resolve";

	public const string Shader_ResolveOES = "AVProVideo/Internal/ResolveOES";

	public const string Shader_Preview = "AVProVideo/Internal/Preview";

	public const string Keyword_AlphaPackTopBottom = "ALPHAPACK_TOP_BOTTOM";

	public const string Keyword_AlphaPackLeftRight = "ALPHAPACK_LEFT_RIGHT";

	public const string Keyword_AlphaPackNone = "ALPHAPACK_NONE";

	public const string Keyword_StereoTopBottom = "STEREO_TOP_BOTTOM";

	public const string Keyword_StereoLeftRight = "STEREO_LEFT_RIGHT";

	public const string Keyword_StereoCustomUV = "STEREO_CUSTOM_UV";

	public const string Keyword_StereoTwoTextures = "STEREO_TWO_TEXTURES";

	public const string Keyword_StereoNone = "MONOSCOPIC";

	public const string Keyword_StereoDebug = "STEREO_DEBUG";

	public const string Keyword_LayoutEquirect180 = "LAYOUT_EQUIRECT180";

	public const string Keyword_LayoutNone = "LAYOUT_NONE";

	public const string Keyword_ForceEyeNone = "FORCEEYE_NONE";

	public const string Keyword_ForceEyeLeft = "FORCEEYE_LEFT";

	public const string Keyword_ForceEyeRight = "FORCEEYE_RIGHT";

	public const string Keyword_ApplyGamma = "APPLY_GAMMA";

	public static readonly LazyShaderProperty PropChromaTex = new LazyShaderProperty("_ChromaTex");

	public static readonly LazyShaderProperty PropMainTex_R = new LazyShaderProperty("_MainTex_R");

	public static readonly LazyShaderProperty PropChromaTex_R = new LazyShaderProperty("_ChromaTex_R");

	public static readonly LazyShaderProperty PropVertScale = new LazyShaderProperty("_VertScale");

	public static readonly LazyShaderProperty PropApplyGamma = new LazyShaderProperty("_ApplyGamma");

	public static readonly LazyShaderProperty PropStereo = new LazyShaderProperty("Stereo");

	public static readonly LazyShaderProperty PropAlphaPack = new LazyShaderProperty("AlphaPack");

	public static readonly LazyShaderProperty PropLayout = new LazyShaderProperty("Layout");

	public static readonly LazyShaderProperty PropViewMatrix = new LazyShaderProperty("_ViewMatrix");

	public static readonly LazyShaderProperty PropTextureMatrix = new LazyShaderProperty("_MainTex_Xfrm");

	public static string Keyword_UseHSBC = "USE_HSBC";

	public static readonly LazyShaderProperty PropHue = new LazyShaderProperty("_Hue");

	public static readonly LazyShaderProperty PropSaturation = new LazyShaderProperty("_Saturation");

	public static readonly LazyShaderProperty PropContrast = new LazyShaderProperty("_Contrast");

	public static readonly LazyShaderProperty PropBrightness = new LazyShaderProperty("_Brightness");

	public static readonly LazyShaderProperty PropInvGamma = new LazyShaderProperty("_InvGamma");

	public static Material CreateResolveMaterial(bool usingAndroidOES)
	{
		return new Material(Shader.Find(usingAndroidOES ? "AVProVideo/Internal/ResolveOES" : "AVProVideo/Internal/Resolve"));
	}

	public static Material CreateIMGUIMaterial()
	{
		return new Material(Shader.Find("AVProVideo/Internal/Preview"));
	}

	public static void SetupLayoutMaterial(Material material, VideoMapping mapping)
	{
		if (mapping != VideoMapping.EquiRectangular180)
		{
			material.DisableKeyword("LAYOUT_EQUIRECT180");
			material.EnableKeyword("LAYOUT_NONE");
		}
		else
		{
			material.DisableKeyword("LAYOUT_NONE");
			material.EnableKeyword("LAYOUT_EQUIRECT180");
		}
	}

	public static void SetupStereoEyeModeMaterial(Material material, StereoEye mode)
	{
		switch (mode)
		{
		case StereoEye.Both:
			material.DisableKeyword("FORCEEYE_LEFT");
			material.DisableKeyword("FORCEEYE_RIGHT");
			material.EnableKeyword("FORCEEYE_NONE");
			break;
		case StereoEye.Left:
			material.DisableKeyword("FORCEEYE_NONE");
			material.DisableKeyword("FORCEEYE_RIGHT");
			material.EnableKeyword("FORCEEYE_LEFT");
			break;
		case StereoEye.Right:
			material.DisableKeyword("FORCEEYE_NONE");
			material.DisableKeyword("FORCEEYE_LEFT");
			material.EnableKeyword("FORCEEYE_RIGHT");
			break;
		}
	}

	public static void SetupStereoMaterial(Material material, StereoPacking packing)
	{
		switch (packing)
		{
		case StereoPacking.Monoscopic:
			material.DisableKeyword("STEREO_TOP_BOTTOM");
			material.DisableKeyword("STEREO_LEFT_RIGHT");
			material.DisableKeyword("STEREO_CUSTOM_UV");
			material.DisableKeyword("STEREO_TWO_TEXTURES");
			material.EnableKeyword("MONOSCOPIC");
			break;
		case StereoPacking.TopBottom:
			material.DisableKeyword("MONOSCOPIC");
			material.DisableKeyword("STEREO_LEFT_RIGHT");
			material.DisableKeyword("STEREO_CUSTOM_UV");
			material.DisableKeyword("STEREO_TWO_TEXTURES");
			material.EnableKeyword("STEREO_TOP_BOTTOM");
			break;
		case StereoPacking.LeftRight:
			material.DisableKeyword("MONOSCOPIC");
			material.DisableKeyword("STEREO_TOP_BOTTOM");
			material.DisableKeyword("STEREO_TWO_TEXTURES");
			material.DisableKeyword("STEREO_CUSTOM_UV");
			material.EnableKeyword("STEREO_LEFT_RIGHT");
			break;
		case StereoPacking.CustomUV:
			material.DisableKeyword("MONOSCOPIC");
			material.DisableKeyword("STEREO_TOP_BOTTOM");
			material.DisableKeyword("STEREO_LEFT_RIGHT");
			material.DisableKeyword("STEREO_TWO_TEXTURES");
			material.EnableKeyword("STEREO_CUSTOM_UV");
			break;
		case StereoPacking.MultiviewLeftPrimary:
		case StereoPacking.MultiviewRightPrimary:
			material.DisableKeyword("MONOSCOPIC");
			material.DisableKeyword("STEREO_TOP_BOTTOM");
			material.DisableKeyword("STEREO_LEFT_RIGHT");
			material.DisableKeyword("STEREO_CUSTOM_UV");
			material.EnableKeyword("STEREO_TWO_TEXTURES");
			break;
		case StereoPacking.RightLeft:
			break;
		}
	}

	public static void SetupGlobalDebugStereoTinting(bool enabled)
	{
		if (enabled)
		{
			Shader.EnableKeyword("STEREO_DEBUG");
		}
		else
		{
			Shader.DisableKeyword("STEREO_DEBUG");
		}
	}

	public static void SetupAlphaPackedMaterial(Material material, AlphaPacking packing)
	{
		switch (packing)
		{
		case AlphaPacking.None:
			material.DisableKeyword("ALPHAPACK_TOP_BOTTOM");
			material.DisableKeyword("ALPHAPACK_LEFT_RIGHT");
			material.EnableKeyword("ALPHAPACK_NONE");
			break;
		case AlphaPacking.TopBottom:
			material.DisableKeyword("ALPHAPACK_NONE");
			material.DisableKeyword("ALPHAPACK_LEFT_RIGHT");
			material.EnableKeyword("ALPHAPACK_TOP_BOTTOM");
			break;
		case AlphaPacking.LeftRight:
			material.DisableKeyword("ALPHAPACK_NONE");
			material.DisableKeyword("ALPHAPACK_TOP_BOTTOM");
			material.EnableKeyword("ALPHAPACK_LEFT_RIGHT");
			break;
		}
	}

	public static void SetupGammaMaterial(Material material, bool playerSupportsLinear)
	{
		if (QualitySettings.activeColorSpace == ColorSpace.Linear && !playerSupportsLinear)
		{
			material.EnableKeyword("APPLY_GAMMA");
		}
		else
		{
			material.DisableKeyword("APPLY_GAMMA");
		}
	}

	public static void SetupTextureMatrix(Material material, float[] transform)
	{
		if (!(material == null))
		{
			if (transform == null || transform.Length != 6)
			{
				transform = new float[6] { 1f, 0f, 0f, 1f, 0f, 0f };
			}
			Vector4 column = new Vector4(transform[0], transform[1], 0f, 0f);
			Vector4 column2 = new Vector4(transform[2], transform[3], 0f, 0f);
			Vector4 column3 = new Vector4(0f, 0f, 1f, 0f);
			material.SetMatrix(value: new Matrix4x4(column, column2, column3, new Vector4(transform[4], transform[5], 0f, 1f)), nameID: PropTextureMatrix.Id);
		}
	}

	public static void SetupTextureMatrix(Material material, Matrix4x4 transform)
	{
		if (!(material == null))
		{
			material.SetMatrix(PropTextureMatrix.Id, transform);
		}
	}

	public static void SetupVerticalFlipMaterial(Material material, bool flip)
	{
		material.SetFloat(PropVertScale.Id, flip ? (-1f) : 1f);
	}

	public static Texture GetTexture(MediaPlayer mediaPlayer, int textureIndex)
	{
		Texture result = null;
		if (mediaPlayer != null)
		{
			if (mediaPlayer.UseResampler && mediaPlayer.FrameResampler != null && mediaPlayer.FrameResampler.OutputTexture != null)
			{
				if (mediaPlayer.FrameResampler.OutputTexture.Length > textureIndex)
				{
					result = mediaPlayer.FrameResampler.OutputTexture[textureIndex];
				}
			}
			else if (mediaPlayer.TextureProducer != null && mediaPlayer.TextureProducer.GetTextureCount() > textureIndex)
			{
				result = mediaPlayer.TextureProducer.GetTexture(textureIndex);
			}
		}
		return result;
	}

	public static void SetupMaterialForMedia(Material material, MediaPlayer mediaPlayer, int texturePropId = -1, Texture fallbackTexture = null, bool forceFallbackTexture = false)
	{
		if (mediaPlayer != null)
		{
			Texture texture = GetTexture(mediaPlayer, 0);
			Matrix4x4 textureTransform = Matrix4x4.identity;
			bool flag = mediaPlayer.IsUsingYCbCr();
			Texture ycbcrTexture = (flag ? GetTexture(mediaPlayer, 1) : null);
			Matrix4x4 ycbcrTransform = Matrix4x4.identity;
			StereoPacking stereoPacking = StereoPacking.Monoscopic;
			AlphaPacking alphaPacking = AlphaPacking.None;
			bool flipVertically = false;
			bool playerSupportsLinear = false;
			if (texturePropId != -1)
			{
				if (texture == null || forceFallbackTexture)
				{
					texture = fallbackTexture;
				}
				material.SetTexture(texturePropId, texture);
			}
			ITextureProducer textureProducer = mediaPlayer.TextureProducer;
			if (textureProducer != null)
			{
				flipVertically = textureProducer.RequiresVerticalFlip();
				if (flag)
				{
					ycbcrTransform = textureProducer.GetYpCbCrTransform();
				}
				stereoPacking = textureProducer.GetTextureStereoPacking();
				alphaPacking = textureProducer.GetTextureAlphaPacking();
				textureTransform = textureProducer.GetTextureMatrix();
			}
			if (mediaPlayer.Info != null)
			{
				playerSupportsLinear = mediaPlayer.Info.PlayerSupportsLinearColorSpace();
			}
			SetupMaterial(material, flipVertically, playerSupportsLinear, ycbcrTransform, ycbcrTexture, textureTransform, mediaPlayer.VideoLayoutMapping, stereoPacking, alphaPacking);
			if (stereoPacking == StereoPacking.MultiviewLeftPrimary || stereoPacking == StereoPacking.MultiviewRightPrimary)
			{
				material.SetTexture(PropMainTex_R.Id, GetTexture(mediaPlayer, 1));
			}
		}
		else
		{
			if (texturePropId != -1)
			{
				material.SetTexture(texturePropId, fallbackTexture);
			}
			SetupMaterial(material, flipVertically: false, playerSupportsLinear: true, Matrix4x4.identity, null, Matrix4x4.identity);
		}
	}

	internal static void SetupMaterial(Material material, bool flipVertically, bool playerSupportsLinear, Matrix4x4 ycbcrTransform, Texture ycbcrTexture, Matrix4x4 textureTransform, VideoMapping mapping = VideoMapping.Normal, StereoPacking stereoPacking = StereoPacking.Monoscopic, AlphaPacking alphaPacking = AlphaPacking.None)
	{
		SetupVerticalFlipMaterial(material, flipVertically);
		if (material.HasProperty(PropLayout.Id))
		{
			SetupLayoutMaterial(material, mapping);
		}
		if (material.HasProperty(PropStereo.Id))
		{
			SetupStereoMaterial(material, stereoPacking);
		}
		if (material.HasProperty(PropAlphaPack.Id))
		{
			SetupAlphaPackedMaterial(material, alphaPacking);
		}
		if (material.HasProperty(PropApplyGamma.Id))
		{
			SetupGammaMaterial(material, playerSupportsLinear);
		}
		SetupTextureMatrix(material, textureTransform);
	}

	public static void SetupResolveMaterial(Material material, VideoResolveOptions options)
	{
		if (options.IsColourAdjust())
		{
			material.EnableKeyword(Keyword_UseHSBC);
			material.SetFloat(PropHue.Id, options.hue);
			material.SetFloat(PropSaturation.Id, options.saturation);
			material.SetFloat(PropBrightness.Id, options.brightness);
			material.SetFloat(PropContrast.Id, options.contrast);
			material.SetFloat(PropInvGamma.Id, 1f / options.gamma);
		}
		else
		{
			material.DisableKeyword(Keyword_UseHSBC);
		}
		material.color = options.tint;
	}

	public static RenderTexture ResolveVideoToRenderTexture(Material resolveMaterial, RenderTexture targetTexture, ITextureProducer texture, ResolveFlags flags, ScaleMode scaleMode = ScaleMode.StretchToFill)
	{
		int width = texture.GetTexture().width;
		int height = texture.GetTexture().height;
		StereoEye eyeMode = StereoEye.Both;
		if ((flags & ResolveFlags.StereoLeft) == ResolveFlags.StereoLeft && (flags & ResolveFlags.StereoRight) != ResolveFlags.StereoRight)
		{
			eyeMode = StereoEye.Left;
		}
		else if ((flags & ResolveFlags.StereoLeft) != ResolveFlags.StereoLeft && (flags & ResolveFlags.StereoRight) == ResolveFlags.StereoRight)
		{
			eyeMode = StereoEye.Right;
		}
		float pixelAspectRatio = 1f;
		GetResolveTextureSize(texture.GetTextureAlphaPacking(), texture.GetTextureStereoPacking(), eyeMode, pixelAspectRatio, texture.GetTextureMatrix(), ref width, ref height);
		if ((bool)targetTexture && (targetTexture.width != width || targetTexture.height != height))
		{
			RenderTexture.ReleaseTemporary(targetTexture);
			targetTexture = null;
		}
		if (!targetTexture)
		{
			GetCompatibleRenderTextureFormatOptions getCompatibleRenderTextureFormatOptions = GetCompatibleRenderTextureFormatOptions.ForResolve;
			if (texture.GetTextureAlphaPacking() != 0)
			{
				getCompatibleRenderTextureFormatOptions |= GetCompatibleRenderTextureFormatOptions.RequiresAlpha;
			}
			RenderTextureFormat compatibleRenderTextureFormat = texture.GetCompatibleRenderTextureFormat(getCompatibleRenderTextureFormatOptions);
			RenderTextureReadWrite readWrite = (((flags & ResolveFlags.ColorspaceSRGB) != ResolveFlags.ColorspaceSRGB) ? RenderTextureReadWrite.Linear : RenderTextureReadWrite.sRGB);
			targetTexture = RenderTexture.GetTemporary(width, height, 0, compatibleRenderTextureFormat, readWrite);
		}
		bool flag = (flags & ResolveFlags.Mipmaps) == ResolveFlags.Mipmaps;
		if (targetTexture.IsCreated() && targetTexture.useMipMap != flag)
		{
			targetTexture.Release();
		}
		if (!targetTexture.IsCreated())
		{
			RenderTexture renderTexture = targetTexture;
			bool useMipMap = (targetTexture.autoGenerateMips = flag);
			renderTexture.useMipMap = useMipMap;
			targetTexture.Create();
		}
		bool sRGBWrite = GL.sRGBWrite;
		GL.sRGBWrite = targetTexture.sRGB;
		RenderTexture active = RenderTexture.active;
		if (scaleMode == ScaleMode.StretchToFill)
		{
			Graphics.Blit(texture.GetTexture(), targetTexture, resolveMaterial);
		}
		else
		{
			RenderTexture.active = targetTexture;
			if (scaleMode == ScaleMode.ScaleToFit)
			{
				GL.Clear(clearDepth: false, clearColor: true, Color.black);
			}
			DrawTexture(new Rect(0f, 0f, targetTexture.width, targetTexture.height), texture.GetTexture(), scaleMode, texture.GetTextureAlphaPacking(), texture.GetTexturePixelAspectRatio(), resolveMaterial);
		}
		RenderTexture.active = active;
		GL.sRGBWrite = sRGBWrite;
		return targetTexture;
	}

	public static void GetResolveTextureSize(AlphaPacking alphaPacking, StereoPacking stereoPacking, StereoEye eyeMode, float pixelAspectRatio, Matrix4x4 textureXfrm, ref int width, ref int height)
	{
		Vector4 vector = new Vector4(width, height, 0f, 0f);
		vector = textureXfrm * vector;
		width = (int)Mathf.Abs(vector.x);
		height = (int)Mathf.Abs(vector.y);
		switch (alphaPacking)
		{
		case AlphaPacking.LeftRight:
			width /= 2;
			break;
		case AlphaPacking.TopBottom:
			height /= 2;
			break;
		}
		if (eyeMode != 0)
		{
			switch (stereoPacking)
			{
			case StereoPacking.LeftRight:
				width /= 2;
				break;
			case StereoPacking.TopBottom:
				height /= 2;
				break;
			}
		}
		if (pixelAspectRatio > 0f)
		{
			if (pixelAspectRatio > 1f)
			{
				width = Mathf.RoundToInt((float)width * pixelAspectRatio);
			}
			else if (pixelAspectRatio < 1f)
			{
				height = Mathf.RoundToInt((float)height / pixelAspectRatio);
			}
		}
	}

	public static bool RequiresResolve(ITextureProducer texture)
	{
		if (texture.GetTextureAlphaPacking() == AlphaPacking.None && !texture.RequiresVerticalFlip() && texture.GetTextureStereoPacking() == StereoPacking.Monoscopic)
		{
			return texture.GetTextureCount() > 1;
		}
		return true;
	}

	public static void DrawTexture(Rect destRect, Texture texture, ScaleMode scaleMode, AlphaPacking alphaPacking, float pixelAspectRatio, Material material)
	{
		if (Event.current != null && Event.current.type != EventType.Repaint)
		{
			return;
		}
		int width = texture.width;
		int height = texture.height;
		Matrix4x4 identity = Matrix4x4.identity;
		GetResolveTextureSize(alphaPacking, StereoPacking.Unknown, StereoEye.Both, pixelAspectRatio, identity, ref width, ref height);
		float num = (float)width / (float)height;
		Rect sourceRect = new Rect(0f, 0f, 1f, 1f);
		switch (scaleMode)
		{
		case ScaleMode.ScaleAndCrop:
		{
			float num5 = destRect.width / destRect.height;
			if (num5 > num)
			{
				float num6 = num / num5;
				sourceRect = new Rect(0f, (1f - num6) * 0.5f, 1f, num6);
			}
			else
			{
				float num7 = num5 / num;
				sourceRect = new Rect(0.5f - num7 * 0.5f, 0f, num7, 1f);
			}
			break;
		}
		case ScaleMode.ScaleToFit:
		{
			float num2 = destRect.width / destRect.height;
			if (num2 > num)
			{
				float num3 = num / num2;
				destRect = new Rect(destRect.xMin + destRect.width * (1f - num3) * 0.5f, destRect.yMin, num3 * destRect.width, destRect.height);
			}
			else
			{
				float num4 = num2 / num;
				destRect = new Rect(destRect.xMin, destRect.yMin + destRect.height * (1f - num4) * 0.5f, destRect.width, num4 * destRect.height);
			}
			break;
		}
		}
		GL.PushMatrix();
		if (RenderTexture.active == null)
		{
			GL.LoadPixelMatrix(0f, Screen.width, Screen.height, 0f);
		}
		else
		{
			GL.LoadPixelMatrix(0f, RenderTexture.active.width, RenderTexture.active.height, 0f);
		}
		Graphics.DrawTexture(destRect, texture, sourceRect, 0, 0, 0, 0, GUI.color, material);
		GL.PopMatrix();
	}
}
