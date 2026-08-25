using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace RenderHeads.Media.AVProVideo;

[Serializable]
public class MediaPlayerEvent : UnityEvent<MediaPlayer, MediaPlayerEvent.EventType, ErrorCode>
{
	public enum EventType
	{
		MetaDataReady = 0,
		ReadyToPlay = 1,
		Started = 2,
		FirstFrameReady = 3,
		FinishedPlaying = 4,
		Closing = 5,
		Error = 6,
		SubtitleChange = 7,
		Stalled = 8,
		Unstalled = 9,
		ResolutionChanged = 10,
		StartedSeeking = 11,
		FinishedSeeking = 12,
		StartedBuffering = 13,
		FinishedBuffering = 14,
		PropertiesChanged = 15,
		PlaylistItemChanged = 16,
		PlaylistFinished = 17,
		TextTracksChanged = 18,
		Paused = 19,
		Unpaused = 20,
		TimedMetadataChanged = 21,
		TextCueChanged = 7
	}

	private List<UnityAction<MediaPlayer, EventType, ErrorCode>> _listeners = new List<UnityAction<MediaPlayer, EventType, ErrorCode>>(4);

	public bool HasListeners()
	{
		if (_listeners.Count <= 0)
		{
			return GetPersistentEventCount() > 0;
		}
		return true;
	}

	public new void AddListener(UnityAction<MediaPlayer, EventType, ErrorCode> call)
	{
		if (!_listeners.Contains(call))
		{
			_listeners.Add(call);
			base.AddListener(call);
		}
	}

	public new void RemoveListener(UnityAction<MediaPlayer, EventType, ErrorCode> call)
	{
		int num = _listeners.IndexOf(call);
		if (num >= 0)
		{
			_listeners.RemoveAt(num);
			base.RemoveListener(call);
		}
	}

	public new void RemoveAllListeners()
	{
		_listeners.Clear();
		base.RemoveAllListeners();
	}
}
