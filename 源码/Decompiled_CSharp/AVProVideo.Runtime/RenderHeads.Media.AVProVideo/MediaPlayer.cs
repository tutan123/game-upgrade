using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace RenderHeads.Media.AVProVideo;

[AddComponentMenu("AVPro Video/Media Player", -100)]
[ExecuteInEditMode]
[HelpURL("https://www.renderheads.com/products/avpro-video/")]
public class MediaPlayer : MonoBehaviour, ISerializationCallbackReceiver
{
	public delegate void ProcessExtractedFrame(Texture2D extractedFrame);

	[Serializable]
	public class PlatformOptions
	{
		public enum TextureFormat
		{
			BGRA = 0,
			YCbCr420_OES = 1,
			[Obsolete]
			YCbCr420 = 1
		}

		public enum Resolution
		{
			NoPreference,
			_480p,
			_720p,
			_1080p,
			_1440p,
			_2160p,
			Custom
		}

		public enum AudioMode
		{
			SystemDirect,
			Unity,
			SystemDirectWithCapture,
			FacebookAudio360
		}

		public enum BitRateUnits
		{
			bps,
			Kbps,
			Mbps
		}

		public HttpHeaderData httpHeaders = new HttpHeaderData();

		public KeyAuthData keyAuth = new KeyAuthData();

		public virtual bool IsUsingAudioCapture => false;

		public virtual bool IsModified()
		{
			if (!httpHeaders.IsModified())
			{
				return keyAuth.IsModified();
			}
			return true;
		}

		public virtual bool HasChanged()
		{
			return false;
		}

		public virtual void ClearChanges()
		{
		}

		public virtual string GetKeyServerAuthToken()
		{
			return keyAuth.keyServerToken;
		}

		public virtual byte[] GetOverrideDecryptionKey()
		{
			return keyAuth.overrideDecryptionKey;
		}

		public virtual bool StartWithHighestBandwidth()
		{
			return false;
		}
	}

	[Serializable]
	public class OptionsWindows : PlatformOptions, ISerializationCallbackReceiver
	{
		public Windows.VideoApi videoApi;

		public bool useHardwareDecoding = true;

		public bool useRendererSync = true;

		public bool useTextureMips;

		public bool use10BitTextures;

		public bool hintAlphaChannel;

		public bool useLowLatency;

		public bool useCustomMovParser;

		public bool useHapNotchLC;

		public bool useStereoDetection = true;

		public bool useTextTrackSupport = true;

		public bool useFacebookAudio360Support = true;

		public bool useAudioDelay;

		public string forceAudioOutputDeviceName = string.Empty;

		public List<string> preferredFilters = new List<string>();

		public Windows.AudioOutput _audioMode;

		public Audio360ChannelMode audio360ChannelMode;

		public bool startWithHighestBitrate;

		public bool useLowLiveLatency;

		[Range(1f, 64f)]
		public int parallelFrameCount = 8;

		[Range(1f, 16f)]
		public int prerollFrameCount = 4;

		[Range(0f, 5f)]
		public uint maxDropMode = 5u;

		[SerializeField]
		[HideInInspector]
		private bool useUnityAudio;

		[SerializeField]
		[HideInInspector]
		private bool enableAudio360;

		public override bool IsUsingAudioCapture => _audioMode == Windows.AudioOutput.Unity;

		public override bool IsModified()
		{
			if (!base.IsModified() && useHardwareDecoding && useRendererSync && !useTextureMips && !use10BitTextures && !hintAlphaChannel && !useLowLatency && !useCustomMovParser && !useHapNotchLC && useStereoDetection && useTextTrackSupport && useFacebookAudio360Support && !useAudioDelay && videoApi == Windows.VideoApi.MediaFoundation && _audioMode == Windows.AudioOutput.System && audio360ChannelMode == Audio360ChannelMode.TBE_8_2 && string.IsNullOrEmpty(forceAudioOutputDeviceName) && preferredFilters.Count == 0 && !startWithHighestBitrate && !useLowLiveLatency && parallelFrameCount == 8 && prerollFrameCount == 4)
			{
				return maxDropMode != 5;
			}
			return true;
		}

		public override bool StartWithHighestBandwidth()
		{
			return startWithHighestBitrate;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (useUnityAudio && _audioMode == Windows.AudioOutput.System)
			{
				_audioMode = Windows.AudioOutput.Unity;
				useUnityAudio = false;
			}
			if (enableAudio360 && _audioMode == Windows.AudioOutput.System)
			{
				_audioMode = Windows.AudioOutput.FacebookAudio360;
				enableAudio360 = false;
			}
		}
	}

	[Serializable]
	public class OptionsWindowsUWP : PlatformOptions
	{
		public bool useHardwareDecoding = true;

		public bool useRendererSync = true;

		public bool useTextureMips;

		public bool use10BitTextures;

		public bool hintOutput10Bit;

		public bool useLowLatency;

		public WindowsUWP.VideoApi videoApi = WindowsUWP.VideoApi.WinRT;

		public WindowsUWP.AudioOutput _audioMode;

		public Audio360ChannelMode audio360ChannelMode;

		public bool startWithHighestBitrate;

		public bool useLowLiveLatency;

		public override bool IsUsingAudioCapture => _audioMode == WindowsUWP.AudioOutput.Unity;

		public override bool IsModified()
		{
			if (!base.IsModified() && useHardwareDecoding && useRendererSync && !useTextureMips && !use10BitTextures && !useLowLatency && _audioMode == WindowsUWP.AudioOutput.System && audio360ChannelMode == Audio360ChannelMode.TBE_8_2 && videoApi == WindowsUWP.VideoApi.WinRT && !startWithHighestBitrate)
			{
				return useLowLiveLatency;
			}
			return true;
		}

		public override bool StartWithHighestBandwidth()
		{
			return startWithHighestBitrate;
		}
	}

	[Serializable]
	public class OptionsApple : PlatformOptions
	{
		[Flags]
		public enum Flags
		{
			None = 0,
			GenerateMipMaps = 1,
			AllowExternalPlayback = 0x100,
			PlayWithoutBuffering = 0x200,
			UseSinglePlayerItem = 0x400,
			ResumeMediaPlaybackAfterAudioSessionRouteChange = 0x10000
		}

		[Flags]
		public enum ChangeFlags
		{
			None = 0,
			PreferredPeakBitRate = 2,
			PreferredForwardBufferDuration = 4,
			PlayWithoutBuffering = 8,
			PreferredMaximumResolution = 0x10,
			AudioMode = 0x20,
			ResumeMediaPlaybackAfterAudioSessionRouteChange = 0x40,
			All = -1
		}

		private readonly TextureFormat DefaultTextureFormat;

		private readonly Flags DefaultFlags;

		public TextureFormat textureFormat;

		private AudioMode _previousAudioMode;

		[SerializeField]
		private AudioMode _audioMode;

		[SerializeField]
		private Flags _flags;

		public float maximumPlaybackRate = 2f;

		private ChangeFlags _changed;

		[SerializeField]
		private float _preferredPeakBitRate;

		[SerializeField]
		private BitRateUnits _preferredPeakBitRateUnits = BitRateUnits.Kbps;

		[SerializeField]
		private double _preferredForwardBufferDuration;

		[SerializeField]
		private Resolution _preferredMaximumResolution;

		[SerializeField]
		private Vector2Int _customPreferredMaximumResolution = Vector2Int.zero;

		public AudioMode previousAudioMode => _previousAudioMode;

		public AudioMode audioMode
		{
			get
			{
				return _audioMode;
			}
			set
			{
				if (_audioMode != value)
				{
					_previousAudioMode = _audioMode;
					_audioMode = value;
					_changed |= ChangeFlags.AudioMode;
				}
			}
		}

		public override bool IsUsingAudioCapture
		{
			get
			{
				if (_audioMode != AudioMode.Unity)
				{
					return _audioMode == AudioMode.SystemDirectWithCapture;
				}
				return true;
			}
		}

		public Flags flags
		{
			get
			{
				return _flags;
			}
			set
			{
				Flags flags = _flags ^ value;
				if (flags != 0)
				{
					if ((flags & Flags.PlayWithoutBuffering) == Flags.PlayWithoutBuffering)
					{
						_changed |= ChangeFlags.PlayWithoutBuffering;
					}
					if ((flags & Flags.ResumeMediaPlaybackAfterAudioSessionRouteChange) == Flags.ResumeMediaPlaybackAfterAudioSessionRouteChange)
					{
						_changed |= ChangeFlags.ResumeMediaPlaybackAfterAudioSessionRouteChange;
					}
					_flags = value;
				}
			}
		}

		public float preferredPeakBitRate
		{
			get
			{
				return _preferredPeakBitRate;
			}
			set
			{
				if (_preferredPeakBitRate != value)
				{
					_changed |= ChangeFlags.PreferredPeakBitRate;
					_preferredPeakBitRate = value;
				}
			}
		}

		public BitRateUnits preferredPeakBitRateUnits
		{
			get
			{
				return _preferredPeakBitRateUnits;
			}
			set
			{
				if (_preferredPeakBitRateUnits != value)
				{
					_changed |= ChangeFlags.PreferredPeakBitRate;
					_preferredPeakBitRateUnits = value;
				}
			}
		}

		public double preferredForwardBufferDuration
		{
			get
			{
				return _preferredForwardBufferDuration;
			}
			set
			{
				if (_preferredForwardBufferDuration != value)
				{
					_changed |= ChangeFlags.PreferredForwardBufferDuration;
					_preferredForwardBufferDuration = value;
				}
			}
		}

		public Resolution preferredMaximumResolution
		{
			get
			{
				return _preferredMaximumResolution;
			}
			set
			{
				if (_preferredMaximumResolution != value)
				{
					_changed |= ChangeFlags.PreferredMaximumResolution;
					_preferredMaximumResolution = value;
				}
			}
		}

		public Vector2Int customPreferredMaximumResolution
		{
			get
			{
				return _customPreferredMaximumResolution;
			}
			set
			{
				if (_customPreferredMaximumResolution != value)
				{
					_changed |= ChangeFlags.PreferredMaximumResolution;
					_customPreferredMaximumResolution = value;
				}
			}
		}

