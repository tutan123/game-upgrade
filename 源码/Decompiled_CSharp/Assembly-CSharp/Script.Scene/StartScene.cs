using System;
using System.Collections.Generic;
using FrameWork;
using FrameWork.Data;
using RenderHeads.Media.AVProVideo;
using Script.Audio;
using Script.Mrg;
using Script.UiTool;
using UnityEngine;

namespace Script.Scene;

public class StartScene : MonoBehaviour
{
	public GameObject loadGameGo;

	public MediaPlayer mediaPlayer;

	public MainVideoData startVideoData;

	private AudioSource _bgmSource;

	private void Awake()
	{
		SingletonAsMono<GlobalMrg>.Instance.videoState = VideoState.None;
		loadGameGo.SetActive(!string.IsNullOrEmpty(GameData.CurKey));
	}

	private void Start()
	{
		_bgmSource = SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.shouYeBgm, loop: true);
		PlayVideo();
		SetVolume(null);
	}

	private void PlayVideo()
	{
		VideoUnlockData videoUnlockData = startVideoData.MeiMei[UnityEngine.Random.Range(0, startVideoData.MeiMei.Count)];
		List<VideoUnlockData> list = new List<VideoUnlockData>();
		Dictionary<string, string> data = GameData.GetCurSavedData().Data;
		if (data.GetProperty("MMFavorability", "Property") > 0)
		{
			list.AddRange(startVideoData.MeiMei);
		}
		if (data.GetProperty("LBNFavorability", "Property") > 0)
		{
			list.AddRange(startVideoData.JiuBaLaoBanNian);
		}
		if (data.GetProperty("BZPFavorability", "Property") > 0)
		{
			list.AddRange(startVideoData.BaoZhuPo);
		}
		if (data.GetProperty("LQTFavorability", "Property") > 0)
		{
			list.AddRange(startVideoData.LiQinTon);
		}
		if (data.GetProperty("XFPFavorability", "Property") > 0)
		{
			list.AddRange(startVideoData.XiaoFuPo);
		}
		if (data.GetProperty("WDLYFavorability", "Property") > 0)
		{
			list.AddRange(startVideoData.GonShiLaoBan);
		}
		if (data.GetProperty("WDLYFavorability", "Property") > 0)
		{
			list.AddRange(startVideoData.GonShiLaoBan);
		}
		if (data.GetProperty("XXGFavorability", "Property") > 0)
		{
			list.AddRange(startVideoData.XiXueGui);
		}
		if (data.GetProperty("XJMFavorability", "Property") > 0)
		{
			list.AddRange(startVideoData.XiJiaoMei);
		}
		if (data.GetProperty("MSFavorability", "Property") > 0)
		{
			list.AddRange(startVideoData.MiShu);
		}
		if (data.GetProperty("PZFavorability", "Property") > 0)
		{
			list.AddRange(startVideoData.PenZai);
		}
		if (list.Count > 0)
		{
			videoUnlockData = list[UnityEngine.Random.Range(0, list.Count)];
		}
		mediaPlayer.OpenMedia(MediaPathType.RelativeToStreamingAssetsFolder, FrameWork.Tool.GetVideoPath(videoUnlockData.unlockVideoPath));
		mediaPlayer.Events.RemoveAllListeners();
		mediaPlayer.Events.AddListener(delegate(MediaPlayer arg0, MediaPlayerEvent.EventType type, ErrorCode code)
		{
			if (type == MediaPlayerEvent.EventType.FirstFrameReady)
			{
				arg0.Loop = true;
				arg0.Control.SetLooping(bLooping: true);
				mediaPlayer.Play();
				SetVolume(null);
			}
		});
	}

	public void LoadGame()
	{
		UiManager.OpenUi<LoadSaveWindows>().InitType();
	}

	public void NewGame()
	{
		GameData.CurKey = Guid.NewGuid().ToString();
		SingletonAsMono<GameDataMrg>.Instance.LoadSaveData();
		LoadMrg.LoadAsNotyMapLoad(Scenes.Game);
	}

	public void OpenSetting()
	{
		UiManager.OpenUi<SettingWindows>();
	}

	public void Quit()
	{
		UiManager.OpenUi<SureWindows>().Init(LanguageMrg.GetText("A1314"), Application.Quit);
	}

	public void OpenRecord()
	{
		UiManager.OpenUi<RecordWindows>();
	}

	public void Continue()
	{
		SingletonAsMono<GameDataMrg>.Instance.LoadSaveData();
		if (!SingletonAsMono<GameDataMrg>.Instance.GetData().is360)
		{
			LoadMrg.Load(Scenes.Map3D);
			return;
		}
		SingletonAsMono<GameDataMrg>.Instance.CurEventKey = SingletonAsMono<GameDataMrg>.Instance.GetData().event360Key;
		LoadMrg.Load(Scenes.Game);
	}

	private void OnEnable()
	{
		EventManager.AddListener(MessageType.Game, GameMessageType.ChangeVolume, SetVolume);
		EventManager.AddListener(MessageType.UiMessage, UiMessageType.Open, CheckUiOpen);
		EventManager.AddListener(MessageType.UiMessage, UiMessageType.Close, CheckUiClose);
		EventManager.AddListener(MessageType.Game, GameMessageType.LoadScene, CloseBg);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.ChangeVolume, SetVolume);
		EventManager.RemoveListener(MessageType.UiMessage, UiMessageType.Open, CheckUiOpen);
		EventManager.RemoveListener(MessageType.UiMessage, UiMessageType.Close, CheckUiClose);
		EventManager.RemoveListener(MessageType.Game, GameMessageType.LoadScene, CloseBg);
	}

	private void OnDestroy()
	{
		SingletonAsMono<AudioMrg>.Instance.Enqueue(_bgmSource);
	}

	private void CloseBg(List<object> objects)
	{
		if ((bool)mediaPlayer && mediaPlayer.Control != null)
		{
			mediaPlayer.Control.SetVolume(0f);
		}
	}

	private void SetVolume(List<object> objects)
	{
		if ((bool)mediaPlayer && mediaPlayer.Control != null)
		{
			mediaPlayer.Control.SetVolume(GameData.GetOpenAsNum("Volume"));
		}
		if (_bgmSource != null)
		{
			_bgmSource.volume = GameData.GetOpenAsNum("Volume");
		}
	}

	private void CheckUiOpen(List<object> objects)
	{
		if ((bool)mediaPlayer && mediaPlayer.Control != null)
		{
			mediaPlayer.Control.SetVolume(0f);
		}
		if (_bgmSource != null)
		{
			_bgmSource.volume = 0f;
		}
	}

	private void CheckUiClose(List<object> objects)
	{
		if (UiManager.IsCanPlay())
		{
			if ((bool)mediaPlayer && mediaPlayer.Control != null)
			{
				mediaPlayer.Control.SetVolume(GameData.GetOpenAsNum("Volume"));
			}
			if (_bgmSource != null)
			{
				_bgmSource.volume = GameData.GetOpenAsNum("Volume");
			}
		}
	}
}
