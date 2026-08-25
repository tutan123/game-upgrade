using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
	[SerializeField]
	[Header("Media Player")]
	private MediaPlayer _MediaPlayer;

	[Header("Page Game Objects")]
	[SerializeField]
	private GameObject _MainMenuGO;

	[SerializeField]
	private GameObject _VideoTrackMenuGO;

	[SerializeField]
	private GameObject _AudioTrackMenuGO;

	[SerializeField]
	private GameObject _SubtitlesMenuGO;

	[SerializeField]
	private GameObject _PlaybackSpeedMenuGO;

	[SerializeField]
	private GameObject _QualityMenuGO;

	[Header("Buttons")]
	[SerializeField]
	private Button _VideoTrackButton;

	[SerializeField]
	private Button _AudioTrackButton;

	[SerializeField]
	private Button _SubtitlesButton;

	[SerializeField]
	private Button _PlaybackSpeedButton;

	[SerializeField]
	private Button _QualityButton;

	[Header("Pages")]
	[SerializeField]
	private OptionsVideoTrackPage _OptionsVideoTrackPage;

	[SerializeField]
	private OptionsAudioTrackPage _OptionsAudioTrackPage;

	[SerializeField]
	private OptionsSubtitlesPage _OptionsSubtitlesPage;

	[SerializeField]
	private OptionsPlaybackSpeedPage _OptionsPlaybackSpeedPage;

	[SerializeField]
	private OptionsQualityPage _OptionsQualityPage;

	[SerializeField]
	[Header("Text")]
	private Text _VideoTrackValueText;

	[SerializeField]
	private Text _AudioTrackValueText;

	[SerializeField]
	private Text _SubtitlesValueText;

	[SerializeField]
	private Text _PlaybackSpeedValueText;

	[SerializeField]
	private Text _QualityValueText;

	private int m_iCachedVideoWidth = -1;

	private int m_iCachedVideoHeight = -1;

	private float m_fCachedVideoFramerate = -1f;

	private int m_iCachedVariantId = -1;

	private void Start()
	{
		if ((bool)_VideoTrackButton)
		{
			_VideoTrackButton.onClick.AddListener(delegate
			{
				if ((bool)_MediaPlayer && _MediaPlayer.VideoTracks.GetVideoTracks().Count > 1)
				{
					MainToVideoTrack();
				}
			});
		}
		if ((bool)_AudioTrackButton)
		{
			_AudioTrackButton.onClick.AddListener(delegate
			{
				if ((bool)_MediaPlayer && _MediaPlayer.AudioTracks.GetAudioTracks().Count > 1)
				{
					MainToAudioTrack();
				}
			});
		}
		if ((bool)_SubtitlesButton)
		{
			_SubtitlesButton.onClick.AddListener(delegate
			{
				if ((bool)_MediaPlayer && _MediaPlayer.TextTracks.GetTextTracks().Count > 0)
				{
					MainToSubtitles();
				}
			});
		}
		if ((bool)_PlaybackSpeedButton)
		{
			_PlaybackSpeedButton.onClick.AddListener(delegate
			{
				MainToPlaybackSpeed();
			});
		}
		if (!_QualityButton)
		{
			return;
		}
		_QualityButton.onClick.AddListener(delegate
		{
			if (_MediaPlayer.Variants != null && _MediaPlayer.Variants.Count > 0)
			{
				MainToQuality();
			}
		});
	}

	private void Update()
	{
		if (!_MainMenuGO || !_MainMenuGO.activeInHierarchy || !_QualityValueText || !_MediaPlayer)
		{
			return;
		}
		int videoWidth = _MediaPlayer.Info.GetVideoWidth();
		int videoHeight = _MediaPlayer.Info.GetVideoHeight();
		float frameRate = _MediaPlayer.Variants.Current.FrameRate;
		int id = _MediaPlayer.Variants.Current.Id;
		if (videoWidth == m_iCachedVideoWidth && videoHeight == m_iCachedVideoHeight && m_fCachedVideoFramerate == frameRate && m_iCachedVariantId == id)
		{
			return;
		}
		if (_MediaPlayer.Variants.Count > 1)
		{
			if (id == Variant.Auto.Id)
			{
				if (frameRate > 0f)
				{
					_QualityValueText.text = $"Auto ({videoWidth}x{videoHeight}@{frameRate}) >";
				}
				else
				{
					_QualityValueText.text = $"Auto ({videoWidth}x{videoHeight}) >";
				}
			}
			else if (frameRate > 0f)
			{
				_QualityValueText.text = $"{videoWidth}x{videoHeight}@{frameRate} >";
			}
			else
			{
				_QualityValueText.text = $"{videoWidth}x{videoHeight} >";
			}
		}
		else if (frameRate > 0f)
		{
			_QualityValueText.text = $"{videoWidth}x{videoHeight}@{frameRate} >";
		}
		else
		{
			_QualityValueText.text = $"{videoWidth}x{videoHeight}";
		}
		m_iCachedVideoWidth = videoWidth;
		m_iCachedVideoHeight = videoHeight;
		m_fCachedVideoFramerate = frameRate;
		m_iCachedVariantId = id;
	}

	public void SetActive(bool bShowOptions)
	{
		if ((bool)_MainMenuGO)
		{
			_MainMenuGO.SetActive(bShowOptions);
		}
		if ((bool)_VideoTrackMenuGO)
		{
			_VideoTrackMenuGO.SetActive(value: false);
		}
		if ((bool)_AudioTrackMenuGO)
		{
			_AudioTrackMenuGO.SetActive(value: false);
		}
		if ((bool)_SubtitlesMenuGO)
		{
			_SubtitlesMenuGO.SetActive(value: false);
		}
		if ((bool)_PlaybackSpeedMenuGO)
		{
			_PlaybackSpeedMenuGO.SetActive(value: false);
		}
		if ((bool)_QualityMenuGO)
		{
			_QualityMenuGO.SetActive(value: false);
		}
		if (bShowOptions && (bool)_MediaPlayer)
		{
			ChangeVideoTrack(_MediaPlayer.VideoTracks.GetVideoTracks().GetActiveTrackIndex(), bSetTrack: false);
			ChangeAudioTrack(_MediaPlayer.AudioTracks.GetAudioTracks().GetActiveTrackIndex(), bSetTrack: false);
			ChangeSubtitleTrack(_MediaPlayer.TextTracks.GetTextTracks().GetActiveTrackIndex(), bSetTrack: false);
		}
	}

	public void ChangeVideoTrack(int iTrackIndex, bool bSetTrack = true)
	{
		if ((bool)_OptionsVideoTrackPage)
		{
			_OptionsVideoTrackPage.ChangeVideoTrack(iTrackIndex, bSetTrack);
		}
		if (!_VideoTrackValueText)
		{
			return;
		}
		VideoTrack videoTrack = (_MediaPlayer ? _MediaPlayer.VideoTracks.GetActiveVideoTrack() : null);
		if (videoTrack != null)
		{
			_VideoTrackValueText.text = "None";
			int count = _MediaPlayer.VideoTracks.GetVideoTracks().Count;
			if (count > 0)
			{
				_VideoTrackValueText.text = videoTrack.DisplayName + ((count > 1) ? "  >" : "");
			}
		}
	}

	public void ChangeAudioTrack(int iTrackIndex, bool bSetTrack = true)
	{
		if ((bool)_OptionsAudioTrackPage)
		{
			_OptionsAudioTrackPage.ChangeAudioTrack(iTrackIndex, bSetTrack);
		}
		if (!_AudioTrackValueText)
		{
			return;
		}
		AudioTrack audioTrack = (_MediaPlayer ? _MediaPlayer.AudioTracks.GetActiveAudioTrack() : null);
		if (audioTrack != null)
		{
			_AudioTrackValueText.text = "None";
			int count = _MediaPlayer.AudioTracks.GetAudioTracks().Count;
			if (count > 0)
			{
				_AudioTrackValueText.text = audioTrack.DisplayName + ((count > 1) ? "  >" : "");
			}
		}
	}

	public void ChangeSubtitleTrack(int iTrackUid, bool bSetTrack = true)
	{
		if ((bool)_OptionsSubtitlesPage)
		{
			_OptionsSubtitlesPage.ChangeSubtitleTrack(iTrackUid, bSetTrack);
		}
		if (!_SubtitlesValueText)
		{
			return;
		}
		TextTracks textTracks = (_MediaPlayer ? _MediaPlayer.TextTracks.GetTextTracks() : null);
		if (textTracks != null)
		{
			_SubtitlesValueText.text = "None";
			if (textTracks.Count > 0)
			{
				int textTrackArrayIndexFromUid = _MediaPlayer.TextTracks.GetTextTrackArrayIndexFromUid(iTrackUid);
				_SubtitlesValueText.text = ((textTrackArrayIndexFromUid > -1) ? textTracks[textTrackArrayIndexFromUid].DisplayName : "Off") + "  >";
			}
		}
	}

	public void ChangePlaybackSpeed(int iIndex)
	{
		if ((bool)_OptionsPlaybackSpeedPage)
		{
			_OptionsPlaybackSpeedPage.ChangeVideoPlaybackSpeed(iIndex);
			if ((bool)_PlaybackSpeedValueText && (_MediaPlayer ? _MediaPlayer.TextTracks.GetTextTracks() : null) != null)
			{
				_PlaybackSpeedValueText.text = _OptionsPlaybackSpeedPage.GetDisplayNameForIndex(iIndex) + "  >";
			}
		}
	}

	public void ChangeVideoVariant(int iVariantIndex)
	{
		if ((bool)_OptionsQualityPage)
		{
			_OptionsQualityPage.ChangeVideoVariant(iVariantIndex);
		}
	}

	public void MainToVideoTrack()
	{
		if ((bool)_MainMenuGO)
		{
			_MainMenuGO.SetActive(value: false);
		}
		if ((bool)_VideoTrackMenuGO)
		{
			_VideoTrackMenuGO.SetActive(value: true);
		}
	}

	public void VideoTrackToMain()
	{
		if ((bool)_MainMenuGO)
		{
			_MainMenuGO.SetActive(value: true);
		}
		if ((bool)_VideoTrackMenuGO)
		{
			_VideoTrackMenuGO.SetActive(value: false);
		}
	}

	public void MainToAudioTrack()
	{
		if ((bool)_MainMenuGO)
		{
			_MainMenuGO.SetActive(value: false);
		}
		if ((bool)_AudioTrackMenuGO)
		{
			_AudioTrackMenuGO.SetActive(value: true);
		}
	}

	public void AudioTrackToMain()
	{
		if ((bool)_MainMenuGO)
		{
			_MainMenuGO.SetActive(value: true);
		}
		if ((bool)_AudioTrackMenuGO)
		{
			_AudioTrackMenuGO.SetActive(value: false);
		}
	}

	public void MainToSubtitles()
	{
		if ((bool)_MainMenuGO)
		{
			_MainMenuGO.SetActive(value: false);
		}
		if ((bool)_SubtitlesMenuGO)
		{
			_SubtitlesMenuGO.SetActive(value: true);
		}
	}

	public void SubtitlesToMain()
	{
		if ((bool)_MainMenuGO)
		{
			_MainMenuGO.SetActive(value: true);
		}
		if ((bool)_SubtitlesMenuGO)
		{
			_SubtitlesMenuGO.SetActive(value: false);
		}
	}

	public void MainToPlaybackSpeed()
	{
		if ((bool)_MainMenuGO)
		{
			_MainMenuGO.SetActive(value: false);
		}
		if ((bool)_PlaybackSpeedMenuGO)
		{
			_PlaybackSpeedMenuGO.SetActive(value: true);
		}
	}

	public void PlaybackSpeedToMain()
	{
		if ((bool)_MainMenuGO)
		{
			_MainMenuGO.SetActive(value: true);
		}
		if ((bool)_PlaybackSpeedMenuGO)
		{
			_PlaybackSpeedMenuGO.SetActive(value: false);
		}
	}

	public void MainToQuality()
	{
		if (_MediaPlayer.Variants != null && _MediaPlayer.Variants.Count >= 2)
		{
			if ((bool)_MainMenuGO)
			{
				_MainMenuGO.SetActive(value: false);
			}
			if ((bool)_QualityMenuGO)
			{
				_QualityMenuGO.SetActive(value: true);
			}
		}
	}

	public void QualityToMain()
	{
		if ((bool)_MainMenuGO)
		{
			_MainMenuGO.SetActive(value: true);
		}
		if ((bool)_QualityMenuGO)
		{
			_QualityMenuGO.SetActive(value: false);
		}
	}
}
