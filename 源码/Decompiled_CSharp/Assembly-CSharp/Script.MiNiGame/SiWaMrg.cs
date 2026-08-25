using System;
using System.Collections.Generic;
using DG.Tweening;
using FrameWork;
using Script.Audio;
using Script.Mrg;
using UnityEngine;
using UnityEngine.Video;
using Xlsx;

namespace Script.MiNiGame;

public class SiWaMrg : MonoBehaviour
{
	public VideoPlayer videoPlayer;

	public List<VideoClip> videoClips;

	public List<VideoClip> erJieClips;

	public List<VideoClip> liuJieClips;

	public List<VideoClip> qiJieClips;

	public List<VideoClip> jiuJieClips;

	public int index;

	public CanvasGroup canvasGroup;

	public GameObject xiaoLianAnim;

	private Xlsx_MiNiGameLevel_Key _xlsxMiNiGameLevelKey;

	private TimeData _xlAnim;

	private Tweener _tweener;

	private void Start()
	{
		_xlsxMiNiGameLevelKey = Enum.Parse<Xlsx_MiNiGameLevel_Key>(SingletonAsMono<GameDataMrg>.Instance.xlsxMiNiGameLevelKey);
		index = UnityEngine.Random.Range(0, videoClips.Count);
		xiaoLianAnim.SetActiveAsCheck(active: false);
		PlayVideo(isPlayAnim: false);
	}

	private void PlayVideo(bool isPlayAnim = true)
	{
		if (isPlayAnim)
		{
			SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.bqbClip);
			Timer.DestroyTimer(_xlAnim);
			_xlAnim = null;
			xiaoLianAnim.SetActive(value: false);
			xiaoLianAnim.SetActiveAsCheck(active: true);
			_xlAnim = Timer.DelayCall(0.9f, delegate
			{
				xiaoLianAnim.SetActiveAsCheck(active: false);
			});
		}
		_tweener?.Kill();
		_tweener = canvasGroup.DOFade(0f, 0.2f);
		Tweener tweener = _tweener;
		tweener.onComplete = (TweenCallback)Delegate.Combine(tweener.onComplete, (TweenCallback)delegate
		{
			if (_xlsxMiNiGameLevelKey == Xlsx_MiNiGameLevel_Key.A6)
			{
				videoPlayer.clip = liuJieClips[index % liuJieClips.Count];
			}
			else if (_xlsxMiNiGameLevelKey == Xlsx_MiNiGameLevel_Key.A7)
			{
				videoPlayer.clip = qiJieClips[index % qiJieClips.Count];
			}
			else if (_xlsxMiNiGameLevelKey == Xlsx_MiNiGameLevel_Key.A9)
			{
				videoPlayer.clip = jiuJieClips[index % jiuJieClips.Count];
			}
			else if (_xlsxMiNiGameLevelKey <= Xlsx_MiNiGameLevel_Key.A5)
			{
				videoPlayer.clip = videoClips[index % videoClips.Count];
			}
			else
			{
				videoPlayer.clip = erJieClips[index % erJieClips.Count];
			}
			videoPlayer.Play();
			index++;
			_tweener = canvasGroup.DOFade(1f, 0.2f).SetDelay(0.2f);
		});
	}

	private void OnEnable()
	{
		EventManager.AddListener(MessageType.Game, GameMessageType.GetXiaoLian, GetXiaoLian);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.GetXiaoLian, GetXiaoLian);
	}

	private void GetXiaoLian(List<object> objects)
	{
		PlayVideo();
	}
}
