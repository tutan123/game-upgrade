using System;
using System.Collections.Generic;
using System.Globalization;
using DG.Tweening;
using FrameWork;
using LitJson;
using Script.Audio;
using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.MiNiGame;

public class MiNiGameMrg : MonoBehaviour
{
	public int score;

	public int getCount;

	public TMP_Text curScore;

	public TMP_Text targetScore;

	public TMP_Text timeText;

	public int curTime;

	public Transform itemParent;

	private Xlsx_MiNiGameLevel _xlsxMiNiGameLevel;

	public Hook hook;

	public SpriteRenderer shiWa;

	public float shiWaAlpha;

	public Transform minPos;

	public Transform maxPos;

	public int spawnBqbCount;

	public bool isOver;

	public static MiNiGameMrg Instance;

	public GameObject showAddGo;

	public TMP_Text addText;

	private AudioSource _audioSource;

	private TimeData _timeData;

	private Tweener _lowerShadowTweener;

	private TimeData _showAddTime;

	private void Awake()
	{
		showAddGo.SetActiveAsCheck(active: false);
		spawnBqbCount = 0;
		Instance = this;
		isOver = false;
		_xlsxMiNiGameLevel = Xlsx_MiNiGameLevel_Query.XlsxDataAsOneKey.ByKeyGetValue(SingletonAsMono<GameDataMrg>.Instance.xlsxMiNiGameLevelKey);
	}

