using System;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace RenderHeads.Media.AVProVideo;

public sealed class WindowsRtMediaPlayer : BaseMediaPlayer
{
	[StructLayout(0, Size = 1)]
	private struct Native
	{
		public enum SeekMode
		{
			Fast,
			Accurate
		}

		[StructLayout(0, Pack = 1)]
		public struct VideoTrack
		{
			public int trackIndex;

			public int frameWidth;

			public int frameHeight;

			public float frameRate;

			public uint averageBitRate;
		}

		[StructLayout(0, Pack = 1)]
		public struct AudioTrack
		{
			public int trackIndex;

			public uint channelCount;

			public uint sampleRate;

			public uint bitsPerSample;

			public uint averageBitRate;
		}

		[StructLayout(0, Pack = 1)]
		public struct SeekParams
		{
			public double timeSeconds;

			public SeekMode mode;
		}

		[StructLayout(0, Pack = 1)]
		public struct AuthData
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string url;

			[MarshalAs(UnmanagedType.LPWStr)]
			public string token;

			public IntPtr keyBytes;

			public int keyBytesLength;
		}

		internal enum TimeRangeTypes
		{
			Seekable,
			Buffered
		}

		private static IntPtr _nativeFunction_UnityRenderEvent;

		[DllImport("AVProVideoWinRT")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsChangedTracks(IntPtr instance, TrackType trackType);

		[DllImport("AVProVideoWinRT")]
		public static extern int GetTrackCount(IntPtr instance, TrackType trackType);

		[DllImport("AVProVideoWinRT")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool GetTrackInfo(IntPtr instance, TrackType trackType, int index, ref int uid, ref bool isActive, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder name, int maxNameLength, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder language, int maxLanguageLength);

		[DllImport("AVProVideoWinRT")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SetActiveTrack(IntPtr instance, TrackType trackType, int trackUid);

		[DllImport("AVProVideoWinRT")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsChangedTextCue(IntPtr instance);

		[DllImport("AVProVideoWinRT")]
		public static extern IntPtr GetCurrentTextCue(IntPtr instance);

		[DllImport("AVProVideoWinRT", EntryPoint = "GetPluginVersion")]
		private static extern IntPtr GetPluginVersionStringPointer();

		public static string GetPluginVersion()
		{
			return Marshal.PtrToStringAnsi(GetPluginVersionStringPointer());
		}

		[DllImport("AVProVideoWinRT")]
		public static extern IntPtr CreatePlayer();

		[DllImport("AVProVideoWinRT")]
		public static extern void DestroyPlayer(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool OpenMedia(IntPtr playerInstance, [MarshalAs(UnmanagedType.LPWStr)] string filePath, [MarshalAs(UnmanagedType.LPWStr)] string httpHeader, FileFormat overrideFileFormat, bool startWithHighestBitrate, bool use10BitTextures);

		[DllImport("AVProVideoWinRT")]
		public static extern void CloseMedia(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern void Pause(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern void Play(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern void SetAudioVolume(IntPtr playerInstance, float volume);

		[DllImport("AVProVideoWinRT")]
		public static extern void SetAudioBalance(IntPtr playerInstance, float balance);

		[DllImport("AVProVideoWinRT")]
		public static extern void SetPlaybackRate(IntPtr playerInstance, float rate);

		[DllImport("AVProVideoWinRT")]
		public static extern void SetAudioMuted(IntPtr playerInstance, bool muted);

		[DllImport("AVProVideoWinRT")]
		public static extern float GetAudioVolume(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsAudioMuted(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern float GetAudioBalance(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern float GetPlaybackRate(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern void SetLooping(IntPtr playerInstance, bool looping);

		[DllImport("AVProVideoWinRT")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsLooping(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern int GetLastErrorCode(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern void Update(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern double GetDuration(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern int GetStereoPacking(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern double GetCurrentPosition(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool GetLatestFrame(IntPtr playerInstance, out IntPtr leftEyeTexturePointer, out IntPtr rightEyeTexturePointer, out ulong frameTimeStamp, out int width, out int height);

		[DllImport("AVProVideoWinRT")]
		public static extern PlaybackState GetPlaybackState(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool GetActiveVideoTrackInfo(IntPtr playerInstance, out VideoTrack videoTrack);

		[DllImport("AVProVideoWinRT")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool GetActiveAudioTrackInfo(IntPtr playerInstance, out AudioTrack audioTrack);

		[DllImport("AVProVideoWinRT")]
		public static extern double GetCurrentDateTimeSecondsSince1970(IntPtr playerInstance);

		[DllImport("AVProVideoWinRT")]
		public static extern void SetLiveOffset(IntPtr playerInstance, double seconds);

		[DllImport("AVProVideoWinRT")]
		public static extern void DebugValues(IntPtr playerInstance, out int isD3D, out int isUnityD3D, out int isTexture, out int isSharedTexture, out int isSurface);

		[DllImport("AVProVideoWinRT")]
		public static extern void Seek(IntPtr playerInstance, ref SeekParams seekParams);

		public static void SetNextAuthData(IntPtr playerInstance, RenderHeads.Media.AVProVideo.AuthData srcAuthData)
		{
			AuthData authData = default(AuthData);
			authData.url = (string.IsNullOrEmpty(srcAuthData.URL) ? null : srcAuthData.URL);
			authData.token = (string.IsNullOrEmpty(srcAuthData.Token) ? null : srcAuthData.Token);
			if (srcAuthData.KeyBytes != null && srcAuthData.KeyBytes.Length != 0)
			{
				authData.keyBytes = Marshal.AllocHGlobal(srcAuthData.KeyBytes.Length);
				Marshal.Copy(srcAuthData.KeyBytes, 0, authData.keyBytes, srcAuthData.KeyBytes.Length);
				authData.keyBytesLength = srcAuthData.KeyBytes.Length;
			}
			else
			{
				authData.keyBytes = IntPtr.Zero;
				authData.keyBytesLength = 0;
			}
			SetNextAuthData(playerInstance, ref authData);
			if (authData.keyBytes != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(authData.keyBytes);
			}
		}

		[DllImport("AVProVideoWinRT")]
		private static extern void SetNextAuthData(IntPtr playerInstance, ref AuthData authData);

		[DllImport("AVProVideoWinRT")]
		public static extern int GetTimeRanges(IntPtr playerInstance, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] TimeRange[] ranges, int rangeCount, TimeRangeTypes timeRangeType);

		[DllImport("AVProVideoWinRT")]
		public static extern IntPtr GetRenderEventFunc();

		public static void IssueRenderThreadEvent_UpdateAllTextures()
		{
			if (_nativeFunction_UnityRenderEvent == IntPtr.Zero)
			{
				_nativeFunction_UnityRenderEvent = GetRenderEventFunc();
			}
			if (_nativeFunction_UnityRenderEvent != IntPtr.Zero)
			{
				GL.IssuePluginEvent(_nativeFunction_UnityRenderEvent, 1);
			}
		}

		public static void IssueRenderThreadEvent_FreeAllTextures()
		{
			if (_nativeFunction_UnityRenderEvent == IntPtr.Zero)
			{
				_nativeFunction_UnityRenderEvent = GetRenderEventFunc();
			}
			if (_nativeFunction_UnityRenderEvent != IntPtr.Zero)
			{
				GL.IssuePluginEvent(_nativeFunction_UnityRenderEvent, 2);
			}
		}
	}

	private class EyeTexture
	{
		public Texture2D texture;

		public IntPtr nativePointer = IntPtr.Zero;

		public void Dispose()
		{
			if ((bool)texture)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(texture);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(texture);
				}
				texture = null;
			}
			nativePointer = IntPtr.Zero;
		}
	}

	private bool _isMediaLoaded;

	private bool _isLooping;

	private float _volume = 1f;

	private bool _use10BitTextures;

	private bool _useLowLiveLatency;

	private AuthData _nextAuthData = new AuthData();

	private static bool _isInitialised = false;

	private static string _version = "Plug-in not yet initialised";

	private ulong _frameTimeStamp;

	private IntPtr _playerInstance;

	private EyeTexture[] _eyeTextures = new EyeTexture[2];

	public AuthData AuthenticationData
	{
		get
		{
			return _nextAuthData;
		}
		set
		{
			_nextAuthData = value;
			Native.SetNextAuthData(_playerInstance, _nextAuthData);
		}
	}

	public WindowsRtMediaPlayer(MediaPlayer.OptionsWindows options)
	{
		_playerDescription = "WinRT";
		SetOptions(options);
		for (int i = 0; i < _eyeTextures.Length; i++)
		{
			_eyeTextures[i] = new EyeTexture();
		}
	}

	public WindowsRtMediaPlayer(MediaPlayer.OptionsWindowsUWP options)
	{
		_playerDescription = "WinRT";
		_use10BitTextures = options.use10BitTextures;
		_useLowLiveLatency = options.useLowLiveLatency;
		for (int i = 0; i < _eyeTextures.Length; i++)
		{
			_eyeTextures[i] = new EyeTexture();
		}
	}

	public void SetOptions(MediaPlayer.OptionsWindows options)
	{
		_use10BitTextures = options.use10BitTextures;
		_useLowLiveLatency = options.useLowLiveLatency;
	}

	public override bool CanPlay()
	{
		return HasMetaData();
	}

	public override void Dispose()
	{
		CloseMedia();
		if (_playerInstance != IntPtr.Zero)
		{
			Native.DestroyPlayer(_playerInstance);
			_playerInstance = IntPtr.Zero;
			Native.IssueRenderThreadEvent_FreeAllTextures();
		}
		for (int i = 0; i < _eyeTextures.Length; i++)
		{
			_eyeTextures[i].Dispose();
		}
	}

	public override bool PlayerSupportsLinearColorSpace()
	{
		return false;
	}

	public override double GetCurrentTime()
	{
		return Native.GetCurrentPosition(_playerInstance);
	}

	public override double GetDuration()
	{
		return Native.GetDuration(_playerInstance);
	}

	public override float GetPlaybackRate()
	{
		return Native.GetPlaybackRate(_playerInstance);
	}

	public override Texture GetTexture(int index = 0)
	{
		Texture result = null;
		if (_frameTimeStamp != 0 && index < _eyeTextures.Length)
		{
			result = _eyeTextures[index].texture;
		}
		return result;
	}

	public override int GetTextureCount()
	{
		if (_eyeTextures[1].texture != null)
		{
			return 2;
		}
		return 1;
	}

	public override int GetTextureFrameCount()
	{
		return (int)_frameTimeStamp;
	}

	internal override StereoPacking InternalGetTextureStereoPacking()
	{
		return Native.GetStereoPacking(_playerInstance) switch
		{
			0 => StereoPacking.Monoscopic, 
			1 => StereoPacking.TopBottom, 
			2 => StereoPacking.LeftRight, 
			3 => StereoPacking.CustomUV, 
			4 => StereoPacking.MultiviewLeftPrimary, 
			_ => StereoPacking.Unknown, 
		};
	}

	public override string GetVersion()
	{
		return _version;
	}

	public override string GetExpectedVersion()
	{
		return "3.4.0";
	}

	public override float GetVideoFrameRate()
	{
		float result = 0f;
		if (Native.GetActiveVideoTrackInfo(_playerInstance, out var videoTrack))
		{
			result = videoTrack.frameRate;
		}
		return result;
	}

	public override int GetVideoWidth()
	{
		int result = 0;
		if ((bool)_eyeTextures[0].texture)
		{
			result = _eyeTextures[0].texture.width;
		}
		return result;
	}

	public override int GetVideoHeight()
	{
		int result = 0;
		if ((bool)_eyeTextures[0].texture)
		{
			result = _eyeTextures[0].texture.height;
		}
		return result;
	}

	public override float GetVolume()
	{
		return _volume;
	}

	public override void SetBalance(float balance)
	{
		Native.SetAudioBalance(_playerInstance, balance);
	}

	public override float GetBalance()
	{
		return Native.GetAudioBalance(_playerInstance);
	}

	public override bool HasAudio()
	{
		return _audioTracks.Count > 0;
	}

	public override bool HasMetaData()
	{
		return Native.GetDuration(_playerInstance) > 0.0;
	}

	public override bool HasVideo()
	{
		return _videoTracks.Count > 0;
	}

	public override bool IsBuffering()
	{
		return (Native.GetPlaybackState(_playerInstance) & PlaybackState.StateMask) == PlaybackState.Buffering;
	}

	public override bool IsFinished()
	{
		bool result = false;
		if (IsPaused() && !IsSeeking() && GetCurrentTime() >= GetDuration())
		{
			result = true;
		}
		return result;
	}

	public override bool IsLooping()
	{
		return _isLooping;
	}

	public override bool IsMuted()
	{
		return Native.IsAudioMuted(_playerInstance);
	}

	public override bool IsPaused()
	{
		return (Native.GetPlaybackState(_playerInstance) & PlaybackState.StateMask) == PlaybackState.Paused;
	}

	public override bool IsPlaying()
	{
		return (Native.GetPlaybackState(_playerInstance) & PlaybackState.StateMask) == PlaybackState.Playing;
	}

	public override bool IsSeeking()
	{
		return (Native.GetPlaybackState(_playerInstance) & PlaybackState.Seeking) != 0;
	}

	public override void MuteAudio(bool bMuted)
	{
		Native.SetAudioMuted(_playerInstance, bMuted);
	}

	public override bool OpenMedia(string path, long offset, string httpHeader, MediaHints mediaHints, int forceFileFormat = 0, bool startWithHighestBitrate = false)
	{
		bool flag = false;
		if (_playerInstance == IntPtr.Zero)
		{
			_playerInstance = Native.CreatePlayer();
			AuthenticationData = _nextAuthData;
		}
		if (_playerInstance != IntPtr.Zero)
		{
			flag = Native.OpenMedia(_playerInstance, path, httpHeader, (FileFormat)forceFileFormat, startWithHighestBitrate, _use10BitTextures);
			if (flag)
			{
				if (_useLowLiveLatency)
				{
					Native.SetLiveOffset(_playerInstance, 0.0);
				}
				Native.SetLooping(_playerInstance, _isLooping);
				Native.SetAudioVolume(_playerInstance, _volume);
			}
			_mediaHints = mediaHints;
		}
		return flag;
	}

	public override void CloseMedia()
	{
		_isMediaLoaded = false;
		_isLooping = false;
		_volume = 1f;
		Native.CloseMedia(_playerInstance);
		base.CloseMedia();
	}

	public override void Pause()
	{
		Native.Pause(_playerInstance);
	}

	public override void Play()
	{
		Native.Play(_playerInstance);
	}

	public override void Render()
	{
		Native.IssueRenderThreadEvent_UpdateAllTextures();
	}

	private void Update_Textures()
	{
		IntPtr leftEyeTexturePointer = IntPtr.Zero;
		IntPtr rightEyeTexturePointer = IntPtr.Zero;
		ulong frameTimeStamp = 0uL;
		if (!Native.GetLatestFrame(_playerInstance, out leftEyeTexturePointer, out rightEyeTexturePointer, out frameTimeStamp, out var width, out var height))
		{
			return;
		}
		bool flag = false;
		bool flag2 = frameTimeStamp > _frameTimeStamp || (_frameTimeStamp == 0L && frameTimeStamp == 0);
		for (int i = 0; i < _eyeTextures.Length; i++)
		{
			EyeTexture eyeTexture = _eyeTextures[i];
			IntPtr intPtr = leftEyeTexturePointer;
			if (i == 1)
			{
				intPtr = rightEyeTexturePointer;
			}
			bool flag3 = eyeTexture.texture != null && (intPtr == IntPtr.Zero || eyeTexture.texture.width != width || eyeTexture.texture.height != height);
			if (!(flag2 || flag3))
			{
				continue;
			}
			if (flag3)
			{
				eyeTexture.Dispose();
			}
			if (eyeTexture.texture != null)
			{
				if (eyeTexture.nativePointer != intPtr)
				{
					eyeTexture.texture.UpdateExternalTexture(intPtr);
					eyeTexture.nativePointer = intPtr;
				}
			}
			else if (intPtr != IntPtr.Zero)
			{
				bool linear = QualitySettings.activeColorSpace == ColorSpace.Linear;
				eyeTexture.texture = Texture2D.CreateExternalTexture(width, height, TextureFormat.BGRA32, mipChain: false, linear, intPtr);
				if (eyeTexture.texture != null)
				{
					eyeTexture.texture.name = "AVProVideo";
					eyeTexture.nativePointer = intPtr;
					ApplyTextureProperties(eyeTexture.texture);
				}
				else
				{
					Debug.LogError("[AVProVideo] Failed to create texture");
				}
			}
			flag = true;
		}
		if (flag)
		{
			_frameTimeStamp = frameTimeStamp;
		}
	}

	public override bool RequiresVerticalFlip()
	{
		return true;
	}

	public override void Seek(double time)
	{
		Native.SeekParams seekParams = default(Native.SeekParams);
		seekParams.timeSeconds = time;
		seekParams.mode = Native.SeekMode.Accurate;
		Native.Seek(_playerInstance, ref seekParams);
	}

	public override void SeekFast(double time)
	{
		Seek(time);
	}

	public override void SetLooping(bool bLooping)
	{
		_isLooping = bLooping;
		Native.SetLooping(_playerInstance, _isLooping);
	}

	public override void SetPlaybackRate(float rate)
	{
		rate = Mathf.Max(0f, rate);
		Native.SetPlaybackRate(_playerInstance, rate);
	}

	public override void SetVolume(float volume)
	{
		_volume = volume;
		Native.SetAudioVolume(_playerInstance, _volume);
	}

	public override void Stop()
	{
		Pause();
	}

	private void UpdateTimeRanges()
	{
		UpdateTimeRange(ref _seekableTimes._ranges, Native.TimeRangeTypes.Seekable);
		UpdateTimeRange(ref _bufferedTimes._ranges, Native.TimeRangeTypes.Buffered);
		_seekableTimes.CalculateRange();
		_bufferedTimes.CalculateRange();
	}

	private void UpdateTimeRange(ref TimeRange[] range, Native.TimeRangeTypes timeRangeType)
	{
		int timeRanges = Native.GetTimeRanges(_playerInstance, range, range.Length, timeRangeType);
		if (timeRanges != range.Length)
		{
			range = new TimeRange[timeRanges];
			Native.GetTimeRanges(_playerInstance, range, range.Length, timeRangeType);
		}
	}

	public override DateTime GetProgramDateTime()
	{
		return Helper.ConvertSecondsSince1970ToDateTime(Native.GetCurrentDateTimeSecondsSince1970(_playerInstance));
	}

	public override void Update()
	{
		Native.Update(_playerInstance);
		UpdateTracks();
		UpdateTextCue();
		_lastError = (ErrorCode)Native.GetLastErrorCode(_playerInstance);
		UpdateTimeRanges();
		UpdateSubtitles();
		Update_Textures();
		UpdateDisplayFrameRate();
		if (_isMediaLoaded)
		{
			return;
		}
		if (HasVideo() && _eyeTextures[0].texture != null)
		{
			if (Native.GetActiveVideoTrackInfo(_playerInstance, out var videoTrack))
			{
				Helper.LogInfo("Using playback path: " + _playerDescription + " (" + videoTrack.frameWidth + "x" + videoTrack.frameHeight + "@" + videoTrack.frameRate.ToString("F2") + ")");
				_isMediaLoaded = true;
			}
		}
		else if (HasAudio() && !HasVideo())
		{
			Helper.LogInfo("Using playback path: " + _playerDescription);
			_isMediaLoaded = true;
		}
	}

	public override void SetKeyServerAuthToken(string token)
	{
		_nextAuthData.Token = token;
		AuthenticationData = _nextAuthData;
	}

	public override void SetOverrideDecryptionKey(byte[] key)
	{
		_nextAuthData.KeyBytes = key;
		AuthenticationData = _nextAuthData;
	}

	internal override bool InternalSetActiveTrack(TrackType trackType, int trackUid)
	{
		return Native.SetActiveTrack(_playerInstance, trackType, trackUid);
	}

	internal override bool InternalIsChangedTracks(TrackType trackType)
	{
		return Native.IsChangedTracks(_playerInstance, trackType);
	}

	internal override int InternalGetTrackCount(TrackType trackType)
	{
		return Native.GetTrackCount(_playerInstance, trackType);
	}

	internal override TrackBase InternalGetTrackInfo(TrackType trackType, int trackIndex, ref bool isActiveTrack)
	{
		TrackBase result = null;
		StringBuilder stringBuilder = new StringBuilder(128);
		StringBuilder stringBuilder2 = new StringBuilder(16);
		int uid = -1;
		if (Native.GetTrackInfo(_playerInstance, trackType, trackIndex, ref uid, ref isActiveTrack, stringBuilder, stringBuilder.Capacity, stringBuilder2, stringBuilder2.Capacity))
		{
			switch (trackType)
			{
			case TrackType.Video:
				result = new VideoTrack(uid, stringBuilder.ToString(), stringBuilder2.ToString(), isDefault: false);
				break;
			case TrackType.Audio:
				result = new AudioTrack(uid, stringBuilder.ToString(), stringBuilder2.ToString(), isDefault: false);
				break;
			case TrackType.Text:
				result = new TextTrack(uid, stringBuilder.ToString(), stringBuilder2.ToString(), isDefault: false);
				break;
			}
		}
		return result;
	}

	internal override bool InternalIsChangedTextCue()
	{
		return Native.IsChangedTextCue(_playerInstance);
	}

	internal override string InternalGetCurrentTextCue()
	{
		string result = null;
		IntPtr currentTextCue = Native.GetCurrentTextCue(_playerInstance);
		if (currentTextCue != IntPtr.Zero)
		{
			result = Marshal.PtrToStringUni(currentTextCue);
		}
		return result;
	}

	public static bool InitialisePlatform()
	{
		if (!_isInitialised)
		{
			try
			{
				if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D12)
				{
					_isInitialised = true;
					_version = Native.GetPluginVersion();
				}
				else
				{
					Debug.LogError("[AVProVideo] Only Direct3D 11 and 12 are supported, graphicsDeviceType not supported: " + SystemInfo.graphicsDeviceType);
				}
			}
			catch (DllNotFoundException ex)
			{
				Debug.LogError("[AVProVideo] Failed to load DLL. " + ex.Message);
			}
		}
		return _isInitialised;
	}

	public static void DeinitPlatform()
	{
		_isInitialised = false;
	}
}
