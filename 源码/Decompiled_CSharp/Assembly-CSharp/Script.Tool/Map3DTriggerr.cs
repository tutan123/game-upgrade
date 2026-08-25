using System.Collections.Generic;
using System.Linq;
using FrameWork;
using Script.Mrg;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.Tool;

public class Map3DTriggerr : MonoBehaviour
{
	public CollType collType;

	public Image tips;

	public float checkDistance = 10f;

	public Xlsx_Event eventData;

	public LanguageComponent languageComponent;

	public VideoGraph videoGraph;

	public Transform roleGroup;

	public Image nor;

	public void CheckShow()
	{
		collType.IsCanTrigger(out var canShow, out var _, out var _, out var graph);
		base.gameObject.SetActiveAsCheck(canShow && (bool)graph);
		FrameWork.Tool.HideAllChild(roleGroup);
		tips.SetActive(active: false);
		videoGraph = graph;
		if (graph == null)
		{
			eventData = null;
		}
		else
		{
			eventData = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(graph.xlsxEventKey.Trim());
		}
		if (graph.IsHasAutoEventSAsNotCheckEq(out var videoG, isNot360: false))
		{
			int num = 0;
			if (num < videoG.Length)
			{
				string xlsxEventKey = videoG[num].xlsxEventKey;
				Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxEventKey);
				if (xlsx_Event != null)
				{
					tips.SetActive(xlsx_Event.AutoSave == 1);
				}
				else
				{
					tips.SetActive(active: true);
				}
			}
			roleGroup.TranFor(videoG.Length, roleGroup.GetChild(0), delegate(int i, GameObject o)
			{
				Xlsx_Event xlsx_Event2 = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(videoG[i].xlsxEventKey);
				if (xlsx_Event2 != null && !string.IsNullOrEmpty(xlsx_Event2.RoleIcon))
				{
					o.transform.GetChild(0).GetComponent<Image>().sprite = ABMrg.Load<Sprite>(xlsx_Event2.RoleIcon);
				}
				else
				{
					o.SetActiveAsCheck(active: false);
				}
			});
			if (roleGroup.ActiveChild() > 0)
			{
				tips.SetActive(active: false);
			}
		}
		else
		{
			if (eventData != null)
			{
				tips.SetActive(eventData.AutoSave == 1);
			}
			if (eventData != null && !string.IsNullOrEmpty(eventData.RoleIcon))
			{
				roleGroup.TranFor(1, roleGroup.GetChild(0), delegate(int i, GameObject o)
				{
					o.transform.GetChild(0).GetComponent<Image>().sprite = ABMrg.Load<Sprite>(eventData.RoleIcon);
				});
			}
			if (roleGroup.ActiveChild() > 0)
			{
				tips.SetActive(active: false);
			}
		}
		nor.SetActive(!tips.gameObject.activeSelf && roleGroup.ActiveChild() == 0);
		if (collType == CollType.ChongZhi)
		{
			base.gameObject.SetActiveAsCheck(active: true);
		}
	}

	public void Trigger()
	{
		int t = (int)collType;
		List<Xlsx_Event> list = (from e in FrameWork.Tool.CheckList(Xlsx_Event_Query.data)
			where e.Type == t
			select e).ToList();
		int curRound = SingletonAsMono<GameDataMrg>.Instance.CurRound;
		int num = -1;
		Xlsx_Pos xlsx_Pos = Xlsx_Pos_Query.XlsxDataAsOneKey.ByKeyGetValue(t);
		if (xlsx_Pos != null && xlsx_Pos.MaxRoundCount > 0)
		{
			num = xlsx_Pos.MaxRoundCount;
		}
		int roundProperty = SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty("EventRound_" + t, "Event");
		if (num != -1 && roundProperty >= num)
		{
			return;
		}
		foreach (Xlsx_Event item in list)
		{
			if (item.Round.Length == 0 || ((item.Round.Length != 1 || curRound != item.Round[0]) && (item.Round.Length != 2 || curRound < item.Round[0] || curRound > item.Round[1])))
			{
				continue;
			}
			bool flag = false;
			if (SingletonAsMono<GameDataMrg>.Instance.IsMorning)
			{
				if (item.IsMorning == 1)
				{
					flag = true;
				}
			}
			else if (item.IsMorning != 1)
			{
				flag = true;
			}
			if (item.IsMorning == -1)
			{
				flag = true;
			}
			if (!flag)
			{
				continue;
			}
			int propertyAsEvent = SingletonAsMono<GameDataMrg>.Instance.GetPropertyAsEvent("Ex_" + item.Key);
			if (item.Count != -1 && propertyAsEvent >= item.Count)
			{
				continue;
			}
			VideoGraph videoGraph = ABMrg.Load<VideoGraph>(item.VideoAsset);
			ChapterNode evenChapterNode = videoGraph.GetEvenChapterNode();
			if (evenChapterNode.propertyType.IsSuc() && evenChapterNode.showPropertyType.IsSuc())
			{
				if (SingletonAsMono<GameDataMrg>.Instance.IsCanAutoSave(item.Key) && item.AutoSave == 1 && item.NotAutoSave != 1)
				{
					SingletonAsMono<GameDataMrg>.Instance.SetCanAutoSave(item.Key, value: false);
					SingletonAsMono<GameDataMrg>.Instance.AutoSave(LanguageMrg.GetText("A434"));
				}
				SingletonAsMono<GameDataMrg>.Instance.AddPropertyAsEvent("Ex_" + item.Key);
				SingletonAsMono<GameDataMrg>.Instance.AddRoundProperty("EventRound_" + t, 1, "Event");
				SingletonAsMono<GameDataMrg>.Instance.CurEventKey = item.Key;
				SingletonAsMono<GameDataMrg>.Instance.CurVideoId = videoGraph.GetEvenFistNode().uniqueID;
				LoadMrg.Load(Scenes.Game);
				return;
			}
			if (evenChapterNode.isUseTips)
			{
				UiManager.ShowTips(LanguageMrg.GetText(evenChapterNode.xlsxLanguageKey));
				return;
			}
		}
		UiManager.ShowTips(LanguageMrg.GetText("A419"));
	}

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere(base.transform.position, checkDistance);
	}
}
