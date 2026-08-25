using System;
using System.Runtime.InteropServices;
using RenderHeads.Media.AVProVideo.Demos.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RenderHeads.Media.AVProVideo.Demos;

public class MediaPlayerUI : MonoBehaviour
{
	[StructLayout(0, Size = 1)]
	private struct UserInteraction
	{
		public static float InactiveTime;

		private static Vector3 _previousMousePos;

		private static int _lastInputFrame;

		public static bool IsUserInputThisFrame()
		{
			if (Time.frameCount == _lastInputFrame)
			{
				return true;
			}
			bool num = Input.touchSupported && Input.touchCount > 0;
			bool flag = Input.mousePresent && (Input.mousePosition != _previousMousePos || Input.mouseScrollDelta != Vector2.zero || Input.GetMouseButton(0));
			if (num || flag)
			{
				_previousMousePos = Input.mousePosition;
				_lastInputFrame = Time.frameCount;
				return true;
			}
			return false;
		}
	}

	[SerializeField]
	private MediaPlayer _mediaPlayer;

	[Header("Options")]
	[SerializeField]
	private float _keyVolumeDelta = 0.05f;

	[SerializeField]
	private float _jumpDeltaTime = 5f;

	[SerializeField]
	private bool _showOptions = true;

	[SerializeField]
	private bool _showDebug;

	[SerializeField]
	private bool _autoHide = true;

	[SerializeField]
	private float _userInactiveDuration = 1.5f;

	[SerializeField]
	private bool _useAudioFading = true;

	[Header("Keyboard Controls")]
	[SerializeField]
	private bool _enableKeyboardControls = true;

	[SerializeField]
	private KeyCode KeyVolumeUp = KeyCode.UpArrow;

	[SerializeField]
	private KeyCode KeyVolumeDown = KeyCode.DownArrow;

	[SerializeField]
	private KeyCode KeyTogglePlayPause = KeyCode.Space;

	[SerializeField]
	private KeyCode KeyToggleMute = KeyCode.M;

	[SerializeField]
	private KeyCode KeyJumpForward = KeyCode.RightArrow;

	[SerializeField]
	private KeyCode KeyJumpBack = KeyCode.LeftArrow;

	[SerializeField]
	[Header("Optional Components")]
	private OverlayManager _overlayManager;

	[SerializeField]
	private MediaPlayer _thumbnailMediaPlayer;

	[SerializeField]
	private RectTransform _timelineTip;

	[Header("UI Components")]
	[SerializeField]
	private RectTransform _canvasTransform;

	[SerializeField]
	private Slider _sliderTime;

	[SerializeField]
	private EventTrigger _videoTouch;

	[SerializeField]
	private CanvasGroup _controlsGroup;

	[SerializeField]
	private OptionsMenu _optionsMenu;

	[Header("UI Components (Optional)")]
	[SerializeField]
	private GameObject _liveItem;

	[SerializeField]
	private Text _textMediaName;

	[SerializeField]
	private Text _textTimeDuration;

	[SerializeField]
	private Slider _sliderVolume;

	[SerializeField]
	private Button _buttonPlayPause;

	[SerializeField]
	private Button _buttonVolume;

	[SerializeField]
	private Button _buttonSubtitles;

	[SerializeField]
	private Button _buttonOptions;

	[SerializeField]
	private Button _buttonDebug;

	[SerializeField]
	private Button _buttonTimeBack;

	[SerializeField]
	private Button _buttonTimeForward;

	[SerializeField]
	private RawImage _imageAudioSpectrum;

	[SerializeField]
	private HorizontalSegmentsPrimitive _segmentsSeek;

	[SerializeField]
	private HorizontalSegmentsPrimitive _segmentsBuffered;

	[SerializeField]
	private HorizontalSegmentsPrimitive _segmentsProgress;

	private bool _wasPlayingBeforeTimelineDrag;

	private float _controlsFade = 1f;

	private Material _playPauseMaterial;

	private Material _volumeMaterial;

	private Material _subtitlesMaterial;

	private Material _optionsMaterial;

	private Material _audioSpectrumMaterial;

	private float[] _spectrumSamples = new float[128];

	private float[] _spectrumSamplesSmooth = new float[128];

	private float _maxValue = 1f;

	private float _audioVolume = 1f;

	private float _audioFade;

	private bool _isAudioFadingUpToPlay = true;

	private const float AudioFadeDuration = 0.25f;

