using System;
using System.Collections.Generic;
using DG.Tweening;
using FrameWork;
using Script.Mrg;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Script.UiTool;

public class HeartbeatTool : MonoBehaviour
{
	public VideoClip oneVideo;

	public VideoClip twoVideo;

	public VideoPlayer videoPlayer;

	public bool isOneVideo;

	public RawImage rawImage;

	public List<HeartbeatUnlockData> showUnLock;

	private FolderDataGroup _folderDataGroup;

	private Tweener _tweener;

	private void Awake()
	{
		_folderDataGroup = ABMrg.Load<FolderDataGroup>("VideoData");
	}

	private void Start()
	{
		PlayVideo(isInit: true);
	}

	private void OnEnable()
	{
		CheckShowUnLock();
	}

	public void PlayVideo(bool isInit = false)
	{
		_tweener?.Kill();
		_tweener = rawImage.DOColor(new Color(1f, 1f, 1f, 0f), 0.1f);
		Tweener tweener = _tweener;
		tweener.onComplete = (TweenCallback)Delegate.Combine(tweener.onComplete, (TweenCallback)delegate
		{
			if (isInit)
			{
				videoPlayer.clip = oneVideo;
				isOneVideo = true;
				videoPlayer.Play();
			}
			else
			{
				if (isOneVideo)
				{
					videoPlayer.clip = twoVideo;
					isOneVideo = false;
				}
				else
				{
					videoPlayer.clip = oneVideo;
					isOneVideo = true;
				}
				videoPlayer.Play();
			}
			CheckShowUnLock();
			_tweener = rawImage.DOColor(Color.white, 0.2f).SetDelay(0.2f);
		});
	}

	private void CheckShowUnLock()
	{
		for (int i = 0; i < showUnLock.Count; i++)
		{
			showUnLock[i].showObj.SetActiveAsCheck(!IsCanShow(showUnLock[i]));
		}
	}

	private bool IsCanShow(HeartbeatUnlockData data)
	{
		return true;
	}

	public void OnClick1Video()
	{
		if (IsCanShow(showUnLock[0]))
		{
			if (isOneVideo)
			{
				UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.LiQinTon.FolderName), _folderDataGroup.LiQinTon.FolderList);
			}
			else
			{
				UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.MiShu.FolderName), _folderDataGroup.MiShu.FolderList);
			}
		}
	}

	public void OnClick2Video()
	{
		if (IsCanShow(showUnLock[1]))
		{
			if (isOneVideo)
			{
				UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.JiuBaLaoBan.FolderName), _folderDataGroup.JiuBaLaoBan.FolderList);
			}
			else
			{
				UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.GonShiLaoBan.FolderName), _folderDataGroup.GonShiLaoBan.FolderList);
			}
		}
	}

	public void OnClick3Video()
	{
		if (IsCanShow(showUnLock[2]))
		{
			if (isOneVideo)
			{
				UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.MeiMei.FolderName), _folderDataGroup.MeiMei.FolderList);
			}
			else
			{
				UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.XiaoFuPo.FolderName), _folderDataGroup.XiaoFuPo.FolderList);
			}
		}
	}

	public void OnClick4Video()
	{
		if (IsCanShow(showUnLock[3]))
		{
			if (isOneVideo)
			{
				UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.LinXiaoYu.FolderName), _folderDataGroup.LinXiaoYu.FolderList);
			}
			else
			{
				UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.ZhiWuNian.FolderName), _folderDataGroup.ZhiWuNian.FolderList);
			}
		}
	}

	public void OnClick5Video()
	{
		if (IsCanShow(showUnLock[4]))
		{
			if (isOneVideo)
			{
				UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.BaoZhuPo.FolderName), _folderDataGroup.BaoZhuPo.FolderList);
			}
			else
			{
				UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.XiXueGui.FolderName), _folderDataGroup.XiXueGui.FolderList);
			}
		}
	}
}
