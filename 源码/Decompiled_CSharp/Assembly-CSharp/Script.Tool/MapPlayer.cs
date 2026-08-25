using System.Collections.Generic;
using System.Linq;
using FrameWork;
using Script.Mrg;
using Script.UiTool;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using Xlsx;

namespace Script.Tool;

public class MapPlayer : MonoBehaviour
{
	private NavMeshAgent agent;

	private SphereCollider sphereCollider;

	public float speed = 5f;

	public TMP_Text timeOutText;

	public TMP_Text infoText;

	public TMP_Text useText;

	public GameObject sureGo;

	public BT sureBtn;

	public bool isMove;

	public Transform pointGroup;

	public Transform roleTran;

	public Transform roleCarTran;

	public Xlsx_Item mtcItem;

	public Xlsx_Item yfItem;

	public GameObject light;

	private List<Map3DTriggerr> _mapTrigger = new List<Map3DTriggerr>();

	private TimeData _timeData;

	private Map3DTriggerr _trigger;

	private void Awake()
	{
		sphereCollider = GetComponent<SphereCollider>();
		agent = GetComponent<NavMeshAgent>();
		NavMesh.pathfindingIterationsPerFrame = 300;
	}

	private void Start()
	{
		for (int i = 0; i < pointGroup.childCount; i++)
		{
			_mapTrigger.Add(pointGroup.GetChild(i).GetComponent<Map3DTriggerr>());
		}
		sureGo.SetActive(value: false);
		sureBtn.onClick.AddListener(Trigger);
		CheckVehicleType();
		CheckYf(null);
	}

	private void Update()
	{
		CheckJoin();
		Move();
		CheckTrigger();
		if (agent != null && agent.hasPath)
		{
			Debug.DrawLine(base.transform.position, agent.steeringTarget, Color.red);
		}
	}

	private void CheckJoin()
	{
		if (sureGo.activeSelf && Input.GetKeyDown(KeyCode.Space))
		{
			Trigger();
		}
	}

	private void CheckTrigger()
	{
		for (int i = 0; i < _mapTrigger.Count; i++)
		{
			if (!_mapTrigger[i].gameObject.activeSelf || !(Vector3.Distance(base.transform.position, _mapTrigger[i].transform.position) <= _mapTrigger[i].checkDistance))
			{
				continue;
			}
			if (!(_trigger != _mapTrigger[i]))
			{
				return;
			}
			Trigger(_mapTrigger[i]);
			if (!SingletonAsMono<GameDataMrg>.Instance.IsUnLockPoint(_mapTrigger[i].collType.ToString()))
			{
				SingletonAsMono<GameDataMrg>.Instance.UnLockPoint(_mapTrigger[i].collType.ToString());
				if (FrameWork.Tool.GetMapLockPoint().Contains(_mapTrigger[i].collType))
				{
					UiManager.ShowTopTips(LanguageMrg.GetText("A5195"));
				}
			}
			return;
		}
		Exit();
	}

