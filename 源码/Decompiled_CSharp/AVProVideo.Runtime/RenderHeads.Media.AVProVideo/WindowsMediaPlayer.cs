using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

namespace RenderHeads.Media.AVProVideo;

public class WindowsMediaPlayer : BaseMediaPlayer
{
	[StructLayout(0, Size = 1)]
	private struct Native
	{
		internal enum TimeRangeTypes
		{
			Seekable,
			Buffered
		}

		public enum RenderThreadEvent
		{
			BeginRender,
			UpdateAllTextures,
			FreeTextures,
			WaitForNewFrame
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

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsChangedTracks(IntPtr instance, TrackType trackType);

		[DllImport("AVProVideo")]
		public static extern int GetTrackCount(IntPtr instance, TrackType trackType);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool GetTrackInfo(IntPtr instance, TrackType trackType, int index, ref int uid, ref bool isActive, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder name, int maxNameLength, [MarshalAs(UnmanagedType.LPWStr)] StringBuilder language, int maxLanguageLength);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool SetActiveTrack(IntPtr instance, TrackType trackType, int trackUid);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsChangedTextCue(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern IntPtr GetCurrentTextCue(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern int GetTimeRanges(IntPtr playerInstance, [Out][MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 2)] TimeRange[] ranges, int rangeCount, TimeRangeTypes timeRangeType);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool Init(bool linearColorSpace);

		[DllImport("AVProVideo")]
		public static extern void Deinit();

		[DllImport("AVProVideo")]
		public static extern IntPtr GetPluginVersion();

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsTrialVersion();

		[DllImport("AVProVideo")]
		public static extern IntPtr BeginOpenSource(IntPtr instance, Windows.VideoApi videoApi, Windows.AudioOutput audioOutput, bool useHardwareDecoding, bool useRendererSync, bool generateTextureMips, bool hintAlphaChannel, bool useLowLatency, bool use10BitTextures, [MarshalAs(UnmanagedType.LPWStr)] string forceAudioOutputDeviceName, int unitySampleRate, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] IntPtr[] preferredFilter, uint numFilters, int audio360ChannelMode, [MarshalAs(UnmanagedType.LPWStr)] string httpHeader, bool startWithHighestBitrate);

		[DllImport("AVProVideo")]
		public static extern IntPtr EndOpenSource(IntPtr instance, [MarshalAs(UnmanagedType.LPWStr)] string path);

		[DllImport("AVProVideo")]
		public static extern IntPtr OpenSourceFromBuffer(IntPtr instance, byte[] buffer, ulong bufferLength, Windows.VideoApi videoApi, Windows.AudioOutput audioOutput, bool useHardwareDecoding, bool useRendererSync, bool generateTextureMips, bool hintAlphaChannel, bool useLowLatency, bool use10BitTextures, [MarshalAs(UnmanagedType.LPWStr)] string forceAudioOutputDeviceName, int unitySampleRate, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] IntPtr[] preferredFilter, uint numFilters);

		[DllImport("AVProVideo")]
		public static extern IntPtr StartOpenSourceFromBuffer(IntPtr instance, Windows.VideoApi videoApi, ulong bufferLength);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool AddChunkToSourceBuffer(IntPtr instance, byte[] buffer, ulong offset, ulong chunkLength);

		[DllImport("AVProVideo")]
		public static extern IntPtr EndOpenSourceFromBuffer(IntPtr instance, Windows.AudioOutput audioOutput, bool useHardwareDecoding, bool useRendererSync, bool generateTextureMips, bool hintAlphaChannel, bool useLowLatency, bool use10BitTextures, [MarshalAs(UnmanagedType.LPWStr)] string forceAudioOutputDeviceName, int unitySampleRate, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] IntPtr[] preferredFilter, uint numFilters);