	private float _audioFadeTime;

	private readonly LazyShaderProperty _propMorph = new LazyShaderProperty("_Morph");

	private readonly LazyShaderProperty _propMute = new LazyShaderProperty("_Mute");

	private readonly LazyShaderProperty _propVolume = new LazyShaderProperty("_Volume");

	private readonly LazyShaderProperty _propSpectrum = new LazyShaderProperty("_Spectrum");

	private readonly LazyShaderProperty _propSpectrumRange = new LazyShaderProperty("_SpectrumRange");

	[Header("Events")]
	public GameObject _eventSystem;

	private bool _isHoveringOverTimeline;

	private void Awake()
	{
		try
		{
			_eventSystem.GetComponent<StandaloneInputModule>().enabled = true;
		}
		catch (Exception ex)
		{
			Debug.LogWarning(ex.Message);
		}
	}

	private void Start()
	{
		if ((bool)_mediaPlayer)
		{
			_audioVolume = _mediaPlayer.AudioVolume;
		}
		SetupPlayPauseButton();
		SetupTimeBackForwardButtons();
		SetupVolumeButton();
		SetupSubtitlesButton();
		SetupOptionsButton();
		SetupDebugButton();
		SetupAudioSpectrum();
		CreateTimelineDragEvents();
		CreateVideoTouchEvents();
		CreateVolumeSliderEvents();
		UpdateVolumeSlider();
		BuildOptionsMenu();
	}

	private Material DuplicateMaterialOnImage(Graphic image)
	{
		image.material = new Material(image.material);
		return image.material;
	}

	private void SetupPlayPauseButton()
	{
		if ((bool)_buttonPlayPause)
		{
			_buttonPlayPause.onClick.AddListener(OnPlayPauseButtonPressed);
			_playPauseMaterial = DuplicateMaterialOnImage(_buttonPlayPause.GetComponent<Image>());
		}
	}

	private void SetupTimeBackForwardButtons()
	{
		if ((bool)_buttonTimeBack)
		{
			_buttonTimeBack.onClick.AddListener(OnPlayTimeBackButtonPressed);
		}
		if ((bool)_buttonTimeForward)
		{
			_buttonTimeForward.onClick.AddListener(OnPlayTimeForwardButtonPressed);
		}
	}

	private void SetupVolumeButton()
	{
		if ((bool)_buttonVolume)
		{
			_buttonVolume.onClick.AddListener(OnVolumeButtonPressed);
			_volumeMaterial = DuplicateMaterialOnImage(_buttonVolume.GetComponent<Image>());
		}
	}

	private void SetupSubtitlesButton()
	{
		if ((bool)_buttonSubtitles)
		{
			_buttonSubtitles.onClick.AddListener(OnSubtitlesButtonPressed);
			_subtitlesMaterial = DuplicateMaterialOnImage(_buttonSubtitles.GetComponent<Image>());
		}
	}

	private void SetupOptionsButton()
	{
		if ((bool)_buttonOptions)
		{
			_buttonOptions.onClick.AddListener(OnOptionsButtonPressed);
			_optionsMaterial = DuplicateMaterialOnImage(_buttonOptions.GetComponent<Image>());
		}
	}

	private void SetupDebugButton()
	{
		if ((bool)_buttonDebug)
		{
			_buttonDebug.onClick.AddListener(OnDebugButtonPressed);
		}
	}

	private void SetupAudioSpectrum()
	{
		if ((bool)_imageAudioSpectrum)
		{
			_audioSpectrumMaterial = DuplicateMaterialOnImage(_imageAudioSpectrum);
		}
	}

	private void OnPlayPauseButtonPressed()
	{
		TogglePlayPause();
	}

	private void OnPlayTimeBackButtonPressed()
	{
		SeekRelative(0f - _jumpDeltaTime);
	}

	private void OnPlayTimeForwardButtonPressed()
	{
		SeekRelative(_jumpDeltaTime);
	}

	private void OnVolumeButtonPressed()
	{
		ToggleMute();
	}

	private void OnSubtitlesButtonPressed()
	{
		ToggleSubtitles();
	}

	private void OnOptionsButtonPressed()
	{
		ToggleOptionsMenu();
	}

	private void OnDebugButtonPressed()
	{
		ToggleDebugMenu();
	}

