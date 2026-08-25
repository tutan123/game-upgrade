using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

[RequireComponent(typeof(AudioSource))]
[AddComponentMenu("AVPro Video/Audio Output", 400)]
[HelpURL("https://www.renderheads.com/products/avpro-video/")]
public class AudioOutput : MonoBehaviour
{
	public enum AudioOutputMode
	{
		OneToAllChannels,
		MultipleChannels
	}

	[SerializeField]
	private MediaPlayer _mediaPlayer;

	[SerializeField]
	private AudioOutputMode _audioOutputMode = AudioOutputMode.MultipleChannels;

	[HideInInspector]
	[SerializeField]
	private int _channelMask = 65535;

	[SerializeField]
	private bool _supportPositionalAudio;

	private int _mediaPlayerInstanceID;

	private AudioSource _audioSource;

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

	public AudioOutputMode OutputMode
	{
		get
		{
			return _audioOutputMode;
		}
		set
		{
			_audioOutputMode = value;
		}
	}

	public int ChannelMask
	{
		get
		{
			return _channelMask;
		}
		set
		{
			_channelMask = value;
		}
	}

	public bool SupportPositionalAudio
	{
		get
		{
			return _supportPositionalAudio;
		}
		set
		{
			_supportPositionalAudio = value;
		}
	}

	private void Awake()
	{
		_audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		AudioSettings.OnAudioConfigurationChanged += OnAudioConfigurationChanged;
		ChangeMediaPlayer(_mediaPlayer);
	}

	private void OnAudioConfigurationChanged(bool deviceChanged)
	{
		if (!(_mediaPlayer == null) && _mediaPlayer.Control != null)
		{
			_mediaPlayer.Control.AudioConfigurationChanged(deviceChanged);
		}
	}

	private void OnDestroy()
	{
		ChangeMediaPlayer(null);
	}

	private void Update()
	{
		if (_mediaPlayer != null && _mediaPlayer.Control != null && _mediaPlayer.Control.IsPlaying())
		{
			ApplyAudioSettings(_mediaPlayer, _audioSource);
		}
	}

	public AudioSource GetAudioSource()
	{
		return _audioSource;
	}

	public void SetAudioSource(AudioSource source)
	{
		_audioSource = source;
		if ((bool)_mediaPlayer)
		{
			_mediaPlayer.AudioSource = source;
		}
	}

	public void ChangeMediaPlayer(MediaPlayer newPlayer)
	{
		if (_mediaPlayer != null)
		{
			_mediaPlayer.AudioSource = null;
			_mediaPlayer.Events.RemoveListener(OnMediaPlayerEvent);
			AudioOutputManager.Instance.RemovePlayerInstance(_mediaPlayerInstanceID);
			_mediaPlayer = null;
			_mediaPlayerInstanceID = 0;
		}
		_mediaPlayer = newPlayer;
		if (_mediaPlayer != null)
		{
			_mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
			_mediaPlayer.AudioSource = _audioSource;
			_mediaPlayerInstanceID = _mediaPlayer.GetInstanceID();
			AudioOutputManager.Instance.AddPlayerInstance(_mediaPlayerInstanceID);
		}
		if (_supportPositionalAudio)
		{
			if (_audioSource.clip == null)
			{
				int num = 20480;
				int num2 = num * Helper.GetUnityAudioSpeakerCount();
				AudioClip audioClip = AudioClip.Create("dummy", num, Helper.GetUnityAudioSpeakerCount(), Helper.GetUnityAudioSampleRate(), stream: false);
				float[] array = new float[num2];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = 1f;
				}
				audioClip.SetData(array, 0);
				_audioSource.clip = audioClip;
				_audioSource.loop = true;
			}
		}
		else if (_audioSource.clip != null)
		{
			_audioSource.clip = null;
		}
	}

	private void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	{
		switch (et)
		{
		case MediaPlayerEvent.EventType.Closing:
			_audioSource.Stop();
			break;
		case MediaPlayerEvent.EventType.Started:
			ApplyAudioSettings(_mediaPlayer, _audioSource);
			_audioSource.Play();
			break;
		}
	}

	private static void ApplyAudioSettings(MediaPlayer player, AudioSource audioSource)
	{
		if (audioSource != null && player != null && player.Control != null)
		{
			float volume = player.Control.GetVolume();
			bool mute = player.Control.IsMuted();
			float playbackRate = player.Control.GetPlaybackRate();
			audioSource.volume = volume;
			audioSource.mute = mute;
			audioSource.pitch = playbackRate;
		}
	}

	private void OnAudioFilterRead(float[] audioData, int channelCount)
	{
		AudioOutputManager.Instance.RequestAudio(this, _mediaPlayer, _mediaPlayerInstanceID, audioData, channelCount, _channelMask, _audioOutputMode, _supportPositionalAudio);
	}
}
