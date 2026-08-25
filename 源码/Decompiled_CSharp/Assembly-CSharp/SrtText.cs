using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using RenderHeads.Media.AVProVideo;
using TMPro;
using UnityEngine;

public class SrtText : MonoBehaviour
{
	[Serializable]
	public class SubtitleBlock
	{
		public float startTime;

		public float endTime;

		public string text;
	}

	[Header("组件引用")]
	public MediaPlayer videoPlayer;

	public TextMeshProUGUI subtitleUI;

	public TextAsset srtFile;

	[Header("设置")]
	public bool showDebugLog;

	private List<SubtitleBlock> _subtitleDatabase = new List<SubtitleBlock>();

	private int _currentIndex;

	private void Start()
	{
		if (srtFile != null)
		{
			ParseSRT(srtFile.text);
		}
	}

	private void Update()
	{
		if ((bool)videoPlayer)
		{
			if (!srtFile)
			{
				subtitleUI.text = "";
			}
			else if (!(videoPlayer == null) && _subtitleDatabase.Count != 0 && !(subtitleUI == null))
			{
				float time = (float)videoPlayer.Control.GetCurrentTime();
				UpdateSubtitleDisplay(time);
			}
		}
	}

	public void ParseSRT(TextAsset srtText)
	{
		srtFile = srtText;
		if (srtText == null)
		{
			subtitleUI.text = "";
		}
		else
		{
			ParseSRT(srtFile.text);
		}
	}

	public void ParseSRT(string srtText)
	{
		_subtitleDatabase.Clear();
		string pattern = "(?<index>\\d+)\\r?\\n(?<start>\\d{2}:\\d{2}:\\d{2},\\d{3}) --> (?<end>\\d{2}:\\d{2}:\\d{2},\\d{3})\\r?\\n(?<text>[\\s\\S]*?)(?=\\r?\\n\\r?\\n|$)";
		foreach (Match item in Regex.Matches(srtText, pattern))
		{
			SubtitleBlock subtitleBlock = new SubtitleBlock();
			subtitleBlock.startTime = TimeSpanToSeconds(item.Groups["start"].Value);
			subtitleBlock.endTime = TimeSpanToSeconds(item.Groups["end"].Value);
			subtitleBlock.text = item.Groups["text"].Value.Trim();
			_subtitleDatabase.Add(subtitleBlock);
		}
	}

	private float TimeSpanToSeconds(string timeString)
	{
		if (TimeSpan.TryParse(timeString.Replace(',', '.'), out var result))
		{
			return (float)result.TotalSeconds;
		}
		return 0f;
	}

	private void UpdateSubtitleDisplay(float time)
	{
		if (_currentIndex < _subtitleDatabase.Count && _currentIndex >= 0)
		{
			SubtitleBlock subtitleBlock = _subtitleDatabase[_currentIndex];
			if (time >= subtitleBlock.startTime && time <= subtitleBlock.endTime)
			{
				subtitleUI.text = subtitleBlock.text;
				return;
			}
		}
		bool flag = false;
		for (int i = 0; i < _subtitleDatabase.Count; i++)
		{
			if (time >= _subtitleDatabase[i].startTime && time <= _subtitleDatabase[i].endTime)
			{
				subtitleUI.text = _subtitleDatabase[i].text;
				_currentIndex = i;
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			subtitleUI.text = "";
		}
	}
}
