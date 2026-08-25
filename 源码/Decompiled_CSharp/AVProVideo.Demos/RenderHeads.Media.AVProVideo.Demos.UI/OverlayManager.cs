using UnityEngine;
using UnityEngine.UI;

namespace RenderHeads.Media.AVProVideo.Demos.UI;

public class OverlayManager : MonoBehaviour
{
	public enum Feedback
	{
		Play,
		Pause,
		SeekForward,
		SeekBack,
		VolumeUp,
		VolumeDown,
		VolumeMute
	}

	[SerializeField]
	private Image _stalledImage;

	[SerializeField]
	private Image _feedbackImage;

	[SerializeField]
	private CanvasGroup _feedbackCanvas;

	[SerializeField]
	private float _startScale = 0.25f;

	[SerializeField]
	private float _endScale = 1f;

	[SerializeField]
	private float _animationSpeed = 1.5f;

	private Material _feedbackMaterial;

	private float _feedbackTimer;

	private readonly LazyShaderProperty _propMute = new LazyShaderProperty("_Mute");

	private readonly LazyShaderProperty _propVolume = new LazyShaderProperty("_Volume");

	private const string KeywordPlay = "UI_PLAY";

	private const string KeywordPause = "UI_PAUSE";

	private const string KeywordSeekBack = "UI_BACK";

	private const string KeywordSeekForward = "UI_FORWARD";

	private const string KeywordVolume = "UI_VOLUME";

	private const string KeywordVolumeUp = "UI_VOLUMEUP";

	private const string KeywordVolumeDown = "UI_VOLUMEDOWN";

	private const string KeywordVolumeMute = "UI_VOLUMEMUTE";

	private void Start()
	{
		_feedbackMaterial = new Material(_feedbackImage.material);
		_feedbackImage.material = _feedbackMaterial;
		_feedbackCanvas.alpha = 0f;
		_feedbackTimer = 1f;
	}

	private void OnDestroy()
	{
		if ((bool)_feedbackMaterial)
		{
			Object.Destroy(_feedbackMaterial);
			_feedbackMaterial = null;
		}
	}

	public void Reset()
	{
		_stalledImage.enabled = false;
	}

	public void TriggerStalled()
	{
		_stalledImage.enabled = true;
	}

	public void TriggerFeedback(Feedback feedback)
	{
		_feedbackMaterial.DisableKeyword("UI_PLAY");
		_feedbackMaterial.DisableKeyword("UI_PAUSE");
		_feedbackMaterial.DisableKeyword("UI_BACK");
		_feedbackMaterial.DisableKeyword("UI_FORWARD");
		_feedbackMaterial.DisableKeyword("UI_VOLUME");
		_feedbackMaterial.DisableKeyword("UI_VOLUMEUP");
		_feedbackMaterial.DisableKeyword("UI_VOLUMEDOWN");
		_feedbackMaterial.DisableKeyword("UI_VOLUMEMUTE");
		string text = null;
		switch (feedback)
		{
		case Feedback.Play:
			text = "UI_PLAY";
			break;
		case Feedback.Pause:
			text = "UI_PAUSE";
			break;
		case Feedback.SeekBack:
			text = "UI_BACK";
			break;
		case Feedback.SeekForward:
			text = "UI_FORWARD";
			break;
		case Feedback.VolumeUp:
			text = "UI_VOLUME";
			_feedbackMaterial.SetFloat(_propMute.Id, 0f);
			_feedbackMaterial.SetFloat(_propVolume.Id, 1f);
			break;
		case Feedback.VolumeDown:
			text = "UI_VOLUME";
			_feedbackMaterial.SetFloat(_propMute.Id, 0f);
			_feedbackMaterial.SetFloat(_propVolume.Id, 0.5f);
			break;
		case Feedback.VolumeMute:
			text = "UI_VOLUME";
			_feedbackMaterial.SetFloat(_propVolume.Id, 1f);
			_feedbackMaterial.SetFloat(_propMute.Id, 1f);
			break;
		}
		if (!string.IsNullOrEmpty(text))
		{
			_feedbackMaterial.EnableKeyword(text);
		}
		_feedbackCanvas.alpha = 1f;
		_feedbackCanvas.transform.localScale = new Vector3(_startScale, _startScale, _startScale);
		_feedbackTimer = 0f;
		Update();
	}

	private void Update()
	{
		float t = Mathf.Clamp01(_feedbackTimer);
		float t2 = Mathf.Clamp01((_feedbackTimer - 0.5f) * 2f);
		_feedbackCanvas.alpha = Mathf.Lerp(1f, 0f, PowerEaseOut(t2, 1f));
		if (_feedbackCanvas.alpha > 0f)
		{
			float num = Mathf.Lerp(_startScale, _endScale, PowerEaseOut(t, 2f));
			_feedbackCanvas.transform.localScale = new Vector3(num, num, num);
		}
		_feedbackTimer += Time.deltaTime * _animationSpeed;
	}

	private static float PowerEaseOut(float t, float power)
	{
		return 1f - Mathf.Abs(Mathf.Pow(t - 1f, power));
	}
}
