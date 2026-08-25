using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FrameWork;
using Script.Audio;
using Script.Mrg;
using Script.Tool;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.PostProcessing;
using Xlsx;

namespace Script.Scene;

public class Map3DScene : MonoBehaviour
{
	public NavMeshAgent player;

	private int[] _weekEnd = new int[4] { 4, 5, 11, 12 };

	public static Map3DScene Instance;

	public Material l1;

	public Material l2;

	public Camera mapCamera;

	public Transform pointGroup;

	public PostProcessProfile day;

	public PostProcessProfile night;

	public PostProcessVolume playerVolume;

	private AudioSource _audioSource;

	private void Awake()
	{
		Instance = this;
	}

	private void Start()
	{
		SingletonAsMono<GameDataMrg>.Instance.GetData().is360 = false;
		player.gameObject.SetActiveAsCheck(active: false);
		SingletonAsMono<GlobalMrg>.Instance.videoState = VideoState.Map;
		if (SingletonAsMono<GameDataMrg>.Instance.IsMorning)
		{
			l1.DisableKeyword("_EMISSION");
			l2.DisableKeyword("_EMISSION");
			playerVolume.profile = day;
			RenderSettings.ambientIntensity = 0.48f;
			RenderSettings.reflectionIntensity = 1f;
		}
		else
		{
			l1.EnableKeyword("_EMISSION");
			l2.EnableKeyword("_EMISSION");
			playerVolume.profile = night;
			RenderSettings.ambientIntensity = 0.2f;
			RenderSettings.reflectionIntensity = 0.113f;
		}
		InitTask();
	}

	private void InitTask()
	{
		List<Xlsx_Task> list = (from task in Xlsx_Task_Query.data
			where SingletonAsMono<GameDataMrg>.Instance.GetTaskState(task.Key) == TaskType.Open && SingletonAsMono<GameDataMrg>.Instance.CurRound <= task.Round
			where !FrameWork.Tool.IsSucProperty(task.PropertyType, task.PropertyTypeValue)
			where task.IsMain == 1
			select task).ToList();
		Mathf.Min(1, list.Count);
		list.Sort((Xlsx_Task task, Xlsx_Task xlsxTask) => -task.Range.CompareTo(xlsxTask.Range));
	}

	public void CloseBg(List<object> objects)
	{
		if (_audioSource != null)
		{
			SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
			_audioSource = null;
		}
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

	public bool IsWeekEnd()
	{
		return _weekEnd.Contains(SingletonAsMono<GameDataMrg>.Instance.CurRound);
	}

	public void ShowActor()
	{
		player.gameObject.SetActiveAsCheck(active: true);
		if (NavMesh.SamplePosition(SingletonAsMono<GameDataMrg>.Instance.MapLoc, out var hit, 50f, -1))
		{
			player.transform.position = hit.position;
			player.Warp(hit.position);
		}
		else
		{
			player.transform.position = SingletonAsMono<GameDataMrg>.Instance.MapLoc;
			player.Warp(SingletonAsMono<GameDataMrg>.Instance.MapLoc);
			MyLog.LogError("目标点附近没有有效的导航网格！");
		}
		mapCamera.Render();
		mapCamera.enabled = false;
		CloseBg(null);
		if (SingletonAsMono<GameDataMrg>.Instance.IsMorning)
		{
			_audioSource = SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.mapBenJiBgmRi, loop: true);
		}
		else
		{
			_audioSource = SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.mapBenJiBgmYe, loop: true);
		}
	}

	public IEnumerator ShowPoint()
	{
		yield return null;
		for (int i = 0; i < pointGroup.childCount; i++)
		{
			try
			{
				pointGroup.GetChild(i).GetComponent<Map3DTriggerr>().CheckShow();
			}
			catch (Exception ex)
			{
				Debug.LogWarning(ex.Message);
			}
			yield return null;
		}
		yield return null;
		try
		{
			if (SingletonAsMono<GameDataMrg>.Instance.IsMorning)
			{
				UiManager.GetUi<MapRiWindows>()?.MapToolView.InitUnlockPointText();
			}
			else
			{
				UiManager.GetUi<MapYeWindows>()?.MapToolView.InitUnlockPointText();
			}
		}
		catch (Exception ex2)
		{
			Debug.LogError(ex2.Message);
		}
	}

	private void OnEnable()
	{
		EventManager.AddListener(MessageType.Game, GameMessageType.LoadScene, CloseBg);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.LoadScene, CloseBg);
	}

	public void MoveTo(Vector3 pos)
	{
		if (player == null || !player.enabled || !player.gameObject.activeInHierarchy)
		{
			MyLog.Log("被return了");
		}
		if (NavMesh.SamplePosition(pos, out var hit, 50f, -1))
		{
			player.SetDestination(hit.position);
		}
		else
		{
			MyLog.LogError("目标点附近没有有效的导航网格！");
		}
	}

	public void SetYe()
	{
		UiManager.OpenUi<SureWindows>().Init(LanguageMrg.GetText("A1596"), delegate
		{
			SingletonAsMono<GameDataMrg>.Instance.MapLoc = player.transform.position;
			SingletonAsMono<GameDataMrg>.Instance.IsMorning = false;
			EventManager.DispatchEvent(MessageType.Video, VideoMessageType.UpdateTime);
			LoadMrg.Load(Scenes.Map3D);
		});
	}
}
