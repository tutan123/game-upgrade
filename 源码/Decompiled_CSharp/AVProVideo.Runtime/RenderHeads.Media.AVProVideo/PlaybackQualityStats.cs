using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public class PlaybackQualityStats
{
	private int _sameFrameCount;

	private long _lastTimeStamp;

	private BaseMediaPlayer _player;

	public int SkippedFrames { get; private set; }

	public int DuplicateFrames { get; private set; }

	public int UnityDroppedFrames { get; private set; }

	public float PerfectFramesT { get; private set; }

	public string VSyncStatus { get; private set; }

	private int PerfectFrames { get; set; }

	private int TotalFrames { get; set; }

	public bool LogIssues { get; set; }

	public void Reset()
	{
		_sameFrameCount = 0;
		if (_player != null)
		{
			_lastTimeStamp = _player.GetTextureTimeStamp();
		}
		SkippedFrames = 0;
		DuplicateFrames = 0;
		UnityDroppedFrames = 0;
		TotalFrames = 0;
		PerfectFrames = 0;
		PerfectFramesT = 0f;
	}

	internal void Start(BaseMediaPlayer player)
	{
		_player = player;
		Reset();
		bool flag = true;
		if (QualitySettings.vSyncCount == 0)
		{
			flag = false;
			if (LogIssues)
			{
				Debug.LogWarning("[AVProVideo][Quality] VSync is currently disabled in Quality Settings");
			}
		}
		if (!IsGameViewVSyncEnabled())
		{
			flag = false;
			if (LogIssues)
			{
				Debug.LogWarning("[AVProVideo][Quality] VSync is currently disabled in the Game View");
			}
		}
		float videoFrameRate = _player.GetVideoFrameRate();
		float num = 1000f / videoFrameRate;
		if (LogIssues)
		{
			Debug.Log($"[AVProVideo][Quality] Video: {videoFrameRate}fps {num}ms");
		}
		if (flag)
		{
			float num2 = (float)Screen.currentResolution.refreshRate / (float)QualitySettings.vSyncCount;
			float num3 = 1000f / num2;
			if (LogIssues)
			{
				Debug.Log($"[AVProVideo][Quality] VSync: {num2}fps {num3}ms");
			}
			float num4 = num / num3;
			if (num4 - (float)Mathf.FloorToInt(num4) > 0.0001f && LogIssues)
			{
				Debug.LogWarning("[AVProVideo][Quality] Video is not a multiple of VSync so playback cannot be perfect");
			}
			VSyncStatus = "VSync " + num4;
		}
		else
		{
			if (LogIssues)
			{
				Debug.LogWarning("[AVProVideo][Quality] Running without VSync enabled");
			}
			VSyncStatus = "No VSync";
		}
	}

	internal void Update()
	{
		if (_player == null || _player.IsPaused() || _player.IsSeeking() || _player.IsFinished())
		{
			return;
		}
		long textureTimeStamp = _player.GetTextureTimeStamp();
		long num = (long)(10000000.0 / (double)_player.GetVideoFrameRate());
		bool flag = true;
		long num2 = textureTimeStamp - _lastTimeStamp;
		if (num2 > 0)
		{
			num2 -= num;
			if (num2 > 10000)
			{
				int num3 = Mathf.FloorToInt((float)num2 / (float)num);
				if (LogIssues)
				{
					Debug.LogWarning("[AVProVideo][Quality] Possible frame skip, at " + textureTimeStamp + " delta " + num2 + " = " + num3 + " frames");
				}
				SkippedFrames += num3;
				flag = false;
			}
		}
		if (QualitySettings.vSyncCount != 0)
		{
			float num4 = Screen.currentResolution.refreshRate;
			long num5 = (long)((double)QualitySettings.vSyncCount * 10000000.0 / (double)num4);
			if (textureTimeStamp != _lastTimeStamp)
			{
				float num6 = (float)num / (float)num5;
				if (num6 - (float)Mathf.FloorToInt(num6) <= 0.0001f && !Mathf.Approximately(_sameFrameCount, (int)num6))
				{
					if (LogIssues)
					{
						Debug.LogWarning("[AVProVideo][Quality] Frame " + textureTimeStamp + " was shown for " + _sameFrameCount + " frames instead of expected " + num6);
					}
					DuplicateFrames++;
					flag = false;
				}
				_sameFrameCount = 1;
			}
			else
			{
				_sameFrameCount++;
			}
			if ((long)((double)Time.deltaTime * 10000000.0) > num5 + num5 / 3)
			{
				if (LogIssues)
				{
					Debug.LogWarning("[AVProVideo][Quality] Possible Unity dropped frame, delta time: " + Time.deltaTime * 1000f + "ms");
				}
				UnityDroppedFrames++;
				flag = false;
			}
		}
		if (_lastTimeStamp != textureTimeStamp)
		{
			if (flag)
			{
				PerfectFrames++;
			}
			TotalFrames++;
			PerfectFramesT = (float)PerfectFrames / (float)TotalFrames;
		}
		_lastTimeStamp = textureTimeStamp;
	}

	private static bool IsGameViewVSyncEnabled()
	{
		return true;
	}
}
