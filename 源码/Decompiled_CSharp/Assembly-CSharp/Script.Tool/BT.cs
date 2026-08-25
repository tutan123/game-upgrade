using System;
using DG.Tweening;
using FrameWork;
using Script.Audio;
using Script.Mrg;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Script.Tool;

public class BT : Button
{
	public AudioClip clickAudio;

	public bool isAnim = true;

	public bool isPlayBtAudio = true;

	private Vector3 _norSize;

	private Vector3 _scaleSize;

	[OdinSerialize]
	public float scaleSize = 1.1f;

	[OdinSerialize]
	public UnityEvent<PointerEventData> click;

	[OdinSerialize]
	public UnityEvent clickRight;

	[OdinSerialize]
	public UnityEvent OnEnter;

	[OdinSerialize]
	public UnityEvent OnExit;

	private float _clickTime = 0.15f;

	private float _currentTime;

	private Tweener _tween;

	protected override void Awake()
	{
		base.Awake();
		_norSize = base.transform.localScale;
		_scaleSize = _norSize * scaleSize;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		_currentTime = 0.15f;
		if (!IsGamepadConnected())
		{
			Navigation navigation = base.navigation;
			navigation.mode = Navigation.Mode.None;
			base.navigation = navigation;
		}
		else
		{
			Navigation navigation2 = base.navigation;
			navigation2.mode = Navigation.Mode.Automatic;
			base.navigation = navigation2;
		}
		base.transform.localScale = _norSize;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		_tween?.Kill();
	}

	public bool IsGamepadConnected()
	{
		string[] joystickNames = Input.GetJoystickNames();
		for (int i = 0; i < joystickNames.Length; i++)
		{
			if (!string.IsNullOrEmpty(joystickNames[i]))
			{
				return true;
			}
		}
		return false;
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (base.interactable)
		{
			base.OnPointerEnter(eventData);
			Enter();
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (base.interactable)
		{
			base.OnPointerExit(eventData);
			Exit();
		}
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (!(_currentTime < _clickTime) && base.interactable)
		{
			if ((bool)clickAudio)
			{
				SingletonAsMono<AudioMrg>.Instance.Play(clickAudio);
				_ = clickAudio.length;
			}
			else if (isPlayBtAudio)
			{
				SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.click);
				_ = AudioDataClip.AudioData.click.length;
			}
			if (eventData.button == PointerEventData.InputButton.Right)
			{
				clickRight?.Invoke();
			}
			else if (eventData.button == PointerEventData.InputButton.Left)
			{
				Click();
			}
			base.OnPointerClick(eventData);
			click?.Invoke(eventData);
		}
	}

	private void Enter()
	{
		OnEnter?.Invoke();
		if (isAnim)
		{
			_tween?.Kill();
			_tween = base.transform.DOScale(_scaleSize, 0.15f);
		}
	}

	private void Exit()
	{
		OnExit?.Invoke();
		if (isAnim)
		{
			_tween?.Kill();
			_tween = base.transform.DOScale(_norSize, 0.15f);
		}
	}

	private void Click()
	{
		if (isAnim)
		{
			_tween?.Kill();
			Vector3 endValue = base.transform.localScale * scaleSize;
			_tween = base.transform.DOScale(endValue, 0.1f).SetEase(Ease.InOutBack);
			Tweener tween = _tween;
			tween.onComplete = (TweenCallback)Delegate.Combine(tween.onComplete, (TweenCallback)delegate
			{
				_tween = base.transform.DOScale(_norSize, 0.1f).SetEase(Ease.InOutBack);
			});
		}
	}

	private void Update()
	{
		_currentTime += Time.deltaTime;
	}
}
