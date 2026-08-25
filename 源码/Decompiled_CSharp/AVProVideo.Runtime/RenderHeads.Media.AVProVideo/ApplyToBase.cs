using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public abstract class ApplyToBase : MonoBehaviour
{
	[SerializeField]
	[Header("Media Source")]
	protected MediaPlayer _media;

	[SerializeField]
	[Header("Display")]
	[Space(8f)]
	private bool _automaticStereoPacking = true;

	[SerializeField]
	private StereoPacking _overrideStereoPacking;

	[SerializeField]
	private bool _stereoRedGreenTint;

	protected bool _isDirty;

	public MediaPlayer Player
	{
		get
		{
			return _media;
		}
		set
		{
			ChangeMediaPlayer(value);
		}
	}

	public bool AutomaticStereoPacking
	{
		get
		{
			return _automaticStereoPacking;
		}
		set
		{
			if (_automaticStereoPacking != value)
			{
				_automaticStereoPacking = value;
				_isDirty = true;
			}
		}
	}

	public StereoPacking OverrideStereoPacking
	{
		get
		{
			return _overrideStereoPacking;
		}
		set
		{
			if (_overrideStereoPacking != value)
			{
				_overrideStereoPacking = value;
				_isDirty = true;
			}
		}
	}

	public bool StereoRedGreenTint
	{
		get
		{
			return _stereoRedGreenTint;
		}
		set
		{
			if (_stereoRedGreenTint != value)
			{
				_stereoRedGreenTint = value;
				_isDirty = true;
			}
		}
	}

	private void Awake()
	{
		ChangeMediaPlayer(_media, force: true);
	}

	private void ChangeMediaPlayer(MediaPlayer player, bool force = false)
	{
		if (_media != player || force)
		{
			if (_media != null)
			{
				_media.Events.RemoveListener(OnMediaPlayerEvent);
			}
			_media = player;
			if (_media != null)
			{
				_media.Events.AddListener(OnMediaPlayerEvent);
			}
			_isDirty = true;
		}
	}

	private void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	{
		if (et == MediaPlayerEvent.EventType.FirstFrameReady || et == MediaPlayerEvent.EventType.PropertiesChanged)
		{
			ForceUpdate();
		}
	}

	public void ForceUpdate()
	{
		_isDirty = true;
		if (base.isActiveAndEnabled)
		{
			Apply();
		}
	}

	private void Start()
	{
		SaveProperties();
		Apply();
	}

	protected virtual void OnEnable()
	{
		SaveProperties();
		ForceUpdate();
	}

	protected virtual void OnDisable()
	{
		RestoreProperties();
	}

	private void OnDestroy()
	{
		ChangeMediaPlayer(null);
	}

	protected virtual void SaveProperties()
	{
	}

	protected virtual void RestoreProperties()
	{
	}

	public abstract void Apply();
}
