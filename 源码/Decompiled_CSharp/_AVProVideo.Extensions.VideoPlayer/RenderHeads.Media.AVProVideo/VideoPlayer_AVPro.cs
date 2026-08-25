using UnityEngine;
using UnityEngine.Video;

namespace RenderHeads.Media.AVProVideo;

public class VideoPlayer_AVPro : MediaPlayer
{
	public delegate void EventHandler(VideoPlayer_AVPro source);

	public delegate void ErrorEventHandler(VideoPlayer_AVPro source, string message);

	public delegate void FrameReadyEventHandler(VideoPlayer_AVPro source, long frameIdx);

	public delegate void TimeEventHandler(VideoPlayer_AVPro source, double seconds);

	public static readonly ushort controlledAudioTrackMaxCount = 64;

	[SerializeField]
	private MediaSource Source;

	[SerializeField]
	private MediaReference Clip;

	[SerializeField]
	private string Url;

	private MediaPath path;

	[SerializeField]
	private bool PlayOnAwake;

	[SerializeField]
	private bool AutoOpening;

	[SerializeField]
	private bool IsLooping;

	[SerializeField]
	private float PlaybackSpeed = 1f;

	public MonoBehaviour _renderModeComponent;

	public DisplayIMGUI displayIMGUI;

	public DisplayUGUI displayUGUI;

	public ApplyToMesh applyToMesh;

	public ApplyToMaterial applyToMaterial;

	public ApplyToFarPlane applyToFarPlane;

	public ResolveToRenderTexture applyToTexture;

	public DisplayType currentRenderMode;

	[SerializeField]
	private Renderer TargetMaterialRenderer;

	[SerializeField]
	private string TargetMateralProperty;

	[SerializeField]
	private DisplayType RenderMode;

	[SerializeField]
	private Material TargetMaterial;

	[SerializeField]
	private Color Colour = Color.white;

	[SerializeField]
	private ScaleMode AspectRatio;

	[SerializeField]
	private VideoResolveOptions.AspectRatio AspectRatioRenderTexture;

	[SerializeField]
	private bool Fullscreen = true;

	[SerializeField]
	private float TargetCameraAlpha = 1f;

	[SerializeField]
	private int AudioOutputMode;

	[SerializeField]
	private ushort ControlledAudioTrackCount;

	[SerializeField]
	private float Volume = 1f;

	[SerializeField]
	private bool Muted;

	[SerializeField]
	private AudioSource AudioSourceE;

	[SerializeField]
	private RenderTexture TargetTexture;

	[SerializeField]
	private Rect UVRect = new Rect(0f, 0f, 1f, 1f);

	public GameObject canvasObj;

	public bool _converted;

	public bool sendFrameReadyEvents;

	public VideoSource source
	{
		get
		{
			if (sourceAVPro == MediaSource.Reference)
			{
				return VideoSource.VideoClip;
			}
			return VideoSource.Url;
		}
		set
		{
			if (value == VideoSource.VideoClip)
			{
				sourceAVPro = MediaSource.Reference;
			}
			else
			{
				sourceAVPro = MediaSource.Path;
			}
		}
	}

	public MediaSource sourceAVPro
	{
		get
		{
			return Source;
		}
		set
		{
			Source = value;
			SetMediaSource(value);
		}
	}

	public MediaReference clip
	{
		get
		{
			return Clip;
		}
		set
		{
			SetMediaReference(value);
			Clip = value;
		}
	}

	public MediaPath url
	{
		get
		{
			return path;
		}
		set
		{
			if (value.PathType == MediaPathType.AbsolutePathOrURL)
			{
				Source = MediaSource.Path;
				SetMediaSource(MediaSource.Path);
			}
			path = value;
			Url = value.Path;
			SetMediaPath(value);
		}
	}

	public bool playOnAwake
	{
		get
		{
			return PlayOnAwake;
		}
		set
		{
			PlayOnAwake = value;
			base.AutoStart = value;
		}
	}

	public bool isLooping
	{
		get
		{
			return IsLooping;
		}
		set
		{
			IsLooping = value;
			base.Loop = value;
		}
	}

	public float playbackSpeed
	{
		get
		{
			return PlaybackSpeed;
		}
		set
		{
			if (canSetPlaybackSpeed)
			{
				PlaybackSpeed = value;
				base.PlaybackRate = value;
			}
		}
	}

	public Renderer targetMaterialRenderer
	{
		get
		{
			return TargetMaterialRenderer;
		}
		set
		{
			TargetMaterialRenderer = value;
		}
	}

