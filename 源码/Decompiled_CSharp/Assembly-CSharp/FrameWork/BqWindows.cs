using System.Collections.Generic;
using DG.Tweening;
using FrameWork.Data;
using RenderHeads.Media.AVProVideo;
using Script.Scene;
using Script.Tool;
using Script.UiTool;
using UnityEngine;

namespace FrameWork;

[UiMode(Mode.Popup, true)]
[ActorInfo("", "BqWindows")]
public class BqWindows : UiActor
{
	private int _qteSucCount;

	private Tweener _tweener;

	public RectTransform RectTransformOther;

	public AddScripts AddScriptsOther;

	public RectTransform RectTransformVideoPlayer;

	public CanvasRenderer CanvasRendererVideoPlayer;

	public AddScripts AddScriptsVideoPlayer;

	public CanvasGroup CanvasGroupVideoPlayer;

	public MediaPlayer MediaPlayerVideoPlayer;

	public DisplayUGUI DisplayUGUIVideoPlayer;

	public ApplyToMaterial ApplyToMaterialVideoPlayer;

	public override void Awake()
	{
		base.Awake();
		RectTransformOther = GetGameObject().transform.Find("Other/").GetComponent<RectTransform>();
		AddScriptsOther = GetGameObject().transform.Find("Other/").GetComponent<AddScripts>();
		RectTransformVideoPlayer = GetGameObject().transform.Find("ViewData/Video Player/").GetComponent<RectTransform>();
		CanvasRendererVideoPlayer = GetGameObject().transform.Find("ViewData/Video Player/").GetComponent<CanvasRenderer>();
		AddScriptsVideoPlayer = GetGameObject().transform.Find("ViewData/Video Player/").GetComponent<AddScripts>();
		CanvasGroupVideoPlayer = GetGameObject().transform.Find("ViewData/Video Player/").GetComponent<CanvasGroup>();
		MediaPlayerVideoPlayer = GetGameObject().transform.Find("ViewData/Video Player/").GetComponent<MediaPlayer>();
		DisplayUGUIVideoPlayer = GetGameObject().transform.Find("ViewData/Video Player/").GetComponent<DisplayUGUI>();
		ApplyToMaterialVideoPlayer = GetGameObject().transform.Find("ViewData/Video Player/").GetComponent<ApplyToMaterial>();
	}

	public BqWindows(Transform trans)
		: base(trans)
	{
	}

	public BqWindows()
	{
	}

	public override void OnEnable()
	{
		base.OnEnable();
		_qteSucCount = 0;
		EventManager.AddListener(MessageType.Game, GameMessageType.QteSuc, QteSuc);
		EventManager.AddListener(MessageType.Game, GameMessageType.QteFail, QteFail);
		VideoItem videoItem = GameScene.Instance.videoItem;
		if ((bool)videoItem && videoItem.GetNode().isHasQteBq)
		{
			LoadVideo();
			PlayVideo(videoItem.GetNode().ptVideo);
		}
	}

	private void LoadVideo()
	{
		VideoItem videoItem = GameScene.Instance.videoItem;
		List<string> videoPaths = new List<string>();
		if (!string.IsNullOrEmpty(videoItem.GetNode().sfVideo.unlockVideoPath))
		{
			videoPaths.Add(videoItem.GetNode().sfVideo.unlockVideoPath);
		}
		if (!string.IsNullOrEmpty(videoItem.GetNode().ptVideo.unlockVideoPath))
		{
			videoPaths.Add(videoItem.GetNode().ptVideo.unlockVideoPath);
		}
		if (!string.IsNullOrEmpty(videoItem.GetNode().nsVideo.unlockVideoPath))
		{
			videoPaths.Add(videoItem.GetNode().nsVideo.unlockVideoPath);
		}
		RectTransformOther.TranFor(videoPaths.Count, RectTransformOther.GetChild(0), delegate(int i, GameObject o)
		{
			o.GetComponent<MediaPlayer>().OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, Tool.GetVideoPath(videoPaths[i]), autoPlay: false);
		});
	}

	public override void OnDisable()
	{
		base.OnDisable();
		EventManager.RemoveListener(MessageType.Game, GameMessageType.QteSuc, QteSuc);
		EventManager.RemoveListener(MessageType.Game, GameMessageType.QteFail, QteFail);
	}

	private void QteSuc(List<object> objects)
	{
		_qteSucCount++;
		VideoItem videoItem = GameScene.Instance.videoItem;
		if ((bool)videoItem && videoItem.GetNode().isHasQteBq)
		{
			if (_qteSucCount >= 4)
			{
				PlayVideo(videoItem.GetNode().sfVideo);
			}
			else
			{
				PlayVideo(videoItem.GetNode().ptVideo);
			}
		}
	}

	private void QteFail(List<object> objects)
	{
		_qteSucCount = 0;
		VideoItem videoItem = GameScene.Instance.videoItem;
		if ((bool)videoItem && videoItem.GetNode().isHasQteBq)
		{
			PlayVideo(videoItem.GetNode().nsVideo);
		}
	}

	private void PlayVideo(VideoUnlockData data)
	{
		if (MediaPlayerVideoPlayer.MediaPath.Path == Tool.GetVideoPath(data.unlockVideoPath))
		{
			return;
		}
		MediaPlayerVideoPlayer.Events.RemoveAllListeners();
		MediaPlayerVideoPlayer.Events.AddListener(delegate(MediaPlayer arg0, MediaPlayerEvent.EventType type, ErrorCode code)
		{
			if (type == MediaPlayerEvent.EventType.FirstFrameReady)
			{
				arg0.Loop = true;
				arg0.Control.SetLooping(bLooping: true);
				MediaPlayerVideoPlayer.Play();
				MediaPlayerVideoPlayer.AudioVolume = GameData.GetOpenAsNum("Volume");
				if ((bool)GameScene.Instance.videoItem)
				{
					MediaPlayerVideoPlayer.Control.Seek(GameScene.Instance.videoItem.GetCurVideoTime());
				}
			}
		});
		MediaPlayerVideoPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, Tool.GetVideoPath(data.unlockVideoPath), autoPlay: false);
	}

	protected override void Play()
	{
	}

	protected override void Pause()
	{
	}
}
