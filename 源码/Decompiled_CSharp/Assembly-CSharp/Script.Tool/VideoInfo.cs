using DG.Tweening;
using FrameWork;
using RenderHeads.Media.AVProVideo;
using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.Tool;

public class VideoInfo : MonoBehaviour
{
	public MediaPlayer videoPlayer;

	public TMP_Text infoText;

	public DisplayUGUI displayUGUI;

	private Tweener _tweener;

	private FileData _fileData;

	private void Awake()
	{
	}

	public void Init(FileData fileData)
	{
		_fileData = fileData;
		if (videoPlayer.MediaPath != null && videoPlayer.MediaPath.Path == FrameWork.Tool.GetVideoPath(fileData.videoPath))
		{
			return;
		}
		infoText.text = ((fileData.FileInfo == Xlsx_Language_Key.A1) ? "" : LanguageMrg.GetText(fileData.FileInfo));
		displayUGUI.color = Color.clear;
		_tweener?.Kill();
		videoPlayer.Events.RemoveAllListeners();
		videoPlayer.Events.AddListener(delegate(MediaPlayer arg0, MediaPlayerEvent.EventType type, ErrorCode code)
		{
			if (type == MediaPlayerEvent.EventType.FirstFrameReady)
			{
				arg0.Loop = true;
				arg0.Control.SetLooping(bLooping: true);
				_tweener = displayUGUI.DOColor(Color.white, 0.3f);
				videoPlayer.Play();
				videoPlayer.AudioVolume = 0f;
			}
		});
		videoPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, FrameWork.Tool.GetVideoPath(fileData.videoPath), autoPlay: false);
	}

	public void PlayVideo()
	{
		UiManager.OpenUi<VideoListPlayVideoWindows>().Init(_fileData);
	}
}
