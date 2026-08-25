using System.Collections.Generic;
using FrameWork;
using FrameWork.ChatTool;
using Script.UiTool;
using UnityEngine;
using XNode;
using Xlsx;

[NodeWidth(300)]
[CreateNodeMenu("Video/VideoNode")]
public class VideoNode : BaseNode
{
	public string tips;

	[Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
	public ButtonNode buttonNode;

	[Input(ShowBackingValue.Unconnected, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode inputVideoNode;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode outVideoNode;

	public List<AchievementData> achievement;

	public TextAsset subtitleChinese;

	public TextAsset subtitleEnglish;

	public string videoPath;

	[HideInInspector]
	public string videName;

	public bool isNowToNext;

	public bool isToNextVideoToLoopStart;

	public bool isLoopVideo;

	public bool isHasBtn;

	public float videoBtnShowTime;

	public float videoBtnHideTime;

	public bool isToNextGroup;

	public string videoGraphName;

	public bool isSavePos;

	public Vector3 pos;

	public bool is360Video;

	public float maxY;

	public float minY;

	public bool imgIsNor;

	public Texture2D img360;

	public bool isPlayerEndOver;

	public bool isPlayerEndPlayerNext;

	public bool isNextCheckShowBtnText;

	public bool isPlayerEndPlayerNextRandom;

	public bool isPlayerEndPlayerNextSequence;

	[HideInInspector]
	public int sequenceIndex;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode nextVideoNode;

	public List<PropertyData> PropertyData = new List<PropertyData>();

	public bool isClickBtnPlayAudio;

	public bool isClickBtnPlayVideo;

	public AudioClip clickAudio;

	public bool isSucVideo;

	public bool isToMap;

	public bool isToGame;

	public bool isCheckZb;

	public AudioClip bgmWinClip;

	public bool isLoopBgm = true;

	public List<EventData> EventValue = new List<EventData>();

	public List<EventData> VideoEndEventValue = new List<EventData>();

	public List<PropertyData> VideoEndPropertyData = new List<PropertyData>();

	public bool isSetPropertyData;

	public List<PropertyData> setPropertyData = new List<PropertyData>();

	public bool isHasWineList;

	public bool isInitJiuLian;

	public float targetJiuLian;

	public bool isHasTutorial;

	public Sprite[] tutorialIcon;

	public float tutorialCheckTime;

	[HideInInspector]
	public bool isExTutorial;

	public bool isHasQteBq;

	public VideoUnlockData sfVideo;

	public VideoUnlockData ptVideo;

	public VideoUnlockData nsVideo;

	public bool isHasQte;

	public bool isInitHp;

	public bool isInitPlayerHp = true;

	public bool isInitQteCount;

	public Xlsx_Language_Key bossName;

	public bool enemyHpIsEqPlayer;

	public float enemyHp;

	public List<QteData> qteDatas = new List<QteData>();

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode qteFail;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode playDieVideoNode;

	[Output(ShowBackingValue.Never, ConnectionType.Multiple, TypeConstraint.None, false)]
	public VideoNode enemyDieVideoNode;

	public List<TaskData> taskData;

	public List<ChatData> chatData;

	public VideoGraph VideoGroupData => ABMrg.Load<VideoGraph>(videoGraphName);

	public void OpenBtnPosSetWindowsAsQte()
	{
	}

	public void OpenBtnPosSetWindows()
	{
		GetOutputPort("outVideoNode").GetAllButtonNodes(isEditor: true);
	}

	private void VideoGroupChange()
	{
	}

	public void VideoChange()
	{
	}

	protected override void Init()
	{
		base.Init();
	}

	public override object GetValue(NodePort port)
	{
		switch (port.fieldName)
		{
		case "Video":
		case "outVideoNode":
		case "nextVideoNode":
			return this;
		default:
			return null;
		}
	}
}
