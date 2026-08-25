using System;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public interface IMediaControl
{
	bool OpenMedia(string path, long offset, string customHttpHeaders, MediaHints mediahints, int forceFileFormat = 0, bool startWithHighestBitrate = false);

	bool OpenMediaFromBuffer(byte[] buffer);

	bool StartOpenMediaFromBuffer(ulong length);

	bool AddChunkToMediaBuffer(byte[] chunk, ulong offset, ulong length);

	bool EndOpenMediaFromBuffer();

	void CloseMedia();

	void SetLooping(bool bLooping);

	bool IsLooping();

	bool HasMetaData();

	bool CanPlay();

	bool IsPlaying();

	bool IsSeeking();

	bool IsPaused();

	bool IsFinished();

	bool IsBuffering();

	void Play();

	void Pause();

	void Stop();

	void Rewind();

	void Seek(double time);

	void SeekFast(double time);

	void SeekWithTolerance(double time, double timeDeltaBefore, double timeDeltaAfter);

	void SeekToFrame(int frame, float overrideFrameRate = 0f);

	void SeekToFrameRelative(int frameOffset, float overrideFrameRate = 0f);

	double GetCurrentTime();

	int GetCurrentTimeFrames(float overrideFrameRate = 0f);

	DateTime GetProgramDateTime();

	float GetPlaybackRate();

	void SetPlaybackRate(float rate);

	void MuteAudio(bool bMute);

	bool IsMuted();

	void SetVolume(float volume);

	void SetBalance(float balance);

	float GetVolume();

	float GetBalance();

	TimeRanges GetSeekableTimes();

	TimeRanges GetBufferedTimes();

	ErrorCode GetLastError();

	long GetLastExtendedErrorCode();

	void SetTextureProperties(FilterMode filterMode = FilterMode.Bilinear, TextureWrapMode wrapMode = TextureWrapMode.Clamp, int anisoLevel = 1);

	void GetTextureProperties(out FilterMode filterMode, out TextureWrapMode wrapMode, out int anisoLevel);

	int GrabAudio(float[] buffer, int sampleCount, int channelCount);

	int GetAudioBufferedSampleCount();

	int GetAudioChannelCount();

	AudioChannelMaskFlags GetAudioChannelMask();

	void AudioConfigurationChanged(bool deviceChanged);

	void SetAudioChannelMode(Audio360ChannelMode channelMode);

	void SetAudioHeadRotation(Quaternion q);

	void ResetAudioHeadRotation();

	void SetAudioFocusEnabled(bool enabled);

	void SetAudioFocusProperties(float offFocusLevel, float widthDegrees);

	void SetAudioFocusRotation(Quaternion q);

	void ResetAudioFocus();

	bool WaitForNextFrame(Camera dummyCamera, int previousFrameCount);

	[Obsolete("SetPlayWithoutBuffering has been deprecated, see platform specific options for how to enable playback without buffering (if supported).")]
	void SetPlayWithoutBuffering(bool playWithoutBuffering);

	void SetKeyServerAuthToken(string token);

	void SetOverrideDecryptionKey(byte[] key);

	bool IsExternalPlaybackActive();

	void SetAllowsExternalPlayback(bool enable);

	void SetExternalPlaybackVideoGravity(ExternalPlaybackVideoGravity gravity);
}