	private void OnTimelineBeginHover(PointerEventData eventData)
	{
		if (eventData.pointerCurrentRaycast.gameObject != null)
		{
			_isHoveringOverTimeline = true;
			_sliderTime.transform.localScale = new Vector3(1f, 2.5f, 1f);
		}
	}

	private void OnTimelineEndHover(PointerEventData eventData)
	{
		_isHoveringOverTimeline = false;
		_sliderTime.transform.localScale = new Vector3(1f, 1f, 1f);
	}

	private void CreateVideoTouchEvents()
	{
		EventTrigger.Entry entry = new EventTrigger.Entry();
		entry.eventID = EventTriggerType.PointerUp;
		entry.callback.AddListener(delegate
		{
			OnVideoPointerUp();
		});
		_videoTouch.triggers.Add(entry);
	}

	private void OnVideoPointerUp()
	{
		if (_showOptions)
		{
			ToggleOptionsMenu();
		}
		else if (_controlsGroup.alpha >= 0.5f && _controlsGroup.gameObject.activeSelf)
		{
			TogglePlayPause();
		}
	}

	private void UpdateAudioFading()
	{
		if (_audioFadeTime < 0.25f)
		{
			_audioFadeTime = Mathf.Clamp(_audioFadeTime + Time.deltaTime, 0f, 0.25f);
		}
		if (_audioFadeTime >= 0.25f && !_isAudioFadingUpToPlay)
		{
			Pause(skipFeedback: true);
		}
		if (_mediaPlayer.Control != null && _mediaPlayer.Control.IsPlaying())
		{
			_audioFade = Mathf.Clamp01(_audioFadeTime / 0.25f);
			if (!_isAudioFadingUpToPlay)
			{
				_audioFade = 1f - _audioFade;
			}
			ApplyAudioVolume();
		}
	}

	public void TogglePlayPause()
	{
		if (!_mediaPlayer || _mediaPlayer.Control == null)
		{
			return;
		}
		if (_useAudioFading && _mediaPlayer.Info.HasAudio())
		{
			if (_mediaPlayer.Control.IsPlaying())
			{
				if ((bool)_overlayManager)
				{
					_overlayManager.TriggerFeedback(OverlayManager.Feedback.Pause);
				}
				_isAudioFadingUpToPlay = false;
			}
			else
			{
				_isAudioFadingUpToPlay = true;
				Play();
			}
			_audioFadeTime = 0f;
		}
		else if (_mediaPlayer.Control.IsPlaying())
		{
			Pause();
		}
		else
		{
			Play();
		}
	}

	private void Play()
	{
		if ((bool)_mediaPlayer && _mediaPlayer.Control != null)
		{
			if ((bool)_overlayManager)
			{
				_overlayManager.TriggerFeedback(OverlayManager.Feedback.Play);
			}
			_mediaPlayer.Play();
		}
	}

	private void Pause(bool skipFeedback = false)
	{
		if ((bool)_mediaPlayer && _mediaPlayer.Control != null)
		{
			if (!skipFeedback && (bool)_overlayManager)
			{
				_overlayManager.TriggerFeedback(OverlayManager.Feedback.Pause);
			}
			_mediaPlayer.Pause();
		}
	}

	public void SeekRelative(float deltaTime)
	{
		if ((bool)_mediaPlayer && _mediaPlayer.Control != null)
		{
			TimeRange timelineRange = GetTimelineRange();
			double val = _mediaPlayer.Control.GetCurrentTime() + (double)deltaTime;
			val = Math.Max(val, timelineRange.startTime);
			val = Math.Min(val, timelineRange.startTime + timelineRange.duration);
			_mediaPlayer.Control.Seek(val);
			if ((bool)_overlayManager)
			{
				_overlayManager.TriggerFeedback((deltaTime > 0f) ? OverlayManager.Feedback.SeekForward : OverlayManager.Feedback.SeekBack);
			}
		}
	}

	public void ChangeAudioVolume(float delta)
	{
		if ((bool)_mediaPlayer && _mediaPlayer.Control != null)
		{
			_audioVolume = Mathf.Clamp01(_audioVolume + delta);
			UpdateVolumeSlider();
			if ((bool)_overlayManager)
			{
				_overlayManager.TriggerFeedback((delta > 0f) ? OverlayManager.Feedback.VolumeUp : OverlayManager.Feedback.VolumeDown);
			}
		}
	}

