using System;
using System.Collections.Generic;
using DG.Tweening;
using Script.Mrg;
using Script.Scene;
using UnityEngine;

namespace FrameWork;

public class UiActor : Actor
{
	protected Tweener _bgTweener;

	protected Tweener _viewTweener;

	protected Tweener _viewBgTweener;

	protected UiActor()
	{
	}

	protected UiActor(Transform trans)
		: base(trans)
	{
	}

	public override void Awake()
	{
		base.Awake();
	}

	public virtual void Open(object[] objects)
	{
		Transform transform = base.transform.Find("Bg");
		if ((bool)transform)
		{
			CanvasGroup component = transform.GetComponent<CanvasGroup>();
			if ((bool)component)
			{
				component.DOFade(1f, 0.3f);
			}
		}
		Transform transform2 = base.transform.Find("View");
		if ((bool)transform2)
		{
			DoScale(transform2.GetComponent<RectTransform>(), new Vector3(0.6f, 0.6f, 0.6f), Vector3.one);
			CanvasGroup component2 = transform2.GetComponent<CanvasGroup>();
			if ((bool)component2)
			{
				component2.DOFade(1f, 0.3f);
			}
		}
		EventManager.DispatchEvent(MessageType.UiMessage, UiMessageType.Open);
		Pause();
	}

	protected virtual void Pause()
	{
		if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.GameScene && (bool)GameScene.Instance)
		{
			GameScene.Instance.PauseEsc();
		}
	}

	protected virtual void Play()
	{
		SingletonAsMono<Mono>.Instance.Frame(delegate
		{
			if (UiManager.IsCanPlay() && SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.GameScene && (bool)GameScene.Instance)
			{
				GameScene.Instance.PlayEsc();
			}
			EventManager.DispatchEvent(MessageType.UiMessage, UiMessageType.Close);
		});
	}

	public override void Start()
	{
		base.Start();
		AddListener(2, 0, ShowUi);
		AddListener(2, 1, HideUi);
		AddListener(2, 2, RemoveUi);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		RemoveListener(2, 0, ShowUi);
		RemoveListener(2, 1, HideUi);
		RemoveListener(2, 2, RemoveUi);
	}

	private void ShowUi(List<object> parma)
	{
		ShowUi((int)parma[0]);
	}

	private void ShowUi(int index)
	{
		if (index.Equals(GetIndex()))
		{
			GetGameObject().SetActive(value: true);
		}
	}

	protected virtual void HideUi(List<object> parma)
	{
		HideUi((int)parma[0]);
	}

	protected void HideUi(int index)
	{
		if (index.Equals(GetIndex()) || index == -1)
		{
			GetGameObject().SetActive(value: false);
		}
	}

	protected virtual void RemoveUi(List<object> parma)
	{
		RemoveUi((int)parma[0]);
	}

	protected void RemoveUi(int index)
	{
		if (index.Equals(GetIndex()) || index == -1)
		{
			_bgTweener?.Kill();
			_viewTweener?.Kill();
			_viewBgTweener?.Kill();
			UiManager.RemoveUi(GetIndex());
		}
	}

	public virtual void CloseUi()
	{
		Transform transform = base.transform.Find("Bg");
		if ((bool)transform)
		{
			CanvasGroup component = transform.GetComponent<CanvasGroup>();
			if ((bool)component)
			{
				_bgTweener = component.DOFade(0f, 0.3f);
			}
		}
		Transform transform2 = base.transform.Find("View");
		if ((bool)transform2)
		{
			DoScale(transform2.GetComponent<RectTransform>(), transform2.localScale, Vector3.one * 0.6f, delegate
			{
				UiManager.RemoveUi(GetIndex());
			});
			CanvasGroup component2 = transform2.GetComponent<CanvasGroup>();
			if ((bool)component2)
			{
				_viewBgTweener = component2.DOFade(0f, 0.3f);
			}
		}
		else
		{
			UiManager.RemoveUi(GetIndex());
		}
	}

	public void DoScale(RectTransform v, Vector3 nor, Vector3 scale, Action end = null, float time = 0.3f)
	{
		v.localScale = nor;
		_viewTweener = v.DOScale(scale, time);
		Tweener viewTweener = _viewTweener;
		viewTweener.onComplete = (TweenCallback)Delegate.Combine(viewTweener.onComplete, (TweenCallback)delegate
		{
			end?.Invoke();
		});
	}

	public virtual void OnClose()
	{
	}

	public virtual string GetUiName()
	{
		return "";
	}

	public override void OnDisable()
	{
		base.OnDisable();
		Play();
	}
}
