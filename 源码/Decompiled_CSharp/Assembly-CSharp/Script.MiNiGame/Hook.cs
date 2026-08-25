using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork;
using Script.Audio;
using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.MiNiGame;

public class Hook : MonoBehaviour
{
	public enum State
	{
		Rotate,
		Shoot,
		Retract
	}

	public Transform target;

	public State currentState;

	[Header("层级引用")]
	public Transform ropeRoot;

	public Transform hookPivot;

	[Header("旋转设置")]
	public float rotateSpeed = 60f;

	public float maxRotation = 70f;

	private float currentAngle;

	private int rotateDirection = 1;

	[Header("移动设置")]
	public float norSpeed = 10f;

	public float shootSpeed = 10f;

	public float maxDistance = 13f;

	private float currentRetractSpeed;

	private Transform caughtItem;

	private Vector3 pivotLocalPos;

	public Transform firePoint;

	public Transform startPoint;

	public float offset;

	public SpriteRenderer line;

	public Transform sheTou;

	public float norDaXiao = 1f;

	public float daXiao;

	public float norJianLi = 1f;

	public float jianLi;

	public TMP_Text shuDuText;

	public TMP_Text daXiaoText;

	public TMP_Text erWaiJianLiText;

	private float _speedAdd;

	private float _daXiaoAdd;

	public GameObject fuZhuXian;

	public GameObject zhaDan;

	public TMP_Text zhaDanText;

	public GameObject daliGo;

	public GameObject shuanBeiXinXin;

	public GameObject fuZhuCao;

	public GameObject zhaDanFx;

	public GameObject tntFx;

	private Dictionary<string, int> _caiDanBaoDic = new Dictionary<string, int>
	{
		{ "Stamina", 1 },
		{ "Wisdom", 1 },
		{ "Charm", 1 },
		{ "Money", 60 }
	};

	private TimeData _timeDataZhaDanFx;

	private TimeData _timeDataTntFx;

	private void Start()
	{
		zhaDanFx.SetActiveAsCheck(active: false);
		tntFx.SetActiveAsCheck(active: false);
		Xlsx_MiNiGameItem xlsx_MiNiGameItem = Xlsx_MiNiGameItem_Query.XlsxDataAsOneKey.ByKeyGetValue("ShuDu");
		_speedAdd = (float)SingletonAsMono<GameDataMrg>.Instance.GetProperty("ShuDu", "Nor", 0L) * xlsx_MiNiGameItem.Add;
		fuZhuXian.SetActiveAsCheck(SingletonAsMono<GameDataMrg>.Instance.GetProperty("FuZhuCao", "Nor", 0L) > 0);
		Xlsx_MiNiGameItem xlsx_MiNiGameItem2 = Xlsx_MiNiGameItem_Query.XlsxDataAsOneKey.ByKeyGetValue("DaXiao");
		_daXiaoAdd = (float)SingletonAsMono<GameDataMrg>.Instance.GetProperty("DaXiao", "Nor", 0L) * xlsx_MiNiGameItem2.Add;
		Xlsx_MiNiGameItem xlsx_MiNiGameItem3 = Xlsx_MiNiGameItem_Query.XlsxDataAsOneKey.ByKeyGetValue("DaXiao");
		jianLi = (float)SingletonAsMono<GameDataMrg>.Instance.GetProperty("ErWaiJianLi", "Nor", 0L) * xlsx_MiNiGameItem3.Add;
		zhaDan.SetActiveAsCheck(SingletonAsMono<GameDataMrg>.Instance.GetProperty("ZhaDan", "Nor", 0L) > 0);
		daliGo.SetActiveAsCheck(SingletonAsMono<GameDataMrg>.Instance.GetProperty("DaLiYaoShui", "Nor", 0L) > 0);
		shuanBeiXinXin.SetActiveAsCheck(active: false);
		fuZhuCao.SetActiveAsCheck(SingletonAsMono<GameDataMrg>.Instance.GetProperty("FuZhuCao", "Nor", 0L) > 0);
		currentRetractSpeed = shootSpeed;
		pivotLocalPos = hookPivot.localPosition;
	}

