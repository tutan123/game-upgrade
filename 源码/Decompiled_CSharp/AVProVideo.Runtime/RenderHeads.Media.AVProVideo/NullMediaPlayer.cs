using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public sealed class NullMediaPlayer : BaseMediaPlayer
{
	private bool _isPlaying;

	private bool _isPaused;

	private double _currentTime;

	private float _volume;

	private float _playbackRate = 1f;

	private bool _bLoop;

	private int _Width = 256;

	private int _height = 256;

	private Texture2D _texture;

	private Texture2D _texture_AVPro;

	private Texture2D _texture_AVPro1;

	private float _fakeFlipTime;

	private int _frameCount;

	private const float FrameRate = 10f;

	public override string GetVersion()
	{
		return "0.0.0";
	}

	public override string GetExpectedVersion()
	{
		return GetVersion();
	}

	public override bool OpenMedia(string path, long offset, string httpHeader, MediaHints mediaHints, int forceFileFormat = 0, bool startWithHighestBitrate = false)
	{
		_texture_AVPro = (Texture2D)Resources.Load("Textures/AVProVideo-NullPlayer-Frame0");
		_texture_AVPro1 = (Texture2D)Resources.Load("Textures/AVProVideo-NullPlayer-Frame1");
		if ((bool)_texture_AVPro)
		{
			_Width = _texture_AVPro.width;
			_height = _texture_AVPro.height;
		}
		_texture = _texture_AVPro;
		_fakeFlipTime = 0f;
		_frameCount = 0;
		return true;
	}

	public override void CloseMedia()
	{
		_frameCount = 0;
		Resources.UnloadAsset(_texture_AVPro);
		Resources.UnloadAsset(_texture_AVPro1);
		base.CloseMedia();
	}

	public override void SetLooping(bool bLooping)
	{
		_bLoop = bLooping;
	}

	public override bool IsLooping()
	{
		return _bLoop;
	}

	public override bool HasMetaData()
	{
		return true;
	}

	public override bool CanPlay()
	{
		return true;
	}

	public override bool HasAudio()
	{
		return false;
	}

	public override bool HasVideo()
	{
		return false;
	}

	public override void Play()
	{
		_isPlaying = true;
		_isPaused = false;
		_fakeFlipTime = 0f;
	}

	public override void Pause()
	{
		_isPlaying = false;
		_isPaused = true;
	}

	public override void Stop()
	{
		_isPlaying = false;
		_isPaused = false;
	}

	public override bool IsSeeking()
	{
		return false;
	}

	public override bool IsPlaying()
	{
		return _isPlaying;
	}

	public override bool IsPaused()
	{
		return _isPaused;
	}

	public override bool IsFinished()
	{
		if (_isPlaying)
		{
			return _currentTime >= GetDuration();
		}
		return false;
	}

	public override bool IsBuffering()
	{
		return false;
	}

	public override double GetDuration()
	{
		return 10.0;
	}

	public override int GetVideoWidth()
	{
		return _Width;
	}

	public override int GetVideoHeight()
	{
		return _height;
	}

	public override float GetVideoDisplayRate()
	{
		return 10f;
	}

	public override Texture GetTexture(int index)
	{
		return _texture;
	}

	public override int GetTextureFrameCount()
	{
		return _frameCount;
	}

	internal override StereoPacking InternalGetTextureStereoPacking()
	{
		return StereoPacking.Unknown;
	}

	public override bool RequiresVerticalFlip()
	{
		return false;
	}

	public override void Seek(double time)
	{
		_currentTime = time;
	}

	public override void SeekFast(double time)
	{
		_currentTime = time;
	}

	public override double GetCurrentTime()
	{
		return _currentTime;
	}

	public override void SetPlaybackRate(float rate)
	{
		_playbackRate = rate;
	}

	public override float GetPlaybackRate()
	{
		return _playbackRate;
	}

	public override void MuteAudio(bool bMuted)
	{
	}

	public override bool IsMuted()
	{
		return true;
	}

	public override void SetVolume(float volume)
	{
		_volume = volume;
	}

	public override float GetVolume()
	{
		return _volume;
	}

	public override float GetVideoFrameRate()
	{
		return 0f;
	}

	public override void Update()
	{
		UpdateSubtitles();
		if (!_isPlaying)
		{
			return;
		}
		_currentTime += Time.deltaTime;
		if (_currentTime >= GetDuration())
		{
			_currentTime = GetDuration();
			if (_bLoop)
			{
				Rewind();
			}
		}
		_fakeFlipTime += Time.deltaTime;
		if ((double)_fakeFlipTime >= 0.1)
		{
			_fakeFlipTime = 0f;
			_texture = ((_texture == _texture_AVPro) ? _texture_AVPro1 : _texture_AVPro);
			_frameCount++;
		}
	}

	public override void Render()
	{
	}

	public override void Dispose()
	{
	}

	internal override bool InternalSetActiveTrack(TrackType trackType, int trackUid)
	{
		return false;
	}

	internal override bool InternalIsChangedTracks(TrackType trackType)
	{
		return false;
	}

	internal override int InternalGetTrackCount(TrackType trackType)
	{
		return 0;
	}

	internal override TrackBase InternalGetTrackInfo(TrackType trackType, int index, ref bool isActiveTrack)
	{
		return null;
	}

	internal override bool InternalIsChangedTextCue()
	{
		return false;
	}

	internal override string InternalGetCurrentTextCue()
	{
		return null;
	}
}
