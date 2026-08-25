using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FrameWork;
using FrameWork.Data;
using Script.Mrg;
using Script.Tool;
using Script.UiTool;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Xlsx;

namespace Script.Scene;

public class GameScene : MonoBehaviour
{
	public PlayIconSet playIconSet;

	public VideoItem videoItem;

	public RectTransform loadRect1;

	public RectTransform loadRect2;

	public RectTransform loadRect3;

	public RectTransform loadRect4;

	public RectTransform playMedia;

	public GameObject srtRect;

	public RawImage rawImage;

	public TMP_Text tmpText;

	public VideoNode lastVideoNode;

	public float videoSpeed = 1f;

	public Transform videoButtonParent;

	public static GameScene Instance;

	public GameObject gameCanvas;

	public GameObject video360Go;

	public UnityAction FirstFrameReady;

	private Dictionary<VideoNode, VideoItem> _dicMediaPlayer = new Dictionary<VideoNode, VideoItem>();

	private bool _isSkip;

	private TimeData _timeData;

	private void Awake()
	{
		Instance = this;
		tmpText.SetActive(active: false);
		rawImage.SetActive(active: false);
		SingletonAsMono<GlobalMrg>.Instance.videoState = VideoState.GameScene;
		ShowGameScene(show: true);
	}

	private void Start()
	{
		PlayVideo();
		LoadQteGo();
	}

	public void Init()
	{
		lastVideoNode = null;
		videoItem = null;
	}

	private void LoadQteGo()
	{
		ABMrg.Load<GameObject>("CondensationGo");
		ABMrg.Load<GameObject>("KeyGo");
		ABMrg.Load<GameObject>("NoneGo");
		ABMrg.Load<GameObject>("SlideGo");
		ABMrg.Load<GameObject>("ClickGo");
	}

	public void ShowGameScene(bool show)
	{
		gameCanvas.SetActiveAsCheck(show);
		video360Go.SetActiveAsCheck(!show);
	}

	private void OnPlayer()
	{
		FirstFrameReady?.Invoke();
		if (rawImage.gameObject.activeSelf)
		{
			rawImage.SetActive(active: false);
		}
	}

	public void Skip()
	{
		if (!_isSkip && (bool)videoItem && !videoItem.IsPause() && !videoItem.GetNode().isLoopVideo && !videoItem.GetNode().isHasQte)
		{
			if (!SingletonAsMono<GameDataMrg>.Instance.IsUnLockGlobalKey(videoItem.GetNode().uniqueID))
			{
				UiManager.ShowTips(LanguageMrg.GetText("A6362"));
				return;
			}
			if (videoItem.GetNode().isHasBtn && !videoItem.isShowButton)
			{
				videoItem.Seek(videoItem.GetNode().videoBtnShowTime);
				return;
			}
			_isSkip = true;
			videoItem.Seek(videoItem.GetVideoTime() - 0.15000000596046448);
		}
	}

	public void SpeedUp()
	{
		if (!SingletonAsMono<GameDataMrg>.Instance.IsUnLockGlobalKey(videoItem.GetNode().uniqueID))
		{
			UiManager.ShowTips(LanguageMrg.GetText("A6362"));
		}
		else if ((bool)videoItem)
		{
			if (UiManager.IsShowUi<CombatWindows>())
			{
				UiManager.ShowTips(LanguageMrg.GetText("A1312"));
			}
			else if (videoItem.GetPlaybackRate() > 1f)
			{
				UiManager.ShowTips(LanguageMrg.GetText("A1584"));
				videoSpeed = 1f;
				videoItem.SetPlaybackRate(1f);
			}
			else
			{
				videoSpeed = 2f;
				UiManager.ShowTips(string.Format(LanguageMrg.GetText("A1583"), videoSpeed));
				videoItem.SetPlaybackRate(2f);
			}
		}
	}

	public void Pause()
	{
		if ((bool)videoItem && !UiManager.IsShowUi<CombatWindows>() && !UiManager.IsShowUi<Combat2Windows>() && !videoItem.GetNode().isHasQte)
		{
			if (!videoItem.IsPause())
			{
				videoItem.Pause("按键控制");
			}
			else
			{
				videoItem.Play();
			}
		}
	}

	public void PauseEsc()
	{
		if ((bool)videoItem && !videoItem.IsPause())
		{
			videoItem.Pause("菜单暂停");
		}
	}