		private static double BitRateInBitsPerSecond(float value, BitRateUnits units)
		{
			return units switch
			{
				BitRateUnits.bps => value, 
				BitRateUnits.Kbps => (double)value * 1000.0, 
				BitRateUnits.Mbps => (double)value * 1000000.0, 
				_ => 0.0, 
			};
		}

		public double GetPreferredPeakBitRateInBitsPerSecond()
		{
			return BitRateInBitsPerSecond(preferredPeakBitRate, preferredPeakBitRateUnits);
		}

		public OptionsApple(TextureFormat defaultTextureFormat, Flags defaultFlags)
		{
			DefaultTextureFormat = defaultTextureFormat;
			DefaultFlags = defaultFlags;
			textureFormat = defaultTextureFormat;
			audioMode = AudioMode.SystemDirect;
			flags = defaultFlags;
		}

		public override bool IsModified()
		{
			if (!base.IsModified() && textureFormat == DefaultTextureFormat && audioMode == AudioMode.SystemDirect && flags == DefaultFlags && preferredMaximumResolution == Resolution.NoPreference && preferredPeakBitRate == 0f)
			{
				return preferredForwardBufferDuration != 0.0;
			}
			return true;
		}

		public override bool HasChanged()
		{
			return HasChanged(ChangeFlags.All);
		}

		public bool HasChanged(ChangeFlags flags)
		{
			return (_changed & flags) != 0;
		}

		public override void ClearChanges()
		{
			_changed = ChangeFlags.None;
		}
	}

	[Serializable]
	public class OptionsAndroid : PlatformOptions, ISerializationCallbackReceiver
	{
		public enum VideoOutputMode
		{
			Texture
		}

		[Flags]
		public enum ChangeFlags
		{
			None = 0,
			PreferredPeakBitRate = 2,
			PreferredMaximumResolution = 4,
			PreferredCustomResolution = 8,
			AudioMode = 0x10,
			GenerateMipmaps = 0x20,
			AllowUnsupportedVideoTrackVariants = 0x40,
			All = -1
		}

		private ChangeFlags _changed;

		public VideoOutputMode videoOutputMode;

		private readonly TextureFormat DefaultTextureFormat;

		public TextureFormat textureFormat;

		[SerializeField]
		private bool _generateMipmaps;

		private AudioMode _previousAudioMode;

		[SerializeField]
		private AudioMode _audioMode;

		[SerializeField]
		private Resolution _preferredMaximumResolution;

		[SerializeField]
		private Vector2Int _customPreferredMaximumResolution = Vector2Int.zero;

		[SerializeField]
		private float _preferredPeakBitRate;

		[SerializeField]
		private BitRateUnits _preferredPeakBitRateUnits = BitRateUnits.Kbps;

		[SerializeField]
		private bool _allowUnsupportedVideoTrackVariants;

		public Android.VideoApi videoApi = Android.VideoApi.ExoPlayer;

		public bool showPosterFrame;

		public Audio360ChannelMode audio360ChannelMode;

		public int audio360LatencyMS;

		public bool preferSoftwareDecoder;

		public bool forceRtpTCP;

		public bool forceEnableMediaCodecAsynchronousQueueing;

		[Tooltip("Byte offset into the file where the media file is located.  This is useful when hiding or packing media files within another file.")]
		[SerializeField]
		public int fileOffset;

		public bool startWithHighestBitrate;

		public int minBufferMs = 10000;

		public int maxBufferMs = 50000;

		public int bufferForPlaybackMs = 1000;

		public int bufferForPlaybackAfterRebufferMs = 2000;

		public bool prioritiseTimeOverSize;

		[Obsolete("useFastOesPath is deprecated and replaced with TextureFormat")]
		public bool useFastOesPath;

		[Obsolete("audioOutput is deprecated and replaced with audioMode")]
		public int audioOutput;

		[Obsolete("blitTextureFiltering is deprecated and its functionality has been removed")]
		public int blitTextureFiltering;

		[Obsolete("forceEnableMediaCodecAsyncQueueing is deprecated and replaced with forceEnableMediaCodecAsynchronousQueueing")]
		public bool forceEnableMediaCodecAsyncQueueing;

		[SerializeField]
		[HideInInspector]
		private bool enableAudio360;

		public bool generateMipmaps
		{
			get
			{
				return _generateMipmaps;
			}
			set
			{
				if (value != _generateMipmaps)
				{
					_generateMipmaps = value;
					_changed |= ChangeFlags.GenerateMipmaps;
				}
			}
		}

		public AudioMode previousAudioMode => _previousAudioMode;

		public AudioMode audioMode
		{
			get
			{
				return _audioMode;
			}
			set
			{
				if (_audioMode != value)
				{
					_previousAudioMode = _audioMode;
					_audioMode = value;
					_changed |= ChangeFlags.AudioMode;
				}
			}
		}

		public override bool IsUsingAudioCapture
		{
			get
			{
				if (_audioMode != AudioMode.Unity)
				{
					return _audioMode == AudioMode.SystemDirectWithCapture;
				}
				return true;
			}
		}

		public Resolution preferredMaximumResolution
		{
			get
			{
				return _preferredMaximumResolution;
			}
			set
			{
				if (_preferredMaximumResolution != value)
				{
					_changed |= ChangeFlags.PreferredMaximumResolution;
					_preferredMaximumResolution = value;
				}
			}
		}

		public Vector2Int customPreferredMaximumResolution
		{
			get
			{
				return _customPreferredMaximumResolution;
			}
			set
			{
				if (_customPreferredMaximumResolution != value)
				{
					_changed |= ChangeFlags.PreferredCustomResolution;
					_customPreferredMaximumResolution = value;
				}
			}
		}

		public float preferredPeakBitRate
		{
			get
			{
				return _preferredPeakBitRate;
			}
			set
			{
				if (_preferredPeakBitRate != value)
				{
					_changed |= ChangeFlags.PreferredPeakBitRate;
					_preferredPeakBitRate = value;
				}
			}
		}

		public BitRateUnits preferredPeakBitRateUnits
		{
			get
			{
				return _preferredPeakBitRateUnits;
			}
			set
			{
				if (_preferredPeakBitRateUnits != value)
				{
					_changed |= ChangeFlags.PreferredPeakBitRate;
					_preferredPeakBitRateUnits = value;
				}
			}
		}

		public bool allowUnsupportedVideoTrackVariants
		{
			get
			{
				return _allowUnsupportedVideoTrackVariants;
			}
			set
			{
				if (_allowUnsupportedVideoTrackVariants != value)
				{
					_changed |= ChangeFlags.AllowUnsupportedVideoTrackVariants;
					_allowUnsupportedVideoTrackVariants = value;
				}
			}
		}

		public override bool IsModified()
		{
			if (!base.IsModified() && videoOutputMode == VideoOutputMode.Texture && fileOffset == 0 && textureFormat == DefaultTextureFormat && audioMode == AudioMode.SystemDirect && videoApi == Android.VideoApi.ExoPlayer && audio360ChannelMode == Audio360ChannelMode.TBE_8_2 && audio360LatencyMS == 0 && !preferSoftwareDecoder && !forceRtpTCP && !forceEnableMediaCodecAsynchronousQueueing && !allowUnsupportedVideoTrackVariants && !startWithHighestBitrate && minBufferMs == 10000 && maxBufferMs == 50000 && bufferForPlaybackMs == 1000 && bufferForPlaybackAfterRebufferMs == 2000 && !prioritiseTimeOverSize && preferredMaximumResolution == Resolution.NoPreference)
			{
				return preferredPeakBitRate != 0f;
			}
			return true;
		}

		private static double BitRateInBitsPerSecond(float value, BitRateUnits units)
		{
			return units switch
			{
				BitRateUnits.bps => value, 
				BitRateUnits.Kbps => (double)value * 1000.0, 
				BitRateUnits.Mbps => (double)value * 1000000.0, 
				_ => 0.0, 
			};
		}

		public double GetPreferredPeakBitRateInBitsPerSecond()
		{
			_changed &= ~ChangeFlags.PreferredPeakBitRate;
			return BitRateInBitsPerSecond(preferredPeakBitRate, preferredPeakBitRateUnits);
		}

		public override bool StartWithHighestBandwidth()
		{
			return startWithHighestBitrate;
		}

		public override bool HasChanged()
		{
			return HasChanged(ChangeFlags.All);
		}

		public bool HasChanged(ChangeFlags flags, bool bClearFlags = false)
		{
			bool result = (_changed & flags) != 0;
			if (bClearFlags)
			{
				_changed = ChangeFlags.None;
			}
			return result;
		}

		public override void ClearChanges()
		{
			_changed = ChangeFlags.None;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			if (enableAudio360 && audioMode == AudioMode.SystemDirect)
			{
				audioMode = AudioMode.FacebookAudio360;
				enableAudio360 = false;
			}
		}
	}

	[Serializable]
	public class OptionsOpenHarmony : PlatformOptions, ISerializationCallbackReceiver
	{
		[Flags]
		public enum ChangeFlags
		{
			None = 0,
			PreferredPeakBitRate = 2,
			PreferredMaximumResolution = 4,
			PreferredCustomResolution = 8,
			AudioMode = 0x10,
			GenerateMipmaps = 0x20,
			UseNormalizedOHMUrl = 0x40,
			All = -1
		}

		private ChangeFlags _changed;

		private readonly TextureFormat DefaultTextureFormat;

		public TextureFormat textureFormat;

		[SerializeField]
		private bool _generateMipmapsOH;

		[SerializeField]
		private bool _useNormalizedOHMUrl = true;

		private AudioMode _previousAudioMode;

		[SerializeField]
		private AudioMode _audioMode;

		[SerializeField]
		private Resolution _preferredMaximumResolution;

		[SerializeField]
		private Vector2Int _customPreferredMaximumResolution = Vector2Int.zero;

