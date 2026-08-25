using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using FrameWork;
using FrameWork.Data;
using RenderHeads.Media.AVProVideo;
using Script.Mrg;
using Script.Scene;
using Script.UiTool;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.Tool;

public class VideoItem : MonoBehaviour
{
	private VideoNode _videoNode;

	private VideoNode _lastVideoNode;

	public MediaPlayer videoPlayer;

	public AudioTool audioTool;

	public CanvasGroup canvasGroup;

	public bool isPlayEndFrame;

	public bool isShowButton;

	public bool isHideButton;

	public RectTransform btnGroup;

	public Slider btnProgress;

	public Image progressBar;

	public GameObject btnProgressGo;

	public RawImage rawImage;

	public RectTransform qteGroup;

	public Image mask;

	private bool _isCanCallPlay;

	private int _failCount;

	private bool _isCanEx;

	private bool _isCanExVideoEnd;

	private bool _isSeekSuc;

	private Transform _parent;

	private float _checkTime = 0.1f;

	private bool _isCanPause;

	private double _startTime;

	private Action _playCall;

	private Tweener _videoAudioVolume;

	private Dictionary<string, PropertyData> _qteProperty = new Dictionary<string, PropertyData>();

	private void Awake()
	{
		canvasGroup.alpha = 0f;
		audioTool.audioSource.volume = 0f;
		btnProgressGo.SetActiveAsCheck(active: false);
		rawImage.SetActive(active: false);
	}

	public void Seek(double time)
	{
		videoPlayer.Control.Seek(time);
	}

	public VideoNode GetNode()
	{
		return _videoNode;
	}

	public void Init(VideoNode videoNode, Transform parent, VideoNode lastNode)
	{
		_isCanPause = true;
		if (_isCanPause)
		{
			_isCanEx = false;
			_isCanExVideoEnd = false;
		}
		_lastVideoNode = lastNode;
		btnProgressGo.SetActiveAsCheck(active: false);
		_parent = parent;
		_videoNode = videoNode;
		audioTool.SetClip(FrameWork.Tool.GetAudioClipPath(videoNode.videoPath));
		videoPlayer.Events.RemoveAllListeners();
		videoPlayer.Events.AddListener(delegate(MediaPlayer arg0, MediaPlayerEvent.EventType type, ErrorCode code)
		{
			switch (type)
			{
			case MediaPlayerEvent.EventType.FinishedPlaying:
				audioTool.SetClip(null);
				VideoEnd(_videoNode);
				break;
			case MediaPlayerEvent.EventType.FirstFrameReady:
				arg0.Loop = videoNode.isLoopVideo;
				arg0.Control.SetLooping(videoNode.isLoopVideo);
				SetPlaybackRate(1f);
				if (_isCanPause)
				{
					Pause("视频首帧开始暂停");
				}
				break;
			}
		});
		PlayVideo(videoNode);
	}

	public void Init(Action playCall = null)
	{
		_playCall = playCall;
		InitValue();
		FrameWork.Tool.HideAllChild(btnGroup);
		canvasGroup.alpha = 1f;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		videoPlayer.Control.SetVolume(GameData.GetOpenAsNum("Volume"));
		audioTool.audioSource.volume = GameData.GetOpenAsNum("RoleVolume");
		if (_videoNode.isLoopVideo)
		{
			VideoNode inputValue = _videoNode.GetInputValue<VideoNode>("inputVideoNode");
			if (inputValue != null && inputValue.isHasBtn && !inputValue.isHasQte)
			{
				List<ButtonNode> allBtn = inputValue.GetAllButtonNodesOutNor();
				List<VideoNode> allVideo = inputValue.GetAllVideoNodesOutNor();
				btnGroup.TranFor(allBtn.Count, btnGroup.GetChild(0), delegate(int i, GameObject o)
				{
					o.transform.localPosition = allBtn[i].buttonPosition;
					o.GetComponent<VideoButton>().Init(LanguageMrg.GetText(allBtn[i].btnName), allBtn[i].buttonSize, _videoNode, allVideo[i], allBtn[i], isDoFade: false);
					o.GetComponent<VideoButton>().SetVideoItem(this);
				});
			}
		}
		mask.SetActive(GameData.IsOpen("AnchorMode") && _videoNode.isCheckZb);
		Play();
	}