	public void PlayEsc()
	{
		if ((bool)videoItem && videoItem.IsPause())
		{
			videoItem.Play();
		}
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	public void RemoveButton()
	{
		if (videoItem.isHideButton)
		{
			return;
		}
		videoItem.isHideButton = true;
		for (int i = 0; i < videoButtonParent.childCount; i++)
		{
			if (videoButtonParent.GetChild(i).gameObject.activeSelf)
			{
				videoButtonParent.GetChild(i).GetComponent<VideoButton>().RemoveBtn();
			}
		}
	}

	public double GetCurVideoTime()
	{
		return videoItem.GetCurVideoTime();
	}

	private void Update()
	{
		if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.GameScene && (bool)videoItem && videoItem.GetNode() != null)
		{
			CheckInput();
		}
	}

	private void CheckInput()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Pause();
		}
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Skip();
		}
	}

	public void PlayText(string text, float showTime)
	{
		Timer.DestroyTimer(_timeData);
		_timeData = null;
		tmpText.SetActive(active: true);
		tmpText.text = text;
		_timeData = Timer.DelayCall(showTime, delegate
		{
			tmpText.SetActive(active: false);
			_timeData = null;
		});
	}

	public void AddQteLoadVideo(List<VideoNode> videoNodes, VideoNode lastNode)
	{
		videoNodes = videoNodes.Where((VideoNode node) => !string.IsNullOrEmpty(node.videoPath) && File.Exists(FrameWork.Tool.GetVideoPath(node.videoPath))).ToList();
		loadRect2.TranForAdd1(videoNodes.Count, loadRect2.GetChild(0), delegate(int i, GameObject o)
		{
			_dicMediaPlayer[videoNodes[i]] = o.GetComponent<VideoItem>();
			o.GetComponent<VideoItem>().Init(videoNodes[i], loadRect2, lastNode);
		});
	}

	public void Add360LoadVideo(List<VideoNode> videoNodes, VideoNode lastNode)
	{
		videoNodes = videoNodes.Where((VideoNode node) => !string.IsNullOrEmpty(node.videoPath) && File.Exists(FrameWork.Tool.GetVideoPath(node.videoPath))).ToList();
		loadRect4.TranForAdd1(videoNodes.Count, loadRect2.GetChild(0), delegate(int i, GameObject o)
		{
			_dicMediaPlayer[videoNodes[i]] = o.GetComponent<VideoItem>();
			o.GetComponent<VideoItem>().Init(videoNodes[i], loadRect4, lastNode);
		});
	}

	public void PlayVideo(VideoNode node, bool isShowRaw = true, bool isCanOnlyVideoLoad = false)
	{
		Debug.Log("播放视频-------------视频路径:" + node.videoPath + "------------------视频ID:" + node.uniqueID);
		if ((bool)videoItem)
		{
			videoItem.ClickBtnSwitch();
		}
		if (!node.isToNextGroup)
		{
			SingletonAsMono<GameDataMrg>.Instance.CurEventKey = ((VideoGraph)node.graph).xlsxEventKey.ToString();
			SingletonAsMono<GameDataMrg>.Instance.CurVideoId = node.uniqueID;
		}
		if (node.isNowToNext || node.isToNextGroup)
		{
			lastVideoNode = node;
			VideoEndAddAndToOther(node);
			return;
		}
		if (node.is360Video)
		{
			SingletonAsMono<GlobalMrg>.Instance.videoState = VideoState.Video360Scene;
			ShowGameScene(show: false);
			Video360Scene.Instance.Init();
			Pause();
			return;
		}
		bool flag = true;
		List<VideoNode> videoNodes = new List<VideoNode>();
		if (node.isHasBtn)
		{
			List<VideoNode> list = node.GetAllVideoNodesOutNor().Select(FrameWork.Tool.GetNextVideoNode).ToList();
			for (int j = 0; j < list.Count; j++)
			{
				if (!videoNodes.Contains(list[j]))
				{
					videoNodes.Add(list[j]);
				}
			}
		}
		if (node.isPlayerEndPlayerNext)
		{
			try
			{
				IEnumerable<VideoNode> collection = node.GetAllVideoNodesOutNor("nextVideoNode").Select(FrameWork.Tool.GetNextVideoNode);
				videoNodes.AddRange(collection);
			}
			catch (Exception ex)
			{
				MyLog.Log("预加载错误:" + ex.Message);
			}
		}
		bool flag2 = false;
		if ((bool)lastVideoNode && lastVideoNode.isToNextVideoToLoopStart)
		{
			flag2 = true;
		}
		if (((bool)videoItem && isShowRaw) || ((bool)videoItem && node.isLoopVideo))
		{
			rawImage.texture = videoItem.CopyRtToTex();
			rawImage.SetActive(active: true);
		}
		if ((bool)videoItem)
		{
			videoItem.FadeVideo();
		}
		if (flag)
		{
			if (!_dicMediaPlayer.ContainsKey(node))
			{
				loadRect1.TranForAdd1(1, loadRect1.GetChild(0), delegate(int i, GameObject o)
				{
					o.GetComponent<VideoItem>().Init(node, loadRect1, lastVideoNode);
					_dicMediaPlayer[node] = o.GetComponent<VideoItem>();
				});
			}
			videoItem = _dicMediaPlayer[node];
			videoItem.transform.SetParent(playMedia);
			videoItem.transform.SetAsLastSibling();
			playIconSet.mediaPlayer = videoItem.videoPlayer;
			videoItem.Init(OnPlayer);
			_isSkip = false;
			if (flag2)
			{
				videoItem.Seek(node.videoBtnShowTime);
			}
			lastVideoNode = node;
		}
		ClearDicVideo();
		videoNodes = videoNodes.Where((VideoNode videoNode) => !string.IsNullOrEmpty(videoNode.videoPath) && File.Exists(FrameWork.Tool.GetVideoPath(videoNode.videoPath))).ToList();
		if (videoNodes.Count > 0)
		{
			loadRect3.TranForAdd1(videoNodes.Count, loadRect3.GetChild(0), delegate(int i, GameObject o)
			{
				if (!string.IsNullOrEmpty(videoNodes[i].videoPath))
				{
					try
					{
						o.GetComponent<VideoItem>().Init(videoNodes[i], loadRect3, node);
						_dicMediaPlayer[videoNodes[i]] = o.GetComponent<VideoItem>();
					}
					catch (Exception ex2)
					{
						MyLog.LogError(ex2.Message);
					}
				}
			});
		}
		for (int k = 0; k < node.PropertyData.Count; k++)
		{
			node.PropertyData[k].isEx = false;
		}
		TextAsset textAssets = null;
		switch (GameData.Language)
		{
		case Xlsx_Language_Type.Chinese:
			textAssets = node.subtitleChinese;
			break;
		case Xlsx_Language_Type.English:
			textAssets = node.subtitleEnglish;
			break;
		}
		if (textAssets != null)
		{
			FrameWork.Tool.HideAllChild(srtRect.transform);
			if (GameData.IsOpen("Subtitle"))
			{
				srtRect.transform.TranFor(1, srtRect.transform.GetChild(0), delegate(int i, GameObject o)
				{
					o.GetComponent<SrtText>().videoPlayer = videoItem.videoPlayer;
					o.GetComponent<SrtText>().ParseSRT(textAssets);
				});
			}
		}
		else
		{
			FrameWork.Tool.HideAllChild(srtRect.transform);
		}
		videoItem.Play();
	}

	public void ClearDicVideo()
	{
		_dicMediaPlayer.Clear();
	}

	public void VideoEndAddAndToOther(VideoNode nodeData)
	{
		AddProperty(nodeData.VideoEndPropertyData, nodeData, isCheckTime: false);
		for (int i = 0; i < nodeData.chatData.Count; i++)
		{
			if (nodeData.chatData[i].chatNode != null)
			{
				SingletonAsMono<GameDataMrg>.Instance.AddChat(nodeData.chatData[i].targetId, nodeData.chatData[i].chatNode.name, nodeData.chatData[i].isAlwaysEx);
			}
			else
			{
				Debug.LogError($"检测连线 有添加一个空的消息 会导致报错 这边拦截了 给添加短信的对象是:{nodeData.chatData[i].targetId}");
			}
		}
		if (nodeData.isSetPropertyData)
		{
			for (int j = 0; j < nodeData.setPropertyData.Count; j++)
			{
				PropertyData propertyData = nodeData.setPropertyData[j];
				if (propertyData.PropertyType == PropertyType.Value)
				{
					SingletonAsMono<GameDataMrg>.Instance.SetProperty(propertyData.TypeName, propertyData.PropertyValue, "Value");
				}
			}
		}
		for (int k = 0; k < nodeData.achievement.Count; k++)
		{
			SdkMrg.UnLockAchievement(nodeData.achievement[k].ToString());
		}
		if (nodeData.VideoEndEventValue.Count > 0)
		{
			for (int l = 0; l < nodeData.VideoEndEventValue.Count; l++)
			{
				EventData eventData = nodeData.VideoEndEventValue[l];
				List<object> eventMsg = EventManager.GetEventMsg();
				eventMsg.Add(eventData.eventValue);
				EventManager.DispatchEvent(MessageType.Video, eventData.eventName, eventMsg);
			}
		}
		SingletonAsMono<GameDataMrg>.Instance.AddUnLockKey(nodeData.uniqueID);
		SingletonAsMono<GameDataMrg>.Instance.AddUnLockKey(nodeData.videoPath);
		if (nodeData.isToGame)
		{
			SingletonAsMono<GlobalMrg>.Instance.videoNode = nodeData;
			LoadMrg.Load(Scenes.MinGameLevelSelect);
			return;
		}
		if (nodeData.isToNextGroup)
		{
			VideoNode evenFistNode = nodeData.VideoGroupData.GetEvenFistNode();
			Instance.PlayVideo(evenFistNode);
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
		if (nodeData.isPlayerEndPlayerNext)
		{
			if (nodeData.isPlayerEndPlayerNextRandom)
			{
				List<VideoNode> allVideoNodesOutNor = nodeData.GetAllVideoNodesOutNor("nextVideoNode");
				Instance.PlayVideo(allVideoNodesOutNor[UnityEngine.Random.Range(0, allVideoNodesOutNor.Count)]);
			}
			else if (nodeData.isPlayerEndPlayerNextSequence)
			{
				List<VideoNode> allVideoNodesOutNor2 = nodeData.GetAllVideoNodesOutNor("nextVideoNode");
				int num = SingletonAsMono<GameDataMrg>.Instance.GetProperty(nodeData.uniqueID, "Index", 0L);
				if (num >= allVideoNodesOutNor2.Count)
				{
					num = allVideoNodesOutNor2.Count - 1;
				}
				Instance.PlayVideo(allVideoNodesOutNor2[num]);
				SingletonAsMono<GameDataMrg>.Instance.AddProperty(nodeData.uniqueID, 1L, "Index");
			}
			else
			{
				Instance.PlayVideo(nodeData.GetAllVideoNodesOutNor("nextVideoNode")[0]);
			}
		}
		else
		{
			if (!nodeData.isHasQte)
			{
				return;
			}
			bool flag = true;
			for (int m = 0; m < nodeData.qteDatas.Count; m++)
			{
				if (!nodeData.qteDatas[m].isSucEx)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				Instance.PlayVideo(nodeData.GetAllVideoNodesOutQueSuc()[0]);
			}
			else
			{
				Instance.PlayVideo(nodeData.GetAllVideoNodesOutQueLose()[0]);
			}
		}
	}

	public void AddProperty(List<PropertyData> propertyDatas, VideoNode nodeData, bool isCheckTime = true)
	{
		for (int i = 0; i < propertyDatas.Count; i++)
		{
			if (!propertyDatas[i].isRound && !propertyDatas[i].isAlwaysEx && SingletonAsMono<GameDataMrg>.Instance.IsUnLockKey(nodeData.uniqueID))
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

	public void PlayVideo()
	{
		string curEventKey = SingletonAsMono<GameDataMrg>.Instance.CurEventKey;
		VideoNode node = ABMrg.Load<VideoGraph>(Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(curEventKey).VideoAsset).GetEvenFistNode();
		if (!string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.CurVideoId))
		{
			node = SingletonAsMono<GameDataMrg>.Instance.GetCurVideo();
		}
		PlayVideo(node, isShowRaw: true, isCanOnlyVideoLoad: true);
	}

	private void OnEnable()
	{
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenBox, OpenBox);
		EventManager.AddListener(MessageType.Game, GameMessageType.LoadScene, CloseBg);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Video, VideoMessageType.OpenBox, OpenBox);
		EventManager.RemoveListener(MessageType.Game, GameMessageType.LoadScene, CloseBg);
	}

	private void CloseBg(List<object> objects)
	{
		if ((bool)videoItem)
		{
			videoItem.Pause();
		}
	}

	private void OpenBox(List<object> objects)
	{
		string key = "798";
		if (objects != null && objects.Count > 0)
		{
			key = objects[0].ToString();
		}
		List<VideoNode> videoNodes = videoItem.GetNode().GetAllVideoNodesOutNor();
		UiManager.OpenUi<InputWindows>().Init(LanguageMrg.GetText("A1593"), delegate(string s)
		{
			if (s == key)
			{
				for (int i = 0; i < videoNodes.Count; i++)
				{
					if (videoNodes[i].isSucVideo)
					{
						PlayVideo(videoNodes[i]);
					}
				}
			}
			else
			{
				UiManager.ShowTips(LanguageMrg.GetText("A5300"));
				for (int j = 0; j < videoNodes.Count; j++)
				{
					if (!videoNodes[j].isSucVideo)
					{
						PlayVideo(videoNodes[j]);
					}
				}
			}
		}, isCanClose: false);
	}
}
