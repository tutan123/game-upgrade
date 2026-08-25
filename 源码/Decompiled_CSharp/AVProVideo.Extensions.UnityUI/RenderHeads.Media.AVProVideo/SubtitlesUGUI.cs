using UnityEngine;
using UnityEngine.UI;

namespace RenderHeads.Media.AVProVideo;

[AddComponentMenu("AVPro Video/Subtitles uGUI", 201)]
[HelpURL("http://renderheads.com/products/avpro-video/")]
public class SubtitlesUGUI : MonoBehaviour
{
	[SerializeField]
	private MediaPlayer _mediaPlayer;

	[SerializeField]
	private Text _text;

	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private int _backgroundHorizontalPadding = 32;

	[SerializeField]
	private int _backgroundVerticalPadding = 16;

	[SerializeField]
	[Range(-1f, 1024f)]
	private int _maxCharacters = 256;

	public MediaPlayer Player
	{
		get
		{
			return _mediaPlayer;
		}
		set
		{
			ChangeMediaPlayer(value);
		}
	}

	public Text Text
	{
		get
		{
			return _text;
		}
		set
		{
			_text = value;
		}
	}

	private void Start()
	{
		ChangeMediaPlayer(_mediaPlayer);
	}

	private void OnDestroy()
	{
		ChangeMediaPlayer(null);
	}

	private void Update()
	{
		UpdateBackgroundRect();
	}

	public void ChangeMediaPlayer(MediaPlayer newPlayer)
	{
		if (_mediaPlayer != null)
		{
			_mediaPlayer.Events.RemoveListener(OnMediaPlayerEvent);
			_mediaPlayer = null;
		}
		SetText(string.Empty);
		if (newPlayer != null)
		{
			newPlayer.Events.AddListener(OnMediaPlayerEvent);
			_mediaPlayer = newPlayer;
		}
	}

	private void SetText(string text)
	{
		_text.text = text;
		UpdateBackgroundRect();
	}

	private string PrepareText(string text)
	{
		if (_maxCharacters >= 0 && text.Length > _maxCharacters)
		{
			text = text.Substring(0, _maxCharacters);
		}
		text = text.Replace("<font color=", "<color=");
		text = text.Replace("</font>", "</color>");
		text = text.Replace("<u>", string.Empty);
		text = text.Replace("</u>", string.Empty);
		return text;
	}

	private void UpdateBackgroundRect()
	{
		if ((bool)_backgroundImage)
		{
			if (string.IsNullOrEmpty(_text.text))
			{
				_backgroundImage.enabled = false;
				return;
			}
			_backgroundImage.enabled = true;
			_backgroundImage.rectTransform.sizeDelta = _text.rectTransform.sizeDelta;
			_backgroundImage.rectTransform.anchoredPosition = _text.rectTransform.anchoredPosition;
			_backgroundImage.rectTransform.offsetMin -= new Vector2(_backgroundHorizontalPadding, _backgroundVerticalPadding);
			_backgroundImage.rectTransform.offsetMax += new Vector2(_backgroundHorizontalPadding, _backgroundVerticalPadding);
		}
	}

	private void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	{
		switch (et)
		{
		case MediaPlayerEvent.EventType.Closing:
			SetText(string.Empty);
			break;
		case MediaPlayerEvent.EventType.SubtitleChange:
			SetText(PrepareText(_mediaPlayer.Subtitles.GetSubtitleText()));
			break;
		}
	}
}
