using System.Collections.Generic;
using System.Linq;
using FrameWork;
using Script.Mrg;
using Script.Scene;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.UiTool;

public class MapItem : MonoBehaviour
{
	public GameObject norIconGo;

	public Transform roleGroup;

	public TMP_Text title;

	public GameObject tipsGo;

	public TipsTool tipsTool;

	public TMP_Text timeOutText;

	public Image timeOutImage;

	private Map3DTriggerr _map3DTriggerr;

	public void Init(Map3DTriggerr map3DTriggerr, Image image)
	{
		_map3DTriggerr = map3DTriggerr;
		title.text = LanguageMrg.GetText(_map3DTriggerr.languageComponent.key);
		Vector3 vector = Map3DScene.Instance.mapCamera.WorldToViewportPoint(map3DTriggerr.transform.position);
		float x = (vector.x - 0.5f) * image.GetComponent<RectTransform>().rect.width;
		float y = (vector.y - 0.5f) * image.GetComponent<RectTransform>().rect.height;
		base.transform.localPosition = new Vector2(x, y);
		if (map3DTriggerr.videoGraph.IsHasAutoEventSAsNotCheckEq(out var group, isNot360: false))
		{
			int num = 0;
			if (num < group.Length)
			{
				string xlsxEventKey = group[num].xlsxEventKey;
				Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxEventKey);
				if (xlsx_Event != null)
				{
					GameObject mono = tipsGo;
					Xlsx_Event eventData = map3DTriggerr.eventData;
					mono.SetActiveAsCheck((eventData != null && eventData.AutoSave == 1) || xlsx_Event.AutoSave == 1);
				}
				else
				{
					GameObject mono2 = tipsGo;
					Xlsx_Event eventData = map3DTriggerr.eventData;
					mono2.SetActiveAsCheck(eventData != null && eventData.AutoSave == 1);
				}
			}
			for (int j = 0; j < group.Length; j++)
			{
				string xlsxEventKey2 = group[j].xlsxEventKey;
				Xlsx_Event xlsx_Event2 = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxEventKey2);
				if (xlsx_Event2 != null && xlsx_Event2.IsShowRound == 1 && xlsx_Event2.Round.Length != 0)
				{
					int num2 = xlsx_Event2.Round.Last() - SingletonAsMono<GameDataMrg>.Instance.CurRound;
					timeOutImage.SetActive(num2 >= 0 && num2 < 3);
					timeOutText.SetActive(num2 >= 0);
					timeOutText.text = "<sprite name=Time>" + (num2 + 1);
					break;
				}
				timeOutText.SetActive(active: false);
				timeOutImage.SetActive(active: false);
			}
			roleGroup.TranFor(group.Length, roleGroup.GetChild(0), delegate(int i, GameObject o)
			{
				Xlsx_Event xlsx_Event3 = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(group[i].xlsxEventKey);
				if (xlsx_Event3 != null && !string.IsNullOrEmpty(xlsx_Event3.RoleIcon))
				{
					o.transform.GetChild(0).GetChild(0).GetComponent<Image>()
						.sprite = ABMrg.Load<Sprite>(xlsx_Event3.RoleIcon);
				}
				else
				{
					o.SetActiveAsCheck(active: false);
				}
			});
			if (roleGroup.ActiveChild() > 0)
			{
				tipsGo.SetActiveAsCheck(active: false);
			}
		}
		else
		{
			GameObject mono3 = tipsGo;
			Xlsx_Event eventData = map3DTriggerr.eventData;
			mono3.SetActiveAsCheck(eventData != null && eventData.AutoSave == 1);
			if (map3DTriggerr.eventData != null && map3DTriggerr.eventData.IsShowRound == 1 && map3DTriggerr.eventData.Round.Length != 0)
			{
				int num3 = map3DTriggerr.eventData.Round.Last() - SingletonAsMono<GameDataMrg>.Instance.CurRound;
				timeOutImage.SetActive(num3 >= 0 && num3 < 3);
				timeOutText.SetActive(num3 >= 0);
				timeOutText.text = "<sprite name=Time>" + (num3 + 1);
			}
			else
			{
				timeOutText.SetActive(active: false);
				timeOutImage.SetActive(active: false);
			}
			roleGroup.TranFor(1, roleGroup.GetChild(0), delegate(int i, GameObject o)
			{
				if (map3DTriggerr.eventData != null && !string.IsNullOrEmpty(map3DTriggerr.eventData.RoleIcon))
				{
					o.transform.GetChild(0).GetChild(0).GetComponent<Image>()
						.sprite = ABMrg.Load<Sprite>(map3DTriggerr.eventData.RoleIcon);
				}
				else
				{
					o.SetActiveAsCheck(active: false);
				}
			});
			if (roleGroup.ActiveChild() > 0)
			{
				tipsGo.SetActiveAsCheck(active: false);
			}
		}
		norIconGo.SetActiveAsCheck(roleGroup.ActiveChild() == 0 && !tipsGo.activeSelf);
	}

	public void OnClick()
	{
		List<object> eventMsg = EventManager.GetEventMsg();
		eventMsg.Add(_map3DTriggerr);
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.ClickEvent, eventMsg);
	}

	public void OnEnable()
	{
		tipsTool.SetActive(active: false);
		EventManager.AddListener(MessageType.Game, GameMessageType.ClickEvent, ClickEvent);
	}

	public void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.ClickEvent, ClickEvent);
	}

	private void ClickEvent(List<object> objects)
	{
		if (objects != null && objects.Count > 0)
		{
			if ((Map3DTriggerr)objects[0] == _map3DTriggerr)
			{
				tipsTool.SetActive(active: true);
				tipsTool.Init(_map3DTriggerr);
			}
			else
			{
				tipsTool.SetActive(active: false);
			}
		}
		else
		{
			tipsTool.SetActive(active: false);
		}
	}
}