	public string targetMateralProperty
	{
		get
		{
			return TargetMateralProperty;
		}
		set
		{
			TargetMateralProperty = value;
		}
	}

	public Material targetMaterial
	{
		get
		{
			return TargetMaterial;
		}
		set
		{
			TargetMaterial = value;
			if ((bool)displayUGUI)
			{
				displayUGUI.material = value;
			}
			if ((bool)applyToMaterial)
			{
				applyToMaterial.Material = value;
			}
		}
	}

	public VideoRenderMode renderMode
	{
		get
		{
			LogAutomaticConversion("DisplayType", "VideoRenderMode");
			switch (rendererMode)
			{
			case DisplayType.IMGUI:
				return VideoRenderMode.CameraNearPlane;
			case DisplayType.uGUI:
			case DisplayType.RenderTexture:
				return VideoRenderMode.RenderTexture;
			case DisplayType.Material:
				return VideoRenderMode.MaterialOverride;
			case DisplayType.Mesh:
				return VideoRenderMode.APIOnly;
			case DisplayType.CameraFarPlane:
				return VideoRenderMode.CameraFarPlane;
			default:
				return VideoRenderMode.CameraNearPlane;
			}
		}
		set
		{
			LogAutomaticConversion("VideoRenderMode", "DisplayType");
			switch (value)
			{
			case VideoRenderMode.CameraFarPlane:
				rendererMode = DisplayType.CameraFarPlane;
				break;
			case VideoRenderMode.CameraNearPlane:
				rendererMode = DisplayType.IMGUI;
				break;
			case VideoRenderMode.RenderTexture:
				rendererMode = DisplayType.RenderTexture;
				break;
			case VideoRenderMode.MaterialOverride:
				rendererMode = DisplayType.Material;
				break;
			case VideoRenderMode.APIOnly:
				rendererMode = DisplayType.None;
				break;
			}
		}
	}

	public DisplayType rendererMode
	{
		get
		{
			return RenderMode;
		}
		set
		{
			RenderMode = value;
			if (currentRenderMode != rendererMode)
			{
				currentRenderMode = rendererMode;
				CreateRendererComponents();
			}
		}
	}

	public ScaleMode aspectRatio
	{
		get
		{
			return AspectRatio;
		}
		set
		{
			AspectRatio = value;
			if (rendererMode == DisplayType.uGUI)
			{
				displayUGUI.ScaleMode = value;
			}
			if (rendererMode == DisplayType.IMGUI)
			{
				displayIMGUI.ScaleMode = value;
			}
		}
	}

	public VideoResolveOptions.AspectRatio aspectRatioRenderTexture
	{
		get
		{
			return AspectRatioRenderTexture;
		}
		set
		{
			AspectRatioRenderTexture = value;
			if (rendererMode == DisplayType.RenderTexture)
			{
				VideoResolveOptions videoResolveOptions = applyToTexture.VideoResolveOptions;
				videoResolveOptions.aspectRatio = value;
				applyToTexture.VideoResolveOptions = videoResolveOptions;
			}
			if ((bool)applyToFarPlane)
			{
				applyToFarPlane.VideoAspectRatio = (VideoAspectRatio)value;
			}
		}
	}

	public VideoAudioOutputMode audioOutputMode
	{
		get
		{
			switch (audioOutputModeAVPro)
			{
			case Windows.AudioOutput.None:
				return VideoAudioOutputMode.None;
			case Windows.AudioOutput.Unity:
				return VideoAudioOutputMode.AudioSource;
			case Windows.AudioOutput.System:
			case Windows.AudioOutput.FacebookAudio360:
				return VideoAudioOutputMode.Direct;
			default:
				return VideoAudioOutputMode.APIOnly;
			}
		}
		set
		{
			switch (value)
			{
			case VideoAudioOutputMode.None:
				audioOutputModeAVPro = Windows.AudioOutput.None;
				break;
			case VideoAudioOutputMode.AudioSource:
				audioOutputModeAVPro = Windows.AudioOutput.Unity;
				break;
			case VideoAudioOutputMode.Direct:
			case VideoAudioOutputMode.APIOnly:
				audioOutputModeAVPro = Windows.AudioOutput.System;
				break;
			}
		}
	}

	public Windows.AudioOutput audioOutputModeAVPro
	{
		get
		{
			return (Windows.AudioOutput)AudioOutputMode;
		}
		set
		{
			AudioOutputMode = (int)value;
			base.PlatformOptionsWindows._audioMode = value;
			CreateAudioComponents();
		}
	}

