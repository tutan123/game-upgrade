using Script.Tool;
using Script.UiTool;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "HeartbeatWindows")]
public class HeartbeatWindows : UiActor
{
	public RectTransform RectTransformVideoPlayer;

	public VideoPlayer VideoPlayerVideoPlayer;

	public CanvasRenderer CanvasRendererVideoPlayer;

	public RawImage RawImageVideoPlayer;

	public HeartbeatTool HeartbeatToolVideoPlayer;

	public AddScripts AddScriptsVideoPlayer;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformSwitchVideo;

	public CanvasRenderer CanvasRendererSwitchVideo;

	public Image ImageSwitchVideo;

	public BT BTSwitchVideo;

	public AddScripts AddScriptsSwitchVideo;

	public override void Awake()
	{
		base.Awake();
		RectTransformVideoPlayer = GetGameObject().transform.Find("Bg/Video Player/").GetComponent<RectTransform>();
		VideoPlayerVideoPlayer = GetGameObject().transform.Find("Bg/Video Player/").GetComponent<VideoPlayer>();
		CanvasRendererVideoPlayer = GetGameObject().transform.Find("Bg/Video Player/").GetComponent<CanvasRenderer>();
		RawImageVideoPlayer = GetGameObject().transform.Find("Bg/Video Player/").GetComponent<RawImage>();
		HeartbeatToolVideoPlayer = GetGameObject().transform.Find("Bg/Video Player/").GetComponent<HeartbeatTool>();
		AddScriptsVideoPlayer = GetGameObject().transform.Find("Bg/Video Player/").GetComponent<AddScripts>();
		RectTransformClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<AddScripts>();
		RectTransformSwitchVideo = GetGameObject().transform.Find("Bg/SwitchVideo/").GetComponent<RectTransform>();
		CanvasRendererSwitchVideo = GetGameObject().transform.Find("Bg/SwitchVideo/").GetComponent<CanvasRenderer>();
		ImageSwitchVideo = GetGameObject().transform.Find("Bg/SwitchVideo/").GetComponent<Image>();
		BTSwitchVideo = GetGameObject().transform.Find("Bg/SwitchVideo/").GetComponent<BT>();
		AddScriptsSwitchVideo = GetGameObject().transform.Find("Bg/SwitchVideo/").GetComponent<AddScripts>();
	}

	public HeartbeatWindows(Transform trans)
		: base(trans)
	{
	}

	public HeartbeatWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(CloseUi);
		BTSwitchVideo.onClick.AddListener(SwitchVideo);
	}

	private void SwitchVideo()
	{
		HeartbeatToolVideoPlayer.PlayVideo();
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.ChangeHeartbeat);
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
	}

	public override void CloseUi()
	{
		UiManager.RemoveUi(GetIndex());
	}
}