	private void Start()
	{
		if (_xlsxMiNiGameLevel.Key == "A6" || _xlsxMiNiGameLevel.Key == "A9")
		{
			_audioSource = SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.xyxBgm69, loop: true);
		}
		else
		{
			_audioSource = SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.xyxBgm, loop: true);
		}
		if (_xlsxMiNiGameLevel.Target == -1)
		{
			targetScore.text = LanguageMrg.GetText("A5493");
		}
		else
		{
			targetScore.text = string.Format(LanguageMrg.GetText("A958"), _xlsxMiNiGameLevel.Target);
		}
		UpdateScore();
		LoadLevel();
		Xlsx_MiNiGameItem xlsx_MiNiGameItem = Xlsx_MiNiGameItem_Query.XlsxDataAsOneKey.ByKeyGetValue("DaXiao");
		float num = (float)SingletonAsMono<GameDataMrg>.Instance.GetProperty("DaXiao", "Nor", 0L) * xlsx_MiNiGameItem.Add;
		curTime = (int)((float)_xlsxMiNiGameLevel.Time * (1f + num));
		OutTime();
		UiManager.OpenUi<LevelTargetWindows>().Init(delegate
		{
			_timeData = Timer.IntervalCall(1f, OutTime);
		});
	}

	private void OutTime()
	{
		if (UiManager.IsCanPlay())
		{
			curTime--;
			timeText.text = curTime.ToString();
			if (curTime <= 0)
			{
				GameOver(score >= _xlsxMiNiGameLevel.Target);
			}
		}
	}

	public void ClearRandomItem(Item item, float range)
	{
		for (int i = 0; i < itemParent.childCount; i++)
		{
			Transform child = itemParent.GetChild(i);
			if (child.gameObject != item.gameObject && Vector3.Distance(child.transform.position, item.transform.position) < range)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
	}

	private void GameOver(bool isSuc = true)
	{
		Debug.Log("小游戏结束");
		Timer.DestroyTimer(_timeData);
		_timeData = null;
		isOver = true;
		if (isSuc)
		{
			if (score >= _xlsxMiNiGameLevel.Target)
			{
				if (!SingletonAsMono<GameDataMrg>.Instance.IsPassMiniGame(_xlsxMiNiGameLevel.Key))
				{
					string[] fistReward = _xlsxMiNiGameLevel.FistReward;
					long[] fistRewardCount = _xlsxMiNiGameLevel.FistRewardCount;
					for (int i = 0; i < fistReward.Length; i++)
					{
						SingletonAsMono<GameDataMrg>.Instance.AddProperty(fistReward[i], fistRewardCount[i], "Property");
					}
				}
				SingletonAsMono<GameDataMrg>.Instance.PassMiniGame(_xlsxMiNiGameLevel.Key);
				string[] passReward = _xlsxMiNiGameLevel.PassReward;
				SingletonAsMono<GameDataMrg>.Instance.AddProperty(passReward[0], int.Parse(passReward[1]), "Property");
			}
		}
		else
		{
			string[] loseReward = _xlsxMiNiGameLevel.LoseReward;
			SingletonAsMono<GameDataMrg>.Instance.AddProperty(loseReward[0], int.Parse(loseReward[1]), "Property");
		}
		VideoNode videoNode = SingletonAsMono<GlobalMrg>.Instance.videoNode;
		try
		{
			Debug.Log($"小游戏视频-------------视频路径:{videoNode.videoPath}------------------视频ID:{videoNode.uniqueID}------是否跳转下一个事件:{videoNode.isToNextGroup}-----是否跳转下一个视频:{videoNode.isPlayerEndPlayerNext}-----是否跳转小地图:{videoNode.isToMap}");
		}
		catch (Exception ex)
		{
			Debug.Log("游戏所引用的资源丢失：" + ex.Message);
		}
		string text = "";
		text = ((!isSuc) ? (string.Format(LanguageMrg.GetText("A5488"), SingletonAsMono<GameDataMrg>.Instance.GetProperty("KPI", "Property", 0L)) + "(+" + _xlsxMiNiGameLevel.LoseReward[1] + ")") : (string.Format(LanguageMrg.GetText("A5488"), SingletonAsMono<GameDataMrg>.Instance.GetProperty("KPI", "Property", 0L)) + "(+" + _xlsxMiNiGameLevel.PassReward[1] + ")"));
		if (videoNode != null && videoNode.isToNextGroup)
		{
			SingletonAsMono<GameDataMrg>.Instance.CurEventKey = ((VideoGraph)videoNode.graph).xlsxEventKey.ToString();
			SingletonAsMono<GameDataMrg>.Instance.CurVideoId = videoNode.uniqueID;
			if (isSuc)
			{
				UiManager.OpenUi<LevelSucceedWindows>().Init(text, delegate
				{
					LoadMrg.Load(Scenes.Game);
				});
			}
			else
			{
				UiManager.OpenUi<LevelFailWindows>().Init(text, delegate
				{
					LoadMrg.Load(Scenes.Game);
				});
			}
		}
		else if (videoNode != null && videoNode.isPlayerEndPlayerNext)
		{
			VideoNode videoNode2 = null;
			if (videoNode.isPlayerEndPlayerNextRandom)
			{
				List<VideoNode> allVideoNodesOutNor = videoNode.GetAllVideoNodesOutNor("nextVideoNode");
				videoNode2 = allVideoNodesOutNor[UnityEngine.Random.Range(0, allVideoNodesOutNor.Count)];
			}
			else if (videoNode.isPlayerEndPlayerNextSequence)
			{
				List<VideoNode> allVideoNodesOutNor2 = videoNode.GetAllVideoNodesOutNor("nextVideoNode");
				VideoNode videoNode3 = videoNode;
				int num = SingletonAsMono<GameDataMrg>.Instance.GetProperty(videoNode3.uniqueID, "Index", 0L);
				if (num >= allVideoNodesOutNor2.Count)
				{
					num = allVideoNodesOutNor2.Count - 1;
				}
				videoNode2 = allVideoNodesOutNor2[num];
				SingletonAsMono<GameDataMrg>.Instance.AddProperty(videoNode3.uniqueID, 1L, "Index");
			}
			else
			{
				videoNode2 = videoNode.GetAllVideoNodesOutNor("nextVideoNode")[0];
			}
			SingletonAsMono<GameDataMrg>.Instance.CurEventKey = ((VideoGraph)videoNode2.graph).xlsxEventKey.ToString();
			SingletonAsMono<GameDataMrg>.Instance.CurVideoId = videoNode2.uniqueID;
			if (videoNode2.isToMap)
			{
				if (isSuc)
				{
					UiManager.OpenUi<LevelSucceedWindows>().Init(text, delegate
					{
						LoadMrg.Load(Scenes.Map3D);
					});
				}
				else
				{
					UiManager.OpenUi<LevelFailWindows>().Init(text, delegate
					{
						LoadMrg.Load(Scenes.Map3D);
					});
				}
			}
			else if (isSuc)
			{
				UiManager.OpenUi<LevelSucceedWindows>().Init(text, delegate
				{
					LoadMrg.Load(Scenes.Game);
				});
			}
			else
			{
				UiManager.OpenUi<LevelFailWindows>().Init(text, delegate
				{
					LoadMrg.Load(Scenes.Game);
				});
			}
		}
		else if (isSuc)
		{
			UiManager.OpenUi<LevelSucceedWindows>().Init(text, delegate
			{
				LoadMrg.Load(Scenes.Map3D);
			});
		}
		else
		{
			UiManager.OpenUi<LevelFailWindows>().Init(text, delegate
			{
				LoadMrg.Load(Scenes.Map3D);
			});
		}
	}

	public void LowerShiWa(float a = 0.2f)
	{
		shiWaAlpha -= a;
		_lowerShadowTweener?.Kill();
		_lowerShadowTweener = shiWa.DOColor(new Color(1f, 1f, 1f, shiWaAlpha), 0.2f);
		SpawnBiaoQinBao();
	}

	public void SpawnBiaoQinBao()
	{
		Vector2 vector = new Vector2(UnityEngine.Random.Range(minPos.position.x, maxPos.position.x), UnityEngine.Random.Range(minPos.position.y, maxPos.position.y));
		GameObject obj = UnityEngine.Object.Instantiate(ABMrg.Load<GameObject>("BiaoQinBao"), itemParent);
		obj.transform.position = vector;
		obj.transform.localScale = Vector3.one;
		spawnBqbCount++;
	}

	public void SpawnCaiDanBao()
	{
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty("CaiDanBao", "Nor", 0L);
		if (property > 0)
		{
			for (int i = 0; i < property; i++)
			{
				Vector2 vector = new Vector2(UnityEngine.Random.Range(minPos.position.x, maxPos.position.x), UnityEngine.Random.Range(minPos.position.y, maxPos.position.y));
				GameObject obj = UnityEngine.Object.Instantiate(ABMrg.Load<GameObject>("CaiDanBao"), itemParent);
				obj.transform.position = vector;
				obj.transform.localScale = Vector3.one;
			}
			SingletonAsMono<GameDataMrg>.Instance.SetProperty("CaiDanBao", 0);
		}
	}

	public void LoadLevel()
	{
		if (_xlsxMiNiGameLevel.IsHasBqb == 1)
		{
			SpawnBiaoQinBao();
		}
		SpawnCaiDanBao();
		shiWaAlpha = 1f;
		List<MiNiGameLevelData> list = JsonMapper.ToObject<List<MiNiGameLevelData>>(ABMrg.Load<TextAsset>(_xlsxMiNiGameLevel.Level).text);
		for (int i = 0; i < list.Count; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate(ABMrg.Load<GameObject>(list[i].prefabName), itemParent);
			float x = float.Parse(list[i].x, CultureInfo.InvariantCulture);
			float y = float.Parse(list[i].y, CultureInfo.InvariantCulture);
			obj.transform.localPosition = new Vector3(x, y, 0f);
			obj.name = list[i].prefabName;
		}
	}

	public void AddScore(Item item)
	{
		getCount++;
		float num = 1f;
		if (item.scoreValue > 0 && SingletonAsMono<GameDataMrg>.Instance.GetProperty("ErWaiJianLi", "Nor", 0L) > 0)
		{
			Xlsx_MiNiGameItem xlsx_MiNiGameItem = Xlsx_MiNiGameItem_Query.XlsxDataAsOneKey.ByKeyGetValue("ErWaiJianLi");
			int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty("ErWaiJianLi", "Nor", 0L);
			num += xlsx_MiNiGameItem.Add * (float)property;
		}
		int num2 = (int)((float)item.scoreValue * num);
		score += num2;
		Timer.DestroyTimer(_showAddTime);
		if (score != 0)
		{
			showAddGo.SetActiveAsCheck(active: false);
			showAddGo.SetActive(value: true);
			addText.text = ((num2 > 0) ? ("+" + num2) : (num2.ToString() ?? ""));
			SingletonAsMono<AudioMrg>.Instance.Play((num2 >= 0) ? AudioDataClip.AudioData.miniGameJiaF : AudioDataClip.AudioData.miniGameJianF);
			_showAddTime = Timer.DelayCall(0.8f, delegate
			{
				showAddGo.SetActive(value: false);
			});
		}
		UpdateScore();
		if (score >= _xlsxMiNiGameLevel.Target && _xlsxMiNiGameLevel.Target != -1)
		{
			GameOver();
		}
	}

	public bool CheckPos(Vector3 pos)
	{
		if (pos.x < minPos.position.x || pos.x > maxPos.position.x)
		{
			return true;
		}
		return false;
	}

	public void CheckGameOver()
	{
		if (itemParent.childCount <= 0 && _xlsxMiNiGameLevel.Target != -1)
		{
			GameOver(score >= _xlsxMiNiGameLevel.Target);
		}
		int num = 0;
		for (int i = 0; i < itemParent.childCount; i++)
		{
			if (itemParent.GetChild(i).TryGetComponent<Item>(out var component) && component.scoreValue > 0)
			{
				num += component.scoreValue;
			}
		}
		if (num <= 0 && _xlsxMiNiGameLevel.Target != -1)
		{
			GameOver(score >= _xlsxMiNiGameLevel.Target);
		}
	}

	private void UpdateScore()
	{
		curScore.text = string.Format(LanguageMrg.GetText("A957"), $"<color=#EBFF00>{score}</color>");
	}

	private PropertyTypeValue GetRandomProperty()
	{
		List<PropertyTypeValue> list = new List<PropertyTypeValue>
		{
			PropertyTypeValue.Charm,
			PropertyTypeValue.Stamina,
			PropertyTypeValue.Wisdom,
			PropertyTypeValue.Morality
		};
		return list[UnityEngine.Random.Range(0, list.Count)];
	}

	private void OnDestroy()
	{
		SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
		if (_timeData != null)
		{
			Timer.DestroyTimer(_timeData);
		}
	}
}