	public Color color
	{
		get
		{
			return Colour;
		}
		set
		{
			Colour = value;
			if ((bool)displayIMGUI)
			{
				displayIMGUI.Color = value;
			}
			else if ((bool)applyToTexture)
			{
				VideoResolveOptions videoResolveOptions = applyToTexture.VideoResolveOptions;
				videoResolveOptions.tint = value;
				applyToTexture.VideoResolveOptions = videoResolveOptions;
			}
			else if ((bool)displayUGUI)
			{
				displayUGUI.color = value;
			}
			else if ((bool)applyToFarPlane)
			{
				applyToFarPlane.MainColor = value;
			}
		}
	}

	public bool fullScreen
	{
		get
		{
			return Fullscreen;
		}
		set
		{
			Fullscreen = value;
			if ((bool)displayIMGUI)
			{
				displayIMGUI.IsAreaFullScreen = value;
			}
		}
	}

	public float volume
	{
		get
		{
			if (GetDirectAudioVolume(0) != Volume)
			{
				Volume = GetDirectAudioVolume(0);
			}
			return Volume;
		}
		set
		{
			Volume = value;
			if (audioOutputModeAVPro == Windows.AudioOutput.Unity && (bool)base.AudioSource)
			{
				base.AudioSource.volume = value;
			}
			SetDirectAudioVolume(0, value);
		}
	}

	public bool muted
	{
		get
		{
			return Muted;
		}
		set
		{
			Muted = value;
			AudioMuted = value;
			if (audioOutputModeAVPro == Windows.AudioOutput.Unity && (bool)base.AudioSource)
			{
				base.AudioSource.mute = value;
			}
		}
	}

	public AudioSource audioSource
	{
		get
		{
			return AudioSourceE;
		}
		set
		{
			AudioSourceE = value;
			SetAudioSource(value);
		}
	}

	public Rect uvRect
	{
		get
		{
			return UVRect;
		}
		set
		{
			UVRect = value;
			if (rendererMode == DisplayType.uGUI && (bool)displayUGUI)
			{
				displayUGUI.uvRect = uvRect;
			}
		}
	}

	public ushort audioTrackCount
	{
		get
		{
			if (AudioTracks != null)
			{
				return (ushort)AudioTracks.GetAudioTracks().Count;
			}
			return 0;
		}
		private set
		{
		}
	}

	public double clockTime
	{
		get
		{
			if (base.UseResampler && base.FrameResampler != null)
			{
				return base.FrameResampler.ElapsedTimeSinceBase;
			}
			if (Info != null)
			{
				PlaybackQualityStats playbackQualityStats = Info.GetPlaybackQualityStats();
				int num = playbackQualityStats.DuplicateFrames + playbackQualityStats.UnityDroppedFrames + playbackQualityStats.SkippedFrames;
				float num2 = 1f - playbackQualityStats.PerfectFramesT;
				return ((float)num / num2 + (float)num) * (float)(long)(10000000.0 / (double)Info.GetVideoFrameRate());
			}
			return -1.0;
		}
		private set
		{
		}
	}

	public double externalReferenceTime
	{
		get
		{
			if (_baseMediaPlayer != null)
			{
				return _baseMediaPlayer.GetCurrentTime();
			}
			return -1.0;
		}
		private set
		{
		}
	}

	public long frame
	{
		get
		{
			if (Control != null)
			{
				return Control.GetCurrentTimeFrames();
			}
			return -1L;
		}
		set
		{
			if (canSetTime && Control != null)
			{
				Control.SeekToFrame((int)value);
			}
		}
	}

	public ulong frameCount
	{
		get
		{
			if (Info != null)
			{
				return (ulong)Info.GetMaxFrameNumber();
			}
			return 0uL;
		}
		private set
		{
		}
	}

	public float frameRate
	{
		get
		{
			if (Info != null)
			{
				return Info.GetVideoFrameRate();
			}
			return -1f;
		}
		private set
		{
		}
	}

	public uint height
	{
		get
		{
			if (Info != null)
			{
				return (uint)Info.GetVideoHeight();
			}
			return 0u;
		}
		private set
		{
		}
	}

	public bool isPaused
	{
		get
		{
			if (Control != null)
			{
				return Control.IsPaused();
			}
			return false;
		}
		private set
		{
		}
	}

	public bool isPlaying
	{
		get
		{
			if (Control != null)
			{
				return Control.IsPlaying();
			}
			return false;
		}
		private set
		{
		}
	}

