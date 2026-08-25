using DG.Tweening;
using FrameWork.Data;
using RenderHeads.Media.AVProVideo;
using Script;
using Script.Mrg;
using Script.Tool;
using Script.UiTool;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace FrameWork;

[ActorInfo("", "VideoListPlayVideoWindows")]
[UiMode(Mode.Normal, true)]
public class VideoListPlayVideoWindows : UiActor
{
	private Tweener _tweener;

	public RectTransform RectTransformMediaPlayer;

	public AddScripts AddScriptsMediaPlayer;

	public CanvasRenderer CanvasRendererMediaPlayer;

	public AudioSource AudioSourceMediaPlayer;

	public MediaPlayer MediaPlayerMediaPlayer;

	public DisplayUGUI DisplayUGUIMediaPlayer;

	public RectTransform RectTransformAVProVideo;

	public CanvasRenderer CanvasRendererAVProVideo;

	public AddScripts AddScriptsAVProVideo;

	public RectTransform RectTransformSrtGroup;

	public AddScripts AddScriptsSrtGroup;

	public RectTransform RectTransformAudio;

	public AudioSource AudioSourceAudio;

	public AddScripts AddScriptsAudio;

	public AudioTool AudioToolAudio;

	public RectTransform RectTransformSpeedUp;

	public CanvasRenderer CanvasRendererSpeedUp;

	public Image ImageSpeedUp;

	public BT BTSpeedUp;

	public AddScripts AddScriptsSpeedUp;

	public RectTransform RectTransformPause;

	public CanvasRenderer CanvasRendererPause;

	public Image ImagePause;

	public BT BTPause;

	public PlayIconSet PlayIconSetPause;

	public AddScripts AddScriptsPause;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public override void Awake()
	{
		base.Awake();
		RectTransformMediaPlayer = GetGameObject().transform.Find("View/MediaPlayer/").GetComponent<RectTransform>();
		AddScriptsMediaPlayer = GetGameObject().transform.Find("View/MediaPlayer/").GetComponent<AddScripts>();
		CanvasRendererMediaPlayer = GetGameObject().transform.Find("View/MediaPlayer/").GetComponent<CanvasRenderer>();
		AudioSourceMediaPlayer = GetGameObject().transform.Find("View/MediaPlayer/").GetComponent<AudioSource>();
		MediaPlayerMediaPlayer = GetGameObject().transform.Find("View/MediaPlayer/").GetComponent<MediaPlayer>();
		DisplayUGUIMediaPlayer = GetGameObject().transform.Find("View/MediaPlayer/").GetComponent<DisplayUGUI>();
		RectTransformAVProVideo = GetGameObject().transform.Find("View/AVPro Video/").GetComponent<RectTransform>();
		CanvasRendererAVProVideo = GetGameObject().transform.Find("View/AVPro Video/").GetComponent<CanvasRenderer>();
		AddScriptsAVProVideo = GetGameObject().transform.Find("View/AVPro Video/").GetComponent<AddScripts>();
		RectTransformSrtGroup = GetGameObject().transform.Find("View/AVPro Video/SrtGroup/").GetComponent<RectTransform>();
		AddScriptsSrtGroup = GetGameObject().transform.Find("View/AVPro Video/SrtGroup/").GetComponent<AddScripts>();
		RectTransformAudio = GetGameObject().transform.Find("View/AVPro Video/Audio/").GetComponent<RectTransform>();
		AudioSourceAudio = GetGameObject().transform.Find("View/AVPro Video/Audio/").GetComponent<AudioSource>();
		AddScriptsAudio = GetGameObject().transform.Find("View/AVPro Video/Audio/").GetComponent<AddScripts>();
		AudioToolAudio = GetGameObject().transform.Find("View/AVPro Video/Audio/").GetComponent<AudioTool>();
		RectTransformSpeedUp = GetGameObject().transform.Find("View/BtnGroup/Group/SpeedUp/").GetComponent<RectTransform>();
		CanvasRendererSpeedUp = GetGameObject().transform.Find("View/BtnGroup/Group/SpeedUp/").GetComponent<CanvasRenderer>();
		ImageSpeedUp = GetGameObject().transform.Find("View/BtnGroup/Group/SpeedUp/").GetComponent<Image>();
		BTSpeedUp = GetGameObject().transform.Find("View/BtnGroup/Group/SpeedUp/").GetComponent<BT>();
		AddScriptsSpeedUp = GetGameObject().transform.Find("View/BtnGroup/Group/SpeedUp/").GetComponent<AddScripts>();
		RectTransformPause = GetGameObject().transform.Find("View/BtnGroup/Group/Pause/").GetComponent<RectTransform>();
		CanvasRendererPause = GetGameObject().transform.Find("View/BtnGroup/Group/Pause/").GetComponent<CanvasRenderer>();
		ImagePause = GetGameObject().transform.Find("View/BtnGroup/Group/Pause/").GetComponent<Image>();
		BTPause = GetGameObject().transform.Find("View/BtnGroup/Group/Pause/").GetComponent<BT>();
		PlayIconSetPause = GetGameObject().transform.Find("View/BtnGroup/Group/Pause/").GetComponent<PlayIconSet>();
		AddScriptsPause = GetGameObject().transform.Find("View/BtnGroup/Group/Pause/").GetComponent<AddScripts>();
		RectTransformClose = GetGameObject().transform.Find("View/BtnGroup/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("View/BtnGroup/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("View/BtnGroup/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("View/BtnGroup/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("View/BtnGroup/Close/").GetComponent<AddScripts>();
	}

	public VideoListPlayVideoWindows(Transform trans)
		: base(trans)
	{
	}

	public VideoListPlayVideoWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(CloseUi);
		BTPause.onClick.AddListener(Pause);
		BTSpeedUp.onClick.AddListener(SetSpeed);
	}

	public void Init(FileData fileData)
	{
		DisplayUGUIMediaPlayer.color = Color.clear;
		_tweener?.Kill();
		MediaPlayerMediaPlayer.Events.RemoveAllListeners();
		MediaPlayerMediaPlayer.Events.AddListener(delegate(MediaPlayer arg0, MediaPlayerEvent.EventType type, ErrorCode code)
		{
			if (type == MediaPlayerEvent.EventType.FirstFrameReady)
			{
				arg0.Loop = true;
				arg0.Control.SetLooping(bLooping: true);
				_tweener = DisplayUGUIMediaPlayer.DOColor(Color.white, 0.1f);
				MediaPlayerMediaPlayer.Play();
				MediaPlayerMediaPlayer.AudioVolume = GameData.GetOpenAsNum("Volume");
			}
		});
		MediaPlayerMediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, Tool.GetVideoPath(fileData.videoPath), autoPlay: false);
		TextAsset textAssets = null;
		switch (GameData.Language)
		{
		case Xlsx_Language_Type.Chinese:
			textAssets = fileData.chineseTextAsset;
			break;
		case Xlsx_Language_Type.English:
			textAssets = fileData.englishTextAsset;
			break;
		}
		if (textAssets != null)
		{
			Tool.HideAllChild(RectTransformSrtGroup.transform);
			if (GameData.IsOpen("Subtitle"))
			{
				RectTransformSrtGroup.transform.TranFor(1, RectTransformSrtGroup.transform.GetChild(0), delegate(int i, GameObject o)
				{
					o.GetComponent<SrtText>().videoPlayer = MediaPlayerMediaPlayer;
					o.GetComponent<SrtText>().ParseSRT(textAssets);
				});
			}
		}
		else
		{
			Tool.HideAllChild(RectTransformSrtGroup.transform);
		}
		AudioToolAudio.SetClip(Tool.GetAudioClipPath(fileData.videoPath));
		AudioToolAudio.audioSource.volume = GameData.GetOpenAsNum("RoleVolume");
		MediaPlayerMediaPlayer.Play();
	}

