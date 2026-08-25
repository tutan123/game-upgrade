using System;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

[Serializable]
[CreateAssetMenu(fileName = "MediaReference", menuName = "AVPro Video/Media Reference", order = 51)]
public class MediaReference : ScriptableObject
{
	[SerializeField]
	private string _alias = string.Empty;

	[SerializeField]
	private MediaPath _mediaPath = new MediaPath();

	[SerializeField]
	[Header("Media Hints")]
	private MediaHints _hints = MediaHints.Default;

	[Header("Platform Overrides")]
	[SerializeField]
	private MediaReference _macOS;

	[SerializeField]
	private MediaReference _windows;

	[SerializeField]
	private MediaReference _android;

	[SerializeField]
	private MediaReference _openharmony;

	[SerializeField]
	private MediaReference _iOS;

	[SerializeField]
	private MediaReference _tvOS;

	[SerializeField]
	private MediaReference _windowsUWP;

	[SerializeField]
	private MediaReference _webGL;

	public string Alias
	{
		get
		{
			return _alias;
		}
		set
		{
			_alias = value;
		}
	}

	public MediaPath MediaPath
	{
		get
		{
			return _mediaPath;
		}
		set
		{
			_mediaPath = value;
		}
	}

	public MediaHints Hints
	{
		get
		{
			return _hints;
		}
		set
		{
			_hints = value;
		}
	}

	public MediaReference GetCurrentPlatformMediaReference()
	{
		MediaReference mediaReference = null;
		mediaReference = GetPlatformMediaReference(Platform.Windows);
		if (mediaReference == null)
		{
			mediaReference = this;
		}
		return mediaReference;
	}

	public MediaReference GetPlatformMediaReference(Platform platform)
	{
		MediaReference result = null;
		switch (platform)
		{
		case Platform.iOS:
			result = _iOS;
			break;
		case Platform.tvOS:
			result = _tvOS;
			break;
		case Platform.macOS:
			result = _macOS;
			break;
		case Platform.Windows:
			result = _windows;
			break;
		case Platform.WindowsUWP:
			result = _windowsUWP;
			break;
		case Platform.Android:
			result = _android;
			break;
		case Platform.OpenHarmony:
			result = _openharmony;
			break;
		case Platform.WebGL:
			result = _webGL;
			break;
		}
		return result;
	}
}