		public bool generateMipmaps
		{
			get
			{
				return _generateMipmapsOH;
			}
			set
			{
				if (value != _generateMipmapsOH)
				{
					_generateMipmapsOH = value;
					_changed |= ChangeFlags.GenerateMipmaps;
				}
			}
		}

		public bool useNormalizedOHMUrl
		{
			get
			{
				return _useNormalizedOHMUrl;
			}
			set
			{
				if (value != _useNormalizedOHMUrl)
				{
					_useNormalizedOHMUrl = value;
					_changed |= ChangeFlags.GenerateMipmaps;
				}
			}
		}

		public AudioMode previousAudioMode => _previousAudioMode;

		public AudioMode audioMode
		{
			get
			{
				return _audioMode;
			}
			set
			{
				if (_audioMode != value)
				{
					_previousAudioMode = _audioMode;
					_audioMode = value;
					_changed |= ChangeFlags.AudioMode;
				}
			}
		}

		public override bool IsUsingAudioCapture
		{
			get
			{
				if (_audioMode != AudioMode.Unity)
				{
					return _audioMode == AudioMode.SystemDirectWithCapture;
				}
				return true;
			}
		}

		public Resolution preferredMaximumResolution
		{
			get
			{
				return _preferredMaximumResolution;
			}
			set
			{
				if (_preferredMaximumResolution != value)
				{
					_changed |= ChangeFlags.PreferredMaximumResolution;
					_preferredMaximumResolution = value;
				}
			}
		}

		public Vector2Int customPreferredMaximumResolution
		{
			get
			{
				return _customPreferredMaximumResolution;
			}
			set
			{
				if (_customPreferredMaximumResolution != value)
				{
					_changed |= ChangeFlags.PreferredCustomResolution;
					_customPreferredMaximumResolution = value;
				}
			}
		}

		public override bool IsModified()
		{
			if (!base.IsModified() && textureFormat == DefaultTextureFormat)
			{
				return audioMode != AudioMode.SystemDirect;
			}
			return true;
		}

		private static double BitRateInBitsPerSecond(float value, BitRateUnits units)
		{
			return units switch
			{
				BitRateUnits.bps => value, 
				BitRateUnits.Kbps => (double)value * 1000.0, 
				BitRateUnits.Mbps => (double)value * 1000000.0, 
				_ => 0.0, 
			};
		}

		public override bool HasChanged()
		{
			return HasChanged(ChangeFlags.All);
		}

		public bool HasChanged(ChangeFlags flags, bool bClearFlags = false)
		{
			bool result = (_changed & flags) != 0;
			if (bClearFlags)
			{
				_changed = ChangeFlags.None;
			}
			return result;
		}

		public override void ClearChanges()
		{
			_changed = ChangeFlags.None;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
		}
	}

	[Serializable]
	public class OptionsWebGL : PlatformOptions
	{
		public enum ChangeFlags
		{
			None = 0,
			PreferredPeakBitRate = 2,
			PreferredMaximumResolution = 4,
			PreferredCustomResolution = 8,
			AudioMode = 16,
			GenerateMipmaps = 32,
			All = -1
		}

		private ChangeFlags _changed;

		public WebGL.ExternalLibrary externalLibrary;

		public bool useTextureMips;

		private AudioMode _previousAudioMode;

		[SerializeField]
		private AudioMode _audioMode;

		public AudioMode previousAudioMode => _previousAudioMode;

		public AudioMode audioMode
		{
			get
			{
				return _audioMode;
			}
			set
			{
				if (_audioMode != value)
				{
					_previousAudioMode = _audioMode;
					_audioMode = value;
					_changed |= ChangeFlags.AudioMode;
				}
			}
		}

		public override bool IsUsingAudioCapture
		{
			get
			{
				if (_audioMode != AudioMode.Unity)
				{
					return _audioMode == AudioMode.SystemDirectWithCapture;
				}
				return true;
			}
		}

		public override bool IsModified()
		{
			if (!base.IsModified() && externalLibrary == WebGL.ExternalLibrary.None)
			{
				return useTextureMips;
			}
			return true;
		}

		public override bool HasChanged()
		{
			return HasChanged(ChangeFlags.All);
		}

		public bool HasChanged(ChangeFlags flags)
		{
			return (_changed & flags) != 0;
		}

		public override void ClearChanges()
		{
			_changed = ChangeFlags.None;
		}

		public override string GetKeyServerAuthToken()
		{
			return null;
		}

		public override byte[] GetOverrideDecryptionKey()
		{
			return null;
		}
	}

	private enum FileLocation
	{
		AbsolutePathOrURL,
		RelativeToProjectFolder,
		RelativeToStreamingAssetsFolder,
		RelativeToDataFolder,
		RelativeToPersistentDataFolder
	}

	[SerializeField]
	private MediaSource _mediaSource;

	[SerializeField]
	private MediaReference _mediaReference;

	[SerializeField]
	private MediaPath _mediaPath = new MediaPath();

	[SerializeField]
	private MediaHints _fallbackMediaHints = MediaHints.Default;

	[SerializeField]
	[FormerlySerializedAs("m_AutoOpen")]
	private bool _autoOpen = true;

	[SerializeField]
	[FormerlySerializedAs("m_AutoStart")]
	private bool _autoPlayOnStart = true;

	[FormerlySerializedAs("m_Loop")]
	[SerializeField]
	private bool _loop;

