using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public static class Helper
{
	public sealed class ExpectedPluginVersion
	{
		public const string Windows = "3.4.0";

		public const string WinRT = "3.4.0";

		public const string Android = "3.4.0";

		public const string Apple = "3.4.0";

		public const string OpenHarmony = "3.3.5";
	}

	public const string AVProVideoVersion = "3.4.0";

	public const string UnityBaseTextureName = "_MainTex";

	public const string UnityBaseTextureName_URP = "_BaseMap";

	public const string UnityBaseTextureName_HDRP = "_BaseColorMap";

	public const double SecondsToHNS = 10000000.0;

	public const double MilliSecondsToHNS = 10000.0;

	private static Matrix4x4 PortraitMatrix = Matrix4x4.TRS(new Vector3(0f, 1f, 0f), Quaternion.Euler(0f, 0f, -90f), Vector3.one);

	private static Matrix4x4 PortraitFlippedMatrix = Matrix4x4.TRS(new Vector3(1f, 0f, 0f), Quaternion.Euler(0f, 0f, 90f), Vector3.one);

	private static Matrix4x4 LandscapeFlippedMatrix = Matrix4x4.TRS(new Vector3(1f, 1f, 0f), Quaternion.Euler(0f, 0f, -180f), Vector3.one);

	public static string GetPath(MediaPathType location)
	{
		string result = string.Empty;
		switch (location)
		{
		case MediaPathType.RelativeToDataFolder:
			result = Application.dataPath;
			break;
		case MediaPathType.RelativeToPersistentDataFolder:
			result = Application.persistentDataPath;
			break;
		case MediaPathType.RelativeToProjectFolder:
		{
			string path = "..";
			result = Path.GetFullPath(Path.Combine(Application.dataPath, path));
			result = result.Replace('\\', '/');
			break;
		}
		case MediaPathType.RelativeToStreamingAssetsFolder:
			result = Application.streamingAssetsPath;
			break;
		}
		return result;
	}

	public static string GetFilePath(string path, MediaPathType location)
	{
		string result = string.Empty;
		if (!string.IsNullOrEmpty(path))
		{
			switch (location)
			{
			case MediaPathType.AbsolutePathOrURL:
				result = path;
				break;
			case MediaPathType.RelativeToProjectFolder:
			case MediaPathType.RelativeToStreamingAssetsFolder:
			case MediaPathType.RelativeToDataFolder:
			case MediaPathType.RelativeToPersistentDataFolder:
				result = Path.Combine(GetPath(location), path);
				break;
			}
		}
		return result;
	}

	public static string GetFriendlyResolutionName(int width, int height, float fps)
	{
		int[] array = new int[10] { 0, 33177600, 8294400, 3686400, 2073600, 921600, 409440, 230400, 102240, 36864 };
		string[] array2 = new string[10] { "Unknown", "8K", "4K", "1440p", "1080p", "720p", "480p", "360p", "240p", "144p" };
		int num = 0;
		int num2 = width * height;
		int num3 = int.MaxValue;
		for (int i = 0; i < array.Length; i++)
		{
			int num4 = Mathf.Abs(array[i] - num2);
			if (num4 < num3)
			{
				num = i;
				num3 = num4;
				if (num4 == 0)
				{
					break;
				}
			}
		}
		string text = array2[num];
		if (fps > 0f && !float.IsNaN(fps))
		{
			text += fps.ToString("0.##");
		}
		return text;
	}

	public static string GetErrorMessage(ErrorCode code)
	{
		string text = string.Empty;
		switch (code)
		{
		case ErrorCode.None:
			text = "No Error";
			break;
		case ErrorCode.LoadFailed:
			text = "Loading failed.  File not found, codec not supported, video resolution too high or insufficient system resources.";
			if (SystemInfo.operatingSystem.StartsWith("Windows XP") || SystemInfo.operatingSystem.StartsWith("Windows Vista"))
			{
				text += " NOTE: Windows XP and Vista don't have native support for H.264 codec.  Consider using an older codec such as DivX or installing 3rd party codecs such as LAV Filters.";
			}
			break;
		case ErrorCode.DecodeFailed:
			text = "Decode failed.  Possible codec not supported, video resolution/bit-depth too high, or insufficient system resources.";
			break;
		}
		return text;
	}

	public static string GetPlatformName(Platform platform)
	{
		string text = "Unknown";
		if (platform == Platform.WindowsUWP)
		{
			return "Windows UWP";
		}
		return platform.ToString();
	}

	public static string[] GetPlatformNames()
	{
		return new string[9]
		{
			GetPlatformName(Platform.Windows),
			GetPlatformName(Platform.macOS),
			GetPlatformName(Platform.iOS),
			GetPlatformName(Platform.tvOS),
			GetPlatformName(Platform.visionOS),
			GetPlatformName(Platform.Android),
			GetPlatformName(Platform.WindowsUWP),
			GetPlatformName(Platform.WebGL),
			GetPlatformName(Platform.OpenHarmony)
		};
	}

	public static void LogInfo(string message, UnityEngine.Object context = null)
	{
		if (context == null)
		{
			Debug.Log("[AVProVideo] " + message);
		}
		else
		{
			Debug.Log("[AVProVideo] " + message, context);
		}
	}

	public static int GetUnityAudioSampleRate()
	{
		if (AudioSettings.GetConfiguration().sampleRate != 0)
		{
			return AudioSettings.outputSampleRate;
		}
		return 0;
	}

	public static int GetUnityAudioSpeakerCount()
	{
		return AudioSettings.GetConfiguration().speakerMode switch
		{
			AudioSpeakerMode.Mono => 1, 
			AudioSpeakerMode.Stereo => 2, 
			AudioSpeakerMode.Quad => 4, 
			AudioSpeakerMode.Surround => 5, 
			AudioSpeakerMode.Mode5point1 => 6, 
			AudioSpeakerMode.Mode7point1 => 8, 
			AudioSpeakerMode.Prologic => 2, 
			_ => 0, 
		};
	}

	public static TimeRange GetTimelineRange(double duration, TimeRanges seekable)
	{
		TimeRange result = default(TimeRange);
		if (duration >= 0.0 && duration < 20000000000.0)
		{
			result.startTime = 0.0;
			result.duration = duration;
		}
		else
		{
			result.startTime = seekable.MinTime;
			result.duration = seekable.Duration;
		}
		return result;
	}

	public static string GetTimeString(double timeSeconds, bool showMilliseconds = false)
	{
		float num = (float)timeSeconds;
		int num2 = Mathf.FloorToInt(num / 3600f);
		float num3 = (float)num2 * 60f * 60f;
		int num4 = Mathf.FloorToInt((num - num3) / 60f);
		num3 += (float)num4 * 60f;
		int num5 = Mathf.FloorToInt(num - num3);
		if (num2 <= 0)
		{
			if (showMilliseconds)
			{
				int num6 = (int)((num - Mathf.Floor(num)) * 1000f);
				return $"{num4:00}:{num5:00}:{num6:000}";
			}
			return $"{num4:00}:{num5:00}";
		}
		if (showMilliseconds)
		{
			int num7 = (int)((num - Mathf.Floor(num)) * 1000f);
			return string.Format("{2}:{0:00}:{1:00}:{3:000}", num4, num5, num2, num7);
		}
		return string.Format("{2}:{0:00}:{1:00}", num4, num5, num2);
	}

	public static Orientation GetOrientation(float[] t)
	{
		Orientation result = Orientation.Landscape;
		if (t != null)
		{
			if (t[0] == 0f && t[1] == 1f && t[2] == -1f && t[3] == 0f)
			{
				result = Orientation.Portrait;
			}
			else if (t[0] == 0f && t[1] == -1f && t[2] == 1f && t[3] == 0f)
			{
				result = Orientation.PortraitFlipped;
			}
			else if (t[0] == 1f && t[1] == 0f && t[2] == 0f && t[3] == 1f)
			{
				result = Orientation.Landscape;
			}
			else if (t[0] == -1f && t[1] == 0f && t[2] == 0f && t[3] == -1f)
			{
				result = Orientation.LandscapeFlipped;
			}
			else if (t[0] == 0f && t[1] == 1f && t[2] == 1f && t[3] == 0f)
			{
				result = Orientation.PortraitHorizontalMirror;
			}
		}
		return result;
	}

	public static Matrix4x4 GetMatrixForOrientation(Orientation ori)
	{
		switch (ori)
		{
		case Orientation.Landscape:
			return Matrix4x4.identity;
		case Orientation.LandscapeFlipped:
			return LandscapeFlippedMatrix;
		case Orientation.Portrait:
			return PortraitMatrix;
		case Orientation.PortraitFlipped:
			return PortraitFlippedMatrix;
		case Orientation.PortraitHorizontalMirror:
		{
			Matrix4x4 result = default(Matrix4x4);
			result.SetColumn(0, new Vector4(0f, 1f, 0f, 0f));
			result.SetColumn(1, new Vector4(1f, 0f, 0f, 0f));
			result.SetColumn(2, new Vector4(0f, 0f, 1f, 0f));
			result.SetColumn(3, new Vector4(0f, 0f, 0f, 1f));
			return result;
		}
		default:
			throw new Exception("Unknown Orientation type");
		}
	}

	public static Matrix4x4 Matrix4x4FromAffineTransform(float[] affineXfrm)
	{
		Vector4 column = new Vector4(affineXfrm[0], affineXfrm[1], 0f, 0f);
		Vector4 column2 = new Vector4(affineXfrm[2], affineXfrm[3], 0f, 0f);
		Vector4 column3 = new Vector4(0f, 0f, 1f, 0f);
		Vector4 column4 = new Vector4(affineXfrm[4], affineXfrm[5], 0f, 1f);
		return new Matrix4x4(column, column2, column3, column4);
	}

	public static int ConvertTimeSecondsToFrame(double seconds, float frameRate)
	{
		seconds = Math.Max(0.0, seconds);
		frameRate = Mathf.Max(0f, frameRate);
		return (int)Math.Floor((double)frameRate * seconds);
	}

	public static double ConvertFrameToTimeSeconds(int frame, float frameRate)
	{
		frame = Mathf.Max(0, frame);
		frameRate = Mathf.Max(0f, frameRate);
		double num = 1.0 / (double)frameRate;
		return (double)frame * num + num * 0.5;
	}

	public static double FindNextKeyFrameTimeSeconds(double seconds, float frameRate, int keyFrameInterval)
	{
		seconds = Math.Max(0.0, seconds);
		frameRate = Mathf.Max(0f, frameRate);
		keyFrameInterval = Mathf.Max(0, keyFrameInterval);
		int num = ConvertTimeSecondsToFrame(seconds, frameRate);
		return ConvertFrameToTimeSeconds(keyFrameInterval * Mathf.CeilToInt((float)(num + 1) / (float)keyFrameInterval), frameRate);
	}

	public static DateTime ConvertSecondsSince1970ToDateTime(double secondsSince1970)
	{
		TimeSpan value = TimeSpan.FromSeconds(secondsSince1970);
		return new DateTime(1970, 1, 1).Add(value);
	}

	[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "GetShortPathNameW", SetLastError = true)]
	private static extern int GetShortPathName([MarshalAs(UnmanagedType.LPWStr)] string pathName, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder shortName, int cbShortName);

	internal static string ConvertLongPathToShortDOS83Path(string path)
	{
		string text = "\\\\?\\" + path.Replace("/", "\\");
		int shortPathName = GetShortPathName(text, null, 0);
		if (shortPathName > 0)
		{
			StringBuilder stringBuilder = new StringBuilder(shortPathName);
			if (GetShortPathName(text, stringBuilder, shortPathName) != 0)
			{
				text = stringBuilder.ToString().Replace("\\\\?\\", "");
				Debug.LogWarning("[AVProVideo] Long path detected. Changing to DOS 8.3 format");
			}
		}
		return text;
	}

	public static Texture2D GetReadableTexture(Texture inputTexture, bool requiresVerticalFlip, Orientation ori, Texture2D targetTexture = null)
	{
		Texture2D texture2D = targetTexture;
		RenderTexture active = RenderTexture.active;
		int width = inputTexture.width;
		int height = inputTexture.height;
		RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
		if (ori == Orientation.Landscape)
		{
			if (!requiresVerticalFlip)
			{
				Graphics.Blit(inputTexture, temporary);
			}
			else
			{
				GL.PushMatrix();
				RenderTexture.active = temporary;
				GL.LoadPixelMatrix(0f, temporary.width, 0f, temporary.height);
				Graphics.DrawTexture(sourceRect: new Rect(0f, 0f, 1f, 1f), screenRect: new Rect(0f, -1f, temporary.width, temporary.height), texture: inputTexture, leftBorder: 0, rightBorder: 0, topBorder: 0, bottomBorder: 0);
				GL.PopMatrix();
				GL.InvalidateState();
			}
		}
		if (texture2D == null)
		{
			texture2D = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false);
		}
		RenderTexture.active = temporary;
		texture2D.ReadPixels(new Rect(0f, 0f, width, height), 0, 0, recalculateMipMaps: false);
		texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: false);
		RenderTexture.ReleaseTemporary(temporary);
		RenderTexture.active = active;
		return texture2D;
	}

	public static Texture2D GetReadableTexture(RenderTexture inputTexture, Texture2D targetTexture = null)
	{
		if (targetTexture == null)
		{
			targetTexture = new Texture2D(inputTexture.width, inputTexture.height, TextureFormat.ARGB32, mipChain: false);
		}
		RenderTexture active = RenderTexture.active;
		RenderTexture.active = inputTexture;
		targetTexture.ReadPixels(new Rect(0f, 0f, inputTexture.width, inputTexture.height), 0, 0, recalculateMipMaps: false);
		targetTexture.Apply(updateMipmaps: false, makeNoLongerReadable: false);
		RenderTexture.active = active;
		return targetTexture;
	}
}