	private void OnEnable()
	{
		EventManager.AddListener(MessageType.Game, GameMessageType.ZbUpdate, CheckVehicleType);
		EventManager.AddListener(MessageType.Game, GameMessageType.ZbUpdate, CheckYf);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.ZbUpdate, CheckVehicleType);
		EventManager.RemoveListener(MessageType.Game, GameMessageType.ZbUpdate, CheckYf);
	}

	private void CheckVehicleType(List<object> objects)
	{
		CheckVehicleType();
	}

	private void CheckYf(List<object> objects)
	{
		if (!string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.YiFu)))
		{
			yfItem = Xlsx_Item_Query.XlsxDataAsOneKey.ByKeyGetValue(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.YiFu));
			FrameWork.Tool.HideAllChild(roleTran);
			roleTran.GetChild(yfItem.YfMode).SetActive(active: true);
			roleTran.GetChild(yfItem.YfMode).localPosition = Vector3.zero;
		}
		else
		{
			yfItem = null;
			FrameWork.Tool.HideAllChild(roleTran);
			roleTran.GetChild(0).SetActive(active: true);
			roleTran.GetChild(0).localPosition = Vector3.zero;
		}
	}

	private void CheckVehicleType()
	{
		string curZbKey = SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.ZaiJu);
		if (!string.IsNullOrEmpty(curZbKey))
		{
			Xlsx_Item xlsx_Item = Xlsx_Item_Query.XlsxDataAsOneKey.ByKeyGetValue(curZbKey);
			if (xlsx_Item.ItemType == 5)
			{
				mtcItem = Xlsx_Item_Query.XlsxDataAsOneKey.ByKeyGetValue(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.ZaiJu));
				speed = mtcItem.MoveSpeed;
				agent.speed = mtcItem.MoveSpeed;
				FrameWork.Tool.HideAllChild(roleCarTran);
				roleCarTran.GetChild(xlsx_Item.ZjMode).SetActive(active: true);
				light.SetActiveAsCheck(!SingletonAsMono<GameDataMrg>.Instance.IsMorning);
			}
			else
			{
				mtcItem = null;
				speed = 20f;
				agent.speed = 20f;
				FrameWork.Tool.HideAllChild(roleCarTran);
				light.SetActiveAsCheck(active: false);
			}
		}
		else
		{
			mtcItem = null;
			speed = 20f;
			agent.speed = 20f;
			FrameWork.Tool.HideAllChild(roleCarTran);
			light.SetActiveAsCheck(active: false);
		}
	}

	public void Move()
	{
		if (!UiManager.IsCanPlay())
		{
			return;
		}
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		if (Mathf.Abs(axis) < 0.01f && Mathf.Abs(axis2) < 0.01f)
		{
			isMove = !Mathf.Approximately(0f, agent.velocity.magnitude);
			return;
		}
		Transform obj = Camera.main.transform;
		Vector3 normalized = Vector3.ProjectOnPlane(obj.forward, Vector3.up).normalized;
		Vector3 normalized2 = Vector3.ProjectOnPlane(obj.right, Vector3.up).normalized;
		Vector3 normalized3 = (normalized * axis2 + normalized2 * axis).normalized;
		isMove = normalized3.magnitude >= 0.1f;
		if (normalized3.magnitude >= 0.1f)
		{
			if (agent.hasPath)
			{
				agent.ResetPath();
			}
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, Quaternion.LookRotation(normalized3), 10f * Time.deltaTime);
			agent.Move(normalized3 * speed * Time.deltaTime);
		}
	}

	private void Trigger(Map3DTriggerr trigger)
	{
		_trigger = trigger;
		trigger.collType.IsCanTrigger(out var _, out var _, out var msg, out var graph);
		_timeData = null;
		sureGo.SetActiveAsCheck(active: true);
		sureBtn.SetActive(active: false);
		infoText.SetActive(active: true);
		useText.text = "";
		FrameWork.Tool.ShowTutorial("TriggerEvent", new Sprite[1] { ABMrg.Load<Sprite>("TriggerEvent") });
		if (trigger.collType == CollType.ChongZhi)
		{
			infoText.text = LanguageMrg.GetText("A1679");
			return;
		}
		if ((bool)trigger && string.IsNullOrEmpty(msg))
		{
			infoText.SetActive(active: false);
			sureBtn.SetActive(active: true);
		}
		else if ((bool)trigger)
		{
			infoText.text = msg;
			sureBtn.SetActive(active: true);
		}
		else
		{
			infoText.text = LanguageMrg.GetText("A1211");
		}
		if (graph.IsHasAutoEventSAsNotCheckEq(out var autoGroup, isNot360: false))
		{
			for (int i = 0; i < autoGroup.Length; i++)
			{
				string xlsxEventKey = autoGroup[i].xlsxEventKey;
				Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxEventKey);
				if (xlsx_Event != null && !string.IsNullOrEmpty(xlsx_Event.ShowText))
				{
					infoText.text = LanguageMrg.GetText(xlsx_Event.ShowText);
					useText.text = autoGroup[i].GetPropertyDataStrAsVideoGroup();
					if (xlsx_Event.IsShowRound == 1 && xlsx_Event.Round.Length != 0)
					{
						int num = xlsx_Event.Round.Last() - SingletonAsMono<GameDataMrg>.Instance.CurRound;
						timeOutText.SetActive(num >= 0);
						timeOutText.text = "<sprite name=TimeOut>" + (num + 1);
						break;
					}
					timeOutText.SetActive(active: false);
				}
				else
				{
					timeOutText.SetActive(active: false);
				}
			}
		}
		else
		{
			useText.text = graph.GetPropertyDataStrAsVideoGroup();
			if (trigger.eventData != null && trigger.eventData.IsShowRound == 1 && trigger.eventData.Round.Length != 0)
			{
				int num2 = trigger.eventData.Round.Last() - SingletonAsMono<GameDataMrg>.Instance.CurRound;
				timeOutText.SetActive(num2 >= 0);
				timeOutText.text = "<sprite name=TimeOut>" + (num2 + 1);
			}
			else
			{
				timeOutText.SetActive(active: false);
			}
		}
		LayoutRebuilder.ForceRebuildLayoutImmediate(sureGo.GetComponent<RectTransform>());
	}

	private void Exit()
	{
		sureGo.SetActiveAsCheck(active: false);
		_trigger = null;
	}

	public void Trigger()
	{
		if (_trigger != null)
		{
			SingletonAsMono<GameDataMrg>.Instance.MapLoc = base.transform.position;
			_trigger.Trigger();
		}
	}
}