	[FormerlySerializedAs("m_Volume")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _audioVolume = 1f;

	[Range(-1f, 1f)]
	[FormerlySerializedAs("m_Balance")]
	[SerializeField]
	private float _audioBalance;

	[FormerlySerializedAs("m_Muted")]
	[SerializeField]
	private bool _audioMuted;

	private AudioSource _audioSource;

	[FormerlySerializedAs("m_PlaybackRate")]
	[Range(-4f, 4f)]
	[SerializeField]
	private float _playbackRate = 1f;

	[SerializeField]
	[FormerlySerializedAs("m_Resample")]
	private bool _useResampler;

	[FormerlySerializedAs("m_ResampleMode")]
	[SerializeField]
	private Resampler.ResampleMode _resampleMode;

	[SerializeField]
	[Range(3f, 10f)]
	[FormerlySerializedAs("m_ResampleBufferSize")]
	private int _resampleBufferSize = 5;

	private Resampler _resampler;

	[SerializeField]
	[FormerlySerializedAs("m_videoMapping")]
	private VideoMapping _videoMapping;

	[SerializeField]
	[FormerlySerializedAs("m_FilterMode")]
	private FilterMode _textureFilterMode = FilterMode.Bilinear;

	[SerializeField]
	[FormerlySerializedAs("m_WrapMode")]
	private TextureWrapMode _textureWrapMode = TextureWrapMode.Clamp;

	[FormerlySerializedAs("m_AnisoLevel")]
	[Range(0f, 16f)]
	[SerializeField]
	private int _textureAnisoLevel;

	[SerializeField]
	[FormerlySerializedAs("m_LoadSubtitles")]
	private bool _sideloadSubtitles;

	[SerializeField]
	private MediaPath _subtitlePath;

	[FormerlySerializedAs("m_AudioHeadTransform")]
	[SerializeField]
	private Transform _audioHeadTransform;

	[FormerlySerializedAs("m_AudioFocusEnabled")]
	[SerializeField]
	private bool _audioFocusEnabled;

	[FormerlySerializedAs("m_AudioFocusTransform")]
	[SerializeField]
	private Transform _audioFocusTransform;

	[FormerlySerializedAs("m_AudioFocusWidthDegrees")]
	[SerializeField]
	[Range(40f, 120f)]
	private float _audioFocusWidthDegrees = 90f;

	[Range(-24f, 0f)]
	[SerializeField]
	[FormerlySerializedAs("m_AudioFocusOffLevelDB")]
	private float _audioFocusOffLevelDB;

	[SerializeField]
	private HttpHeaderData _httpHeaders = new HttpHeaderData();

	[SerializeField]
	private KeyAuthData _keyAuth = new KeyAuthData();

	[SerializeField]
	[FormerlySerializedAs("m_events")]
	private MediaPlayerEvent _events;

	[SerializeField]
	[FormerlySerializedAs("m_eventMask")]
	private int _eventMask = -1;

	[SerializeField]
	private bool _pauseMediaOnAppPause = true;

	[SerializeField]
	private bool _playMediaOnAppUnpause = true;

	[FormerlySerializedAs("m_Persistent")]
	[SerializeField]
	private bool _persistent;

	[SerializeField]
	[FormerlySerializedAs("m_forceFileFormat")]
	private FileFormat _forceFileFormat;

	protected BaseMediaPlayer _baseMediaPlayer;

	private IMediaControl _controlInterface;

	private ITextureProducer _textureInterface;

	private IMediaInfo _infoInterface;

	private IMediaPlayer _playerInterface;

	private IMediaSubtitles _subtitlesInterface;

	private IMediaCache _cacheInterface;

	private IVideoTracks _videoTracksInterface;

	private IAudioTracks _audioTracksInterface;

	private ITextTracks _textTracksInterface;

	private ITimedMetadata _timedMetadataInterface;

	private IVariants _variantsInterface;

	private IDisposable _disposeInterface;

	private bool _isMediaOpened;

	private bool _autoPlayOnStartTriggered;

	private bool _wasPlayingOnPause;

	private Coroutine _renderingCoroutine;

	private static bool s_GlobalStartup;

	private static bool s_TrialVersion;

	private MediaPath _queueSubtitlePath;

	private Coroutine _loadSubtitlesRoutine;

	private static Camera _dummyCamera;

	private bool _eventFired_MetaDataReady;

	private bool _eventFired_ReadyToPlay;

	private bool _eventFired_Started;

	private bool _eventFired_FirstFrameReady;

	private bool _eventFired_FinishedPlaying;

	private bool _eventState_PlaybackBuffering;

	private bool _eventState_PlaybackSeeking;

	private bool _eventState_PlaybackStalled;

	private int _eventState_PreviousWidth;

	private int _eventState_PreviousHeight;

	private int _previousSubtitleIndex = -1;

	private bool _finishedFrameOpenCheck;

	private bool _eventState_Paused;

	[SerializeField]
	private OptionsWindows _optionsWindows = new OptionsWindows();

	[SerializeField]
	private OptionsApple _options_macOS = new OptionsApple(PlatformOptions.TextureFormat.BGRA, OptionsApple.Flags.None);

	[SerializeField]
	private OptionsApple _options_iOS = new OptionsApple(PlatformOptions.TextureFormat.BGRA, OptionsApple.Flags.None);

	[SerializeField]
	private OptionsApple _options_tvOS = new OptionsApple(PlatformOptions.TextureFormat.BGRA, OptionsApple.Flags.None);

	[SerializeField]
	private OptionsApple _options_visionOS = new OptionsApple(PlatformOptions.TextureFormat.BGRA, OptionsApple.Flags.None);

	[SerializeField]
	private OptionsAndroid _optionsAndroid = new OptionsAndroid();

	[SerializeField]
	private OptionsOpenHarmony _optionsOpenHarmony = new OptionsOpenHarmony();

	[SerializeField]
	private OptionsWindowsUWP _optionsWindowsUWP = new OptionsWindowsUWP();

	[SerializeField]
	private OptionsWebGL _optionsWebGL = new OptionsWebGL();

	[HideInInspector]
	[SerializeField]
	private string m_VideoPath;

	[SerializeField]
	[HideInInspector]
	private FileLocation m_VideoLocation = FileLocation.RelativeToStreamingAssetsFolder;

	public MediaSource MediaSource
	{
		get
		{
			return _mediaSource;
		}
		internal set
		{
			_mediaSource = value;
		}
	}

	public MediaReference MediaReference
	{
		get
		{
			return _mediaReference;
		}
		internal set
		{
			_mediaReference = value;
		}
	}

	public MediaPath MediaPath
	{
		get
		{
			return _mediaPath;
		}
		internal set
		{
			_mediaPath = value;
		}
	}

	public MediaHints FallbackMediaHints
	{
		get
		{
			return _fallbackMediaHints;
		}
		set
		{
			_fallbackMediaHints = value;
		}
	}

	public bool AutoOpen
	{
		get
		{
			return _autoOpen;
		}
		set
		{
			_autoOpen = value;
		}
	}

	public bool AutoStart
	{
		get
		{
			return _autoPlayOnStart;
		}
		set
		{
			_autoPlayOnStart = value;
		}
	}

	public bool Loop
	{
		get
		{
			if (_controlInterface == null)
			{
				return _loop;
			}
			return _controlInterface.IsLooping();
		}
		set
		{
			_loop = value;
			if (_controlInterface != null)
			{
				_controlInterface.SetLooping(_loop);
			}
		}
	}

	public virtual float AudioVolume
	{
		get
		{
			if (_controlInterface == null)
			{
				return _audioVolume;
			}
			return _controlInterface.GetVolume();
		}
		set
		{
			_audioVolume = Mathf.Clamp01(value);
			if (_controlInterface != null)
			{
				_controlInterface.SetVolume(_audioVolume);
			}
		}
	}

	public float AudioBalance
	{
		get
		{
			if (_controlInterface == null)
			{
				return _audioBalance;
			}
			return _controlInterface.GetBalance();
		}
		set
		{
			_audioBalance = Mathf.Clamp(value, -1f, 1f);
			if (_controlInterface != null)
			{
				_controlInterface.SetBalance(_audioBalance);
			}
		}
	}

	public virtual bool AudioMuted
	{
		get
		{
			if (_controlInterface == null)
			{
				return _audioMuted;
			}
			return _controlInterface.IsMuted();
		}
		set
		{
			_audioMuted = value;
			if (_controlInterface != null)
			{
				_controlInterface.MuteAudio(_audioMuted);
			}
		}
	}

	public AudioSource AudioSource
	{
		get
		{
			return _audioSource;
		}
		internal set
		{
			_audioSource = value;
		}
	}

	public float PlaybackRate
	{
		get
		{
			if (_controlInterface == null)
			{
				return _playbackRate;
			}
			return _controlInterface.GetPlaybackRate();
		}
		set
		{
			_playbackRate = value;
			if (_controlInterface != null)
			{
				_controlInterface.SetPlaybackRate(_playbackRate);
			}
		}
	}

	public bool UseResampler
	{
		get
		{
			return _useResampler;
		}
		set
		{
			_useResampler = value;
		}
	}

	public Resampler.ResampleMode ResampleMode
	{
		get
		{
			return _resampleMode;
		}
		set
		{
			_resampleMode = value;
		}
	}

	public int ResampleBufferSize
	{
		get
		{
			return _resampleBufferSize;
		}
		set
		{
			_resampleBufferSize = value;
		}
	}

	public Resampler FrameResampler => _resampler;

	public VideoMapping VideoLayoutMapping
	{
		get
		{
			return _videoMapping;
		}
		set
		{
			_videoMapping = value;
		}
	}

	public FilterMode TextureFilterMode
	{
		get
		{
			if (_controlInterface != null)
			{
				FilterMode filterMode = FilterMode.Point;
				TextureWrapMode wrapMode = TextureWrapMode.Repeat;
				int anisoLevel = 0;
				_controlInterface.GetTextureProperties(out filterMode, out wrapMode, out anisoLevel);
				return filterMode;
			}
			return _textureFilterMode;
		}
		set
		{
			_textureFilterMode = value;
			if (_controlInterface != null)
			{
				_controlInterface.SetTextureProperties(_textureFilterMode, _textureWrapMode, _textureAnisoLevel);
			}
		}
	}

	public TextureWrapMode TextureWrapMode
	{
		get
		{
			if (_controlInterface != null)
			{
				FilterMode filterMode = FilterMode.Point;
				TextureWrapMode wrapMode = TextureWrapMode.Repeat;
				int anisoLevel = 0;
				_controlInterface.GetTextureProperties(out filterMode, out wrapMode, out anisoLevel);
				return wrapMode;
			}
			return _textureWrapMode;
		}
		set
		{
			_textureWrapMode = value;
			if (_controlInterface != null)
			{
				_controlInterface.SetTextureProperties(_textureFilterMode, _textureWrapMode, _textureAnisoLevel);
			}
		}
	}

	public int TextureAnisoLevel
	{
		get
		{
			if (_controlInterface != null)
			{
				FilterMode filterMode = FilterMode.Point;
				TextureWrapMode wrapMode = TextureWrapMode.Repeat;
				int anisoLevel = 0;
				_controlInterface.GetTextureProperties(out filterMode, out wrapMode, out anisoLevel);
				return anisoLevel;
			}
			return _textureAnisoLevel;
		}
		set
		{
			_textureAnisoLevel = value;
			if (_controlInterface != null)
			{
				_controlInterface.SetTextureProperties(_textureFilterMode, _textureWrapMode, _textureAnisoLevel);
			}
		}
	}

	public bool SideloadSubtitles
	{
		get
		{
			return _sideloadSubtitles;
		}
		set
		{
			_sideloadSubtitles = value;
		}
	}

	public MediaPath SubtitlePath
	{
		get
		{
			return _subtitlePath;
		}
		set
		{
			_subtitlePath = value;
		}
	}

	public Transform AudioHeadTransform
	{
		get
		{
			return _audioHeadTransform;
		}
		set
		{
			_audioHeadTransform = value;
		}
	}

	public bool AudioFocusEnabled
	{
		get
		{
			return _audioFocusEnabled;
		}
		set
		{
			_audioFocusEnabled = value;
		}
	}

	public Transform AudioFocusTransform
	{
		get
		{
			return _audioFocusTransform;
		}
		set
		{
			_audioFocusTransform = value;
		}
	}

	public float AudioFocusWidthDegrees
	{
		get
		{
			return _audioFocusWidthDegrees;
		}
		set
		{
			_audioFocusWidthDegrees = value;
		}
	}

	public float AudioFocusOffLevelDB
	{
		get
		{
			return _audioFocusOffLevelDB;
		}
		set
		{
			_audioFocusOffLevelDB = value;
		}
	}

	public HttpHeaderData HttpHeaders
	{
		get
		{
			return _httpHeaders;
		}
		set
		{
			_httpHeaders = value;
		}
	}

	public KeyAuthData KeyAuth
	{
		get
		{
			return _keyAuth;
		}
		set
		{
			_keyAuth = value;
		}
	}

	public MediaPlayerEvent Events
	{
		get
		{
			if (_events == null)
			{
				_events = new MediaPlayerEvent();
			}
			return _events;
		}
	}

	public int EventMask
	{
		get
		{
			return _eventMask;
		}
		set
		{
			_eventMask = value;
		}
	}

	public bool PauseMediaOnAppPause
	{
		get
		{
			return _pauseMediaOnAppPause;
		}
		set
		{
			_pauseMediaOnAppPause = value;
		}
	}

	public bool PlayMediaOnAppUnpause
	{
		get
		{
			return _playMediaOnAppUnpause;
		}
		set
		{
			_playMediaOnAppUnpause = value;
		}
	}

	public bool Persistent
	{
		get
		{
			return _persistent;
		}
		set
		{
			_persistent = value;
		}
	}

	public FileFormat ForceFileFormat
	{
		get
		{
			return _forceFileFormat;
		}
		set
		{
			_forceFileFormat = value;
		}
	}

	public virtual IMediaInfo Info => _infoInterface;

	public virtual IMediaControl Control => _controlInterface;

	public virtual IMediaPlayer Player => _playerInterface;

	public virtual ITextureProducer TextureProducer => _textureInterface;

	public virtual IMediaSubtitles Subtitles => _subtitlesInterface;

	public virtual IVideoTracks VideoTracks => _videoTracksInterface;

	public virtual IAudioTracks AudioTracks => _audioTracksInterface;

	public virtual ITextTracks TextTracks => _textTracksInterface;

	public virtual ITimedMetadata TimedMetadata => _timedMetadataInterface;

	public virtual IVariants Variants => _variantsInterface;

	public virtual IMediaCache Cache => _cacheInterface;

	public bool MediaOpened => _isMediaOpened;

	public OptionsWindows PlatformOptionsWindows => _optionsWindows;

	public OptionsApple PlatformOptions_macOS => _options_macOS;

	public OptionsApple PlatformOptions_iOS => _options_iOS;

	public OptionsApple PlatformOptions_tvOS => _options_tvOS;

	public OptionsApple PlatformOptions_visionOS => _options_visionOS;

	public OptionsAndroid PlatformOptionsAndroid => _optionsAndroid;

	public OptionsOpenHarmony PlatformOptionsOpenHarmony => _optionsOpenHarmony;

	public OptionsWindowsUWP PlatformOptionsWindowsUWP => _optionsWindowsUWP;

	public OptionsWebGL PlatformOptionsWebGL => _optionsWebGL;

	public void SetMediaSource(MediaSource source)
	{
		_mediaSource = source;
	}

	public void SetMediaReference(MediaReference media)
	{
		MediaReference = media;
	}

	public void SetMediaPath(MediaPath path)
	{
		MediaPath = path;
	}

	public void SetAudioSource(AudioSource audioSource)
	{
		AudioSource = audioSource;
	}

	protected virtual void Awake()
	{
		if (_persistent)
		{
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		}
	}

	protected void Initialise()
	{
		BaseMediaPlayer baseMediaPlayer = CreateMediaPlayer();
		if (baseMediaPlayer != null)
		{
			_baseMediaPlayer = baseMediaPlayer;
			_controlInterface = baseMediaPlayer;
			_textureInterface = baseMediaPlayer;
			_infoInterface = baseMediaPlayer;
			_playerInterface = baseMediaPlayer;
			_subtitlesInterface = baseMediaPlayer;
			_videoTracksInterface = baseMediaPlayer;
			_audioTracksInterface = baseMediaPlayer;
			_textTracksInterface = baseMediaPlayer;
			_timedMetadataInterface = baseMediaPlayer;
			_variantsInterface = baseMediaPlayer;
			_disposeInterface = baseMediaPlayer;
			_cacheInterface = baseMediaPlayer;
			string version = baseMediaPlayer.GetVersion();
			string expectedVersion = baseMediaPlayer.GetExpectedVersion();
			if (!version.StartsWith(expectedVersion))
			{
				Debug.LogError("[AVProVideo] Plugin version number " + version + " doesn't match the expected version number " + expectedVersion + ".  It looks like the plugin didn't upgrade correctly.  To resolve this please restart Unity and try to upgrade the package again.");
			}
			s_TrialVersion = version.Contains("-trial");
			if (!s_GlobalStartup)
			{
				Helper.LogInfo(string.Format("Initialising AVPro Video v{0} (native plugin v{1}) on {2}/{3} (MT {4}) on {5}", "3.4.0", version, SystemInfo.graphicsDeviceName, SystemInfo.graphicsDeviceVersion, SystemInfo.graphicsMultiThreaded, Application.platform));
				s_GlobalStartup = true;
			}
		}
	}

	protected virtual void Start()
	{
		if (_controlInterface != null || !Application.isPlaying)
		{
			return;
		}
		Initialise();
		if (_controlInterface == null)
		{
			return;
		}
		if (_autoOpen)
		{
			OpenMedia(_autoPlayOnStart);
			if (_sideloadSubtitles && _subtitlesInterface != null && _subtitlePath != null && !string.IsNullOrEmpty(_subtitlePath.Path))
			{
				EnableSubtitles(_subtitlePath);
			}
		}
		StartRenderCoroutine();
	}

	public bool OpenMedia(MediaPath path, bool autoPlay = true)
	{
		return OpenMedia(path.PathType, path.Path, autoPlay);
	}

	public bool OpenMedia(MediaPathType pathType, string path, bool autoPlay = true)
	{
		_mediaSource = MediaSource.Path;
		_mediaPath.Path = path;
		_mediaPath.PathType = pathType;
		return OpenMedia(autoPlay);
	}

	public bool OpenMedia(MediaReference mediaReference, bool autoPlay = true)
	{
		_mediaSource = MediaSource.Reference;
		_mediaReference = mediaReference;
		return OpenMedia(autoPlay);
	}

	public bool OpenMedia(bool autoPlay = true)
	{
		_autoPlayOnStart = autoPlay;
		if (_controlInterface == null)
		{
			Initialise();
		}
		return InternalOpenMedia();
	}

	private bool InternalOpenMedia()
	{
		bool result = false;
		if (_controlInterface != null)
		{
			CloseMedia();
			_isMediaOpened = true;
			_autoPlayOnStartTriggered = !_autoPlayOnStart;
			_finishedFrameOpenCheck = true;
			long platformFileOffset = GetPlatformFileOffset();
			MediaPath mediaPath = null;
			MediaHints mediahints = _fallbackMediaHints;
			if (_mediaSource == MediaSource.Reference)
			{
				if (_mediaReference != null)
				{
					mediaPath = _mediaReference.GetCurrentPlatformMediaReference().MediaPath;
					mediahints = _mediaReference.GetCurrentPlatformMediaReference().Hints;
					if (string.IsNullOrEmpty(mediaPath.Path))
					{
						mediaPath = null;
					}
				}
				else
				{
					Debug.LogError("[AVProVideo] No MediaReference specified", this);
				}
			}
			else if (_mediaSource == MediaSource.Path)
			{
				if (!string.IsNullOrEmpty(_mediaPath.Path))
				{
					mediaPath = _mediaPath;
				}
				else
				{
					Debug.LogError("[AVProVideo] No file path specified", this);
				}
			}
			if (null != mediaPath)
			{
				string resolvedFullPath = mediaPath.GetResolvedFullPath();
				string customHttpHeaders = null;
				bool flag = true;
				if (resolvedFullPath.Contains("://"))
				{
					flag = false;
					customHttpHeaders = GetPlatformHttpHeadersAsString();
				}
				if (flag && !File.Exists(resolvedFullPath))
				{
					Debug.LogError("[AVProVideo] File not found: " + resolvedFullPath, this);
				}
				else
				{
					Helper.LogInfo($"Opening {resolvedFullPath} (offset {platformFileOffset}) with API {GetPlatformVideoApiString()}", this);
					if (_optionsWindows._audioMode == Windows.AudioOutput.FacebookAudio360)
					{
						_controlInterface.SetAudioChannelMode(_optionsWindows.audio360ChannelMode);
					}
					else
					{
						_controlInterface.SetAudioChannelMode(Audio360ChannelMode.INVALID);
					}
					PlatformOptions currentPlatformOptions = GetCurrentPlatformOptions();
					bool startWithHighestBitrate = false;
					if (currentPlatformOptions != null)
					{
						startWithHighestBitrate = currentPlatformOptions.StartWithHighestBandwidth();
					}
					SetLoadOptions();
					SetPlaybackOptions();
					if (_controlInterface.OpenMedia(resolvedFullPath, platformFileOffset, customHttpHeaders, mediahints, (int)_forceFileFormat, startWithHighestBitrate))
					{
						StartRenderCoroutine();
						result = true;
					}
					else
					{
						Debug.LogError("[AVProVideo] Failed to open " + resolvedFullPath, this);
					}
				}
			}
			else
			{
				Debug.LogError("[AVProVideo] No file path specified", this);
			}
		}
		return result;
	}

	private void SetLoadOptions()
	{
		if (_optionsWindows.videoApi == Windows.VideoApi.WinRT)
		{
			if (_baseMediaPlayer is WindowsMediaPlayer)
			{
				_baseMediaPlayer = CreateMediaPlayer();
			}
			((WindowsRtMediaPlayer)_baseMediaPlayer).SetOptions(_optionsWindows);
		}
		else
		{
			if (_baseMediaPlayer is WindowsRtMediaPlayer)
			{
				_baseMediaPlayer = CreateMediaPlayer();
			}
			((WindowsMediaPlayer)_baseMediaPlayer).SetOptions(_optionsWindows);
		}
		PlatformOptions currentPlatformOptions = GetCurrentPlatformOptions();
		if (currentPlatformOptions != null)
		{
			_controlInterface.SetKeyServerAuthToken(currentPlatformOptions.GetKeyServerAuthToken());
			_controlInterface.SetOverrideDecryptionKey(currentPlatformOptions.GetOverrideDecryptionKey());
		}
	}

	private void SetPlaybackOptions()
	{
		if (_controlInterface != null)
		{
			_controlInterface.SetLooping(_loop);
			_controlInterface.SetPlaybackRate(_playbackRate);
			_controlInterface.SetVolume(_audioVolume);
			_controlInterface.SetBalance(_audioBalance);
			_controlInterface.MuteAudio(_audioMuted);
			_controlInterface.SetTextureProperties(_textureFilterMode, _textureWrapMode, _textureAnisoLevel);
		}
	}

	public void CloseMedia()
	{
		if (_controlInterface != null)
		{
			if (_events != null && _isMediaOpened && _events.HasListeners() && IsHandleEvent(MediaPlayerEvent.EventType.Closing))
			{
				_events.Invoke(this, MediaPlayerEvent.EventType.Closing, ErrorCode.None);
			}
			_autoPlayOnStartTriggered = false;
			_isMediaOpened = false;
			ResetEvents();
			if (_loadSubtitlesRoutine != null)
			{
				StopCoroutine(_loadSubtitlesRoutine);
				_loadSubtitlesRoutine = null;
			}
			_controlInterface.CloseMedia();
		}
		if (_resampler != null)
		{
			_resampler.Reset();
		}
	}

	public virtual void Play()
	{
		if (_controlInterface != null && _controlInterface.CanPlay())
		{
			_controlInterface.Play();
			_eventFired_ReadyToPlay = true;
		}
		else
		{
			_autoPlayOnStart = true;
			_autoPlayOnStartTriggered = false;
		}
	}

	public virtual void Pause()
	{
		if (_controlInterface != null && _controlInterface.IsPlaying())
		{
			_controlInterface.Pause();
		}
		_wasPlayingOnPause = false;
	}

	public virtual void Stop()
	{
		if (_controlInterface != null)
		{
			_controlInterface.Stop();
		}
	}

	public void Rewind(bool pause)
	{
		if (_controlInterface != null)
		{
			if (pause)
			{
				Pause();
			}
			_controlInterface.Rewind();
		}
	}

	public void SeekToLiveTime(double offset = 0.0)
	{
		if (_controlInterface != null)
		{
			double maxTime = _controlInterface.GetBufferedTimes().MaxTime;
			if (maxTime > 0.0)
			{
				_controlInterface.Seek(maxTime - offset);
			}
		}
	}

	protected virtual void Update()
	{
		if (_controlInterface != null)
		{
			if (_isMediaOpened && _autoPlayOnStart && !_autoPlayOnStartTriggered && _controlInterface.CanPlay())
			{
				_autoPlayOnStartTriggered = true;
				Play();
			}
			if (Application.isPlaying && _renderingCoroutine == null && _controlInterface.CanPlay())
			{
				StartRenderCoroutine();
			}
			if (_subtitlesInterface != null && _queueSubtitlePath != null && !string.IsNullOrEmpty(_queueSubtitlePath.Path))
			{
				EnableSubtitles(_queueSubtitlePath);
				_queueSubtitlePath = null;
			}
			UpdateAudioHeadTransform();
			UpdateAudioFocus();
			_playerInterface.Update();
			UpdateErrors();
			UpdateEvents();
			_playerInterface.EndUpdate();
		}
	}

	private void LateUpdate()
	{
		UpdateResampler();
		if (_playerInterface != null)
		{
			_playerInterface.BeginRender();
		}
	}

	private void UpdateResampler()
	{
		if (_useResampler && _resampler == null)
		{
			_resampler = new Resampler(this, base.gameObject.name, _resampleBufferSize, _resampleMode);
		}
		if (_resampler != null)
		{
			_resampler.Update();
			_resampler.UpdateTimestamp();
		}
	}

	protected virtual void OnEnable()
	{
		if (_controlInterface != null && _wasPlayingOnPause)
		{
			Play();
			_wasPlayingOnPause = false;
		}
		if (_playerInterface != null)
		{
			_playerInterface.OnEnable();
			StartRenderCoroutine();
		}
	}

	protected virtual void OnDisable()
	{
		if (_controlInterface != null && _controlInterface.IsPlaying())
		{
			Pause();
			Update();
			_wasPlayingOnPause = true;
		}
		StopRenderCoroutine();
	}

	protected virtual void OnDestroy()
	{
		CloseMedia();
		_baseMediaPlayer = null;
		_controlInterface = null;
		_textureInterface = null;
		_infoInterface = null;
		_playerInterface = null;
		_subtitlesInterface = null;
		_cacheInterface = null;
		_videoTracksInterface = null;
		_audioTracksInterface = null;
		_textTracksInterface = null;
		_variantsInterface = null;
		if (_disposeInterface != null)
		{
			_disposeInterface.Dispose();
			_disposeInterface = null;
		}
		if (_resampler != null)
		{
			_resampler.Release();
			_resampler = null;
		}
	}

	public void ForceDispose()
	{
		OnDisable();
		OnDestroy();
	}

	private static void AllPlayersDispose()
	{
		MediaPlayer[] array = Resources.FindObjectsOfTypeAll<MediaPlayer>();
		if (array != null && array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ForceDispose();
			}
		}
	}

