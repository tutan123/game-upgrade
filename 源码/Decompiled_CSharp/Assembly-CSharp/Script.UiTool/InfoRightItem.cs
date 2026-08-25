using System;
using DG.Tweening;
using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;

namespace Script.UiTool;

public class InfoRightItem : MonoBehaviour
{
	public TMP_Text text;

	public RectTransform animRect;

	public TMP_Text info;

	private TaskTipsData _taskTipsData;

	private TimeData _timeData;

	private Tweener _animTweener;

	public void Init(TaskTipsData info)
	{
		_animTweener?.Kill();
		_taskTipsData = info;
		text.text = _taskTipsData.Text;
		info.Text = _taskTipsData.Info;
		animRect.anchoredPosition = new Vector2(427f, 0f);
		SingletonAsMono<AudioMrg>.Instance.Play(_taskTipsData.AudioClip);
		_animTweener = animRect.DOAnchorPos(new Vector2(0f, 0f), 0.2f).SetEase(Ease.Linear);
		if (info.TaskType == TaskType.Suc)
		{
			_timeData = Timer.DelayCall(2f, Back);
		}
	}

	public void Back()
	{
		Timer.DestroyTimer(_timeData);
		_animTweener?.Kill();
		_animTweener = animRect.DOAnchorPos(new Vector2(427f, 0f), 0.2f).SetEase(Ease.Linear);
		Tweener animTweener = _animTweener;
		animTweener.onComplete = (TweenCallback)Delegate.Combine(animTweener.onComplete, (TweenCallback)delegate
		{
			base.gameObject.SetActiveAsCheck(active: false);
		});
	}

	public void OpenTask()
	{
		UiManager.OpenUi<MainWindows>().OpenTask().InitTask(_taskTipsData.XlsxTask, _taskTipsData.TaskType);
		Back();
	}
}
