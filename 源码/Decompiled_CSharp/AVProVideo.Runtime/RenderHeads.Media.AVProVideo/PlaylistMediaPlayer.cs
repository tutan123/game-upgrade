using System;
using System.Collections;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

[HelpURL("https://www.renderheads.com/products/avpro-video/")]
[AddComponentMenu("AVPro Video/Playlist Media Player", -80)]
public class PlaylistMediaPlayer : MediaPlayer, ITextureProducer
{
	public enum Transition
	{
		None,
		Fade,
		Black,
		White,
		Transparent,
		Horiz,
		Vert,
		Diag,
		MirrorH,
		MirrorV,
		MirrorD,
		ScrollV,
		ScrollH,
		Circle,
		Diamond,
		Blinds,
		Arrows,
		SlideH,
		SlideV,
		Zoom,
		RectV,
		Random
	}

	public enum PlaylistLoopMode
	{
		None,
		Loop
	}

	public enum StartMode
	{
		Immediate,
		Manual
	}

	public enum ProgressMode
	{
		OnFinish,
		BeforeFinish,
		Manual
	}

	[SerializeField]
	private Shader _transitionShader;

	[SerializeField]
	private MediaPlayer _playerA;

	[SerializeField]
	private MediaPlayer _playerB;

	[SerializeField]
	private bool _playlistAutoProgress = true;

	[Tooltip("Close the video on the other MediaPlayer when it is not visible any more. This is useful for freeing up memory and GPU decoding resources.")]
	[SerializeField]
	private bool _autoCloseVideo = true;

	[SerializeField]
	private PlaylistLoopMode _playlistLoopMode;

	[SerializeField]
	private MediaPlaylist _playlist = new MediaPlaylist();

	[Tooltip("Pause the previously playing video. This is useful for systems that will struggle to play 2 videos at once")]
	[SerializeField]
	private bool _pausePreviousOnTransition = true;

	[SerializeField]
	private Transition _defaultTransition;

	[SerializeField]
	private float _defaultTransitionDuration = 1f;

	[SerializeField]
	private Easing.Preset _defaultTransitionEasing = Easing.Preset.Linear;

	[SerializeField]
	[Range(0f, 1f)]
	private float _playlistAudioVolume = 1f;

	[SerializeField]
	private bool _playlistAudioMuted;

	private static readonly LazyShaderProperty PropFromTex = new LazyShaderProperty("_FromTex");

	private static readonly LazyShaderProperty PropFade = new LazyShaderProperty("_Fade");

	private bool _isPaused;

	private int _playlistIndex;

	private MediaPlayer _nextPlayer;

	private Material _material;

	private Transition _currentTransition;

	private string _currentTransitionName = "LERP_NONE";

	private float _currentTransitionDuration = 1f;

	private Easing.Preset _currentTransitionEasing = Easing.Preset.Linear;

	private float _transitionTimer = float.MaxValue;

	private Func<float, float> _easeFunc;

	private RenderTexture _rt;

	private MediaPlaylist.MediaItem _currentItem;

	private MediaPlaylist.MediaItem _nextItem;

	private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

	public MediaPlayer CurrentPlayer
	{
		get
		{
			if (NextPlayer == _playerA)
			{
				return _playerB;
			}
			return _playerA;
		}
	}

	public MediaPlayer NextPlayer => _nextPlayer;

	public MediaPlaylist Playlist => _playlist;

	public int PlaylistIndex => _playlistIndex;

	public MediaPlaylist.MediaItem PlaylistItem
	{
		get
		{
			if (!_playlist.HasItemAt(_playlistIndex))
			{
				return null;
			}
			return _playlist.Items[_playlistIndex];
		}
	}

	public Transition DefaultTransition
	{
		get
		{
			return _defaultTransition;
		}
		set
		{
			_defaultTransition = value;
		}
	}

	public float DefaultTransitionDuration
	{
		get
		{
			return _defaultTransitionDuration;
		}
		set
		{
			_defaultTransitionDuration = value;
		}
	}

