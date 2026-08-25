using FrameWork.Data;
using RenderHeads.Media.AVProVideo;
using Script.Tool;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "PlayVideoWindows")]
public class PlayVideoWindows : UiActor
{
	public Transform TransformMediaPlayer;

	public AddScripts AddScriptsMediaPlayer;

	public MediaPlayer MediaPlayerMediaPlayer;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public override void Awake()
	{
		base.Awake();
		TransformMediaPlayer = GetGameObject().transform.Find("View/MediaPlayer/").GetComponent<Transform>();
		AddScriptsMediaPlayer = GetGameObject().transform.Find("View/MediaPlayer/").GetComponent<AddScripts>();
		MediaPlayerMediaPlayer = GetGameObject().transform.Find("View/MediaPlayer/").GetComponent<MediaPlayer>();
		RectTransformClose = GetGameObject().transform.Find("View/AVPro Video/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("View/AVPro Video/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("View/AVPro Video/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("View/AVPro Video/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("View/AVPro Video/Close/").GetComponent<AddScripts>();
	}

	public PlayVideoWindows(Transform trans)
		: base(trans)
	{
	}

	public PlayVideoWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(CloseUi);
	}

	public void Init(string videoPath)
	{
		MediaPlayerMediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, Tool.GetVideoPath(videoPath));
		MediaPlayerMediaPlayer.Events.AddListener(delegate(MediaPlayer arg0, MediaPlayerEvent.EventType type, ErrorCode code)
		{
			if (type == MediaPlayerEvent.EventType.FirstFrameReady)
			{
				arg0.Loop = true;
				arg0.Control.SetLooping(bLooping: true);
				arg0.Control.SetVolume(GameData.GetOpenAsNum("Volume"));
			}
		});
	}
}