	private void OnApplicationQuit()
	{
		if (s_GlobalStartup)
		{
			Helper.LogInfo("Shutdown");
			AllPlayersDispose();
			WindowsMediaPlayer.DeinitPlatform();
			WindowsRtMediaPlayer.DeinitPlatform();
			s_GlobalStartup = false;
		}
	}

	protected void StartRenderCoroutine()
	{
		if (base.gameObject.activeInHierarchy && _renderingCoroutine == null)
		{
			_renderingCoroutine = StartCoroutine(FinalRenderCapture());
		}
	}

	private void StopRenderCoroutine()
	{
		if (_renderingCoroutine != null)
		{
			StopCoroutine(_renderingCoroutine);
			_renderingCoroutine = null;
		}
	}

	private IEnumerator FinalRenderCapture()
	{
		YieldInstruction wait = new WaitForEndOfFrame();
		while (Application.isPlaying)
		{
			yield return wait;
			if (base.enabled && _playerInterface != null)
			{
				_playerInterface.Render();
			}
		}
	}

	public static Platform GetPlatform()
	{
		return Platform.Windows;
	}

	public PlatformOptions GetCurrentPlatformOptions()
	{
		return _optionsWindows;
	}

	private string GetPlatformVideoApiString()
	{
		_ = string.Empty;
		return _optionsWindows.videoApi.ToString();
	}