	public Easing.Preset DefaultTransitionEasing
	{
		get
		{
			return _defaultTransitionEasing;
		}
		set
		{
			_defaultTransitionEasing = value;
		}
	}

	public bool AutoCloseVideo
	{
		get
		{
			return _autoCloseVideo;
		}
		set
		{
			_autoCloseVideo = value;
		}
	}

	public PlaylistLoopMode LoopMode
	{
		get
		{
			return _playlistLoopMode;
		}
		set
		{
			_playlistLoopMode = value;
		}
	}

	public bool AutoProgress
	{
		get
		{
			return _playlistAutoProgress;
		}
		set
		{
			_playlistAutoProgress = value;
		}
	}

	public override IMediaInfo Info
	{
		get
		{
			if (!(CurrentPlayer != null))
			{
				return null;
			}
			return CurrentPlayer.Info;
		}
	}

	public override IMediaControl Control
	{
		get
		{
			if (!(CurrentPlayer != null))
			{
				return null;
			}
			return CurrentPlayer.Control;
		}
	}

	public override ITextureProducer TextureProducer => this;

	public override float AudioVolume
	{
		get
		{
			return _playlistAudioVolume;
		}
		set
		{
			_playlistAudioVolume = Mathf.Clamp01(value);
			if (!IsTransitioning() && CurrentPlayer != null)
			{
				CurrentPlayer.AudioVolume = _playlistAudioVolume;
			}
		}
	}

	public override bool AudioMuted
	{
		get
		{
			return _playlistAudioMuted;
		}
		set
		{
			_playlistAudioMuted = value;
			if (!IsTransitioning() && CurrentPlayer != null)
			{
				CurrentPlayer.AudioMuted = _playlistAudioMuted;
			}
		}
	}

	public override void Play()
	{
		_isPaused = false;
		if (Control != null)
		{
			Control.Play();
		}
		if (IsTransitioning() && !_pausePreviousOnTransition && NextPlayer.Control != null)
		{
			NextPlayer.Control.Play();
		}
	}

	public override void Pause()
	{
		_isPaused = true;
		if (Control != null)
		{
			Control.Pause();
		}
		if (IsTransitioning() && NextPlayer.Control != null)
		{
			NextPlayer.Control.Pause();
		}
	}

	public override void Stop()
	{
		_isPaused = true;
		if (Control != null)
		{
			Control.Stop();
		}
		if (IsTransitioning() && NextPlayer.Control != null)
		{
			NextPlayer.Control.Stop();
		}
	}

	public bool IsPaused()
	{
		return _isPaused;
	}

	private IEnumerator SwapPlayers()
	{
		yield return _waitForEndOfFrame;
		if (_pausePreviousOnTransition)
		{
			CurrentPlayer.Pause();
		}
		base.Events.Invoke(this, MediaPlayerEvent.EventType.PlaylistItemChanged, ErrorCode.None);
		if (_currentTransition != 0)
		{
			Texture currentPlayerTexture = GetCurrentPlayerTexture();
			Texture nextTexture = GetNextTexture();
			if (currentPlayerTexture != null && nextTexture != null)
			{
				int num = Mathf.Max(nextTexture.width, currentPlayerTexture.width);
				int num2 = Mathf.Max(nextTexture.height, currentPlayerTexture.height);
				if (_rt != null && (_rt.width != num || _rt.height != num2))
				{
					RenderTexture.ReleaseTemporary(_rt);
					_rt = null;
				}
				if (_rt == null)
				{
					_rt = RenderTexture.GetTemporary(num, num2, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Default, 1);
				}
				_material.SetTexture(PropFromTex.Id, currentPlayerTexture);
				_material.SetFloat(PropFade.Id, 0f);
				Graphics.Blit(nextTexture, _rt, _material);
				_easeFunc = Easing.GetFunction(_currentTransitionEasing);
				_transitionTimer = 0f;
			}
			else
			{
				_transitionTimer = float.MaxValue;
				NextPlayer.AudioVolume = AudioVolume;
				CurrentPlayer.AudioVolume = 0f;
				if (_autoCloseVideo)
				{
					CurrentPlayer.MediaPath.Path = string.Empty;
					CurrentPlayer.CloseMedia();
				}
			}
		}
		if (NextPlayer == _playerA)
		{
			_nextPlayer = _playerB;
		}
		else
		{
			_nextPlayer = _playerA;
		}
		_currentItem = _nextItem;
		_nextItem = null;
	}

