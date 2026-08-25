using System;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

[Serializable]
public class MediaPath
{
	[SerializeField]
	private MediaPathType _pathType = MediaPathType.RelativeToStreamingAssetsFolder;

	[SerializeField]
	private string _path = string.Empty;

	public MediaPathType PathType
	{
		get
		{
			return _pathType;
		}
		internal set
		{
			_pathType = value;
		}
	}

	public string Path
	{
		get
		{
			return _path;
		}
		internal set
		{
			_path = value;
		}
	}

	public MediaPath()
	{
		_pathType = MediaPathType.RelativeToStreamingAssetsFolder;
		_path = string.Empty;
	}

	public MediaPath(MediaPath copy)
	{
		_pathType = copy.PathType;
		_path = copy.Path;
	}

	public MediaPath(string path, MediaPathType pathType)
	{
		_pathType = pathType;
		_path = path;
	}

	public string GetResolvedFullPath()
	{
		string text = Helper.GetFilePath(_path, _pathType);
		if (text.Length > 200 && !text.Contains("://"))
		{
			text = Helper.ConvertLongPathToShortDOS83Path(text);
		}
		return text;
	}

	public static implicit operator MediaPath(string s)
	{
		return new MediaPath(s, MediaPathType.AbsolutePathOrURL);
	}

	public static bool operator ==(MediaPath a, MediaPath b)
	{
		return a?.Equals(b) ?? ((object)b == null);
	}

	public static bool operator !=(MediaPath a, MediaPath b)
	{
		return !(a == b);
	}

	public override bool Equals(object obj)
	{
		if (obj == null || GetType() != obj.GetType())
		{
			return false;
		}
		MediaPath mediaPath = (MediaPath)obj;
		if (_pathType == mediaPath._pathType)
		{
			return _path == mediaPath._path;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return _pathType.GetHashCode() ^ _path.GetHashCode();
	}
}
