using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Load, true)]
[ActorInfo("", "LoadWindows")]
public class LoadWindows : UiActor
{
	private Tweener _tweener;

	public RectTransform RectTransformBg;

	public CanvasRenderer CanvasRendererBg;

	public Image ImageBg;

	public AddScripts AddScriptsBg;

	public override void Awake()
	{
		base.Awake();
		RectTransformBg = GetGameObject().transform.Find("Bg/").GetComponent<RectTransform>();
		CanvasRendererBg = GetGameObject().transform.Find("Bg/").GetComponent<CanvasRenderer>();
		ImageBg = GetGameObject().transform.Find("Bg/").GetComponent<Image>();
		AddScriptsBg = GetGameObject().transform.Find("Bg/").GetComponent<AddScripts>();
	}

	public LoadWindows(Transform trans)
		: base(trans)
	{
	}

	public LoadWindows()
	{
	}

	public void Init(float time = 1f, Action action = null)
	{
		_tweener?.Kill();
		ImageBg.color = Color.black;
		_tweener = ImageBg.DOColor(Color.clear, time).SetEase(Ease.Linear);
		Tweener tweener = _tweener;
		tweener.onComplete = (TweenCallback)Delegate.Combine(tweener.onComplete, (TweenCallback)delegate
		{
			CloseUi();
			action?.Invoke();
		});
	}
}