	private Texture GetCurrentPlayerTexture(int index = 0)
	{
		if (CurrentPlayer != null && CurrentPlayer.TextureProducer != null)
		{
			return CurrentPlayer.TextureProducer.GetTexture(index);
		}
		return null;
	}

	private Texture GetNextTexture(int index = 0)
	{
		if (_nextPlayer != null && _nextPlayer.TextureProducer != null)
		{
			return _nextPlayer.TextureProducer.GetTexture(index);
		}
		return null;
	}

	protected override void Awake()
	{
		_nextPlayer = _playerA;
		if (_transitionShader == null)
		{
			_transitionShader = Shader.Find("AVProVideo/Internal/Transition");
			if (_transitionShader == null)
			{
				Debug.LogError("[AVProVideo] Missing transition shader");
			}
		}
		_material = new Material(_transitionShader);
		_easeFunc = Easing.GetFunction(_defaultTransitionEasing);
	}

	protected override void OnDestroy()
	{
		if (_rt != null)
		{
			RenderTexture.ReleaseTemporary(_rt);
			_rt = null;
		}
		if (_material != null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(_material);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(_material);
			}
			_material = null;
		}
		base.OnDestroy();
	}

	protected override void Start()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if ((bool)CurrentPlayer)
		{
			CurrentPlayer.Events.AddListener(OnMediaPlayerEvent);
			if ((bool)NextPlayer)
			{
				NextPlayer.Events.AddListener(OnMediaPlayerEvent);
			}
		}
		JumpToItem(0);
	}

	public void OnMediaPlayerEvent(MediaPlayer mediaPlayer, MediaPlayerEvent.EventType eventType, ErrorCode errorCode)
	{
		if (mediaPlayer == CurrentPlayer)
		{
			base.Events.Invoke(mediaPlayer, eventType, errorCode);
		}
		switch (eventType)
		{
		case MediaPlayerEvent.EventType.FirstFrameReady:
			if (mediaPlayer == NextPlayer)
			{
				StartCoroutine(SwapPlayers());
				base.Events.Invoke(mediaPlayer, eventType, errorCode);
			}
			break;
		case MediaPlayerEvent.EventType.FinishedPlaying:
			if (mediaPlayer == CurrentPlayer && _playlistAutoProgress && _currentItem.progressMode == ProgressMode.OnFinish)
			{
				NextItem();
			}
			break;
		}
	}

	public bool PrevItem()
	{
		return JumpToItem(_playlistIndex - 1);
	}

	public bool NextItem()
	{
		bool num = JumpToItem(_playlistIndex + 1);
		if (!num)
		{
			base.Events.Invoke(this, MediaPlayerEvent.EventType.PlaylistFinished, ErrorCode.None);
		}
		return num;
	}

	public bool CanJumpToItem(int index)
	{
		if (_playlistLoopMode == PlaylistLoopMode.Loop && _playlist.Items.Count > 0)
		{
			index %= _playlist.Items.Count;
			if (index < 0)
			{
				index += _playlist.Items.Count;
			}
		}
		return _playlist.HasItemAt(index);
	}

	public bool JumpToItem(int index)
	{
		if (_playlistLoopMode == PlaylistLoopMode.Loop && _playlist.Items.Count > 0)
		{
			index %= _playlist.Items.Count;
			if (index < 0)
			{
				index += _playlist.Items.Count;
			}
		}
		if (_playlist.HasItemAt(index))
		{
			_playlistIndex = index;
			_nextItem = _playlist.Items[_playlistIndex];
			OpenVideoFile(_nextItem);
			return true;
		}
		return false;
	}

	public void OpenVideoFile(MediaPlaylist.MediaItem mediaItem)
	{
		bool flag = false;
		if (NextPlayer.MediaPath == mediaItem.mediaPath)
		{
			flag = true;
		}
		if (!mediaItem.isOverrideTransition)
		{
			SetTransition(_defaultTransition, _defaultTransitionDuration, _defaultTransitionEasing);
		}
		else
		{
			SetTransition(mediaItem.overrideTransition, mediaItem.overrideTransitionDuration, mediaItem.overrideTransitionEasing);
		}
		bool loop2 = (NextPlayer.Loop = mediaItem.loop);
		base.Loop = loop2;
		NextPlayer.MediaPath = new MediaPath(mediaItem.mediaPath);
		base.MediaPath = new MediaPath(mediaItem.mediaPath);
		NextPlayer.AudioMuted = _playlistAudioMuted;
		NextPlayer.AudioVolume = _playlistAudioVolume;
		if (_transitionTimer < _currentTransitionDuration && _currentTransition != 0)
		{
			NextPlayer.AudioVolume = 0f;
		}
		if (flag)
		{
			NextPlayer.Rewind(pause: false);
			if (_nextItem.startMode == StartMode.Immediate)
			{
				NextPlayer.Play();
			}
			StartCoroutine(SwapPlayers());
		}
		else if (string.IsNullOrEmpty(NextPlayer.MediaPath.Path))
		{
			NextPlayer.CloseMedia();
		}
		else
		{
			NextPlayer.OpenMedia(NextPlayer.MediaPath.PathType, NextPlayer.MediaPath.Path, _nextItem.startMode == StartMode.Immediate);
		}
	}

	private bool IsTransitioning()
	{
		bool num = _currentTransition != Transition.None;
		bool flag = _transitionTimer < _currentTransitionDuration;
		return num && flag;
	}

	private void SetTransition(Transition transition, float duration, Easing.Preset easing)
	{
		if (transition == Transition.Random)
		{
			transition = (Transition)UnityEngine.Random.Range(0, 21);
		}
		if (transition != _currentTransition)
		{
			if (!string.IsNullOrEmpty(_currentTransitionName))
			{
				_material.DisableKeyword(_currentTransitionName);
			}
			_currentTransition = transition;
			_currentTransitionName = GetTransitionName(transition);
			_material.EnableKeyword(_currentTransitionName);
		}
		_currentTransitionDuration = duration;
		_currentTransitionEasing = easing;
	}

	protected override void Update()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (!IsPaused())
		{
			if (IsTransitioning())
			{
				_transitionTimer += Time.deltaTime;
				float num = _easeFunc(Mathf.Clamp01(_transitionTimer / _currentTransitionDuration));
				NextPlayer.AudioVolume = (1f - num) * AudioVolume;
				CurrentPlayer.AudioVolume = num * AudioVolume;
				_material.SetFloat(PropFade.Id, num);
				_rt.DiscardContents();
				Graphics.Blit(GetCurrentPlayerTexture(), _rt, _material);
				if (!IsTransitioning())
				{
					if (_autoCloseVideo)
					{
						if (NextPlayer != null)
						{
							NextPlayer.MediaPath.Path = string.Empty;
							NextPlayer.CloseMedia();
						}
					}
					else if (!_pausePreviousOnTransition && NextPlayer != null && NextPlayer.Control.IsPlaying())
					{
						NextPlayer.Pause();
					}
				}
			}
			else if (_playlistAutoProgress)
			{
				if (_nextItem == null && _currentItem != null && _currentItem.progressMode == ProgressMode.BeforeFinish && Control != null && Control.HasMetaData() && Control.GetCurrentTime() >= Info.GetDuration() - (double)_currentItem.progressTimeSeconds)
				{
					NextItem();
				}
				else if (_currentItem == null)
				{
					JumpToItem(_playlistIndex);
				}
			}
		}
		base.Update();
	}

	public Texture GetTexture(int index = 0)
	{
		if (IsTransitioning())
		{
			return _rt;
		}
		if (CurrentPlayer.TextureProducer != null)
		{
			return CurrentPlayer.TextureProducer.GetTexture(index);
		}
		return null;
	}

	public int GetTextureCount()
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return 0;
		}
		return CurrentPlayer.TextureProducer.GetTextureCount();
	}

	public int GetTextureFrameCount()
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return 0;
		}
		return CurrentPlayer.TextureProducer.GetTextureFrameCount();
	}

	public bool SupportsTextureFrameCount()
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return false;
		}
		return CurrentPlayer.TextureProducer.SupportsTextureFrameCount();
	}

	public long GetTextureTimeStamp()
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return 0L;
		}
		return CurrentPlayer.TextureProducer.GetTextureTimeStamp();
	}

	public float GetTexturePixelAspectRatio()
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return 0f;
		}
		return CurrentPlayer.TextureProducer.GetTexturePixelAspectRatio();
	}

	public bool RequiresVerticalFlip()
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return false;
		}
		return CurrentPlayer.TextureProducer.RequiresVerticalFlip();
	}

	public Matrix4x4 GetYpCbCrTransform()
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return Matrix4x4.identity;
		}
		return CurrentPlayer.TextureProducer.GetYpCbCrTransform();
	}

	public StereoPacking GetTextureStereoPacking()
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return StereoPacking.Monoscopic;
		}
		return CurrentPlayer.TextureProducer.GetTextureStereoPacking();
	}

	public TransparencyMode GetTextureTransparency()
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return TransparencyMode.Opaque;
		}
		return CurrentPlayer.TextureProducer.GetTextureTransparency();
	}

	public AlphaPacking GetTextureAlphaPacking()
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return AlphaPacking.None;
		}
		return CurrentPlayer.TextureProducer.GetTextureAlphaPacking();
	}

	public float[] GetAffineTransform()
	{
		if (CurrentPlayer.TextureProducer != null)
		{
			return CurrentPlayer.TextureProducer.GetAffineTransform();
		}
		return new float[6] { 1f, 0f, 0f, 0f, 1f, 0f };
	}

	public Matrix4x4 GetTextureMatrix()
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return Matrix4x4.identity;
		}
		return CurrentPlayer.TextureProducer.GetTextureMatrix();
	}

	public RenderTextureFormat GetCompatibleRenderTextureFormat(GetCompatibleRenderTextureFormatOptions options, int plane)
	{
		if (CurrentPlayer.TextureProducer == null)
		{
			return RenderTextureFormat.Default;
		}
		return CurrentPlayer.TextureProducer.GetCompatibleRenderTextureFormat(options, plane);
	}

	private static string GetTransitionName(Transition transition)
	{
		return transition switch
		{
			Transition.None => "LERP_NONE", 
			Transition.Fade => "LERP_FADE", 
			Transition.Black => "LERP_BLACK", 
			Transition.White => "LERP_WHITE", 
			Transition.Transparent => "LERP_TRANSP", 
			Transition.Horiz => "LERP_HORIZ", 
			Transition.Vert => "LERP_VERT", 
			Transition.Diag => "LERP_DIAG", 
			Transition.MirrorH => "LERP_HORIZ_MIRROR", 
			Transition.MirrorV => "LERP_VERT_MIRROR", 
			Transition.MirrorD => "LERP_DIAG_MIRROR", 
			Transition.ScrollV => "LERP_SCROLL_VERT", 
			Transition.ScrollH => "LERP_SCROLL_HORIZ", 
			Transition.Circle => "LERP_CIRCLE", 
			Transition.Diamond => "LERP_DIAMOND", 
			Transition.Blinds => "LERP_BLINDS", 
			Transition.Arrows => "LERP_ARROW", 
			Transition.SlideH => "LERP_SLIDE_HORIZ", 
			Transition.SlideV => "LERP_SLIDE_VERT", 
			Transition.Zoom => "LERP_ZOOM_FADE", 
			Transition.RectV => "LERP_RECTS_VERT", 
			_ => string.Empty, 
		};
	}
}