	private long GetPlatformFileOffset()
	{
		return 0L;
	}

	private string GetPlatformHttpHeadersAsString()
	{
		string text = null;
		text = _optionsWindows.httpHeaders.ToValidatedString();
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Trim();
		}
		string text2 = _httpHeaders.ToValidatedString();
		if (!string.IsNullOrEmpty(text2))
		{
			text += text2;
			text = text.Trim();
		}
		return text;
	}

	private string GetResolvedFilePath(string filePath, MediaPathType fileLocation)
	{
		_ = string.Empty;
		return Helper.GetFilePath(filePath, fileLocation);
	}

	private static BaseMediaPlayer CreateMediaPlayer(OptionsWindows options)
	{
		BaseMediaPlayer baseMediaPlayer = null;
		if (options.videoApi == Windows.VideoApi.WinRT)
		{
			if (WindowsRtMediaPlayer.InitialisePlatform())
			{
				baseMediaPlayer = new WindowsRtMediaPlayer(options);
			}
			else
			{
				Debug.LogWarning($"[AVProVideo] Failed to initialise WinRT API - platform {SystemInfo.operatingSystem} may not support it.  Trying another video API...");
			}
		}
		if (baseMediaPlayer == null && WindowsMediaPlayer.InitialisePlatform())
		{
			baseMediaPlayer = new WindowsMediaPlayer(options);
		}
		return baseMediaPlayer;
	}

	private static BaseMediaPlayer CreateMediaPlayerNull()
	{
		return new NullMediaPlayer();
	}

	public virtual BaseMediaPlayer CreateMediaPlayer()
	{
		BaseMediaPlayer baseMediaPlayer = null;
		baseMediaPlayer = CreateMediaPlayer(_optionsWindows);
		if (baseMediaPlayer == null)
		{
			Debug.LogError($"[AVProVideo] Not supported on this platform {Application.platform} {SystemInfo.deviceModel} {SystemInfo.processorType} {SystemInfo.operatingSystem}.  Using null media player!");
			baseMediaPlayer = CreateMediaPlayerNull();
		}
		return baseMediaPlayer;
	}

	private void UpdateAudioFocus()
	{
		_controlInterface.SetAudioFocusEnabled(_audioFocusEnabled);
		_controlInterface.SetAudioFocusProperties(_audioFocusOffLevelDB, _audioFocusWidthDegrees);
		_controlInterface.SetAudioFocusRotation((_audioFocusTransform == null) ? Quaternion.identity : _audioFocusTransform.rotation);
	}

	private void UpdateAudioHeadTransform()
	{
		if (_audioHeadTransform != null)
		{
			_controlInterface.SetAudioHeadRotation(_audioHeadTransform.rotation);
		}
		else
		{
			_controlInterface.ResetAudioHeadRotation();
		}
	}

	private void UpdateErrors()
	{
		ErrorCode lastError = _controlInterface.GetLastError();
		if (lastError != 0)
		{
			Debug.LogError("[AVProVideo] Error: " + Helper.GetErrorMessage(lastError));
			_ = 100;
			if (_events != null && _events.HasListeners() && IsHandleEvent(MediaPlayerEvent.EventType.Error))
			{
				_events.Invoke(this, MediaPlayerEvent.EventType.Error, lastError);
			}
		}
	}

	public bool IsUsingAndroidOESPath()
	{
		return false;
	}

	public bool IsUsingYCbCr()
	{
		return false;
	}

	[ContextMenu("Save Frame To PNG")]
	public void SaveFrameToPng()
	{
		Texture2D texture2D = ExtractFrame(null);
		if (texture2D != null)
		{
			byte[] array = texture2D.EncodeToPNG();
			if (array != null)
			{
				string text = Mathf.FloorToInt((float)(Control.GetCurrentTime() * 1000.0)).ToString("D8");
				File.WriteAllBytes("frame-" + text + ".png", array);
			}
			UnityEngine.Object.Destroy(texture2D);
		}
	}

	[ContextMenu("Save Frame To EXR")]
	public void SaveFrameToExr()
	{
		Texture texture = TextureProducer.GetTexture();
		if (texture != null)
		{
			RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
			renderTexture.Create();
			Graphics.Blit(texture, renderTexture);
			Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBAFloat, mipChain: false, linear: true);
			RenderTexture.active = renderTexture;
			texture2D.ReadPixels(new Rect(0f, 0f, texture.width, texture.height), 0, 0, recalculateMipMaps: false);
			texture2D.Apply(updateMipmaps: false, makeNoLongerReadable: false);
			RenderTexture.active = null;
			byte[] array = texture2D.EncodeToEXR();
			if (array != null)
			{
				string text = Mathf.FloorToInt((float)(Control.GetCurrentTime() * 1000.0)).ToString("D8");
				File.WriteAllBytes("frame-" + text + ".exr", array);
			}
			UnityEngine.Object.Destroy(texture);
			UnityEngine.Object.Destroy(texture2D);
			UnityEngine.Object.Destroy(renderTexture);
		}
	}

	private void OnApplicationFocus(bool focusStatus)
	{
	}

	private void OnApplicationPause(bool pauseStatus)
	{
	}

	private void ResetEvents()
	{
		_eventFired_MetaDataReady = false;
		_eventFired_ReadyToPlay = false;
		_eventFired_Started = false;
		_eventFired_FirstFrameReady = false;
		_eventFired_FinishedPlaying = false;
		_eventState_PlaybackBuffering = false;
		_eventState_PlaybackSeeking = false;
		_eventState_PlaybackStalled = false;
		_eventState_PreviousWidth = 0;
		_eventState_PreviousHeight = 0;
		_previousSubtitleIndex = -1;
		_finishedFrameOpenCheck = false;
	}

	private void CheckAndClearStartedAndFinishedEvents()
	{
		_finishedFrameOpenCheck = false;
		if (IsHandleEvent(MediaPlayerEvent.EventType.FinishedPlaying) && FireEventIfPossible(MediaPlayerEvent.EventType.FinishedPlaying, _eventFired_FinishedPlaying))
		{
			_eventFired_FinishedPlaying = !_finishedFrameOpenCheck;
		}
		if (_eventFired_FinishedPlaying && IsHandleEvent(MediaPlayerEvent.EventType.FinishedPlaying) && _controlInterface.IsPlaying() && !_controlInterface.IsFinished())
		{
			_eventFired_FinishedPlaying = false;
		}
	}

	private void HandleOneShotEvents()
	{
		_eventFired_MetaDataReady = FireEventIfPossible(MediaPlayerEvent.EventType.MetaDataReady, _eventFired_MetaDataReady);
		_eventFired_ReadyToPlay = FireEventIfPossible(MediaPlayerEvent.EventType.ReadyToPlay, _eventFired_ReadyToPlay);
		_eventFired_Started = FireEventIfPossible(MediaPlayerEvent.EventType.Started, _eventFired_Started);
		_eventFired_FirstFrameReady = FireEventIfPossible(MediaPlayerEvent.EventType.FirstFrameReady, _eventFired_FirstFrameReady);
	}

	private void HandleRecurringEvents()
	{
		if (FireEventIfPossible(MediaPlayerEvent.EventType.SubtitleChange, hasFired: false))
		{
			_previousSubtitleIndex = _subtitlesInterface.GetSubtitleIndex();
		}
		if (FireEventIfPossible(MediaPlayerEvent.EventType.ResolutionChanged, hasFired: false))
		{
			_eventState_PreviousWidth = _infoInterface.GetVideoWidth();
			_eventState_PreviousHeight = _infoInterface.GetVideoHeight();
		}
		FireEventIfPossible(MediaPlayerEvent.EventType.TimedMetadataChanged, hasFired: false);
		if (IsHandleEvent(MediaPlayerEvent.EventType.Stalled))
		{
			bool flag = _infoInterface.IsPlaybackStalled();
			if (flag != _eventState_PlaybackStalled)
			{
				_eventState_PlaybackStalled = flag;
				MediaPlayerEvent.EventType eventType = (_eventState_PlaybackStalled ? MediaPlayerEvent.EventType.Stalled : MediaPlayerEvent.EventType.Unstalled);
				FireEventIfPossible(eventType, hasFired: false);
			}
		}
		if (IsHandleEvent(MediaPlayerEvent.EventType.StartedSeeking))
		{
			bool flag2 = _controlInterface.IsSeeking();
			if (flag2 != _eventState_PlaybackSeeking)
			{
				_eventState_PlaybackSeeking = flag2;
				MediaPlayerEvent.EventType eventType2 = (_eventState_PlaybackSeeking ? MediaPlayerEvent.EventType.StartedSeeking : MediaPlayerEvent.EventType.FinishedSeeking);
				FireEventIfPossible(eventType2, hasFired: false);
			}
		}
		if (IsHandleEvent(MediaPlayerEvent.EventType.StartedBuffering))
		{
			bool flag3 = _controlInterface.IsBuffering();
			if (flag3 != _eventState_PlaybackBuffering)
			{
				_eventState_PlaybackBuffering = flag3;
				MediaPlayerEvent.EventType eventType3 = (_eventState_PlaybackBuffering ? MediaPlayerEvent.EventType.StartedBuffering : MediaPlayerEvent.EventType.FinishedBuffering);
				FireEventIfPossible(eventType3, hasFired: false);
			}
		}
		if (IsHandleEvent(MediaPlayerEvent.EventType.Paused))
		{
			bool flag4 = _controlInterface.IsPaused();
			if (flag4 != _eventState_Paused)
			{
				_eventState_Paused = flag4;
				MediaPlayerEvent.EventType eventType4 = (_eventState_Paused ? MediaPlayerEvent.EventType.Paused : MediaPlayerEvent.EventType.Unpaused);
				FireEventIfPossible(eventType4, hasFired: false);
			}
		}
	}

	private void UpdateEvents()
	{
		if (_controlInterface != null && _events != null && _events.HasListeners())
		{
			CheckAndClearStartedAndFinishedEvents();
			HandleOneShotEvents();
			HandleRecurringEvents();
		}
	}

	protected bool IsHandleEvent(MediaPlayerEvent.EventType eventType)
	{
		return ((uint)_eventMask & (1 << (int)eventType)) != 0;
	}

	private bool FireEventIfPossible(MediaPlayerEvent.EventType eventType, bool hasFired)
	{
		if (CanFireEvent(eventType, hasFired))
		{
			hasFired = true;
			_events.Invoke(this, eventType, ErrorCode.None);
		}
		return hasFired;
	}

	private bool CanFireEvent(MediaPlayerEvent.EventType et, bool hasFired)
	{
		if (_controlInterface == null)
		{
			return false;
		}
		if (_events == null)
		{
			return false;
		}
		if (hasFired)
		{
			return false;
		}
		if (!IsHandleEvent(et))
		{
			return false;
		}
		bool flag = false;
		switch (et)
		{
		case MediaPlayerEvent.EventType.FinishedPlaying:
			flag = !_controlInterface.IsLooping() && _controlInterface.CanPlay() && _controlInterface.IsFinished();
			break;
		case MediaPlayerEvent.EventType.MetaDataReady:
			flag = _controlInterface.HasMetaData();
			break;
		case MediaPlayerEvent.EventType.FirstFrameReady:
			flag = _textureInterface != null && _controlInterface.CanPlay() && _textureInterface.GetTextureFrameCount() > 0;
			break;
		case MediaPlayerEvent.EventType.ReadyToPlay:
			flag = !_controlInterface.IsPlaying() && _controlInterface.CanPlay() && !_autoPlayOnStart;
			break;
		case MediaPlayerEvent.EventType.Started:
			flag = _controlInterface.IsPlaying();
			break;
		case MediaPlayerEvent.EventType.SubtitleChange:
			flag = _previousSubtitleIndex != _subtitlesInterface.GetSubtitleIndex();
			if (!flag)
			{
				flag = _baseMediaPlayer.InternalIsChangedTextCue();
			}
			break;
		case MediaPlayerEvent.EventType.Stalled:
			flag = _infoInterface.IsPlaybackStalled();
			break;
		case MediaPlayerEvent.EventType.Unstalled:
			flag = !_infoInterface.IsPlaybackStalled();
			break;
		case MediaPlayerEvent.EventType.StartedSeeking:
			flag = _controlInterface.IsSeeking();
			break;
		case MediaPlayerEvent.EventType.FinishedSeeking:
			flag = !_controlInterface.IsSeeking();
			break;
		case MediaPlayerEvent.EventType.StartedBuffering:
			flag = _controlInterface.IsBuffering();
			break;
		case MediaPlayerEvent.EventType.FinishedBuffering:
			flag = !_controlInterface.IsBuffering();
			break;
		case MediaPlayerEvent.EventType.ResolutionChanged:
			flag = _infoInterface != null && (_eventState_PreviousWidth != _infoInterface.GetVideoWidth() || _eventState_PreviousHeight != _infoInterface.GetVideoHeight());
			break;
		case MediaPlayerEvent.EventType.Paused:
			flag = _controlInterface.IsPaused();
			break;
		case MediaPlayerEvent.EventType.Unpaused:
			flag = !_controlInterface.IsPaused();
			break;
		case MediaPlayerEvent.EventType.TimedMetadataChanged:
			flag = _baseMediaPlayer.HasNewTimedMetadataItem();
			break;
		default:
			Debug.LogWarning("[AVProVideo] Unhandled event type");
			break;
		}
		return flag;
	}

	private bool ForceWaitForNewFrame(int lastFrameCount, float timeoutMs)
	{
		bool result = false;
		DateTime now = DateTime.Now;
		int num = 0;
		while (Control != null && (DateTime.Now - now).TotalMilliseconds < (double)timeoutMs)
		{
			_playerInterface.Update();
			if (lastFrameCount != TextureProducer.GetTextureFrameCount())
			{
				result = true;
				break;
			}
			num++;
		}
		_playerInterface.Render();
		return result;
	}

	private static Camera GetDummyCamera()
	{
		if (_dummyCamera == null)
		{
			GameObject gameObject = GameObject.Find("AVPro Video Dummy Camera");
			if (gameObject == null)
			{
				gameObject = new GameObject("AVPro Video Dummy Camera");
				gameObject.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
				gameObject.SetActive(value: false);
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
				_dummyCamera = gameObject.AddComponent<Camera>();
				_dummyCamera.hideFlags = HideFlags.DontSave | HideFlags.HideInInspector;
				_dummyCamera.cullingMask = 0;
				_dummyCamera.clearFlags = CameraClearFlags.Nothing;
				_dummyCamera.enabled = false;
			}
			else
			{
				_dummyCamera = gameObject.GetComponent<Camera>();
			}
		}
		return _dummyCamera;
	}

	private IEnumerator ExtractFrameCoroutine(Texture2D target, ProcessExtractedFrame callback, double timeSeconds = -1.0, bool accurateSeek = true, int timeoutMs = 1000, int timeThresholdMs = 100)
	{
		Texture2D result = target;
		Texture texture = null;
		if (_controlInterface != null)
		{
			if (timeSeconds >= 0.0)
			{
				Pause();
				if (TextureProducer.GetTexture() != null && Math.Abs(_controlInterface.GetCurrentTime() - timeSeconds) < (double)timeThresholdMs / 1000.0)
				{
					texture = TextureProducer.GetTexture();
				}
				else
				{
					int textureFrameCount = _textureInterface.GetTextureFrameCount();
					if (accurateSeek)
					{
						_controlInterface.Seek(timeSeconds);
					}
					else
					{
						_controlInterface.SeekFast(timeSeconds);
					}
					if (!_controlInterface.WaitForNextFrame(GetDummyCamera(), textureFrameCount))
					{
						int currFc = TextureProducer.GetTextureFrameCount();
						int iterations = 0;
						int maxIterations = 50;
						while (currFc + 1 >= TextureProducer.GetTextureFrameCount() && iterations++ < maxIterations)
						{
							yield return null;
						}
					}
					texture = TextureProducer.GetTexture();
				}
			}
			else
			{
				texture = TextureProducer.GetTexture();
			}
		}
		if (texture != null)
		{
			result = Helper.GetReadableTexture(texture, TextureProducer.RequiresVerticalFlip(), Helper.GetOrientation(Info.GetTextureTransform()), target);
		}
		callback(result);
		yield return null;
	}

	public void ExtractFrameAsync(Texture2D target, ProcessExtractedFrame callback, double timeSeconds = -1.0, bool accurateSeek = true, int timeoutMs = 1000, int timeThresholdMs = 100)
	{
		StartCoroutine(ExtractFrameCoroutine(target, callback, timeSeconds, accurateSeek, timeoutMs, timeThresholdMs));
	}

	public Texture2D ExtractFrame(Texture2D target, double timeSeconds = -1.0, bool accurateSeek = true, int timeoutMs = 1000, int timeThresholdMs = 100)
	{
		Texture2D result = target;
		Texture texture = ExtractFrame(timeSeconds, accurateSeek, timeoutMs, timeThresholdMs);
		if (texture != null)
		{
			result = Helper.GetReadableTexture(texture, TextureProducer.RequiresVerticalFlip(), Helper.GetOrientation(Info.GetTextureTransform()), target);
		}
		return result;
	}

	private Texture ExtractFrame(double timeSeconds = -1.0, bool accurateSeek = true, int timeoutMs = 1000, int timeThresholdMs = 100)
	{
		Texture result = null;
		if (_controlInterface != null)
		{
			if (timeSeconds >= 0.0)
			{
				Pause();
				if (TextureProducer.GetTexture() != null && Math.Abs(_controlInterface.GetCurrentTime() - timeSeconds) < (double)timeThresholdMs / 1000.0)
				{
					result = TextureProducer.GetTexture();
				}
				else
				{
					int textureFrameCount = TextureProducer.GetTextureFrameCount();
					if (accurateSeek)
					{
						_controlInterface.Seek(timeSeconds);
					}
					else
					{
						_controlInterface.SeekFast(timeSeconds);
					}
					ForceWaitForNewFrame(textureFrameCount, timeoutMs);
					result = TextureProducer.GetTexture();
				}
			}
			else
			{
				result = TextureProducer.GetTexture();
			}
		}
		return result;
	}

	public bool OpenMediaFromBuffer(byte[] buffer, bool autoPlay = true)
	{
		_mediaPath = new MediaPath("buffer", MediaPathType.AbsolutePathOrURL);
		_autoPlayOnStart = autoPlay;
		if (_controlInterface == null)
		{
			Initialise();
		}
		return OpenMediaFromBufferInternal(buffer);
	}

	public bool StartOpenChunkedMediaFromBuffer(ulong length, bool autoPlay = true)
	{
		_mediaPath = new MediaPath("buffer", MediaPathType.AbsolutePathOrURL);
		_autoPlayOnStart = autoPlay;
		if (_controlInterface == null)
		{
			Initialise();
		}
		return StartOpenMediaFromBufferInternal(length);
	}

	public bool AddChunkToVideoBuffer(byte[] chunk, ulong offset, ulong chunkSize)
	{
		return AddChunkToBufferInternal(chunk, offset, chunkSize);
	}

	public bool EndOpenChunkedVideoFromBuffer()
	{
		return EndOpenMediaFromBufferInternal();
	}

	private bool OpenMediaFromBufferInternal(byte[] buffer)
	{
		bool result = false;
		if (_controlInterface != null)
		{
			CloseMedia();
			_isMediaOpened = true;
			_autoPlayOnStartTriggered = !_autoPlayOnStart;
			Helper.LogInfo("Opening buffer of length " + buffer.Length, this);
			if (!_controlInterface.OpenMediaFromBuffer(buffer))
			{
				Debug.LogError("[AVProVideo] Failed to open buffer", this);
				if (GetCurrentPlatformOptions() != PlatformOptionsWindows || PlatformOptionsWindows.videoApi != Windows.VideoApi.DirectShow)
				{
					Debug.LogError("[AVProVideo] Loading from buffer is currently only supported in Windows when using the DirectShow API");
				}
			}
			else
			{
				SetPlaybackOptions();
				result = true;
				StartRenderCoroutine();
			}
		}
		return result;
	}

	private bool StartOpenMediaFromBufferInternal(ulong length)
	{
		bool result = false;
		if (_controlInterface != null)
		{
			CloseMedia();
			_isMediaOpened = true;
			_autoPlayOnStartTriggered = !_autoPlayOnStart;
			Helper.LogInfo("Starting Opening buffer of length " + length, this);
			if (!_controlInterface.StartOpenMediaFromBuffer(length))
			{
				Debug.LogError("[AVProVideo] Failed to start open video from buffer", this);
				if (GetCurrentPlatformOptions() != PlatformOptionsWindows || PlatformOptionsWindows.videoApi != Windows.VideoApi.DirectShow)
				{
					Debug.LogError("[AVProVideo] Loading from buffer is currently only supported in Windows when using the DirectShow API");
				}
			}
			else
			{
				SetPlaybackOptions();
				result = true;
				StartRenderCoroutine();
			}
		}
		return result;
	}

	private bool AddChunkToBufferInternal(byte[] chunk, ulong offset, ulong chunkSize)
	{
		if (Control != null)
		{
			return Control.AddChunkToMediaBuffer(chunk, offset, chunkSize);
		}
		return false;
	}

	private bool EndOpenMediaFromBufferInternal()
	{
		if (Control != null)
		{
			return Control.EndOpenMediaFromBuffer();
		}
		return false;
	}

	public bool EnableSubtitles(MediaPath mediaPath)
	{
		bool result = false;
		if (_subtitlesInterface != null)
		{
			if (mediaPath != null && !string.IsNullOrEmpty(mediaPath.Path))
			{
				string resolvedFullPath = mediaPath.GetResolvedFullPath();
				bool flag = true;
				if (resolvedFullPath.Contains("://"))
				{
					flag = false;
				}
				if (flag && !File.Exists(resolvedFullPath))
				{
					Debug.LogError("[AVProVideo] Subtitle file not found: " + resolvedFullPath, this);
				}
				else
				{
					Helper.LogInfo("Opening subtitles " + resolvedFullPath, this);
					_previousSubtitleIndex = -1;
					try
					{
						if (resolvedFullPath.Contains("://"))
						{
							if (_loadSubtitlesRoutine != null)
							{
								StopCoroutine(_loadSubtitlesRoutine);
								_loadSubtitlesRoutine = null;
							}
							_loadSubtitlesRoutine = StartCoroutine(LoadSubtitlesCoroutine(resolvedFullPath, mediaPath));
						}
						else
						{
							string data = File.ReadAllText(resolvedFullPath);
							if (_subtitlesInterface.LoadSubtitlesSRT(data))
							{
								_subtitlePath = mediaPath;
								_sideloadSubtitles = false;
								result = true;
							}
							else
							{
								Debug.LogError("[AVProVideo] Failed to load subtitles" + resolvedFullPath, this);
							}
						}
					}
					catch (Exception exception)
					{
						Debug.LogError("[AVProVideo] Failed to load subtitles " + resolvedFullPath, this);
						Debug.LogException(exception, this);
					}
				}
			}
			else
			{
				Debug.LogError("[AVProVideo] No subtitle file path specified", this);
			}
		}
		else
		{
			_queueSubtitlePath = mediaPath;
		}
		return result;
	}

	private IEnumerator LoadSubtitlesCoroutine(string url, MediaPath mediaPath)
	{
		UnityWebRequest www = UnityWebRequest.Get(url);
		yield return www.SendWebRequest();
		string data = string.Empty;
		if (www.result == UnityWebRequest.Result.Success)
		{
			data = www.downloadHandler.text;
		}
		else
		{
			Debug.LogError("[AVProVideo] Error loading subtitles '" + www.error + "' from " + url);
		}
		if (_subtitlesInterface.LoadSubtitlesSRT(data))
		{
			_subtitlePath = mediaPath;
			_sideloadSubtitles = false;
		}
		else
		{
			Debug.LogError("[AVProVideo] Failed to load subtitles" + url, this);
		}
		_loadSubtitlesRoutine = null;
		www.Dispose();
	}

	public void DisableSubtitles()
	{
		if (_loadSubtitlesRoutine != null)
		{
			StopCoroutine(_loadSubtitlesRoutine);
			_loadSubtitlesRoutine = null;
		}
		if (_subtitlesInterface != null)
		{
			_previousSubtitleIndex = -1;
			_sideloadSubtitles = false;
			_subtitlesInterface.LoadSubtitlesSRT(string.Empty);
		}
		else
		{
			_queueSubtitlePath = null;
		}
	}

	void ISerializationCallbackReceiver.OnBeforeSerialize()
	{
	}

	void ISerializationCallbackReceiver.OnAfterDeserialize()
	{
		if (!string.IsNullOrEmpty(m_VideoPath))
		{
			MediaPathType mediaPathType = MediaPathType.AbsolutePathOrURL;
			_mediaPath = new MediaPath(pathType: m_VideoLocation switch
			{
				FileLocation.RelativeToProjectFolder => MediaPathType.RelativeToProjectFolder, 
				FileLocation.RelativeToStreamingAssetsFolder => MediaPathType.RelativeToStreamingAssetsFolder, 
				FileLocation.RelativeToDataFolder => MediaPathType.RelativeToDataFolder, 
				FileLocation.RelativeToPersistentDataFolder => MediaPathType.RelativeToPersistentDataFolder, 
				_ => MediaPathType.AbsolutePathOrURL, 
			}, path: m_VideoPath);
			_mediaSource = MediaSource.Path;
			m_VideoPath = null;
		}
	}
}
