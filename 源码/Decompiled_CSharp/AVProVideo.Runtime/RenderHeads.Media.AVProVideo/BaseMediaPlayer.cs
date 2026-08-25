using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public abstract class BaseMediaPlayer : IMediaPlayer, IMediaControl, IMediaInfo, IMediaCache, ITextureProducer, IMediaSubtitles, IVideoTracks, IAudioTracks, ITextTracks, ITimedMetadata, IVariants, IEnumerable, IDisposable
{
	protected string _playerDescription = string.Empty;

	protected ErrorCode _lastError;

	protected FilterMode _defaultTextureFilterMode = FilterMode.Bilinear;

	protected TextureWrapMode _defaultTextureWrapMode = TextureWrapMode.Clamp;

	protected int _defaultTextureAnisoLevel = 1;

	protected MediaHints _mediaHints;

	protected TimeRanges _seekableTimes = new TimeRanges();

	protected TimeRanges _bufferedTimes = new TimeRanges();

	private float _displayRateTimer;

	private int _displayRateLastFrameCount;

	private float _displayRate = 1f;

	private float _stallDetectionTimer;

	private int _stallDetectionFrame;

	private int _stallDetectionGuard;

	protected List<Subtitle> _subtitles;

	protected Subtitle _currentSubtitle;

	protected PlaybackQualityStats _playbackQualityStats = new PlaybackQualityStats();

	protected TextCue _currentTextCue;

	private TimedMetadataItem _timedMetadataItem;

	private bool _hasNewTimedMetadataItem;

	protected VideoTracks _videoTracks = new VideoTracks();

	protected AudioTracks _audioTracks = new AudioTracks();

	protected TextTracks _textTracks = new TextTracks();

	protected TrackCollection[] _trackCollections;

	protected List<Variant> _variants = new List<Variant>();

	public int Count => _variants.Count;

	public Variant Current => GetSelectedVariant();

	public Variant this[int index] => _variants[index];

	public BaseMediaPlayer()
	{
		InitTracks();
	}

	public abstract string GetVersion();

	public abstract string GetExpectedVersion();

	public abstract bool OpenMedia(string path, long offset, string customHttpHeaders, MediaHints mediaHints, int forceFileFormat = 0, bool startWithHighestBitrate = false);

	public virtual bool OpenMediaFromBuffer(byte[] buffer)
	{
		return false;
	}

	public virtual bool StartOpenMediaFromBuffer(ulong length)
	{
		return false;
	}

	public virtual bool AddChunkToMediaBuffer(byte[] chunk, ulong offset, ulong length)
	{
		return false;
	}

	public virtual bool EndOpenMediaFromBuffer()
	{
		return false;
	}

	public virtual void CloseMedia()
	{
		_displayRateTimer = 0f;
		_displayRateLastFrameCount = 0;
		_displayRate = 0f;
		_stallDetectionTimer = 0f;
		_stallDetectionFrame = 0;
		_lastError = ErrorCode.None;
		_textTracks.Clear();
		_audioTracks.Clear();
		_videoTracks.Clear();
		_currentTextCue = null;
		_mediaHints = default(MediaHints);
	}

	public abstract void SetLooping(bool looping);

	public abstract bool IsLooping();

	public abstract bool HasMetaData();

	public abstract bool CanPlay();

	public abstract void Play();

	public abstract void Pause();

	public abstract void Stop();

	public virtual void Rewind()
	{
		SeekFast(0.0);
	}

	public abstract void Seek(double time);

	public abstract void SeekFast(double time);

	public virtual void SeekWithTolerance(double time, double timeDeltaBefore, double timeDeltaAfter)
	{
		Seek(time);
	}

	public abstract double GetCurrentTime();

	public virtual DateTime GetProgramDateTime()
	{
		return DateTime.MinValue;
	}

	public abstract float GetPlaybackRate();

	public abstract void SetPlaybackRate(float rate);

	public abstract double GetDuration();

	public abstract int GetVideoWidth();

	public abstract int GetVideoHeight();

	public abstract float GetVideoFrameRate();

	public virtual float GetVideoDisplayRate()
	{
		return _displayRate;
	}

	public abstract bool HasAudio();

	public abstract bool HasVideo();

	public bool IsVideoStereo()
	{
		return GetTextureStereoPacking() != StereoPacking.Monoscopic;
	}

	public abstract bool IsSeeking();

	public abstract bool IsPlaying();

	public abstract bool IsPaused();

	public abstract bool IsFinished();

	public abstract bool IsBuffering();

	public virtual bool WaitForNextFrame(Camera dummyCamera, int previousFrameCount)
	{
		return false;
	}

	public virtual int GetTextureCount()
	{
		return 1;
	}

	public abstract Texture GetTexture(int index = 0);

	public abstract int GetTextureFrameCount();

	public virtual bool SupportsTextureFrameCount()
	{
		return true;
	}

	public virtual long GetTextureTimeStamp()
	{
		return long.MinValue;
	}

	public abstract bool RequiresVerticalFlip();

	public virtual float GetTexturePixelAspectRatio()
	{
		return 1f;
	}

	public virtual Matrix4x4 GetYpCbCrTransform()
	{
		return Matrix4x4.identity;
	}

	public virtual float[] GetAffineTransform()
	{
		return new float[6] { 1f, 0f, 0f, 1f, 0f, 0f };
	}

	public virtual float[] GetTextureTransform()
	{
		return GetAffineTransform();
	}

	public virtual Matrix4x4 GetTextureMatrix()
	{
		float[] affineTransform = GetAffineTransform();
		if (affineTransform == null || affineTransform.Length != 6)
		{
			return Matrix4x4.identity;
		}
		Vector4 column = new Vector4(affineTransform[0], affineTransform[1], 0f, 0f);
		Vector4 column2 = new Vector4(affineTransform[2], affineTransform[3], 0f, 0f);
		Vector4 column3 = new Vector4(0f, 0f, 1f, 0f);
		Vector4 column4 = new Vector4(affineTransform[4], affineTransform[5], 0f, 1f);
		return new Matrix4x4(column, column2, column3, column4);
	}

	public virtual RenderTextureFormat GetCompatibleRenderTextureFormat(GetCompatibleRenderTextureFormatOptions options, int plane)
	{
		return RenderTextureFormat.Default;
	}

	public StereoPacking GetTextureStereoPacking()
	{
		StereoPacking stereoPacking = InternalGetTextureStereoPacking();
		if (stereoPacking == StereoPacking.Unknown)
		{
			stereoPacking = _mediaHints.stereoPacking;
		}
		return stereoPacking;
	}

	internal abstract StereoPacking InternalGetTextureStereoPacking();

	public virtual TransparencyMode GetTextureTransparency()
	{
		return _mediaHints.transparency;
	}

	public AlphaPacking GetTextureAlphaPacking()
	{
		if (GetTextureTransparency() == TransparencyMode.Transparent)
		{
			return _mediaHints.alphaPacking;
		}
		return AlphaPacking.None;
	}

	public abstract void MuteAudio(bool bMuted);

	public abstract bool IsMuted();

	public abstract void SetVolume(float volume);

	public virtual void SetBalance(float balance)
	{
	}

	public abstract float GetVolume();

	public virtual float GetBalance()
	{
		return 0f;
	}

	public virtual int GetAudioChannelCount()
	{
		return -1;
	}

	public virtual AudioChannelMaskFlags GetAudioChannelMask()
	{
		return AudioChannelMaskFlags.Unspecified;
	}

	public virtual int GrabAudio(float[] audioData, int audioDataFloatCount, int channelCount)
	{
		return 0;
	}

	public virtual int GetAudioBufferedSampleCount()
	{
		return 0;
	}

	public virtual void AudioConfigurationChanged(bool deviceChanged)
	{
	}

	public virtual void SetAudioHeadRotation(Quaternion q)
	{
	}

	public virtual void ResetAudioHeadRotation()
	{
	}

	public virtual void SetAudioChannelMode(Audio360ChannelMode channelMode)
	{
	}

	public virtual void SetAudioFocusEnabled(bool enabled)
	{
	}

	public virtual void SetAudioFocusProperties(float offFocusLevel, float widthDegrees)
	{
	}

	public virtual void SetAudioFocusRotation(Quaternion q)
	{
	}

	public virtual void ResetAudioFocus()
	{
	}

	public virtual long GetEstimatedTotalBandwidthUsed()
	{
		return -1L;
	}

	public virtual void SetPlayWithoutBuffering(bool playWithoutBuffering)
	{
	}

	public virtual bool IsMediaCachingSupported()
	{
		return false;
	}

	public virtual void AddMediaToCache(string url, string headers, MediaCachingOptions options)
	{
	}

	public virtual void CancelDownloadOfMediaToCache(string url)
	{
	}

	public virtual void PauseDownloadOfMediaToCache(string url)
	{
	}

	public virtual void ResumeDownloadOfMediaToCache(string url)
	{
	}

	public virtual void RemoveMediaFromCache(string url)
	{
	}

	public virtual CachedMediaStatus GetCachedMediaStatus(string url, ref float progress)
	{
		return CachedMediaStatus.NotCached;
	}

	public virtual bool IsExternalPlaybackSupported()
	{
		return false;
	}

	public virtual bool IsExternalPlaybackActive()
	{
		return false;
	}

	public virtual void SetAllowsExternalPlayback(bool enable)
	{
	}

	public virtual void SetExternalPlaybackVideoGravity(ExternalPlaybackVideoGravity gravity)
	{
	}

	public virtual void SetKeyServerAuthToken(string token)
	{
	}

	public virtual void SetOverrideDecryptionKey(byte[] key)
	{
	}

	public abstract void Update();

	public virtual void BeginRender()
	{
	}

	public abstract void Render();

	public abstract void Dispose();

	public virtual bool GetDecoderPerformance(ref int activeDecodeThreadCount, ref int decodedFrameCount, ref int droppedFrameCount)
	{
		return false;
	}

	public virtual void EndUpdate()
	{
	}

	public virtual IntPtr GetNativePlayerHandle()
	{
		return IntPtr.Zero;
	}

	public ErrorCode GetLastError()
	{
		ErrorCode lastError = _lastError;
		_lastError = ErrorCode.None;
		return lastError;
	}

	public virtual long GetLastExtendedErrorCode()
	{
		return 0L;
	}

	public string GetPlayerDescription()
	{
		return _playerDescription;
	}

	public virtual bool PlayerSupportsLinearColorSpace()
	{
		return true;
	}

	public TimeRanges GetSeekableTimes()
	{
		return _seekableTimes;
	}

	public TimeRanges GetBufferedTimes()
	{
		return _bufferedTimes;
	}

	public void GetTextureProperties(out FilterMode filterMode, out TextureWrapMode wrapMode, out int anisoLevel)
	{
		filterMode = _defaultTextureFilterMode;
		wrapMode = _defaultTextureWrapMode;
		anisoLevel = _defaultTextureAnisoLevel;
	}

	public void SetTextureProperties(FilterMode filterMode = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp, int anisoLevel = 0)
	{
		_defaultTextureFilterMode = filterMode;
		_defaultTextureWrapMode = wrapMode;
		_defaultTextureAnisoLevel = anisoLevel;
		for (int i = 0; i < GetTextureCount(); i++)
		{
			ApplyTextureProperties(GetTexture(i));
		}
	}

	protected virtual void ApplyTextureProperties(Texture texture)
	{
		if (texture != null)
		{
			texture.filterMode = _defaultTextureFilterMode;
			texture.wrapMode = _defaultTextureWrapMode;
			texture.anisoLevel = _defaultTextureAnisoLevel;
		}
	}

	protected void UpdateDisplayFrameRate()
	{
		if (_displayRateTimer >= 0.5f)
		{
			int textureFrameCount = GetTextureFrameCount();
			int num = textureFrameCount - _displayRateLastFrameCount;
			_displayRate = (float)num / _displayRateTimer;
			_displayRateTimer -= 0.5f;
			if (_displayRateTimer >= 0.5f)
			{
				_displayRateTimer -= 0.5f;
			}
			if (_displayRateTimer >= 0.5f)
			{
				_displayRateTimer = 0f;
			}
			_displayRateLastFrameCount = textureFrameCount;
		}
		float deltaTime = Time.deltaTime;
		_displayRateTimer += deltaTime;
	}

	protected bool IsExpectingNewVideoFrame()
	{
		if (HasVideo() && !IsFinished() && !IsPaused() && IsPlaying() && GetPlaybackRate() != 0f && (GetTextureFrameCount() <= 0 || GetDurationFrames() != 1))
		{
			return true;
		}
		return false;
	}

	public virtual bool IsPlaybackStalled()
	{
		if (SupportsTextureFrameCount() && IsExpectingNewVideoFrame())
		{
			int textureFrameCount = GetTextureFrameCount();
			if (textureFrameCount != _stallDetectionFrame)
			{
				_stallDetectionTimer = 0f;
				_stallDetectionFrame = textureFrameCount;
			}
			else if (_stallDetectionGuard != Time.frameCount)
			{
				_stallDetectionTimer += Time.deltaTime;
			}
			_stallDetectionGuard = Time.frameCount;
			float num = 0.5f;
			num = Mathf.Max(num / Mathf.Abs(GetPlaybackRate()), 0.5f);
			float videoFrameRate = GetVideoFrameRate();
			if (videoFrameRate > 0f && !float.IsNaN(videoFrameRate))
			{
				num = Mathf.Max(num, 2f / videoFrameRate);
			}
			return _stallDetectionTimer > num;
		}
		_stallDetectionTimer = 0f;
		return false;
	}

	public bool LoadSubtitlesSRT(string data)
	{
		if (string.IsNullOrEmpty(data))
		{
			_subtitles = null;
			_currentSubtitle = null;
		}
		else
		{
			_subtitles = SubtitleUtils.ParseSubtitlesSRT(data);
			_currentSubtitle = null;
		}
		return _subtitles != null;
	}

	public virtual void UpdateSubtitles()
	{
		if (_subtitles == null)
		{
			return;
		}
		double currentTime = GetCurrentTime();
		int num = 0;
		if (_currentSubtitle != null && !_currentSubtitle.IsTime(currentTime))
		{
			if (currentTime > _currentSubtitle.timeEnd)
			{
				num = _currentSubtitle.index + 1;
			}
			_currentSubtitle = null;
		}
		if (_currentSubtitle != null)
		{
			return;
		}
		for (int i = num; i < _subtitles.Count; i++)
		{
			if (_subtitles[i].IsTime(currentTime))
			{
				_currentSubtitle = _subtitles[i];
				break;
			}
		}
	}

	public virtual int GetSubtitleIndex()
	{
		int result = -1;
		if (_currentSubtitle != null)
		{
			result = _currentSubtitle.index;
		}
		return result;
	}

	public virtual string GetSubtitleText()
	{
		string result = string.Empty;
		if (_currentSubtitle != null)
		{
			result = _currentSubtitle.text;
		}
		else if (_currentTextCue != null)
		{
			result = _currentTextCue.Text;
		}
		return result;
	}

	public virtual void OnEnable()
	{
	}

	public int GetCurrentTimeFrames(float overrideFrameRate = 0f)
	{
		int result = 0;
		float num = ((overrideFrameRate > 0f) ? overrideFrameRate : GetVideoFrameRate());
		if (num > 0f)
		{
			result = Helper.ConvertTimeSecondsToFrame(GetCurrentTime(), num);
			result = Mathf.Min(result, GetMaxFrameNumber(overrideFrameRate));
		}
		return result;
	}

	public int GetDurationFrames(float overrideFrameRate = 0f)
	{
		int result = 0;
		float num = ((overrideFrameRate > 0f) ? overrideFrameRate : GetVideoFrameRate());
		if (num > 0f)
		{
			result = Helper.ConvertTimeSecondsToFrame(GetDuration(), num);
		}
		return result;
	}

	public int GetMaxFrameNumber(float overrideFrameRate = 0f)
	{
		int durationFrames = GetDurationFrames(overrideFrameRate);
		return Mathf.Max(0, durationFrames - 1);
	}

	public void SeekToFrameRelative(int frameOffset, float overrideFrameRate = 0f)
	{
		float num = ((overrideFrameRate > 0f) ? overrideFrameRate : GetVideoFrameRate());
		if (num > 0f)
		{
			double time = Helper.ConvertFrameToTimeSeconds(Mathf.Clamp(Helper.ConvertTimeSecondsToFrame(GetCurrentTime(), num) + frameOffset, 0, GetMaxFrameNumber(num)), num);
			Seek(time);
		}
	}

	public void SeekToFrame(int frame, float overrideFrameRate = 0f)
	{
		float num = ((overrideFrameRate > 0f) ? overrideFrameRate : GetVideoFrameRate());
		if (num > 0f)
		{
			frame = Mathf.Clamp(frame, 0, GetMaxFrameNumber(num));
			double time = Helper.ConvertFrameToTimeSeconds(frame, num);
			Seek(time);
		}
	}

	public PlaybackQualityStats GetPlaybackQualityStats()
	{
		return _playbackQualityStats;
	}

	public TextCue GetCurrentTextCue()
	{
		return _currentTextCue;
	}

	protected bool UpdateTextCue(bool force = false)
	{
		bool result = false;
		if (force || InternalIsChangedTextCue())
		{
			_currentTextCue = null;
			string text = InternalGetCurrentTextCue();
			if (!string.IsNullOrEmpty(text))
			{
				_currentTextCue = new TextCue(text);
			}
			result = true;
		}
		return result;
	}

	internal abstract bool InternalIsChangedTextCue();

	internal abstract string InternalGetCurrentTextCue();

	public bool HasNewTimedMetadataItem()
	{
		return _hasNewTimedMetadataItem;
	}

	public TimedMetadataItem GetTimedMetadataItem()
	{
		_hasNewTimedMetadataItem = false;
		return _timedMetadataItem;
	}

	protected void UpdateTimedMetadata()
	{
		if (InternalHasUpdatedTimedMetadata())
		{
			_timedMetadataItem = InternalGetTimedMetadataItem();
			_hasNewTimedMetadataItem = true;
		}
	}

	protected virtual bool InternalHasUpdatedTimedMetadata()
	{
		return false;
	}

	protected virtual TimedMetadataItem InternalGetTimedMetadataItem()
	{
		return null;
	}

	public VideoTracks GetVideoTracks()
	{
		return _videoTracks;
	}

	public AudioTracks GetAudioTracks()
	{
		return _audioTracks;
	}

	public TextTracks GetTextTracks()
	{
		return _textTracks;
	}

	public VideoTrack GetActiveVideoTrack()
	{
		return _videoTracks.ActiveTrack;
	}

	public AudioTrack GetActiveAudioTrack()
	{
		return _audioTracks.ActiveTrack;
	}

	public TextTrack GetActiveTextTrack()
	{
		return _textTracks.ActiveTrack;
	}

	public void SetActiveVideoTrack(VideoTrack track)
	{
		if (track != null)
		{
			SetActiveTrack(_videoTracks, track);
		}
	}

	public void SetActiveAudioTrack(AudioTrack track)
	{
		if (track != null)
		{
			SetActiveTrack(_audioTracks, track);
		}
	}

	public void SetActiveTextTrack(TextTrack track)
	{
		SetActiveTrack(_textTracks, track);
	}

	public int GetTextTrackArrayIndexFromUid(int Uid)
	{
		return _trackCollections[2].GetTrackArrayIndexFromUid(Uid);
	}

	internal abstract bool InternalIsChangedTracks(TrackType trackType);

	internal abstract int InternalGetTrackCount(TrackType trackType);

	internal abstract bool InternalSetActiveTrack(TrackType trackType, int trackUid);

	internal abstract TrackBase InternalGetTrackInfo(TrackType trackType, int trackIndex, ref bool isActiveTrack);

	private void InitTracks()
	{
		_trackCollections = new TrackCollection[3] { _videoTracks, _audioTracks, _textTracks };
	}

	protected void UpdateTracks()
	{
		TrackCollection[] trackCollections = _trackCollections;
		foreach (TrackCollection trackCollection in trackCollections)
		{
			if (InternalIsChangedTracks(trackCollection.TrackType))
			{
				PopulateTrackCollection(trackCollection);
			}
		}
	}

	private void PopulateTrackCollection(TrackCollection collection)
	{
		collection.Clear();
		int num = InternalGetTrackCount(collection.TrackType);
		for (int i = 0; i < num; i++)
		{
			bool isActiveTrack = false;
			TrackBase trackBase = InternalGetTrackInfo(collection.TrackType, i, ref isActiveTrack);
			if (trackBase != null)
			{
				collection.Add(trackBase);
				if (isActiveTrack)
				{
					collection.SetActiveTrack(trackBase);
				}
			}
			else
			{
				Debug.LogWarning($"[AVProVideo] Failed to enumerate {collection.TrackType} track {i} ");
			}
		}
	}

	private void SetActiveTrack(TrackCollection collection, TrackBase track)
	{
		if (collection.IsActiveTrack(track))
		{
			return;
		}
		int trackUid = -1;
		if (track != null)
		{
			trackUid = track.Uid;
		}
		if (InternalSetActiveTrack(collection.TrackType, trackUid))
		{
			collection.SetActiveTrack(track);
			if (collection.TrackType == TrackType.Text)
			{
				UpdateTextCue(force: true);
			}
		}
	}

	public virtual IEnumerator GetEnumerator()
	{
		return _variants.GetEnumerator();
	}

	public virtual Variant GetSelectedVariant()
	{
		return Variant.Auto;
	}

	public virtual void SelectVariant(Variant variant)
	{
	}

	protected virtual void UpdateVariants()
	{
		_variants.Clear();
		int num = InternalGetVariantCount();
		for (int i = 0; i < num; i++)
		{
			Variant variant = InternalGetVariantAtIndex(i);
			if (variant != null)
			{
				_variants.Add(variant);
			}
		}
		_variants.Sort(delegate(Variant x, Variant y)
		{
			int num2 = x.VideoCodecType.CompareTo(y.VideoCodecType);
			if (num2 == 0)
			{
				num2 = x.PeakDataRate.CompareTo(y.PeakDataRate);
			}
			return num2;
		});
	}

	internal virtual int InternalGetVariantCount()
	{
		return 0;
	}

	internal virtual Variant InternalGetVariantAtIndex(int index)
	{
		return null;
	}
}
