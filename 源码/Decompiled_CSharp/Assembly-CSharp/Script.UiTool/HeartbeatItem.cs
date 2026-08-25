using System.Collections.Generic;
using FrameWork;
using Script.Mrg;
using UnityEngine;

namespace Script.UiTool;

public class HeartbeatItem : MonoBehaviour
{
	public List<HeartbeatData> heartbeatData;

	public Animator animator;

	public float changeTime = 50f;

	private HeartbeatData _currentHeartbeatData;

	private List<HeartbeatData> _heartbeatData = new List<HeartbeatData>();

	private FolderDataGroup _folderDataGroup;

	private int _index;

	private int _time;

	private void Awake()
	{
		_folderDataGroup = ABMrg.Load<FolderDataGroup>("VideoData");
	}

	private void OnEnable()
	{
		_heartbeatData.Clear();
		for (int i = 0; i < heartbeatData.Count; i++)
		{
			_heartbeatData.Add(heartbeatData[i]);
		}
		if (_heartbeatData.Count > 0)
		{
			animator.gameObject.SetActiveAsCheck(active: true);
			StartPlay();
		}
		else
		{
			animator.gameObject.SetActiveAsCheck(active: false);
		}
		EventManager.AddListener(MessageType.Game, GameMessageType.ChangeHeartbeat, SwitchVideo);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.ChangeHeartbeat, SwitchVideo);
	}

	private void SwitchVideo(List<object> objects)
	{
		_index++;
		SwitchAnim();
	}

	private void StartPlay()
	{
		_time = 0;
		_index = Random.Range(0, 100);
		SwitchAnim();
	}

	private void SwitchAnim()
	{
		HeartbeatData heartbeatData = (_currentHeartbeatData = _heartbeatData[_index % _heartbeatData.Count]);
		animator.enabled = false;
		animator.runtimeAnimatorController = heartbeatData.animatorController;
		animator.enabled = true;
		animator.Rebind();
		animator.Update(0f);
	}

	public void OnClick()
	{
		switch (_currentHeartbeatData.heartbeatType)
		{
		case HeartbeatType.GonShiLaoBan:
			UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.GonShiLaoBan.FolderName), _folderDataGroup.GonShiLaoBan.FolderList);
			break;
		case HeartbeatType.BaoZhuPo:
			UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.BaoZhuPo.FolderName), _folderDataGroup.BaoZhuPo.FolderList);
			break;
		case HeartbeatType.XiXueGui:
			UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.XiXueGui.FolderName), _folderDataGroup.XiXueGui.FolderList);
			break;
		case HeartbeatType.MeiMei:
			UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.MeiMei.FolderName), _folderDataGroup.MeiMei.FolderList);
			break;
		case HeartbeatType.XiaoQi:
			UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.XiaoFuPo.FolderName), _folderDataGroup.XiaoFuPo.FolderList);
			break;
		case HeartbeatType.LiQinTon:
			UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.LiQinTon.FolderName), _folderDataGroup.LiQinTon.FolderList);
			break;
		case HeartbeatType.LingXiaoYu:
			UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.LinXiaoYu.FolderName), _folderDataGroup.LinXiaoYu.FolderList);
			break;
		case HeartbeatType.ZhiWuNian:
			UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.ZhiWuNian.FolderName), _folderDataGroup.ZhiWuNian.FolderList);
			break;
		case HeartbeatType.MiShu:
			UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.MiShu.FolderName), _folderDataGroup.MiShu.FolderList);
			break;
		case HeartbeatType.JiuBaLaoBan:
			UiManager.OpenUi<VideoListWindows>().Init(LanguageMrg.GetText(_folderDataGroup.JiuBaLaoBan.FolderName), _folderDataGroup.JiuBaLaoBan.FolderList);
			break;
		}
	}
}
