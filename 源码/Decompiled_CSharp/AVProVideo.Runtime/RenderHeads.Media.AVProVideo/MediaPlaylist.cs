using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

[Serializable]
public class MediaPlaylist
{
	[Serializable]
	public class MediaItem
	{
		public enum SourceType
		{
			AVProVideoPlayer
		}

		[SerializeField]
		public string name = string.Empty;

		[SerializeField]
		public SourceType sourceType;

		[SerializeField]
		public MediaPath mediaPath = new MediaPath();

		[SerializeField]
		public Texture2D texture;

		[SerializeField]
		public float textureDuration;

		[SerializeField]
		public bool loop;

		[SerializeField]
		public PlaylistMediaPlayer.StartMode startMode;

		[SerializeField]
		public PlaylistMediaPlayer.ProgressMode progressMode;

		[SerializeField]
		public float progressTimeSeconds = 0.5f;

		[SerializeField]
		public bool isOverrideTransition;

		[SerializeField]
		public PlaylistMediaPlayer.Transition overrideTransition;

		[SerializeField]
		public float overrideTransitionDuration = 1f;

		[SerializeField]
		public Easing.Preset overrideTransitionEasing = Easing.Preset.Linear;
	}

	[SerializeField]
	private List<MediaItem> _items = new List<MediaItem>(8);

	public List<MediaItem> Items => _items;

	public bool HasItemAt(int index)
	{
		if (index >= 0)
		{
			return index < _items.Count;
		}
		return false;
	}
}
