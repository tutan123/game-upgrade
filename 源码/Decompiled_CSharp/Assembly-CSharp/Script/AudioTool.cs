using System;
using FrameWork;
using RenderHeads.Media.AVProVideo;
using UnityEngine;

namespace Script;

public class AudioTool : MonoBehaviour
{
	public MediaPlayer mediaPlayer;

	public AudioSource audioSource;

	[Header("同步设置")]
	public float syncThreshold = 0.02f;

	private void Update()
	{
		if (!(mediaPlayer == null) && mediaPlayer.Control != null && !(audioSource == null))
		{
			float playbackRate = mediaPlayer.PlaybackRate;
			HandlePlaybackState();
			if (!Mathf.Approximately(audioSource.pitch, playbackRate))
			{
				ApplyPitchAndSpeed(playbackRate);
			}
			SyncTimeLogic();
		}
	}

	public void SetClip(AudioClip clip)
	{
		audioSource.clip = clip;
	}

	private void HandlePlaybackState()
	{
		if (mediaPlayer.Control.IsPlaying())
		{
			if (!audioSource.isPlaying && audioSource.enabled)
			{
				audioSource.Play();
			}
		}
		else if (audioSource.isPlaying && audioSource.enabled)
		{
			audioSource.Pause();
		}
	}

	private void ApplyPitchAndSpeed(float rate)
	{
		audioSource.pitch = rate;
	}

	private void SyncTimeLogic()
	{
		if (audioSource == null || audioSource.clip == null || audioSource.clip.loadState != AudioDataLoadState.Loaded)
		{
			return;
		}
		float length = audioSource.clip.length;
		if (length <= 0.01f)
		{
			return;
		}
		double currentTime = mediaPlayer.Control.GetCurrentTime();
		if (double.IsNaN(currentTime) || double.IsInfinity(currentTime))
		{
			return;
		}
		double num = audioSource.time;
		if (!(Math.Abs(currentTime - num) > (double)syncThreshold))
		{
			return;
		}
		float max = length - 0.01f;
		float num2 = Mathf.Clamp((float)currentTime, 0f, max);
		try
		{
			if (audioSource.clip != null)
			{
				audioSource.time = num2;
			}
		}
		catch (Exception ex)
		{
			MyLog.LogWarning($"[AudioTool] Seek failed at {num2}: {ex.Message}");
		}
	}
}