		[DllImport("AVProVideo")]
		public static extern void CloseSource(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern IntPtr GetPlayerDescription(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern void SetCustomMovParserEnabled(IntPtr instance, bool enabled);

		[DllImport("AVProVideo")]
		public static extern void SetHapNotchLCEnabled(IntPtr instance, bool enabled);

		[DllImport("AVProVideo")]
		public static extern void SetStereoDetectEnabled(IntPtr instance, bool enabled);

		[DllImport("AVProVideo")]
		public static extern void SetTextTrackSupportEnabled(IntPtr instance, bool enabled);

		[DllImport("AVProVideo")]
		public static extern void SetAudioDelayEnabled(IntPtr instance, bool enabled, bool isAutomatic, double timeSeconds);

		[DllImport("AVProVideo")]
		public static extern void SetFacebookAudio360SupportEnabled(IntPtr instance, bool enabled);

		[DllImport("AVProVideo")]
		public static extern void SetDecoderHints(IntPtr instance, int parallelFrameCount, int prerollFrameCount, uint maxDropMode);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool GetDecoderPerformance(IntPtr instance, ref int activeDecodeThreadCount, ref int decodedFrameCount, ref int droppedFrameCount);

		[DllImport("AVProVideo")]
		public static extern int GetLastErrorCode(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern long GetLastExtendedErrorCode(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern void Play(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern void Pause(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern void SetMuted(IntPtr instance, bool muted);

		[DllImport("AVProVideo")]
		public static extern void SetVolume(IntPtr instance, float volume);

		[DllImport("AVProVideo")]
		public static extern void SetBalance(IntPtr instance, float volume);

		[DllImport("AVProVideo")]
		public static extern void SetLooping(IntPtr instance, bool looping);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool HasVideo(IntPtr instance);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool HasAudio(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern int GetWidth(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern int GetHeight(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern float GetFrameRate(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern int GetStereoPacking(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern double GetDuration(IntPtr instance);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsPlaybackStalled(IntPtr instance);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool HasMetaData(IntPtr instance);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool CanPlay(IntPtr instance);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsSeeking(IntPtr instance);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsPlaying(IntPtr instance);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsFinished(IntPtr instance);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsBuffering(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern double GetCurrentTime(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern void SetCurrentTime(IntPtr instance, double time, bool fast);

		[DllImport("AVProVideo")]
		public static extern float GetPlaybackRate(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern void SetPlaybackRate(IntPtr instance, float rate);

		[DllImport("AVProVideo")]
		public static extern void StartExtractFrame(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern void WaitForExtract(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern void Update(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern void EndUpdate(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern IntPtr GetTexturePointer(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern int GetTextureFormat(IntPtr instance);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsTextureTopDown(IntPtr instance);

		[DllImport("AVProVideo")]
		[return: MarshalAs(UnmanagedType.I1)]
		public static extern bool IsTextureSampleLinear(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern int GetTextureFrameCount(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern long GetTextureTimeStamp(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern float GetTexturePixelAspectRatio(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern void ReleaseTextureFrame(IntPtr instance, ref TextureFrame textureFrame);

		[DllImport("AVProVideo")]
		public static extern IntPtr GetRenderEventFunc();

		[DllImport("AVProVideo")]
		public static extern int GrabAudio(IntPtr instance, float[] buffer, int sampleCount, int channelCount);

		[DllImport("AVProVideo")]
		public static extern int GetAudioBufferedSampleCount(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern int GetAudioChannelCount(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern int GetAudioChannelMask(IntPtr instance);

		[DllImport("AVProVideo")]
		public static extern int SetAudioChannelMode(IntPtr instance, int audio360ChannelMode);

		[DllImport("AVProVideo")]
		public static extern void SetHeadOrientation(IntPtr instance, float x, float y, float z, float w);

		[DllImport("AVProVideo")]
		public static extern void SetAudioFocusEnabled(IntPtr instance, bool enabled);

		[DllImport("AVProVideo")]
		public static extern void SetAudioFocusProps(IntPtr instance, float offFocusLevel, float widthDegrees);

		[DllImport("AVProVideo")]
		public static extern void SetAudioFocusRotation(IntPtr instance, float x, float y, float z, float w);

		public static void SetNextAuthData(IntPtr instance, RenderHeads.Media.AVProVideo.AuthData srcAuthData)
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
			SetNextAuthData(instance, ref authData);
			if (authData.keyBytes != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(authData.keyBytes);
			}
		}

		[DllImport("AVProVideo")]
		private static extern void SetNextAuthData(IntPtr instance, ref AuthData authData);
	}

	private Windows.AudioOutput _audioOutput;

	private string _audioDeviceOutputName = string.Empty;

	private List<string> _preferredFilters = new List<string>();

	private Audio360ChannelMode _audio360ChannelMode;

	private bool _useCustomMovParser;

	private bool _useStereoDetection = true;

	private bool _useHapNotchLC = true;

	private bool _useTextTrackSupport = true;

	private bool _useFacebookAudio360Support = true;

	private bool _useAudioDelay;

	private int _decoderParallelFrameCount = 8;

	private int _decodePrerollFrameCount = 4;

	private uint _decodeMaxDropMode = 5u;

	private bool _isPlaying;

	private bool _isPaused;

	private bool _isLooping;

	private bool _canPlay;

	private bool _hasMetaData;

	private int _width;

	private int _height;

	private float _frameRate;

	private float _playBackRate = 1f;

	private bool _audioMuted;

	private float _volume = 1f;

	private float _balance;

	private bool _hasAudio;

	private bool _hasVideo;

	private bool _isTextureTopDown = true;

	private IntPtr _nativeTexture = IntPtr.Zero;

	private Texture2D _texture;

	private RenderTexture _resolvedTexture;

	private IntPtr _instance = IntPtr.Zero;

	private Windows.VideoApi _videoApi;

	private bool _useHardwareDecoding = true;

	private bool _useRendererSync = true;

	private bool _useTextureMips;

	private bool _use10BitTextures;

	private bool _hintAlphaChannel;

	private bool _useLowLatency;

	private bool _supportsLinearColorSpace = true;

	private static bool _isInitialised = false;

	private static string _version = "Plug-in not yet initialised";

	private static IntPtr _nativeFunction_UnityRenderEvent;

	private static int _lastUpdateAllTexturesFrame = -1;

	private AuthData _nextAuthData = new AuthData();

	public AuthData AuthenticationData
	{
		get
		{
			return _nextAuthData;
		}
		set
		{
			_nextAuthData = value;
			Native.SetNextAuthData(_instance, _nextAuthData);
		}
	}

	public static bool InitialisePlatform()
	{
		if (!_isInitialised)
		{
			try
			{
				if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null || SystemInfo.graphicsDeviceType == GraphicsDeviceType.OpenGLCore || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D11 || SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D12)
				{
					if (!Native.Init(QualitySettings.activeColorSpace == ColorSpace.Linear))
					{
						Debug.LogError("[AVProVideo] Failing to initialise platform");
					}
					else
					{
						_isInitialised = true;
						_version = GetPluginVersion();
						_nativeFunction_UnityRenderEvent = Native.GetRenderEventFunc();
						if (_nativeFunction_UnityRenderEvent != IntPtr.Zero)
						{
							_isInitialised = true;
						}
					}
				}
				else
				{
					Debug.LogError("[AVProVideo] graphicsDeviceType not supported: " + SystemInfo.graphicsDeviceType);
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
		Native.Deinit();
		_isInitialised = false;
	}

	public override int GetAudioChannelCount()
	{
		return Native.GetAudioChannelCount(_instance);
	}

	public override AudioChannelMaskFlags GetAudioChannelMask()
	{
		return (AudioChannelMaskFlags)Native.GetAudioChannelMask(_instance);
	}

	public WindowsMediaPlayer(MediaPlayer.OptionsWindows options)
	{
		SetOptions(options);
	}

	public WindowsMediaPlayer(MediaPlayer.OptionsWindowsUWP options)
	{
		Windows.VideoApi videoApi = ((options.videoApi != 0) ? Windows.VideoApi.WinRT : Windows.VideoApi.MediaFoundation);
		Windows.AudioOutput audioMode = (Windows.AudioOutput)options._audioMode;
		SetOptions(videoApi, audioMode, options.useHardwareDecoding, options.useRendererSync, options.useTextureMips, options.use10BitTextures, hintAlphaChannel: false, options.useLowLatency, string.Empty, null, useCustomMovParser: false, 1, 0, 5u, useHapNotchLC: false, useStereoDetection: true, useTextTrackSupport: false, useFacebookAudio360Support: true, useAudioDelay: false);
	}

	public void SetOptions(MediaPlayer.OptionsWindows options)
	{
		SetOptions(options.videoApi, options._audioMode, options.useHardwareDecoding, options.useRendererSync, options.useTextureMips, options.use10BitTextures, options.hintAlphaChannel, options.useLowLatency, options.forceAudioOutputDeviceName, options.preferredFilters, options.useCustomMovParser, options.parallelFrameCount, options.prerollFrameCount, options.maxDropMode, options.useHapNotchLC, options.useStereoDetection, options.useTextTrackSupport, options.useFacebookAudio360Support, options.useAudioDelay);
	}

	public void SetOptions(Windows.VideoApi videoApi, Windows.AudioOutput audioOutput, bool useHardwareDecoding, bool useRendererSync, bool useTextureMips, bool use10BitTextures, bool hintAlphaChannel, bool useLowLatency, string audioDeviceOutputName, List<string> preferredFilters, bool useCustomMovParser, int parallelFrameCount, int prerollFrameCount, uint maxDropMode, bool useHapNotchLC, bool useStereoDetection, bool useTextTrackSupport, bool useFacebookAudio360Support, bool useAudioDelay)
	{
		_videoApi = videoApi;
		_audioOutput = audioOutput;
		_useHardwareDecoding = useHardwareDecoding;
		_useRendererSync = useRendererSync;
		_useTextureMips = useTextureMips;
		_use10BitTextures = use10BitTextures;
		_hintAlphaChannel = hintAlphaChannel;
		_useLowLatency = useLowLatency;
		_useStereoDetection = useStereoDetection;
		_useTextTrackSupport = useTextTrackSupport;
		_useFacebookAudio360Support = useFacebookAudio360Support;
		_useHapNotchLC = useHapNotchLC;
		_useCustomMovParser = useCustomMovParser;
		_decoderParallelFrameCount = parallelFrameCount;
		_decodePrerollFrameCount = prerollFrameCount;
		_decodeMaxDropMode = maxDropMode;
		_useAudioDelay = useAudioDelay;
		_audioDeviceOutputName = audioDeviceOutputName;
		if (!string.IsNullOrEmpty(_audioDeviceOutputName))
		{
			_audioDeviceOutputName = _audioDeviceOutputName.Trim();
		}
		_preferredFilters = preferredFilters;
		if (_preferredFilters == null)
		{
			return;
		}
		for (int i = 0; i < _preferredFilters.Count; i++)
		{
			if (!string.IsNullOrEmpty(_preferredFilters[i]))
			{
				_preferredFilters[i] = _preferredFilters[i].Trim();
			}
		}
	}

	public override string GetVersion()
	{
		return _version;
	}

	public override string GetExpectedVersion()
	{
		return "3.4.0";
	}

	private bool UseNativeMips()
	{
		return _useTextureMips;
	}

	public override bool OpenMedia(string path, long offset, string httpHeader, MediaHints mediaHints, int forceFileFormat = 0, bool startWithHighestBitrate = false)
	{
		uint numFilters = 0u;
		IntPtr[] array = null;
		if (_preferredFilters != null && _preferredFilters.Count > 0)
		{
			numFilters = (uint)_preferredFilters.Count;
			array = new IntPtr[_preferredFilters.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Marshal.StringToHGlobalUni(_preferredFilters[i]);
			}
		}
		_instance = Native.BeginOpenSource(_instance, _videoApi, _audioOutput, _useHardwareDecoding, _useRendererSync, UseNativeMips(), mediaHints.transparency == TransparencyMode.Transparent, _useLowLatency, _use10BitTextures, _audioDeviceOutputName, (_audioOutput == Windows.AudioOutput.Unity) ? Helper.GetUnityAudioSampleRate() : 0, array, numFilters, (int)_audio360ChannelMode, httpHeader, startWithHighestBitrate);
		if (_instance != IntPtr.Zero)
		{
			AuthenticationData = _nextAuthData;
			Native.SetCustomMovParserEnabled(_instance, _useCustomMovParser);
			Native.SetHapNotchLCEnabled(_instance, _useHapNotchLC);
			Native.SetStereoDetectEnabled(_instance, _useStereoDetection);
			Native.SetTextTrackSupportEnabled(_instance, _useTextTrackSupport);
			Native.SetAudioDelayEnabled(_instance, _useAudioDelay, isAutomatic: true, 0.0);
			Native.SetFacebookAudio360SupportEnabled(_instance, _useFacebookAudio360Support);
			Native.SetDecoderHints(_instance, _decoderParallelFrameCount, _decodePrerollFrameCount, _decodeMaxDropMode);
			_instance = Native.EndOpenSource(_instance, path);
			Native.SetLooping(_instance, _isLooping);
			Native.SetPlaybackRate(_instance, _playBackRate);
			Native.SetVolume(_instance, _volume);
		}
		if (array != null)
		{
			for (int j = 0; j < array.Length; j++)
			{
				Marshal.FreeHGlobal(array[j]);
			}
		}
		if (_instance == IntPtr.Zero)
		{
			DisplayLoadFailureSuggestion(path);
			return false;
		}
		_mediaHints = mediaHints;
		return true;
	}

	public override bool OpenMediaFromBuffer(byte[] buffer)
	{
		IntPtr[] array;
		if (_preferredFilters.Count == 0)
		{
			array = null;
		}
		else
		{
			array = new IntPtr[_preferredFilters.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Marshal.StringToHGlobalUni(_preferredFilters[i]);
			}
		}
		_instance = Native.OpenSourceFromBuffer(_instance, buffer, (ulong)buffer.Length, _videoApi, _audioOutput, _useHardwareDecoding, _useRendererSync, UseNativeMips(), _mediaHints.transparency == TransparencyMode.Transparent, _useLowLatency, _use10BitTextures, _audioDeviceOutputName, (_audioOutput == Windows.AudioOutput.Unity) ? Helper.GetUnityAudioSampleRate() : 0, array, (uint)_preferredFilters.Count);
		if (array != null)
		{
			for (int j = 0; j < array.Length; j++)
			{
				Marshal.FreeHGlobal(array[j]);
			}
		}
		if (_instance == IntPtr.Zero)
		{
			return false;
		}
		return true;
	}

	public override bool StartOpenMediaFromBuffer(ulong length)
	{
		CloseMedia();
		_instance = Native.StartOpenSourceFromBuffer(_instance, _videoApi, length);
		return _instance != IntPtr.Zero;
	}

	public override bool AddChunkToMediaBuffer(byte[] chunk, ulong offset, ulong length)
	{
		return Native.AddChunkToSourceBuffer(_instance, chunk, offset, length);
	}

	public override bool EndOpenMediaFromBuffer()
	{
		IntPtr[] array;
		if (_preferredFilters.Count == 0)
		{
			array = null;
		}
		else
		{
			array = new IntPtr[_preferredFilters.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Marshal.StringToHGlobalUni(_preferredFilters[i]);
			}
		}
		_instance = Native.EndOpenSourceFromBuffer(_instance, _audioOutput, _useHardwareDecoding, _useRendererSync, UseNativeMips(), _hintAlphaChannel, _useLowLatency, _use10BitTextures, _audioDeviceOutputName, (_audioOutput == Windows.AudioOutput.Unity) ? Helper.GetUnityAudioSampleRate() : 0, array, (uint)_preferredFilters.Count);
		if (array != null)
		{
			for (int j = 0; j < array.Length; j++)
			{
				Marshal.FreeHGlobal(array[j]);
			}
		}
		if (_instance == IntPtr.Zero)
		{
			return false;
		}
		return true;
	}

	private void DisplayLoadFailureSuggestion(string path)
	{
		if ((_videoApi == Windows.VideoApi.DirectShow || SystemInfo.operatingSystem.Contains("Windows 7") || SystemInfo.operatingSystem.Contains("Windows Vista") || SystemInfo.operatingSystem.Contains("Windows XP")) && path.Contains(".mp4"))
		{
			Debug.LogWarning("[AVProVideo] The native Windows DirectShow H.264 decoder doesn't support videos with resolution above 1920x1080. You may need to reduce your video resolution, switch to another codec (such as DivX or Hap), or install 3rd party DirectShow codec (eg LAV Filters).  This shouldn't be a problem for Windows 8 and above as it has a native limitation of 3840x2160.");
		}
	}

	public override void CloseMedia()
	{
		_width = 0;
		_height = 0;
		_frameRate = 0f;
		_hasAudio = (_hasVideo = false);
		_hasMetaData = false;
		_canPlay = false;
		_isPaused = true;
		_isPlaying = false;
		_isLooping = false;
		_audioMuted = false;
		_playBackRate = 1f;
		_volume = 1f;
		_balance = 0f;
		_supportsLinearColorSpace = true;
		ReleaseTexture();
		if (_instance != IntPtr.Zero)
		{
			Native.CloseSource(_instance);
			_instance = IntPtr.Zero;
		}
		IssueRenderThreadEvent(Native.RenderThreadEvent.FreeTextures);
		base.CloseMedia();
	}

	public override void SetLooping(bool looping)
	{
		_isLooping = looping;
		Native.SetLooping(_instance, looping);
	}

	public override bool IsLooping()
	{
		return _isLooping;
	}

	public override bool HasMetaData()
	{
		return _hasMetaData;
	}

	public override bool HasAudio()
	{
		return _hasAudio;
	}

	public override bool HasVideo()
	{
		return _hasVideo;
	}

	public override bool CanPlay()
	{
		return _canPlay;
	}

	public override void Play()
	{
		_isPlaying = true;
		_isPaused = false;
		Native.Play(_instance);
	}

	public override void Pause()
	{
		_isPlaying = false;
		_isPaused = true;
		Native.Pause(_instance);
	}

	public override void Stop()
	{
		Pause();
	}

	public override bool IsSeeking()
	{
		return Native.IsSeeking(_instance);
	}

	public override bool IsPlaying()
	{
		return Native.IsPlaying(_instance);
	}

	public override bool IsPaused()
	{
		if (!_isPaused)
		{
			if (_isPlaying)
			{
				return !Native.IsPlaying(_instance);
			}
			return false;
		}
		return true;
	}

	public override bool IsFinished()
	{
		bool result = false;
		if (!IsLooping())
		{
			result = Native.IsFinished(_instance);
		}
		return result;
	}

	public override bool IsBuffering()
	{
		return Native.IsBuffering(_instance);
	}

	public override double GetDuration()
	{
		return Native.GetDuration(_instance);
	}

	public override int GetVideoWidth()
	{
		return _width;
	}

	public override int GetVideoHeight()
	{
		return _height;
	}

	public override float GetVideoFrameRate()
	{
		return _frameRate;
	}

	public override Texture GetTexture(int index)
	{
		Texture result = null;
		if (GetTextureFrameCount() > 0)
		{
			result = ((!_resolvedTexture) ? ((Texture)_texture) : ((Texture)_resolvedTexture));
		}
		return result;
	}

	public override int GetTextureFrameCount()
	{
		return Native.GetTextureFrameCount(_instance);
	}

	public override long GetTextureTimeStamp()
	{
		return Native.GetTextureTimeStamp(_instance);
	}

	public override float GetTexturePixelAspectRatio()
	{
		if (_videoApi == Windows.VideoApi.DirectShow || _videoApi == Windows.VideoApi.MediaFoundation)
		{
			return Native.GetTexturePixelAspectRatio(_instance);
		}
		return 1f;
	}

	public override bool RequiresVerticalFlip()
	{
		return _isTextureTopDown;
	}

	internal override StereoPacking InternalGetTextureStereoPacking()
	{
		return Native.GetStereoPacking(_instance) switch
		{
			0 => StereoPacking.Monoscopic, 
			1 => StereoPacking.TopBottom, 
			2 => StereoPacking.LeftRight, 
			3 => StereoPacking.CustomUV, 
			4 => StereoPacking.MultiviewLeftPrimary, 
			_ => StereoPacking.Unknown, 
		};
	}

	public override void Seek(double time)
	{
		Native.SetCurrentTime(_instance, time, fast: false);
	}

	public override void SeekFast(double time)
	{
		Native.SetCurrentTime(_instance, time, fast: true);
	}

	public override double GetCurrentTime()
	{
		return Native.GetCurrentTime(_instance);
	}

	public override void SetPlaybackRate(float rate)
	{
		_playBackRate = rate;
		Native.SetPlaybackRate(_instance, rate);
	}

	public override float GetPlaybackRate()
	{
		return _playBackRate;
	}

	public override void MuteAudio(bool bMuted)
	{
		_audioMuted = bMuted;
		Native.SetMuted(_instance, _audioMuted);
	}

	public override bool IsMuted()
	{
		return _audioMuted;
	}

	public override void SetVolume(float volume)
	{
		_volume = volume;
		Native.SetVolume(_instance, volume);
	}

	public override float GetVolume()
	{
		return _volume;
	}

	public override void SetBalance(float balance)
	{
		_balance = balance;
		Native.SetBalance(_instance, balance);
	}

	public override float GetBalance()
	{
		return _balance;
	}

	public override bool IsPlaybackStalled()
	{
		bool flag = Native.IsPlaybackStalled(_instance);
		if (!flag)
		{
			flag = base.IsPlaybackStalled();
		}
		return flag;
	}

	public override bool WaitForNextFrame(Camera dummyCamera, int previousFrameCount)
	{
		Native.StartExtractFrame(_instance);
		IssueRenderThreadEvent(Native.RenderThreadEvent.WaitForNewFrame);
		dummyCamera.Render();
		Native.WaitForExtract(_instance);
		return previousFrameCount != Native.GetTextureFrameCount(_instance);
	}

	public override void SetAudioChannelMode(Audio360ChannelMode channelMode)
	{
		_audio360ChannelMode = channelMode;
		Native.SetAudioChannelMode(_instance, (int)channelMode);
	}

	public override void SetAudioHeadRotation(Quaternion q)
	{
		Native.SetHeadOrientation(_instance, q.x, q.y, q.z, q.w);
	}

	public override void ResetAudioHeadRotation()
	{
		Native.SetHeadOrientation(_instance, Quaternion.identity.x, Quaternion.identity.y, Quaternion.identity.z, Quaternion.identity.w);
	}

	public override void SetAudioFocusEnabled(bool enabled)
	{
		Native.SetAudioFocusEnabled(_instance, enabled);
	}

	public override void SetAudioFocusProperties(float offFocusLevel, float widthDegrees)
	{
		Native.SetAudioFocusProps(_instance, offFocusLevel, widthDegrees);
	}

	public override void SetAudioFocusRotation(Quaternion q)
	{
		Native.SetAudioFocusRotation(_instance, q.x, q.y, q.z, q.w);
	}

	public override void ResetAudioFocus()
	{
		Native.SetAudioFocusEnabled(_instance, enabled: false);
		Native.SetAudioFocusProps(_instance, 0f, 90f);
		Native.SetAudioFocusRotation(_instance, 0f, 0f, 0f, 1f);
	}

	public override void Update()
	{
		Native.Update(_instance);
		UpdateTracks();
		UpdateTextCue();
		_lastError = (ErrorCode)Native.GetLastErrorCode(_instance);
		UpdateTimeRanges();
		UpdateSubtitles();
		if (!_canPlay)
		{
			if (!_hasMetaData && Native.HasMetaData(_instance))
			{
				if (Native.HasVideo(_instance))
				{
					_width = Native.GetWidth(_instance);
					_height = Native.GetHeight(_instance);
					_frameRate = Native.GetFrameRate(_instance);
					if (_width > 0 && _height > 0)
					{
						_hasVideo = true;
						if (Mathf.Max(_width, _height) > SystemInfo.maxTextureSize)
						{
							Debug.LogError($"[AVProVideo] Video dimensions ({_width}x{_height}) larger than maxTextureSize ({SystemInfo.maxTextureSize} for current build target)");
							_width = (_height = 0);
							_hasVideo = false;
						}
					}
					if (_hasVideo && Native.HasAudio(_instance))
					{
						_hasAudio = true;
					}
				}
				else if (Native.HasAudio(_instance))
				{
					_hasAudio = true;
				}
				if (_hasVideo || _hasAudio)
				{
					_hasMetaData = true;
				}
				_playerDescription = Marshal.PtrToStringAnsi(Native.GetPlayerDescription(_instance));
				_supportsLinearColorSpace = Native.IsTextureSampleLinear(_instance);
				Helper.LogInfo("Using playback path: " + _playerDescription + " (" + _width + "x" + _height + "@" + GetVideoFrameRate().ToString("F2") + ")");
				if (_hasVideo)
				{
					OnTextureSizeChanged();
				}
			}
			if (_hasMetaData)
			{
				_canPlay = Native.CanPlay(_instance);
			}
		}
		if (_hasVideo)
		{
			IntPtr zero = IntPtr.Zero;
			zero = Native.GetTexturePointer(_instance);
			UpdateTexture(zero);
		}
		_playbackQualityStats.Update();
	}

	private void ReleaseTexture()
	{
		_nativeTexture = IntPtr.Zero;
		if ((bool)_resolvedTexture)
		{
			UnityEngine.Object.Destroy(_resolvedTexture);
		}
		if ((bool)_texture)
		{
			UnityEngine.Object.Destroy(_texture);
		}
		_resolvedTexture = null;
		_texture = null;
	}

	public override RenderTextureFormat GetCompatibleRenderTextureFormat(GetCompatibleRenderTextureFormatOptions options, int plane)
	{
		RenderTextureFormat result = RenderTextureFormat.Default;
		if (Native.GetTextureFormat(_instance) == 4 && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGB2101010))
		{
			result = RenderTextureFormat.ARGB2101010;
		}
		return result;
	}

	private void UpdateTexture(IntPtr newPtr)
	{
		if (_texture != null && _nativeTexture != IntPtr.Zero && _nativeTexture != newPtr)
		{
			_width = Native.GetWidth(_instance);
			_height = Native.GetHeight(_instance);
			if (newPtr == IntPtr.Zero)
			{
				ReleaseTexture();
			}
			else if (_width != _texture.width || _height != _texture.height)
			{
				Helper.LogInfo("Texture size changed: " + _width + " X " + _height);
				OnTextureSizeChanged();
				ReleaseTexture();
			}
			else if (_nativeTexture != newPtr)
			{
				if (newPtr != IntPtr.Zero)
				{
					_texture.UpdateExternalTexture(newPtr);
				}
				_nativeTexture = newPtr;
			}
		}
		if (_texture == null && _width > 0 && _height > 0 && newPtr != IntPtr.Zero)
		{
			_isTextureTopDown = Native.IsTextureTopDown(_instance);
			bool linear = !_supportsLinearColorSpace && QualitySettings.activeColorSpace == ColorSpace.Linear;
			int textureFormat = Native.GetTextureFormat(_instance);
			TextureFormat format = TextureFormat.RGBA32;
			switch (textureFormat)
			{
			case 3:
				format = TextureFormat.BGRA32;
				break;
			case 5:
				format = TextureFormat.RGBA64;
				break;
			case 4:
				format = TextureFormat.RGB24;
				break;
			case 8:
				format = TextureFormat.DXT1;
				break;
			case 9:
				format = TextureFormat.DXT5;
				break;
			case 12:
				format = TextureFormat.BC7;
				break;
			}
			_texture = Texture2D.CreateExternalTexture(_width, _height, format, UseNativeMips(), linear, newPtr);
			if (_texture != null)
			{
				_texture.name = "AVProVideo";
				_nativeTexture = newPtr;
				_playbackQualityStats.Start(this);
				ApplyTextureProperties(_texture);
				if (_useTextureMips && !UseNativeMips())
				{
					_resolvedTexture = new RenderTexture(_width, _height, 0);
					RenderTexture resolvedTexture = _resolvedTexture;
					bool useMipMap = (_resolvedTexture.autoGenerateMips = false);
					resolvedTexture.useMipMap = useMipMap;
				}
			}
			else
			{
				Debug.LogError("[AVProVideo] Failed to create texture");
			}
		}
		if ((bool)_texture && (bool)_resolvedTexture)
		{
			RenderTexture resolvedTexture2 = _resolvedTexture;
			bool useMipMap = (_resolvedTexture.autoGenerateMips = _useTextureMips);
			resolvedTexture2.useMipMap = useMipMap;
			Graphics.Blit(_texture, _resolvedTexture);
		}
	}

	public override void EndUpdate()
	{
		Native.EndUpdate(_instance);
	}

	public override long GetLastExtendedErrorCode()
	{
		return Native.GetLastExtendedErrorCode(_instance);
	}

	private void OnTextureSizeChanged()
	{
	}

	public override void BeginRender()
	{
		IssueRenderThreadEvent(Native.RenderThreadEvent.BeginRender);
	}

	public override void Render()
	{
		UpdateDisplayFrameRate();
		IssueRenderThreadEvent(Native.RenderThreadEvent.UpdateAllTextures);
	}

	public override void Dispose()
	{
	}

	public override int GrabAudio(float[] buffer, int sampleCount, int channelCount)
	{
		return Native.GrabAudio(_instance, buffer, sampleCount, channelCount);
	}

	public override int GetAudioBufferedSampleCount()
	{
		return Native.GetAudioBufferedSampleCount(_instance);
	}

	public override bool PlayerSupportsLinearColorSpace()
	{
		return _supportsLinearColorSpace;
	}

	public override bool GetDecoderPerformance(ref int activeDecodeThreadCount, ref int decodedFrameCount, ref int droppedFrameCount)
	{
		return Native.GetDecoderPerformance(_instance, ref activeDecodeThreadCount, ref decodedFrameCount, ref droppedFrameCount);
	}

	private static void IssueRenderThreadEvent(Native.RenderThreadEvent renderEvent)
	{
		if (renderEvent == Native.RenderThreadEvent.BeginRender || renderEvent == Native.RenderThreadEvent.UpdateAllTextures)
		{
			if (_lastUpdateAllTexturesFrame == Time.frameCount)
			{
				return;
			}
			if (renderEvent == Native.RenderThreadEvent.UpdateAllTextures)
			{
				_lastUpdateAllTexturesFrame = Time.frameCount;
			}
		}
		GL.IssuePluginEvent(_nativeFunction_UnityRenderEvent, (int)renderEvent);
	}

	private static string GetPluginVersion()
	{
		return Marshal.PtrToStringAnsi(Native.GetPluginVersion());
	}

	internal override bool InternalSetActiveTrack(TrackType trackType, int trackUid)
	{
		bool result = false;
		if ((uint)trackType <= 2u)
		{
			result = Native.SetActiveTrack(_instance, trackType, trackUid);
		}
		return result;
	}

	internal override bool InternalIsChangedTextCue()
	{
		return Native.IsChangedTextCue(_instance);
	}

	internal override string InternalGetCurrentTextCue()
	{
		string result = null;
		IntPtr currentTextCue = Native.GetCurrentTextCue(_instance);
		if (currentTextCue != IntPtr.Zero)
		{
			result = Marshal.PtrToStringUni(currentTextCue);
		}
		return result;
	}

	internal override bool InternalIsChangedTracks(TrackType trackType)
	{
		bool result = false;
		if ((uint)trackType <= 2u)
		{
			result = Native.IsChangedTracks(_instance, trackType);
		}
		return result;
	}

	internal override int InternalGetTrackCount(TrackType trackType)
	{
		int result = 0;
		if ((uint)trackType <= 2u)
		{
			result = Native.GetTrackCount(_instance, trackType);
		}
		return result;
	}

	internal override TrackBase InternalGetTrackInfo(TrackType trackType, int trackIndex, ref bool isActiveTrack)
	{
		TrackBase result = null;
		if ((uint)trackType <= 2u)
		{
			StringBuilder stringBuilder = new StringBuilder(128);
			StringBuilder stringBuilder2 = new StringBuilder(16);
			int uid = -1;
			if (Native.GetTrackInfo(_instance, trackType, trackIndex, ref uid, ref isActiveTrack, stringBuilder, stringBuilder.Capacity, stringBuilder2, stringBuilder2.Capacity))
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
		}
		return result;
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

	private void UpdateTimeRanges()
	{
		UpdateTimeRange(ref _seekableTimes._ranges, Native.TimeRangeTypes.Seekable);
		UpdateTimeRange(ref _bufferedTimes._ranges, Native.TimeRangeTypes.Buffered);
		_seekableTimes.CalculateRange();
		_bufferedTimes.CalculateRange();
	}

	private void UpdateTimeRange(ref TimeRange[] range, Native.TimeRangeTypes timeRangeType)
	{
		int timeRanges = Native.GetTimeRanges(_instance, range, range.Length, timeRangeType);
		if (timeRanges != range.Length)
		{
			range = new TimeRange[timeRanges];
			Native.GetTimeRanges(_instance, range, range.Length, timeRangeType);
		}
	}
}