	public bool isPrepared
	{
		get
		{
			if (Application.isPlaying)
			{
				return Control.CanPlay();
			}
			return false;
		}
		private set
		{
		}
	}

	public double length
	{
		get
		{
			if (Info != null)
			{
				return Info.GetDuration();
			}
			return -1.0;
		}
		private set
		{
		}
	}

	public Texture texture
	{
		get
		{
			if (TextureProducer != null)
			{
				return TextureProducer.GetTexture();
			}
			return null;
		}
		private set
		{
		}
	}

	public double time
	{
		get
		{
			if (Control != null)
			{
				return Control.GetCurrentTime();
			}
			return -1.0;
		}
		set
		{
			if (canSetTime && Control != null)
			{
				Control.SeekFast(value);
			}
		}
	}

	public uint width
	{
		get
		{
			if (Info != null)
			{
				return (uint)Info.GetVideoWidth();
			}
			return 0u;
		}
		private set
		{
		}
	}

	public float targetCameraAlpha
	{
		get
		{
			try
			{
				switch (rendererMode)
				{
				case DisplayType.uGUI:
					return displayUGUI.color.a;
				case DisplayType.IMGUI:
					return displayIMGUI.Color.a;
				case DisplayType.Mesh:
					return applyToMesh.MeshRenderer.material.color.a;
				case DisplayType.Material:
					return applyToMaterial.Material.color.a;
				case DisplayType.CameraFarPlane:
					return applyToFarPlane.Alpha;
				case DisplayType.RenderTexture:
					return applyToTexture.VideoResolveOptions.tint.a;
				}
			}
			catch
			{
				return 0f;
			}
			return 0f;
		}
		set
		{
			try
			{
				switch (rendererMode)
				{
				case DisplayType.uGUI:
					displayUGUI.color = new Color(displayUGUI.color.r, displayUGUI.color.g, displayUGUI.color.b, value);
					break;
				case DisplayType.IMGUI:
					displayIMGUI.Color = new Color(displayIMGUI.Color.r, displayIMGUI.Color.g, displayIMGUI.Color.b, value);
					break;
				case DisplayType.Mesh:
					applyToMesh.MeshRenderer.material.color = new Color(applyToMesh.MeshRenderer.material.color.r, applyToMesh.MeshRenderer.material.color.g, applyToMesh.MeshRenderer.material.color.b, value);
					break;
				case DisplayType.Material:
					applyToMaterial.Material.color = new Color(applyToMaterial.Material.color.r, applyToMaterial.Material.color.g, applyToMaterial.Material.color.b, value);
					break;
				case DisplayType.CameraFarPlane:
					applyToFarPlane.Alpha = value;
					break;
				case DisplayType.RenderTexture:
				{
					VideoResolveOptions videoResolveOptions = applyToTexture.VideoResolveOptions;
					videoResolveOptions.tint = new Color(videoResolveOptions.tint.r, videoResolveOptions.tint.g, videoResolveOptions.tint.b, value);
					applyToTexture.VideoResolveOptions = videoResolveOptions;
					break;
				}
				}
			}
			catch
			{
			}
		}
	}

	public RenderTexture targetTexture
	{
		get
		{
			if ((bool)applyToTexture)
			{
				return applyToTexture.ExternalTexture;
			}
			return null;
		}
		set
		{
			if ((bool)applyToTexture)
			{
				applyToTexture.ExternalTexture = value;
			}
		}
	}

	public Camera targetCamera
	{
		get
		{
			if ((bool)applyToFarPlane)
			{
				return applyToFarPlane.Camera;
			}
			return null;
		}
		set
		{
			if ((bool)applyToFarPlane)
			{
				applyToFarPlane.Camera = value;
			}
		}
	}

	public bool canSetDirectAudioVolume
	{
		get
		{
			if (isPrepared)
			{
				return true;
			}
			return false;
		}
		private set
		{
		}
	}

	public bool canSetPlaybackSpeed
	{
		get
		{
			if (isPrepared)
			{
				return true;
			}
			return false;
		}
		private set
		{
		}
	}

	public bool canSetSkipOnDrop
	{
		get
		{
			if (isPrepared)
			{
				return true;
			}
			return false;
		}
		private set
		{
		}
	}

	public bool canSetTime
	{
		get
		{
			if (isPrepared)
			{
				return true;
			}
			return false;
		}
		private set
		{
		}
	}

	public bool canSetTimeUpdateMode
	{
		get
		{
			if (isPrepared)
			{
				return true;
			}
			return false;
		}
		private set
		{
		}
	}