	public void ToggleMute()
	{
		if ((bool)_mediaPlayer && _mediaPlayer.Control != null)
		{
			if (_mediaPlayer.AudioMuted)
			{
				MuteAudio(mute: false);
			}
			else
			{
				MuteAudio(mute: true);
			}
		}
	}

	private void MuteAudio(bool mute)
	{
		if ((bool)_mediaPlayer && _mediaPlayer.Control != null)
		{
			_mediaPlayer.AudioMuted = mute;
			if ((bool)_overlayManager)
			{
				_overlayManager.TriggerFeedback(mute ? OverlayManager.Feedback.VolumeMute : OverlayManager.Feedback.VolumeUp);
			}
		}
	}

	public void ToggleSubtitles()
	{
		if ((bool)_mediaPlayer && _mediaPlayer.TextTracks != null && _mediaPlayer.TextTracks.GetTextTracks().Count > 0)
		{
			int iTrackUid = -1;
			if (_mediaPlayer.TextTracks.GetActiveTextTrack() != null)
			{
				_mediaPlayer.TextTracks.SetActiveTextTrack(null);
			}
			else
			{
				TextTrack textTrack = _mediaPlayer.TextTracks.GetTextTracks()[0];
				_mediaPlayer.TextTracks.SetActiveTextTrack(textTrack);
				iTrackUid = textTrack.Uid;
			}
			if ((bool)_optionsMenu)
			{
				_optionsMenu.ChangeSubtitleTrack(iTrackUid);
			}
		}
	}

	private void ToggleOptionsMenu()
	{
		_showOptions = !_showOptions;
		BuildOptionsMenu();
	}

	private void ToggleDebugMenu()
	{
		_showDebug = !_showDebug;
		_videoTouch.enabled = !_showDebug;
	}

	private void BuildOptionsMenu()
	{
		if ((bool)_optionsMenu)
		{
			_optionsMenu.SetActive(_showOptions);
		}
	}