	public void Init(VideoNode videoNode)
	{
		DisplayUGUIMediaPlayer.color = Color.clear;
		_tweener?.Kill();
		MediaPlayerMediaPlayer.Events.RemoveAllListeners();
		MediaPlayerMediaPlayer.Events.AddListener(delegate(MediaPlayer arg0, MediaPlayerEvent.EventType type, ErrorCode code)
		{
			if (type == MediaPlayerEvent.EventType.FirstFrameReady)
			{
				arg0.Loop = true;
				arg0.Control.SetLooping(bLooping: true);
				_tweener = DisplayUGUIMediaPlayer.DOColor(Color.white, 0.1f);
				MediaPlayerMediaPlayer.Play();
				MediaPlayerMediaPlayer.AudioVolume = GameData.GetOpenAsNum("Volume");
			}
		});
		MediaPlayerMediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, Tool.GetVideoPath(videoNode.videoPath), autoPlay: false);
		TextAsset textAssets = null;
		switch (GameData.Language)
		{
		case Xlsx_Language_Type.Chinese:
			textAssets = videoNode.subtitleChinese;
			break;
		case Xlsx_Language_Type.English:
			textAssets = videoNode.subtitleEnglish;
			break;
		}
		if (textAssets != null)
		{
			Tool.HideAllChild(RectTransformSrtGroup.transform);
			if (GameData.IsOpen("Subtitle"))
			{
				RectTransformSrtGroup.transform.TranFor(1, RectTransformSrtGroup.transform.GetChild(0), delegate(int i, GameObject o)
				{
					o.GetComponent<SrtText>().videoPlayer = MediaPlayerMediaPlayer;
					o.GetComponent<SrtText>().ParseSRT(textAssets);
				});
			}
		}
		else
		{
			Tool.HideAllChild(RectTransformSrtGroup.transform);
		}
		AudioToolAudio.SetClip(Tool.GetAudioClipPath(videoNode.videoPath));
		AudioToolAudio.audioSource.volume = GameData.GetOpenAsNum("RoleVolume");
		MediaPlayerMediaPlayer.Play();
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
	}

	private new void Pause()
	{
		if (MediaPlayerMediaPlayer.Control.IsPlaying())
		{
			MediaPlayerMediaPlayer.Pause();
		}
		else
		{
			MediaPlayerMediaPlayer.Play();
		}
	}

	private void SetSpeed()
	{
		if (MediaPlayerMediaPlayer.PlaybackRate > 1f)
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1584"));
			MediaPlayerMediaPlayer.PlaybackRate = 1f;
		}
		else
		{
			UiManager.ShowTips(string.Format(LanguageMrg.GetText("A1583"), 2));
			MediaPlayerMediaPlayer.PlaybackRate = 2f;
		}
	}
}