	public bool canStep
	{
		get
		{
			if (isPrepared)
			{
				return true;
			}
			return false;
		}
		private set
		{
		}
	}

	public int controlledAudioTrackCount
	{
		get
		{
			return 1;
		}
		set
		{
		}
	}

	public uint pixelAspectRationDenominator
	{
		get
		{
			LogNoSimWarning("pixelAspectRationDenominator");
			return 1u;
		}
		private set
		{
		}
	}

	public uint pixelAspectRationNumerator
	{
		get
		{
			LogNoSimWarning("pixelAspectRationNumerator");
			return 1u;
		}
		private set
		{
		}
	}

	public bool skipOnDrop
	{
		get
		{
			LogNoSimWarning("skipOnDrop");
			return false;
		}
		set
		{
			LogNoSimWarning("skipOnDrop");
		}
	}

	public int targetCamera3DLayout
	{
		get
		{
			LogNoSimWarning("targetCamera3DLayout");
			return 0;
		}
		set
		{
			LogNoSimWarning("targetCamera3DLayout");
		}
	}

	public int timeReference
	{
		get
		{
			LogNoSimWarning("timeReference");
			return 0;
		}
		set
		{
			LogNoSimWarning("timeReference");
		}
	}

	public bool WaitForFirstFrame
	{
		get
		{
			LogNoSimWarning("WaitForFirstFrame");
			return true;
		}
		set
		{
			LogNoSimWarning("WaitForFirstFrame");
		}
	}

	public event EventHandler prepareCompleted;

	public event EventHandler started;

	public event ErrorEventHandler errorReceived;

	public event EventHandler seekCompleted;

	public event FrameReadyEventHandler frameReady;

	public event EventHandler loopPointReached;

	public event EventHandler frameDropped;

	public event TimeEventHandler clockResyncOccurred;

	public void EventCallbacks(MediaPlayer mediaPlayer, MediaPlayerEvent.EventType eventType, ErrorCode errorCode)
	{
		switch (eventType)
		{
		case MediaPlayerEvent.EventType.ReadyToPlay:
			this.prepareCompleted?.Invoke((VideoPlayer_AVPro)mediaPlayer);
			return;
		case MediaPlayerEvent.EventType.Started:
			this.started?.Invoke((VideoPlayer_AVPro)mediaPlayer);
			return;
		case MediaPlayerEvent.EventType.Error:
			this.errorReceived?.Invoke((VideoPlayer_AVPro)mediaPlayer, errorCode.ToString());
			return;
		case MediaPlayerEvent.EventType.FinishedSeeking:
			this.seekCompleted?.Invoke((VideoPlayer_AVPro)mediaPlayer);
			return;
		}
		if (this.loopPointReached != null && this.loopPointReached.GetInvocationList().Length != 0)
		{
			LogNoSimWarning("loopPointReached Event", "AVPro does not contain a event callback for when the loop point is reached");
		}
		if (this.frameDropped != null && this.frameDropped.GetInvocationList().Length != 0)
		{
			LogNoSimWarning("frameDropped Event", "AVPro does not contain a event callback for when a frame is dropped");
		}
		if (this.clockResyncOccurred != null && this.clockResyncOccurred.GetInvocationList().Length != 0)
		{
			LogNoSimWarning("clockResyncOccurred Event", "AVPro does not contain a event callback for when clock resync is restored");
		}
		if (this.frameReady != null && this.frameReady.GetInvocationList().Length != 0)
		{
			LogNoSimWarning("frameReady Event", "AVPro does not contain a event callback for when each frame is ready, it does have one for when the first frame is ready though");
		}
	}

	public void EnableAudioTrack(ushort trackIndex, bool enabled)
	{
		if (Control.IsPlaying())
		{
			Debug.LogWarning("Audio track not changed, warning you cannot change audio track whilst playing the media");
			return;
		}
		AudioTracks audioTracks = AudioTracks.GetAudioTracks();
		if (audioTracks.Count == 0)
		{
			Debug.LogWarning("Warning: their are currenrly no audio tracks");
		}
		else if (audioTracks[trackIndex] != AudioTracks.GetActiveAudioTrack())
		{
			if (trackIndex >= audioTracks.Count)
			{
				Debug.LogError($"Error: trackIndex ({trackIndex}) is larger than total audioTrack count ({audioTracks.Count})");
				return;
			}
			AudioTracks.SetActiveAudioTrack(audioTracks[trackIndex]);
			Control.MuteAudio(enabled);
		}
	}