	private void CreateTimelineDragEvents()
	{
		EventTrigger component = _sliderTime.gameObject.GetComponent<EventTrigger>();
		if (component != null)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener(delegate
			{
				OnTimeSliderBeginDrag();
			});
			component.triggers.Add(entry);
			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Drag;
			entry.callback.AddListener(delegate
			{
				OnTimeSliderDrag();
			});
			component.triggers.Add(entry);
			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerUp;
			entry.callback.AddListener(delegate
			{
				OnTimeSliderEndDrag();
			});
			component.triggers.Add(entry);
			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerEnter;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				OnTimelineBeginHover((PointerEventData)data);
			});
			component.triggers.Add(entry);
			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerExit;
			entry.callback.AddListener(delegate(BaseEventData data)
			{
				OnTimelineEndHover((PointerEventData)data);
			});
			component.triggers.Add(entry);
		}
	}

	private void CreateVolumeSliderEvents()
	{
		if (!(_sliderVolume != null))
		{
			return;
		}
		EventTrigger component = _sliderVolume.gameObject.GetComponent<EventTrigger>();
		if (component != null)
		{
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener(delegate
			{
				OnVolumeSliderDrag();
			});
			component.triggers.Add(entry);
			entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.Drag;
			entry.callback.AddListener(delegate
			{
				OnVolumeSliderDrag();
			});
			component.triggers.Add(entry);
		}
	}

	private void OnVolumeSliderDrag()
	{
		if ((bool)_mediaPlayer && _mediaPlayer.Control != null)
		{
			_audioVolume = _sliderVolume.value;
			ApplyAudioVolume();
		}
	}

	private void ApplyAudioVolume()
	{
		if ((bool)_mediaPlayer)
		{
			_mediaPlayer.AudioVolume = _audioVolume * _audioFade;
		}
	}

	private void UpdateVolumeSlider()
	{
		if ((bool)_sliderVolume && (bool)_mediaPlayer)
		{
			_sliderVolume.value = _audioVolume;
		}
	}

	private void UpdateAudioSpectrum()
	{
		bool active = false;
		if (_mediaPlayer == null)
		{
			return;
		}
		MediaPlayer.PlatformOptions currentPlatformOptions = _mediaPlayer.GetCurrentPlatformOptions();
		if (currentPlatformOptions == null || !currentPlatformOptions.IsUsingAudioCapture || _mediaPlayer.Control == null)
		{
			return;
		}
		AudioSource audioSource = _mediaPlayer.AudioSource;
		if ((bool)audioSource && (bool)_audioSpectrumMaterial)
		{
			active = true;
			float num = Helper.GetUnityAudioSampleRate() / 2;
			int num2 = Mathf.FloorToInt(Mathf.Clamp01(18000f / num) * (float)_spectrumSamples.Length);
			audioSource.GetSpectrumData(_spectrumSamples, 0, FFTWindow.BlackmanHarris);
			float num3 = -1f;
			for (int i = 0; i < num2; i++)
			{
				if (_spectrumSamples[i] > num3)
				{
					num3 = _spectrumSamples[i];
				}
			}
			_maxValue = Mathf.Lerp(_maxValue, 0f, Mathf.Clamp01(2f * Time.deltaTime));
			_maxValue = Mathf.Max(_maxValue, num3);
			if (_maxValue <= 0.01f)
			{
				_maxValue = 1f;
			}
			for (int j = 0; j < num2; j++)
			{
				float b = _spectrumSamples[j] / _maxValue;
				_spectrumSamplesSmooth[j] = Mathf.Lerp(_spectrumSamplesSmooth[j], b, Mathf.Clamp01(15f * Time.deltaTime));
			}
			_audioSpectrumMaterial.SetFloatArray(_propSpectrum.Id, _spectrumSamplesSmooth);
			_audioSpectrumMaterial.SetFloat(_propSpectrumRange.Id, num2);
		}
		if ((bool)_imageAudioSpectrum && !_imageAudioSpectrum.gameObject.activeSelf)
		{
			_imageAudioSpectrum.gameObject.SetActive(active);
		}
	}

	private void OnTimeSliderBeginDrag()
	{
		if ((bool)_mediaPlayer && _mediaPlayer.Control != null)
		{
			_wasPlayingBeforeTimelineDrag = _mediaPlayer.Control.IsPlaying();
			if (_wasPlayingBeforeTimelineDrag)
			{
				_mediaPlayer.Pause();
			}
			OnTimeSliderDrag();
		}
	}

	private void OnTimeSliderDrag()
	{
		if ((bool)_mediaPlayer && _mediaPlayer.Control != null)
		{
			TimeRange timelineRange = GetTimelineRange();
			double time = timelineRange.startTime + (double)_sliderTime.value * timelineRange.duration;
			_mediaPlayer.Control.Seek(time);
			_isHoveringOverTimeline = true;
		}
	}

	private void OnTimeSliderEndDrag()
	{
		if ((bool)_mediaPlayer && _mediaPlayer.Control != null && _wasPlayingBeforeTimelineDrag)
		{
			_mediaPlayer.Play();
			_wasPlayingBeforeTimelineDrag = false;
		}
	}

	private TimeRange GetTimelineRange()
	{
		if (_mediaPlayer.Info != null)
		{
			return Helper.GetTimelineRange(_mediaPlayer.Info.GetDuration(), _mediaPlayer.Control.GetSeekableTimes());
		}
		return default(TimeRange);
	}

	private bool CanHideControls()
	{
		bool result = true;
		if (!_autoHide)
		{
			result = false;
		}
		else if (Input.mousePresent)
		{
			RectTransform component = _controlsGroup.GetComponent<RectTransform>();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(component, Input.mousePosition, null, out var localPoint);
			result = !RectTransformUtility.PixelAdjustRect(component, null).Contains(localPoint);
		}
		return result;
	}

	private void UpdateControlsVisibility()
	{
		if (UserInteraction.IsUserInputThisFrame() || !CanHideControls())
		{
			UserInteraction.InactiveTime = 0f;
			FadeUpControls();
			return;
		}
		UserInteraction.InactiveTime += Time.unscaledDeltaTime;
		if (UserInteraction.InactiveTime >= _userInactiveDuration)
		{
			FadeDownControls();
		}
		else
		{
			FadeUpControls();
		}
	}

	private void FadeUpControls()
	{
		if (!_controlsGroup.gameObject.activeSelf)
		{
			_controlsGroup.gameObject.SetActive(value: true);
		}
		_controlsFade = Mathf.Min(1f, _controlsFade + Time.deltaTime * 8f);
		_controlsGroup.alpha = Mathf.Pow(_controlsFade, 5f);
	}

	private void FadeDownControls()
	{
		if (_controlsGroup.gameObject.activeSelf)
		{
			_controlsFade = Mathf.Max(0f, _controlsFade - Time.deltaTime * 3f);
			_controlsGroup.alpha = Mathf.Pow(_controlsFade, 5f);
			if (_controlsGroup.alpha <= 0f)
			{
				_controlsGroup.gameObject.SetActive(value: false);
			}
		}
	}

	private void Update()
	{
		if (!_mediaPlayer)
		{
			return;
		}
		UpdateControlsVisibility();
		UpdateAudioFading();
		UpdateAudioSpectrum();
		if (_mediaPlayer.Info == null)
		{
			return;
		}
		TimeRange timelineRange = GetTimelineRange();
		if (_timelineTip != null)
		{
			if (_isHoveringOverTimeline)
			{
				RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasTransform, Input.mousePosition, null, out var localPoint);
				_segmentsSeek.gameObject.SetActive(value: true);
				_timelineTip.gameObject.SetActive(value: true);
				Vector3 vector = _canvasTransform.TransformPoint(localPoint);
				_timelineTip.position = new Vector2(vector.x, _timelineTip.position.y);
				if (UserInteraction.IsUserInputThisFrame())
				{
					Bounds bounds = RectTransformUtility.CalculateRelativeRectTransformBounds(_sliderTime.GetComponent<RectTransform>());
					float num = Mathf.Clamp01((localPoint.x - bounds.min.x) / bounds.size.x);
					double num2 = (double)num * timelineRange.Duration;
					if (_thumbnailMediaPlayer != null && _thumbnailMediaPlayer.Control != null)
					{
						_thumbnailMediaPlayer.Control.SeekFast(num2);
					}
					Text componentInChildren = _timelineTip.GetComponentInChildren<Text>();
					if (componentInChildren != null)
					{
						num2 -= timelineRange.startTime;
						num2 = Math.Max(num2, 0.0);
						num2 = Math.Min(num2, timelineRange.Duration);
						componentInChildren.text = Helper.GetTimeString(num2);
					}
					if (_segmentsSeek != null)
					{
						float[] array = new float[2];
						if (timelineRange.Duration > 0.0)
						{
							double num3 = (_mediaPlayer.Control.GetCurrentTime() - timelineRange.startTime) / timelineRange.duration;
							array[1] = num;
							array[0] = (float)num3;
						}
						_segmentsSeek.Segments = array;
					}
				}
			}
			else
			{
				_timelineTip.gameObject.SetActive(value: false);
				_segmentsSeek.gameObject.SetActive(value: false);
			}
		}
		if ((bool)_overlayManager)
		{
			_overlayManager.Reset();
			if (_mediaPlayer.Info.IsPlaybackStalled())
			{
				_overlayManager.TriggerStalled();
			}
		}
		if (_enableKeyboardControls)
		{
			if (Input.GetKeyDown(KeyTogglePlayPause))
			{
				TogglePlayPause();
			}
			if (Input.GetKeyDown(KeyJumpBack))
			{
				SeekRelative(0f - _jumpDeltaTime);
			}
			else if (Input.GetKeyDown(KeyJumpForward))
			{
				SeekRelative(_jumpDeltaTime);
			}
			if (Input.GetKeyDown(KeyVolumeUp))
			{
				ChangeAudioVolume(_keyVolumeDelta);
			}
			else if (Input.GetKeyDown(KeyVolumeDown))
			{
				ChangeAudioVolume(0f - _keyVolumeDelta);
			}
			if (Input.GetKeyDown(KeyToggleMute))
			{
				ToggleMute();
			}
		}
		if (_playPauseMaterial != null)
		{
			float @float = _playPauseMaterial.GetFloat(_propMorph.Id);
			float num4 = 1f;
			if (_mediaPlayer.Control.IsPlaying())
			{
				num4 = -1f;
			}
			@float += num4 * Time.deltaTime * 6f;
			@float = Mathf.Clamp01(@float);
			_playPauseMaterial.SetFloat(_propMorph.Id, @float);
		}
		if (_volumeMaterial != null)
		{
			float float2 = _volumeMaterial.GetFloat(_propMute.Id);
			float num5 = 1f;
			if (!_mediaPlayer.AudioMuted)
			{
				num5 = -1f;
			}
			float2 += num5 * Time.deltaTime * 6f;
			float2 = Mathf.Clamp01(float2);
			_volumeMaterial.SetFloat(_propMute.Id, float2);
			_volumeMaterial.SetFloat(_propVolume.Id, _audioVolume);
		}
		if ((bool)_subtitlesMaterial)
		{
			float float3 = _subtitlesMaterial.GetFloat(_propMorph.Id);
			float num6 = 1f;
			if (_mediaPlayer.TextTracks.GetActiveTextTrack() == null)
			{
				num6 = -1f;
			}
			float3 += num6 * Time.deltaTime * 6f;
			float3 = Mathf.Clamp01(float3);
			_subtitlesMaterial.SetFloat(_propMorph.Id, float3);
		}
		if ((bool)_optionsMaterial)
		{
			float float4 = _optionsMaterial.GetFloat(_propMorph.Id);
			float num7 = 1f;
			if (!_showOptions)
			{
				num7 = -1f;
			}
			float4 += num7 * Time.deltaTime * 6f;
			float4 = Mathf.Clamp01(float4);
			_optionsMaterial.SetFloat(_propMorph.Id, float4);
		}
		if ((bool)_textTimeDuration)
		{
			string timeString = Helper.GetTimeString(_mediaPlayer.Control.GetCurrentTime() - timelineRange.startTime);
			string timeString2 = Helper.GetTimeString(timelineRange.duration);
			_textTimeDuration.text = $"{timeString} / {timeString2}";
		}
		if (!_useAudioFading)
		{
			UpdateVolumeSlider();
		}
		if ((bool)_sliderTime && !_isHoveringOverTimeline)
		{
			double num8 = 0.0;
			if (timelineRange.duration > 0.0)
			{
				num8 = (_mediaPlayer.Control.GetCurrentTime() - timelineRange.startTime) / timelineRange.duration;
			}
			_sliderTime.value = Mathf.Clamp01((float)num8);
		}
		if ((bool)_liveItem)
		{
			_liveItem.SetActive(double.IsInfinity(_mediaPlayer.Info.GetDuration()));
		}
		if ((bool)_buttonSubtitles)
		{
			_buttonSubtitles.gameObject.SetActive(_mediaPlayer.TextTracks.GetTextTracks().Count > 0);
		}
		if ((bool)_textMediaName)
		{
			string text = string.Empty;
			if (_mediaPlayer.Info.GetVideoWidth() > 0)
			{
				float videoFrameRate = _mediaPlayer.Info.GetVideoFrameRate();
				text = ((!(videoFrameRate > 0f) || float.IsNaN(videoFrameRate)) ? $"{_mediaPlayer.Info.GetVideoWidth()} x {_mediaPlayer.Info.GetVideoHeight()}" : string.Format("{0} x {1} @ {2}", _mediaPlayer.Info.GetVideoWidth(), _mediaPlayer.Info.GetVideoHeight(), videoFrameRate.ToString("0.00")));
			}
			_textMediaName.text = text;
		}
		if ((bool)_segmentsBuffered)
		{
			TimeRanges bufferedTimes = _mediaPlayer.Control.GetBufferedTimes();
			float[] array2 = null;
			if (bufferedTimes.Count > 0 && timelineRange.duration > 0.0)
			{
				array2 = new float[bufferedTimes.Count * 2];
				for (int i = 0; i < bufferedTimes.Count; i++)
				{
					array2[i * 2] = Mathf.Max(0f, (float)((bufferedTimes[i].StartTime - timelineRange.startTime) / timelineRange.duration));
					array2[i * 2 + 1] = Mathf.Min(1f, (float)((bufferedTimes[i].EndTime - timelineRange.startTime) / timelineRange.duration));
				}
			}
			_segmentsBuffered.Segments = array2;
		}
		if ((bool)_segmentsProgress)
		{
			TimeRanges bufferedTimes2 = _mediaPlayer.Control.GetBufferedTimes();
			float[] array3 = null;
			if (bufferedTimes2.Count > 0 && timelineRange.Duration > 0.0)
			{
				array3 = new float[2];
				double num9 = (bufferedTimes2.MinTime - timelineRange.startTime) / timelineRange.duration;
				double num10 = (_mediaPlayer.Control.GetCurrentTime() - timelineRange.startTime) / timelineRange.duration;
				array3[0] = Mathf.Max(0f, (float)num9);
				array3[1] = Mathf.Min(1f, (float)num10);
			}
			_segmentsProgress.Segments = array3;
		}
	}

	private void OnGUI()
	{
		if (!_showDebug || !_mediaPlayer || _mediaPlayer.Control == null)
		{
			return;
		}
		GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(2f, 2f, 1f));
		GUI.backgroundColor = Color.red;
		GUILayout.BeginVertical(GUI.skin.box);
		GUI.backgroundColor = Color.white;
		GUILayout.Label(string.Format("Duration: {0}s\tFPS: {1}", _mediaPlayer.Info.GetDuration(), _mediaPlayer.Info.GetVideoDisplayRate().ToString("F2")));
		GUILayout.BeginHorizontal();
		GUILayout.Label("States: ");
		GUILayout.Toggle(_mediaPlayer.Control.HasMetaData(), "HasMetaData", GUI.skin.button);
		GUILayout.Toggle(_mediaPlayer.Control.IsPaused(), "Paused", GUI.skin.button);
		GUILayout.Toggle(_mediaPlayer.Control.IsPlaying(), "Playing", GUI.skin.button);
		GUILayout.Toggle(_mediaPlayer.Control.IsBuffering(), "Buffering", GUI.skin.button);
		GUILayout.Toggle(_mediaPlayer.Control.IsSeeking(), "Seeking", GUI.skin.button);
		GUILayout.Toggle(_mediaPlayer.Control.IsFinished(), "Finished", GUI.skin.button);
		GUILayout.EndHorizontal();
		TimeRanges bufferedTimes = _mediaPlayer.Control.GetBufferedTimes();
		if (bufferedTimes != null)
		{
			GUILayout.Label("Buffered Range " + bufferedTimes.MinTime + " - " + bufferedTimes.MaxTime);
		}
		TimeRanges seekableTimes = _mediaPlayer.Control.GetSeekableTimes();
		if (seekableTimes != null)
		{
			GUILayout.Label("Seek Range " + seekableTimes.MinTime + " - " + seekableTimes.MaxTime);
		}
		GUILayout.Label("Video Tracks: " + _mediaPlayer.VideoTracks.GetVideoTracks().Count);
		GUILayout.BeginVertical();
		VideoTrack videoTrack = null;
		foreach (VideoTrack videoTrack2 in _mediaPlayer.VideoTracks.GetVideoTracks())
		{
			bool flag = videoTrack2 == _mediaPlayer.VideoTracks.GetActiveVideoTrack();
			if (flag)
			{
				GUI.color = Color.green;
			}
			if (GUILayout.Button(videoTrack2.DisplayName, GUILayout.ExpandWidth(expand: false)))
			{
				videoTrack = videoTrack2;
			}
			if (flag)
			{
				GUI.color = Color.white;
			}
		}
		GUILayout.EndHorizontal();
		if (videoTrack != null)
		{
			_mediaPlayer.VideoTracks.SetActiveVideoTrack(videoTrack);
		}
		GUILayout.Label("Audio Tracks: " + _mediaPlayer.AudioTracks.GetAudioTracks().Count);
		GUILayout.BeginVertical();
		AudioTrack audioTrack = null;
		foreach (AudioTrack audioTrack2 in _mediaPlayer.AudioTracks.GetAudioTracks())
		{
			bool flag2 = audioTrack2 == _mediaPlayer.AudioTracks.GetActiveAudioTrack();
			if (flag2)
			{
				GUI.color = Color.green;
			}
			if (GUILayout.Button(audioTrack2.DisplayName, GUILayout.ExpandWidth(expand: false)))
			{
				audioTrack = audioTrack2;
			}
			if (flag2)
			{
				GUI.color = Color.white;
			}
		}
		GUILayout.EndHorizontal();
		if (audioTrack != null)
		{
			_mediaPlayer.AudioTracks.SetActiveAudioTrack(audioTrack);
		}
		GUILayout.Label("Text Tracks: " + _mediaPlayer.TextTracks.GetTextTracks().Count);
		GUILayout.BeginVertical();
		TextTrack textTrack = null;
		foreach (TextTrack textTrack2 in _mediaPlayer.TextTracks.GetTextTracks())
		{
			bool flag3 = textTrack2 == _mediaPlayer.TextTracks.GetActiveTextTrack();
			if (flag3)
			{
				GUI.color = Color.green;
			}
			if (GUILayout.Button(textTrack2.DisplayName, GUILayout.ExpandWidth(expand: false)))
			{
				textTrack = textTrack2;
			}
			if (flag3)
			{
				GUI.color = Color.white;
			}
		}
		GUILayout.EndHorizontal();
		if (textTrack != null)
		{
			_mediaPlayer.TextTracks.SetActiveTextTrack(textTrack);
		}
		GUILayout.EndVertical();
	}
}