	public void ClickBtnSwitch()
	{
		_isCanEx = false;
		FadeVideo();
	}

	private void PlayVideo(VideoNode videoNode)
	{
		videoPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, FrameWork.Tool.GetVideoPath(videoNode.videoPath), autoPlay: false);
		videoPlayer.Control.Seek(0.0);
	}

	public void Play()
	{
		if (UiManager.IsCanPlay())
		{
			videoPlayer.Play();
		}
	}

	public void Pause(string info = "")
	{
		videoPlayer.Pause();
		Debug.Log(info);
	}

	public bool IsPause()
	{
		return videoPlayer.Control.IsPaused();
	}

	public float GetPlaybackRate()
	{
		return videoPlayer.PlaybackRate;
	}

	public void SetPlaybackRate(float rate)
	{
		videoPlayer.PlaybackRate = rate;
	}

	private void Update()
	{
		if (!(_videoNode == null) && _isCanEx)
		{
			CheckPlay();
			if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.GameScene)
			{
				CheckVideoFrame();
				CheckAddValue();
				CheckEvent();
				CheckQte();
				CheckBtn();
				CheckTutorial();
				CheckTask();
				CheckAchievement();
			}
		}
	}

	private void CheckAchievement()
	{
		for (int i = 0; i < _videoNode.achievement.Count; i++)
		{
			if (!_videoNode.achievement[i].IsEx && GetCurVideoTime() >= (double)_videoNode.achievement[i].exTime)
			{
				_videoNode.achievement[i].IsEx = true;
				Xlsx_Achievement xlsx_Achievement = Xlsx_Achievement_Query.XlsxDataAsOneKey.ByKeyGetValue(_videoNode.achievement[i].ToString());
				if (xlsx_Achievement != null)
				{
					SdkMrg.UnLockAchievement(xlsx_Achievement.Api);
				}
			}
		}
	}

	private void CheckPlay()
	{
		if (_isCanCallPlay && videoPlayer.Control != null && videoPlayer.Control.IsPlaying() && GetCurVideoTime() > 0.0)
		{
			_playCall?.Invoke();
			_isCanCallPlay = false;
		}
	}

	private void CheckTutorial()
	{
		if (_videoNode.isHasTutorial && !_videoNode.isExTutorial && GetCurVideoTime() >= (double)_videoNode.tutorialCheckTime && !_videoNode.isExTutorial)
		{
			FrameWork.Tool.ShowTutorial(_videoNode.uniqueID, _videoNode.tutorialIcon);
			_videoNode.isExTutorial = true;
		}
	}

	private void InitValue()
	{
		_isCanCallPlay = true;
		_isCanPause = false;
		_failCount = 0;
		FrameWork.Tool.HideAllChild(qteGroup);
		_isCanEx = true;
		_isCanExVideoEnd = true;
		isPlayEndFrame = false;
		isShowButton = false;
		isHideButton = false;
		if (_videoNode != null)
		{
			if (_videoNode.isInitQteCount)
			{
				SingletonAsMono<GameDataMrg>.Instance.SetProperty("QteSucCount", 0, "Other");
				SingletonAsMono<GameDataMrg>.Instance.SetProperty("QteFailCount", 0, "Other");
			}
			_videoNode.isExTutorial = false;
			for (int i = 0; i < _videoNode.achievement.Count; i++)
			{
				_videoNode.achievement[i].IsEx = false;
			}
			if (_videoNode.isSetPropertyData)
			{
				for (int j = 0; j < _videoNode.setPropertyData.Count; j++)
				{
					PropertyData propertyData = _videoNode.setPropertyData[j];
					if (propertyData.PropertyType == PropertyType.Value)
					{
						SingletonAsMono<GameDataMrg>.Instance.SetProperty(propertyData.TypeName, propertyData.PropertyValue, "Value");
					}
				}
			}
			for (int k = 0; k < _videoNode.taskData.Count; k++)
			{
				_videoNode.taskData[k].isEx = false;
			}
			for (int l = 0; l < _videoNode.EventValue.Count; l++)
			{
				_videoNode.EventValue[l].isEx = false;
			}
			for (int m = 0; m < _videoNode.VideoEndEventValue.Count; m++)
			{
				_videoNode.VideoEndEventValue[m].isEx = false;
			}
			for (int n = 0; n < _videoNode.PropertyData.Count; n++)
			{
				_videoNode.PropertyData[n].isEx = false;
			}
			for (int num = 0; num < _videoNode.VideoEndPropertyData.Count; num++)
			{
				_videoNode.VideoEndPropertyData[num].isEx = false;
			}
			if (_videoNode.isInitHp)
			{
				if (_videoNode.isInitPlayerHp)
				{
					SingletonAsMono<GameDataMrg>.Instance.TmpPlayerHp = SingletonAsMono<GameDataMrg>.Instance.PlayerHp;
				}
				if (_videoNode.enemyHpIsEqPlayer)
				{
					SingletonAsMono<GameDataMrg>.Instance.EnemyHp = SingletonAsMono<GameDataMrg>.Instance.PlayerHp;
					SingletonAsMono<GameDataMrg>.Instance.EnemyHpMax = SingletonAsMono<GameDataMrg>.Instance.PlayerHp;
				}
				else
				{
					SingletonAsMono<GameDataMrg>.Instance.EnemyHp = (int)_videoNode.enemyHp;
					SingletonAsMono<GameDataMrg>.Instance.EnemyHpMax = (int)_videoNode.enemyHp;
				}
				if (_videoNode.isInitPlayerHp)
				{
					SingletonAsMono<GameDataMrg>.Instance.SetProperty("TmpJiNengDian", SingletonAsMono<GameDataMrg>.Instance.GetProperty("JiNengDian", "Property", 0L), "Property");
				}
				if (_videoNode.isInitPlayerHp)
				{
					SingletonAsMono<GameDataMrg>.Instance.SetProperty("ZiJiZhengJia", 0, "Property");
				}
				SingletonAsMono<GameDataMrg>.Instance.SetProperty("DiFanZhengJia", 0, "Property");
			}
			if (_videoNode.isHasQte)
			{
				SingletonAsMono<GameDataMrg>.Instance.SetProperty("JiNengDian", 0, "Property");
			}
			if (_videoNode.isHasWineList && _videoNode.isInitJiuLian)
			{
				SingletonAsMono<GameDataMrg>.Instance.TargetJiuLian = (int)_videoNode.targetJiuLian;
				SingletonAsMono<GameDataMrg>.Instance.TmpTargetJiuLian = 0;
				SingletonAsMono<GameDataMrg>.Instance.TmpPlayerJiuLian = 0;
			}
			for (int num2 = 0; num2 < _videoNode.qteDatas.Count; num2++)
			{
				_videoNode.qteDatas[num2].isSucEx = false;
				_videoNode.qteDatas[num2].sumProperty.Clear();
				_videoNode.qteDatas[num2].isSpawnEx = false;
			}
		}
		_qteProperty.Clear();
	}

	public void FadeVideo()
	{
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		if ((bool)audioTool)
		{
			audioTool.audioSource.volume = 0f;
		}
		if ((bool)videoPlayer)
		{
			videoPlayer.Control.SetVolume(0f);
		}
		if ((bool)this)
		{
			base.transform.SetParent(_parent);
		}
		canvasGroup.alpha = 0f;
		FrameWork.Tool.HideAllChild(qteGroup);
		Pause();
	}

	public Texture2D CopyRtToTex()
	{
		return videoPlayer.ExtractFrame(null);
	}

	public void VideoPlayerEndToNext(VideoNode nodeData)
	{
		if (!_isCanEx)
		{
			return;
		}
		FadeVideo();
		if (!nodeData.isPlayerEndPlayerNext)
		{
			return;
		}
		if (nodeData.isPlayerEndPlayerNextRandom)
		{
			List<VideoNode> allVideoNodesOutNor = nodeData.GetAllVideoNodesOutNor("nextVideoNode");
			GameScene.Instance.PlayVideo(allVideoNodesOutNor[UnityEngine.Random.Range(0, allVideoNodesOutNor.Count)]);
		}
		else if (nodeData.isPlayerEndPlayerNextSequence)
		{
			List<VideoNode> allVideoNodesOutNor2 = nodeData.GetAllVideoNodesOutNor("nextVideoNode");
			int num = SingletonAsMono<GameDataMrg>.Instance.GetProperty(nodeData.uniqueID, "Index", 0L);
			if (num >= allVideoNodesOutNor2.Count)
			{
				num = allVideoNodesOutNor2.Count - 1;
			}
			GameScene.Instance.PlayVideo(allVideoNodesOutNor2[num]);
			SingletonAsMono<GameDataMrg>.Instance.AddProperty(nodeData.uniqueID, 1L, "Index");
		}
		else
		{
			GameScene.Instance.PlayVideo(nodeData.GetAllVideoNodesOutNor("nextVideoNode")[0]);
		}
	}

	public double GetVideoTime()
	{
		return videoPlayer.Info.GetDuration();
	}

	private void CheckBtn()
	{
		if (!_videoNode.isHasBtn)
		{
			return;
		}
		double curVideoTime = GetCurVideoTime();
		if (!isShowButton && curVideoTime >= (double)_videoNode.videoBtnShowTime)
		{
			btnProgressGo.SetActiveAsCheck((double)_videoNode.videoBtnHideTime < GetVideoTime());
			isShowButton = true;
			if (!_videoNode.isLoopVideo && GetVideoTime() > (double)_videoNode.videoBtnHideTime)
			{
				btnProgressGo.SetActiveAsCheck(active: true);
			}
			List<ButtonNode> allBtn = _videoNode.GetAllButtonNodesOutNor();
			List<VideoNode> allVideo = _videoNode.GetAllVideoNodesOutNor();
			if (_videoNode.isHasQte && UiManager.IsShowUi<CombatWindows>())
			{
				Dictionary<string, VideoNode> dictionary = new Dictionary<string, VideoNode>();
				Dictionary<string, ButtonNode> dictionary2 = new Dictionary<string, ButtonNode>();
				for (int j = 0; j < allBtn.Count; j++)
				{
					dictionary.TryAdd(allBtn[j].btnType.ToString(), allVideo[j]);
					dictionary2.TryAdd(allBtn[j].btnType.ToString(), allBtn[j]);
				}
				UiManager.GetUi<CombatWindows>().Init(dictionary, dictionary2);
			}
			else
			{
				btnGroup.TranFor(allBtn.Count, btnGroup.GetChild(0), delegate(int i, GameObject o)
				{
					o.transform.localPosition = allBtn[i].buttonPosition;
					o.GetComponent<VideoButton>().Init(LanguageMrg.GetText(allBtn[i].btnName), allBtn[i].buttonSize, _videoNode, allVideo[i], allBtn[i]);
					o.GetComponent<VideoButton>().SetVideoItem(this);
				});
			}
		}
		btnProgress.value = ((float)curVideoTime - _videoNode.videoBtnShowTime) / (_videoNode.videoBtnHideTime - _videoNode.videoBtnShowTime);
		progressBar.transform.localScale = new Vector3(Mathf.Max(1f - btnProgress.value, 0f), 1f, 1f);
		if (!isHideButton && curVideoTime >= (double)_videoNode.videoBtnHideTime && !_videoNode.isLoopVideo)
		{
			RemoveBtn();
			isHideButton = true;
		}
	}

	public void RemoveBtn()
	{
		if (!isHideButton)
		{
			isHideButton = true;
			for (int i = 0; i < btnGroup.childCount; i++)
			{
				btnGroup.GetChild(i).GetComponent<VideoButton>().RemoveBtn();
			}
			btnProgressGo.SetActiveAsCheck(active: false);
		}
	}

	public void ToNextVideoNode(VideoNode nodeData)
	{
		_isCanEx = false;
		_isCanExVideoEnd = false;
		FadeVideo();
		GameScene.Instance.PlayVideo(nodeData);
	}

	private void CheckVideoFrame()
	{
		if (!videoPlayer || videoPlayer.Control == null || !videoPlayer.Control.IsPlaying() || _videoNode.isLoopVideo)
		{
			return;
		}
		double curVideoTime = GetCurVideoTime();
		double videoTime = GetVideoTime();
		if (curVideoTime >= videoTime - (double)_checkTime && !isPlayEndFrame)
		{
			isPlayEndFrame = true;
			AddPropertyData(_videoNode);
			if (!_videoNode.isToNextGroup && !_videoNode.isToMap && !_videoNode.isToGame && !_videoNode.isPlayerEndOver)
			{
				VideoPlayerEndToNext(_videoNode);
			}
		}
	}

	public void EndVideoNode()
	{
		isPlayEndFrame = true;
		List<PropertyData> propertyData = _videoNode.PropertyData;
		for (int i = 0; i < propertyData.Count; i++)
		{
			if ((propertyData[i].isRound || propertyData[i].isAlwaysEx || !SingletonAsMono<GameDataMrg>.Instance.IsUnLockKey(_videoNode.uniqueID)) && !propertyData[i].isEx)
			{
				propertyData[i].AddTypeValueAsShowTips();
			}
		}
		for (int j = 0; j < _videoNode.taskData.Count; j++)
		{
			if (!_videoNode.taskData[j].isEx)
			{
				FrameWork.Tool.SetTask(_videoNode.taskData[j]);
			}
		}
		for (int k = 0; k < _videoNode.EventValue.Count; k++)
		{
			if (!_videoNode.EventValue[k].isEx)
			{
				_videoNode.EventValue[k].isEx = true;
				EventData eventData = _videoNode.EventValue[k];
				List<object> eventMsg = EventManager.GetEventMsg();
				eventMsg.Add(eventData.eventValue);
				EventManager.DispatchEvent(MessageType.Video, eventData.eventName, eventMsg);
			}
		}
		AddPropertyData(_videoNode);
		if (_videoNode.isToNextGroup || _videoNode.isToMap || _videoNode.isToGame || _videoNode.isPlayerEndOver)
		{
			VideoEnd(_videoNode);
		}
		else
		{
			VideoPlayerEndToNext(_videoNode);
		}
	}

	public void CheckTask()
	{
		for (int i = 0; i < _videoNode.taskData.Count; i++)
		{
			if (!_videoNode.taskData[i].isEx && GetCurVideoTime() >= (double)_videoNode.taskData[i].exTime)
			{
				FrameWork.Tool.SetTask(_videoNode.taskData[i]);
				_videoNode.taskData[i].isEx = true;
			}
		}
	}

	public void AddPropertyData(VideoNode nodeData)
	{
		if (_videoNode.isSavePos)
		{
			SingletonAsMono<GameDataMrg>.Instance.MapLoc = _videoNode.pos;
		}
		AddProperty(_videoNode.VideoEndPropertyData, isCheckTime: false);
		for (int i = 0; i < nodeData.chatData.Count; i++)
		{
			if (nodeData.chatData[i] != null && !(nodeData.chatData[i].chatNode == null))
			{
				SingletonAsMono<GameDataMrg>.Instance.AddChat(nodeData.chatData[i].targetId, nodeData.chatData[i].chatNode.name, nodeData.chatData[i].isAlwaysEx);
			}
		}
		if (nodeData.VideoEndEventValue.Count > 0)
		{
			for (int j = 0; j < nodeData.VideoEndEventValue.Count; j++)
			{
				EventData eventData = nodeData.VideoEndEventValue[j];
				List<object> eventMsg = EventManager.GetEventMsg();
				eventMsg.Add(eventData.eventValue);
				EventManager.DispatchEvent(MessageType.Video, eventData.eventName, eventMsg);
			}
		}
		SingletonAsMono<GameDataMrg>.Instance.AddUnLockKey(nodeData.uniqueID);
		SingletonAsMono<GameDataMrg>.Instance.AddUnLockKey(nodeData.videoPath);
	}

	public void VideoEnd(VideoNode nodeData)
	{
		if (!_isCanExVideoEnd)
		{
			return;
		}
		_isCanExVideoEnd = false;
		if (nodeData.isToGame)
		{
			SingletonAsMono<GlobalMrg>.Instance.videoNode = nodeData;
			LoadMrg.Load(Scenes.MinGameLevelSelect);
			return;
		}
		if (nodeData.isToNextGroup)
		{
			VideoNode evenFistNode = nodeData.VideoGroupData.GetEvenFistNode();
			GameScene.Instance.PlayVideo(evenFistNode);
			return;
		}
		if (nodeData.isToMap)
		{
			LoadMrg.Load(Scenes.Map3D);
		}
		if (nodeData.isPlayerEndOver)
		{
			LoadMrg.Load(Scenes.Start);
		}
	}

	private void CheckQte()
	{
		if (!_videoNode.isHasQte)
		{
			return;
		}
		double curVideoTime = GetCurVideoTime();
		for (int i = 0; i < _videoNode.qteDatas.Count; i++)
		{
			QteData item = _videoNode.qteDatas[i];
			if (item.isSucEx || item.isSpawnEx || !(curVideoTime >= (double)item.qteStartTime))
			{
				continue;
			}
			item.isSpawnEx = true;
			GameObject obj = UnityEngine.Object.Instantiate(ABMrg.Load<GameObject>(item.qteType.ToString()), qteGroup);
			obj.transform.localPosition = item.qteLoc;
			obj.GetComponent<QteBtn>().Init(item, delegate
			{
				SingletonAsMono<GameDataMrg>.Instance.AddProperty("QteSucCount", 1L, "Other");
				SingletonAsMono<GameDataMrg>.Instance.SetProperty("QteFailCount", 0, "Other");
				EventManager.DispatchEvent(MessageType.Game, GameMessageType.QteSuc);
				for (int l = 0; l < item.sucPropertyData.Count; l++)
				{
					PropertyData propertyData2 = item.sucPropertyData[l];
					string text2 = "";
					text2 = ((propertyData2.PropertyType != PropertyType.Property) ? propertyData2.TypeName : propertyData2.propertyTypeValue.ToString());
					if (_qteProperty.ContainsKey(text2))
					{
						_qteProperty[text2].PropertyValue += item.sucPropertyData[l].PropertyValue;
					}
					else
					{
						_qteProperty[text2] = new PropertyData
						{
							PropertyType = item.sucPropertyData[l].PropertyType,
							propertyTypeValue = item.sucPropertyData[l].propertyTypeValue,
							PropertyValue = item.sucPropertyData[l].PropertyValue,
							TypeName = item.sucPropertyData[l].TypeName,
							itemType = item.sucPropertyData[l].itemType
						};
					}
				}
				item.isSucEx = true;
				if (item.isExAdd)
				{
					List<PropertyData> list3 = _qteProperty.Select((KeyValuePair<string, PropertyData> pair) => pair.Value).ToList();
					for (int m = 0; m < list3.Count; m++)
					{
						list3[m].AddTypeValueAsShowTips();
					}
					_qteProperty.Clear();
					if (SingletonAsMono<GameDataMrg>.Instance.EnemyHp <= 0)
					{
						List<VideoNode> qteNode2 = _videoNode.GetAllVideoNodesOutNor("enemyDieVideoNode");
						if (qteNode2.Count > 0)
						{
							FrameWork.Tool.HideAllChild(qteGroup);
							_isCanEx = false;
							if ((bool)CameraShakeMrg.Instance)
							{
								CameraShakeMrg.Instance.TriggerShake(0.2f, 40f, 5f);
							}
							UiManager.HideUi<CombatWindows>();
							UiManager.OpenUi<BatVicWindwos>();
							SetPlaybackRate(0.4f);
							_videoAudioVolume?.Kill();
							_videoAudioVolume = DOTween.To(() => videoPlayer.AudioVolume, delegate(float value)
							{
								videoPlayer.AudioVolume = value;
							}, 0f, 0.5f);
							audioTool.audioSource.volume = 0f;
							Timer.DelayCall(3f, delegate
							{
								Pause();
								GameScene.Instance.PlayVideo(qteNode2[0], isShowRaw: false);
							});
						}
					}
				}
				if (_videoNode.isHasQte)
				{
					List<VideoNode> list4 = new List<VideoNode>();
					List<VideoNode> allVideoNodesOutNor4 = _videoNode.GetAllVideoNodesOutNor("qteFail");
					if (allVideoNodesOutNor4.Count > 0)
					{
						list4.AddRange(allVideoNodesOutNor4);
					}
					GameScene.Instance.AddQteLoadVideo(list4, _videoNode);
				}
				if (item.isExCheck)
				{
					List<VideoNode> allVideoNodesOutNor5 = _videoNode.GetAllVideoNodesOutNor("playDieVideoNode");
					if (SingletonAsMono<GameDataMrg>.Instance.TmpPlayerHp <= 0 && allVideoNodesOutNor5.Count > 0)
					{
						GameScene.Instance.PlayVideo(allVideoNodesOutNor5[0]);
					}
					else if (_failCount > 0)
					{
						List<VideoNode> allVideoNodesOutNor6 = _videoNode.GetAllVideoNodesOutNor("qteFail");
						if (allVideoNodesOutNor6.Count > 0)
						{
							GameScene.Instance.PlayVideo(allVideoNodesOutNor6[0]);
						}
					}
				}
			}, delegate
			{
				_failCount++;
				SingletonAsMono<GameDataMrg>.Instance.AddProperty("QteFailCount", 1L, "Other");
				EventManager.DispatchEvent(MessageType.Game, GameMessageType.QteFail);
				for (int j = 0; j < item.losePropertyData.Count; j++)
				{
					PropertyData propertyData = item.losePropertyData[j];
					string text = "";
					text = ((propertyData.PropertyType != PropertyType.Property) ? propertyData.TypeName : propertyData.propertyTypeValue.ToString());
					if (_qteProperty.ContainsKey(text))
					{
						_qteProperty[text].PropertyValue += item.losePropertyData[j].PropertyValue;
					}
					else
					{
						_qteProperty[text] = new PropertyData
						{
							PropertyType = item.losePropertyData[j].PropertyType,
							propertyTypeValue = item.losePropertyData[j].propertyTypeValue,
							PropertyValue = item.losePropertyData[j].PropertyValue,
							TypeName = item.losePropertyData[j].TypeName,
							itemType = item.losePropertyData[j].itemType
						};
					}
				}
				if (item.isExAdd)
				{
					List<PropertyData> list = _qteProperty.Select((KeyValuePair<string, PropertyData> pair) => pair.Value).ToList();
					for (int k = 0; k < list.Count; k++)
					{
						list[k].AddTypeValueAsShowTips();
					}
					_qteProperty.Clear();
					if (SingletonAsMono<GameDataMrg>.Instance.EnemyHp <= 0)
					{
						List<VideoNode> qteNode = _videoNode.GetAllVideoNodesOutNor("enemyDieVideoNode");
						if (qteNode.Count > 0)
						{
							FrameWork.Tool.HideAllChild(qteGroup);
							_isCanEx = false;
							if ((bool)CameraShakeMrg.Instance)
							{
								CameraShakeMrg.Instance.TriggerShake(0.2f, 40f, 5f);
							}
							UiManager.HideUi<CombatWindows>();
							UiManager.OpenUi<BatVicWindwos>();
							SetPlaybackRate(0.4f);
							_videoAudioVolume = DOTween.To(() => videoPlayer.AudioVolume, delegate(float value)
							{
								videoPlayer.AudioVolume = value;
							}, 0f, 0.5f);
							videoPlayer.AudioVolume = 0f;
							audioTool.audioSource.volume = 0f;
							Timer.DelayCall(3f, delegate
							{
								Pause();
								GameScene.Instance.PlayVideo(qteNode[0], isShowRaw: false);
							});
						}
					}
				}
				if (_videoNode.isHasQte)
				{
					List<VideoNode> list2 = new List<VideoNode>();
					List<VideoNode> allVideoNodesOutNor = _videoNode.GetAllVideoNodesOutNor("qteFail");
					if (allVideoNodesOutNor.Count > 0)
					{
						list2.AddRange(allVideoNodesOutNor);
					}
					GameScene.Instance.AddQteLoadVideo(list2, _videoNode);
				}
				if (item.isExCheck)
				{
					List<VideoNode> allVideoNodesOutNor2 = _videoNode.GetAllVideoNodesOutNor("playDieVideoNode");
					if (SingletonAsMono<GameDataMrg>.Instance.TmpPlayerHp <= 0 && allVideoNodesOutNor2.Count > 0)
					{
						GameScene.Instance.PlayVideo(allVideoNodesOutNor2[0]);
					}
					else if (_failCount > 0)
					{
						List<VideoNode> allVideoNodesOutNor3 = _videoNode.GetAllVideoNodesOutNor("qteFail");
						if (allVideoNodesOutNor3.Count > 0)
						{
							GameScene.Instance.PlayVideo(allVideoNodesOutNor3[0]);
						}
					}
				}
			});
		}
	}

	private void CheckEvent()
	{
		if (_videoNode.EventValue.Count <= 0)
		{
			return;
		}
		double curVideoTime = GetCurVideoTime();
		for (int i = 0; i < _videoNode.EventValue.Count; i++)
		{
			if (!_videoNode.EventValue[i].isEx && curVideoTime >= (double)_videoNode.EventValue[i].evenTime)
			{
				_videoNode.EventValue[i].isEx = true;
				EventData eventData = _videoNode.EventValue[i];
				List<object> eventMsg = EventManager.GetEventMsg();
				eventMsg.Add(eventData.eventValue);
				EventManager.DispatchEvent(MessageType.Video, eventData.eventName, eventMsg);
			}
		}
	}

	private void CheckAddValue()
	{
		if (!(_videoNode == null))
		{
			AddProperty(_videoNode.PropertyData);
		}
	}

	public void AddProperty(List<PropertyData> propertyDatas, bool isCheckTime = true)
	{
		for (int i = 0; i < propertyDatas.Count; i++)
		{
			if (!propertyDatas[i].isRound && !propertyDatas[i].isAlwaysEx && SingletonAsMono<GameDataMrg>.Instance.IsUnLockKey(_videoNode.uniqueID))
			{
				continue;
			}
			PropertyData propertyData = propertyDatas[i];
			if (isCheckTime)
			{
				if (!propertyData.isEx && GetCurVideoTime() >= (double)propertyData.addPropertyTime)
				{
					propertyData.isEx = true;
					propertyData.AddTypeValueAsShowTips();
				}
			}
			else
			{
				propertyData.AddTypeValueAsShowTips();
			}
		}
	}

	private void PlayVideoAsGameScene(VideoNode node)
	{
		GameScene.Instance.PlayVideo(node);
	}

	public double GetCurVideoTime()
	{
		if (videoPlayer.Control == null)
		{
			return 9999.0;
		}
		return videoPlayer.Control.GetCurrentTime();
	}
}