	public ushort GetAudioChannelCount(ushort trackIndex)
	{
		AudioTracks audioTracks = AudioTracks.GetAudioTracks();
		if (trackIndex >= audioTracks.Count)
		{
			Debug.LogError($"Error: trackIndex ({trackIndex}) is larger than total audioTrack count ({audioTracks.Count})");
			return 0;
		}
		return (ushort)audioTracks[trackIndex].ChannelCount;
	}

	public string GetAudioLanguageCode(ushort trackIndex)
	{
		AudioTracks audioTracks = AudioTracks.GetAudioTracks();
		if (trackIndex >= audioTracks.Count)
		{
			Debug.LogError($"Error: trackIndex ({trackIndex}) is larger than total audioTrack count ({audioTracks.Count})");
			return null;
		}
		return audioTracks[trackIndex].Language;
	}

	public uint GetAudioSampleRate(ushort trackIndex)
	{
		if (AudioSettings.GetConfiguration().sampleRate != 0)
		{
			return (uint)AudioSettings.outputSampleRate;
		}
		return 0u;
	}

	public bool GetDirectAudioMute(ushort trackIndex)
	{
		return AudioMuted;
	}

	public float GetDirectAudioVolume(ushort trackIndex)
	{
		return AudioVolume;
	}

	public AudioSource GetTargetSource(ushort trackIndex)
	{
		if (base.PlatformOptionsWindows.videoApi == Windows.VideoApi.MediaFoundation && base.PlatformOptionsWindows._audioMode == Windows.AudioOutput.Unity)
		{
			return base.AudioSource;
		}
		return null;
	}

	public bool IsAudioTrackEnabled(ushort trackIndex)
	{
		AudioTracks audioTracks = AudioTracks.GetAudioTracks();
		if (trackIndex >= audioTracks.Count)
		{
			Debug.LogError($"Error: trackIndex ({trackIndex}) is larger than total audioTrack count ({audioTracks.Count})");
			return false;
		}
		AudioTrack activeAudioTrack = AudioTracks.GetActiveAudioTrack();
		if (audioTracks[trackIndex] == activeAudioTrack)
		{
			return true;
		}
		return false;
	}

	public new void Pause()
	{
		base.Pause();
	}

	public new void Play()
	{
		if (!base.MediaOpened)
		{
			OpenMedia();
		}
		switch (rendererMode)
		{
		case DisplayType.Mesh:
			if ((bool)applyToMesh)
			{
				applyToMesh.enabled = true;
			}
			break;
		case DisplayType.Material:
			if ((bool)applyToMaterial)
			{
				applyToMaterial.enabled = true;
			}
			break;
		case DisplayType.uGUI:
			if ((bool)displayUGUI)
			{
				displayUGUI.enabled = true;
			}
			break;
		case DisplayType.IMGUI:
			if ((bool)displayIMGUI)
			{
				displayIMGUI.enabled = true;
			}
			break;
		case DisplayType.CameraFarPlane:
			if ((bool)applyToFarPlane)
			{
				applyToFarPlane.enabled = true;
			}
			break;
		case DisplayType.RenderTexture:
			if ((bool)applyToTexture)
			{
				applyToTexture.enabled = true;
			}
			break;
		}
		base.Play();
	}

	public void Prepare()
	{
		OpenMedia(autoPlay: false);
	}

	public void SetDirectAudioMute(ushort trackIndex, bool mute)
	{
		AudioMuted = mute;
	}

	public void SetDirectAudioVolume(ushort trackIndex, float volume)
	{
		if (canSetDirectAudioVolume)
		{
			AudioVolume = volume;
		}
	}

	public void SetTargetAudioSource(ushort trackIndex, AudioSource source)
	{
		if (base.PlatformOptionsWindows.videoApi == Windows.VideoApi.MediaFoundation && base.PlatformOptionsWindows._audioMode == Windows.AudioOutput.Unity)
		{
			SetAudioSource(source);
		}
	}

	public void StepForward()
	{
		if (canStep)
		{
			int currentTimeFrames = Control.GetCurrentTimeFrames();
			Control.SeekToFrame(currentTimeFrames + 1);
			Control.SeekToFrameRelative(1);
		}
	}

