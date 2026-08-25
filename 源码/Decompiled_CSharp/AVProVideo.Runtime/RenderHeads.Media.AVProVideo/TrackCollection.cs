using System.Collections;
using System.Collections.Generic;

namespace RenderHeads.Media.AVProVideo;

public abstract class TrackCollection : IEnumerable
{
	public virtual TrackType TrackType { get; private set; }

	public abstract int Count { get; }

	public abstract IEnumerator GetEnumerator();

	public abstract int GetTrackArrayIndexFromUid(int Uid);

	internal abstract void Clear();

	internal abstract void Add(TrackBase track);

	internal abstract bool HasActiveTrack();

	internal abstract bool IsActiveTrack(TrackBase track);

	internal abstract void SetActiveTrack(TrackBase track);

	internal abstract void SetFirstTrackActive();

	public abstract int GetActiveTrackIndex();
}
public class TrackCollection<T> : TrackCollection where T : TrackBase
{
	internal List<T> _tracks = new List<T>(4);

	public T this[int index] => _tracks[index];

	internal T ActiveTrack { get; set; }

	public override int Count => _tracks.Count;

	internal TrackCollection()
	{
	}

	public override IEnumerator GetEnumerator()
	{
		return _tracks.GetEnumerator();
	}

	internal override bool HasActiveTrack()
	{
		return ActiveTrack != null;
	}

	internal override bool IsActiveTrack(TrackBase track)
	{
		return ActiveTrack == track;
	}

	public override int GetTrackArrayIndexFromUid(int Uid)
	{
		int num = 0;
		foreach (T track in _tracks)
		{
			if (track.Uid == Uid)
			{
				return num;
			}
			num++;
		}
		return -1;
	}

	internal override void Clear()
	{
		_tracks.Clear();
		ActiveTrack = null;
	}

	internal override void Add(TrackBase track)
	{
		_tracks.Add(track as T);
	}

	internal override void SetActiveTrack(TrackBase track)
	{
		ActiveTrack = track as T;
	}

	internal override void SetFirstTrackActive()
	{
		if (_tracks.Count > 0)
		{
			ActiveTrack = _tracks[0];
		}
	}

	public override int GetActiveTrackIndex()
	{
		if (ActiveTrack != null)
		{
			return ActiveTrack.Uid;
		}
		return -1;
	}
}