	private void Update()
	{
		if (!UiManager.IsCanPlay())
		{
			return;
		}
		switch (currentState)
		{
		case State.Rotate:
			HandleRotation();
			if (Input.GetMouseButtonDown(0) && !MiNiGameMrg.Instance.isOver)
			{
				SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.xYxZhuaQu);
				currentState = State.Shoot;
			}
			break;
		case State.Shoot:
			HandleShoot();
			break;
		case State.Retract:
			HandleRetract();
			break;
		}
		DrawRope();
		UpdateShuXin();
	}

	public void UpdateShuXin()
	{
		zhaDanText.text = SingletonAsMono<GameDataMrg>.Instance.GetProperty("ZhaDan", "Nor", 0L).ToString() ?? "";
		daXiao = norDaXiao * (1f + _daXiaoAdd);
		shootSpeed = norSpeed * (1f + _speedAdd);
		shuDuText.text = string.Format(LanguageMrg.GetText("A1279"), (int)(shootSpeed / norSpeed * 100f));
		daXiaoText.text = string.Format(LanguageMrg.GetText("A1280"), daXiao * 100f);
		erWaiJianLiText.text = string.Format(LanguageMrg.GetText("A1281"), jianLi * 100f);
	}

	private void HandleRotation()
	{
		currentAngle += (float)rotateDirection * rotateSpeed * Time.deltaTime;
		if (currentAngle >= maxRotation)
		{
			currentAngle = maxRotation;
			rotateDirection = -1;
		}
		else if (currentAngle <= 0f - maxRotation)
		{
			currentAngle = 0f - maxRotation;
			rotateDirection = 1;
		}
		ropeRoot.rotation = Quaternion.Euler(0f, 0f, currentAngle);
	}

	private void HandleShoot()
	{
		firePoint.Translate(Vector3.up * shootSpeed * Time.deltaTime);
		if (Vector2.Distance(firePoint.position, FireNorLoc()) > maxDistance)
		{
			StartRetract(null);
		}
	}

	private void HandleRetract()
	{
		firePoint.position = Vector3.MoveTowards(firePoint.position, FireNorLoc(), currentRetractSpeed * Time.deltaTime);
		if (caughtItem != null)
		{
			caughtItem.position = target.position;
		}
		CheckZhaDan();
		if (Vector3.Distance(firePoint.position, FireNorLoc()) < 0.05f)
		{
			CompleteRetract();
		}
	}

	private void CompleteRetract()
	{
		if (caughtItem != null)
		{
			Item component = caughtItem.GetComponent<Item>();
			if (component.itemType == ItemType.BiaoQinBao)
			{
				MiNiGameMrg.Instance.LowerShiWa();
				EventManager.DispatchEvent(MessageType.Game, GameMessageType.GetXiaoLian);
			}
			if (component.itemType == ItemType.CaiDanBao)
			{
				List<string> list = _caiDanBaoDic.Keys.ToList();
				string text = list[UnityEngine.Random.Range(0, list.Count)];
				new PropertyData
				{
					propertyTypeValue = Enum.Parse<PropertyTypeValue>(text),
					PropertyType = PropertyType.Property,
					PropertyValue = _caiDanBaoDic[text]
				}.AddTypeValueAsShowTips();
			}
			caughtItem.transform.SetParent(null);
			UnityEngine.Object.Destroy(caughtItem.gameObject);
			MiNiGameMrg.Instance.AddScore(component);
			caughtItem = null;
		}
		firePoint.position = FireNorLoc();
		currentRetractSpeed = shootSpeed;
		currentState = State.Rotate;
		MiNiGameMrg.Instance.CheckGameOver();
	}

	private void CheckZhaDan()
	{
		if (Input.GetKeyDown(KeyCode.Space) && caughtItem != null && SingletonAsMono<GameDataMrg>.Instance.GetProperty("ZhaDan", "Nor", 0L) > 0)
		{
			Timer.DestroyTimer(_timeDataZhaDanFx);
			zhaDanFx.SetActiveAsCheck(active: false);
			zhaDanFx.SetActiveAsCheck(active: true);
			zhaDanFx.transform.position = caughtItem.position;
			UnityEngine.Object.Destroy(caughtItem.gameObject);
			caughtItem = null;
			currentRetractSpeed = shootSpeed;
			SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.miniGameZhaDan);
			SingletonAsMono<GameDataMrg>.Instance.AddProperty("ZhaDan", -1L);
			_timeDataZhaDanFx = Timer.DelayCall(0.5f, delegate
			{
				zhaDanFx.SetActiveAsCheck(active: false);
			});
		}
	}

	private Vector3 FireNorLoc()
	{
		return startPoint.position + startPoint.up * offset;
	}

	private void DrawRope()
	{
		float y = Vector3.Distance(startPoint.position, firePoint.position);
		line.size = new Vector2(0.15f, y);
		sheTou.position = firePoint.position;
	}

	public void StartRetract(Item item)
	{
		if (item != null && item.itemType == ItemType.ZhaDan && SingletonAsMono<GameDataMrg>.Instance.GetProperty("DaLiYaoShui", "Nor", 0L) > 0)
		{
			return;
		}
		currentState = State.Retract;
		if (item != null)
		{
			caughtItem = item.transform;
			currentRetractSpeed = shootSpeed / item.weight;
			item.GetComponent<Collider2D>().enabled = false;
			if (item.itemType == ItemType.ZhaDan)
			{
				Timer.DestroyTimer(_timeDataTntFx);
				tntFx.SetActiveAsCheck(active: false);
				tntFx.SetActiveAsCheck(active: true);
				tntFx.transform.position = caughtItem.position;
				_timeDataTntFx = Timer.DelayCall(1.5f, delegate
				{
					tntFx.SetActiveAsCheck(active: false);
				});
				MiNiGameMrg.Instance.ClearRandomItem(item, item.exRange);
				SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.miniGameTnt);
				item.transform.SetParent(null);
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
		MiNiGameMrg.Instance.CheckGameOver();
	}
}