	public new void Stop()
	{
		if (Control == null)
		{
			return;
		}
		switch (rendererMode)
		{
		case DisplayType.Mesh:
			if ((bool)applyToMesh)
			{
				applyToMesh.enabled = false;
			}
			break;
		case DisplayType.Material:
			if ((bool)applyToMaterial)
			{
				applyToMaterial.enabled = false;
			}
			break;
		case DisplayType.uGUI:
			if ((bool)displayUGUI)
			{
				displayUGUI.enabled = false;
			}
			break;
		case DisplayType.IMGUI:
			if ((bool)displayIMGUI)
			{
				displayIMGUI.enabled = false;
			}
			break;
		case DisplayType.CameraFarPlane:
			if ((bool)applyToFarPlane)
			{
				applyToFarPlane.enabled = false;
			}
			break;
		case DisplayType.RenderTexture:
			if ((bool)applyToTexture)
			{
				applyToTexture.enabled = false;
			}
			break;
		}
		Control.SeekToFrame(0);
		base.Stop();
	}

	public void CreateAudioComponents()
	{
		DestroyAudioOutputBehaviour();
		if (audioOutputModeAVPro == Windows.AudioOutput.Unity)
		{
			AudioOutput audioOutput = base.gameObject.AddComponent<AudioOutput>();
			if (!base.gameObject.TryGetComponent<AudioSource>(out var component))
			{
				component = base.gameObject.AddComponent<AudioSource>();
			}
			else
			{
				audioSource = component;
			}
			audioOutput.SetAudioSource(component);
			audioOutput.Player = this;
		}
	}

	public void CreateRendererComponents()
	{
		DestroyRendererBehaviours();
		switch (rendererMode)
		{
		case DisplayType.Mesh:
			if (!base.gameObject.GetComponent<ApplyToMesh>())
			{
				applyToMesh = base.gameObject.AddComponent<ApplyToMesh>();
			}
			else
			{
				applyToMesh = base.gameObject.GetComponent<ApplyToMesh>();
			}
			applyToMesh.Player = this;
			_renderModeComponent = applyToMesh;
			break;
		case DisplayType.Material:
		{
			if (!base.gameObject.GetComponent<ApplyToMaterial>())
			{
				applyToMaterial = base.gameObject.AddComponent<ApplyToMaterial>();
			}
			else
			{
				applyToMaterial = base.gameObject.GetComponent<ApplyToMaterial>();
			}
			applyToMaterial.Player = this;
			_renderModeComponent = applyToMaterial;
			if (base.gameObject.TryGetComponent<Renderer>(out var component))
			{
				targetMaterial = component.material;
				applyToMaterial.Material = component.material;
			}
			break;
		}
		case DisplayType.uGUI:
			if (!canvasObj)
			{
				Debug.LogWarning("[AVProVideo] Warning, No Canvas Object Set For uGUI, Overriding DisplayType");
				break;
			}
			if (!canvasObj.GetComponent<DisplayUGUI>())
			{
				displayUGUI = canvasObj.AddComponent<DisplayUGUI>();
			}
			else
			{
				displayUGUI = canvasObj.GetComponent<DisplayUGUI>();
			}
			displayUGUI.Player = this;
			_renderModeComponent = displayUGUI;
			break;
		case DisplayType.IMGUI:
			if (!base.gameObject.GetComponent<DisplayIMGUI>())
			{
				displayIMGUI = base.gameObject.AddComponent<DisplayIMGUI>();
			}
			else
			{
				displayIMGUI = base.gameObject.GetComponent<DisplayIMGUI>();
			}
			displayIMGUI.Player = this;
			_renderModeComponent = displayIMGUI;
			displayIMGUI.Color = Color.white;
			break;
		case DisplayType.CameraFarPlane:
			if (!base.gameObject.GetComponent<ApplyToFarPlane>())
			{
				applyToFarPlane = base.gameObject.AddComponent<ApplyToFarPlane>();
			}
			else
			{
				applyToFarPlane = base.gameObject.GetComponent<ApplyToFarPlane>();
			}
			applyToFarPlane.Player = this;
			_renderModeComponent = applyToFarPlane;
			applyToFarPlane.VideoAspectRatio = (VideoAspectRatio)aspectRatioRenderTexture;
			applyToFarPlane.MainColor = color;
			break;
		case DisplayType.RenderTexture:
			if (!base.gameObject.GetComponent<ResolveToRenderTexture>())
			{
				applyToTexture = base.gameObject.AddComponent<ResolveToRenderTexture>();
			}
			else
			{
				applyToTexture = base.gameObject.GetComponent<ResolveToRenderTexture>();
			}
			applyToTexture.MediaPlayer = this;
			_renderModeComponent = applyToTexture;
			applyToTexture.ExternalTexture = TargetTexture;
			break;
		case DisplayType.None:
			_renderModeComponent = null;
			break;
		default:
			Debug.LogError("Error: Invalid Render Mode selected");
			break;
		}
	}

