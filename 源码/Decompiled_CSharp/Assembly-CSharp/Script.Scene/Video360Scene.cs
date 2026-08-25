using System;
using System.Collections.Generic;
using FrameWork;
using Script.Audio;
using Script.Mrg;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Script.Scene;

public class Video360Scene : MonoBehaviour
{
	public bool isLockClick;

	public static Video360Scene Instance;

	public Material material;

	private VideoNode videoNode;

	public Image norImg;

	public Transform video360;

	public TMP_Text showMoveToText;

	public GameObject showMoveToTextObj;

	public TMP_Text timeOutText;

	public TMP_Text useText;

	public Camera camera360;

	public Transform yawParent;

	public Transform pitchChild;

	public Drag drag;

	private AudioSource _audioSource;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		GameScene instance = GameScene.Instance;
		instance.FirstFrameReady = (UnityAction)Delegate.Combine(instance.FirstFrameReady, new UnityAction(FirstFrameReady));
	}

	private void OnEnable()
	{
		if (!SingletonAsMono<GameDataMrg>.Instance.GetData().isStart)
		{
			SingletonAsMono<GameDataMrg>.Instance.GetData().isStart = true;
		}
		EventManager.AddListener(MessageType.Game, GameMessageType.LoadScene, CloseBg);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.LoadScene, CloseBg);
		CloseBg(null);
	}

	public void SaveVideoPos()
	{
		SingletonAsMono<GameDataMrg>.Instance.Video360Rotation = new Vector3(pitchChild.localEulerAngles.x, yawParent.eulerAngles.y, 0f);
	}

	private void CloseBg(List<object> go)
	{
		if (_audioSource != null)
		{
			SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
			_audioSource = null;
		}
	}

	public void FirstFrameReady()
	{
		if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.Video360Scene)
		{
			UiManager.OpenUi<LoadWindows>().Init();
			GameScene.Instance.video360Go.SetActive(value: false);
			SingletonAsMono<GlobalMrg>.Instance.videoState = VideoState.GameScene;
			UiManager.HideUi<Img360BtnWindows>();
		}
	}

	public void SetShowText(string text, string use, string time)
	{
		showMoveToText.text = text;
		useText.text = use;
		useText.SetActive(!string.IsNullOrEmpty(use));
		timeOutText.text = time;
		timeOutText.SetActive(!string.IsNullOrEmpty(time));
	}

	private void OnDestroy()
	{
		if (_audioSource != null)
		{
			SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
			_audioSource = null;
		}
		Instance = null;
	}

	public void HideAllBtn()
	{
		FrameWork.Tool.HideAllChild(norImg.transform);
		FrameWork.Tool.HideAllChild(video360.transform);
	}

	public void Init()
	{
		videoNode = SingletonAsMono<GameDataMrg>.Instance.GetCurVideo();
		UiManager.HideAllUi();
		UiManager.HideUi<Img360BtnWindows>();
		yawParent.eulerAngles = new Vector3(0f, SingletonAsMono<GameDataMrg>.Instance.Video360Rotation.y, 0f);
		pitchChild.localEulerAngles = new Vector3(Mathf.Clamp(SingletonAsMono<GameDataMrg>.Instance.Video360Rotation.x, videoNode.minY, videoNode.maxY), 0f, 0f);
		GameScene.Instance.Init();
		if (SingletonAsMono<GameDataMrg>.Instance.IsMorning)
		{
			EventManager.DispatchEvent(MessageType.Video, VideoMessageType.CheckMsgRi);
		}
		else
		{
			EventManager.DispatchEvent(MessageType.Video, VideoMessageType.CheckMsgYe);
		}
		if (SingletonAsMono<GameDataMrg>.Instance.IsShowPhoneRed())
		{
			UiManager.OpenUi<MainWindows>().Init(delegate
			{
				if (videoNode.isHasTutorial)
				{
					FrameWork.Tool.ShowTutorial("Video360Scene", videoNode.tutorialIcon);
				}
			});
			UiManager.GetUi<MainWindows>().OpenMessage();
		}
		else if (videoNode.isHasTutorial)
		{
			FrameWork.Tool.ShowTutorial("Video360Scene", videoNode.tutorialIcon);
		}
		UiManager.OpenUi<LoadWindows>().Init();
		SingletonAsMono<GameDataMrg>.Instance.GetData().is360 = true;
		SingletonAsMono<GameDataMrg>.Instance.GetData().event360Key = ((VideoGraph)videoNode.graph).xlsxEventKey;
		if (SingletonAsMono<GameDataMrg>.Instance.IsMorning && SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty("SaveData") <= 0)
		{
			SingletonAsMono<GameDataMrg>.Instance.AddRoundProperty("SaveData", 1);
			SingletonAsMono<GameDataMrg>.Instance.AutoSave(LanguageMrg.GetText("A434"));
		}
		norImg.SetActive(active: false);
		List<ButtonNode> btns = videoNode.GetAllButtonNodesOutNor();
		if (videoNode.imgIsNor)
		{
			Sprite sprite = Sprite.Create(videoNode.img360, new Rect(0f, 0f, videoNode.img360.width, videoNode.img360.height), Vector2.zero);
			UiManager.OpenUi<Img360BtnWindows>().Init(sprite, videoNode, btns);
		}
		else
		{
			video360.transform.TranFor(btns.Count, video360.transform.GetChild(0), delegate(int i, GameObject o)
			{
				o.transform.localPosition = btns[i].buttonPosition;
				o.GetComponent<VideoButton>().Init(LanguageMrg.GetText(btns[i].btnName), btns[i].buttonSize, videoNode, btns[i].GetOutVideo(), btns[i]);
			});
			drag.maxY = videoNode.maxY;
			drag.minY = videoNode.minY;
			material.SetTexture("_MainTex", videoNode.img360);
		}
		CloseBg(null);
		if (SingletonAsMono<GameDataMrg>.Instance.IsMorning)
		{
			_audioSource = SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.beiJin360Ri, loop: true);
		}
		else
		{
			_audioSource = SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.beiJin360Ye, loop: true);
		}
	}

	public void PlayVideo(VideoNode node)
	{
		SingletonAsMono<GameDataMrg>.Instance.CurEventKey = ((VideoGraph)node.graph).xlsxEventKey.ToString();
		SingletonAsMono<GameDataMrg>.Instance.CurVideoId = node.uniqueID;
		if (node.is360Video)
		{
			Init();
			return;
		}
		GameScene.Instance.gameCanvas.SetActive(value: true);
		Instance.HideAllBtn();
		GameScene.Instance.PlayVideo(node, isShowRaw: true, isCanOnlyVideoLoad: true);
	}
}