	public void DestroyAudioOutputBehaviour()
	{
		if (base.gameObject.TryGetComponent<AudioOutput>(out var component))
		{
			Object.DestroyImmediate(component);
		}
	}

	public void DestroyRendererBehaviours()
	{
		if (base.gameObject.TryGetComponent<ApplyToMesh>(out var component))
		{
			Object.DestroyImmediate(component);
		}
		if (base.gameObject.TryGetComponent<ApplyToMaterial>(out var component2))
		{
			Object.DestroyImmediate(component2);
		}
		if ((bool)canvasObj && canvasObj.TryGetComponent<DisplayUGUI>(out var component3))
		{
			Object.DestroyImmediate(component3);
		}
		if (base.gameObject.TryGetComponent<DisplayIMGUI>(out var component4))
		{
			Object.DestroyImmediate(component4);
		}
		if (base.gameObject.TryGetComponent<ApplyToFarPlane>(out var component5))
		{
			Object.DestroyImmediate(component5);
		}
		if (base.gameObject.TryGetComponent<ResolveToRenderTexture>(out var component6))
		{
			Object.DestroyImmediate(component6);
		}
	}

	public void SetOutputModeContextual()
	{
		DestroyRendererBehaviours();
		Renderer component3;
		if (base.gameObject.TryGetComponent<CanvasRenderer>(out var _) && base.gameObject.TryGetComponent<RectTransform>(out var _))
		{
			DisplayUGUI displayUGUI = base.gameObject.AddComponent<DisplayUGUI>();
			if (!displayUGUI)
			{
				_renderModeComponent = null;
				this.displayUGUI = null;
				rendererMode = DisplayType.None;
			}
			else
			{
				_renderModeComponent = displayUGUI;
				this.displayUGUI = displayUGUI;
				rendererMode = DisplayType.uGUI;
				displayUGUI.Player = this;
			}
		}
		else if (base.gameObject.TryGetComponent<Renderer>(out component3) && (bool)component3.sharedMaterial)
		{
			ApplyToMaterial applyToMaterial = (this.applyToMaterial = (ApplyToMaterial)(_renderModeComponent = base.gameObject.AddComponent<ApplyToMaterial>()));
			rendererMode = DisplayType.Material;
			applyToMaterial.Player = this;
		}
		else
		{
			_renderModeComponent = null;
			rendererMode = DisplayType.None;
		}
	}

	public bool OutputModeSet()
	{
		if (!applyToFarPlane && !applyToMaterial && !applyToMesh && !applyToTexture && !displayIMGUI)
		{
			return displayUGUI;
		}
		return true;
	}

	protected override void Start()
	{
		if (!_converted && _renderModeComponent == null && !OutputModeSet())
		{
			SetOutputModeContextual();
		}
		base.Start();
	}

	protected override void Awake()
	{
		if (Control == null && Application.isPlaying)
		{
			Initialise();
			if (Control != null)
			{
				if (AutoOpening)
				{
					OpenMedia(playOnAwake);
				}
				StartRenderCoroutine();
			}
		}
		base.Awake();
	}

	protected override void OnEnable()
	{
		base.Events.AddListener(EventCallbacks);
		base.OnEnable();
	}

	protected override void OnDisable()
	{
		this.prepareCompleted = null;
		this.started = null;
		this.errorReceived = null;
		this.seekCompleted = null;
		this.loopPointReached = null;
		this.frameDropped = null;
		this.clockResyncOccurred = null;
		base.Events.RemoveListener(EventCallbacks);
		base.OnDisable();
	}

	private void LogAutomaticConversionSolution(string value1, string value2, string solution)
	{
		Debug.LogWarning("[VideoPlayer_AVPro] Warning: Automatic Conversion Occuring from " + value1 + " to " + value2 + ". " + solution);
	}

	private void LogAutomaticConversion(string value1, string value2)
	{
		Debug.LogWarning("[VideoPlayer_AVPro] Warning: Automatic conversion occuring from " + value1 + " to " + value2 + ", unexpected behaviour may occur");
	}

	private void LogNoSimWarning(string behaviourName, string reason = "")
	{
		Debug.LogWarning("[VideoPlayer_AVPro] Warning: AVPro contains no similar Behaviour to \"" + behaviourName + "\". " + reason);
	}

	private void LogNoFunctionality(string behaviourName, string reason = "")
	{
		Debug.LogWarning("[VideoPlayer_AVPro] Warning: AVPro method/Parameter: \"" + behaviourName + "\" does not contain any functionality. " + reason);
	}
}
