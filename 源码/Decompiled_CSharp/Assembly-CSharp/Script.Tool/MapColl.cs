using System.Collections.Generic;
using System.Linq;
using FrameWork;
using Script.Mrg;
using UnityEngine;
using Xlsx;

namespace Script.Tool;

public class MapColl : MonoBehaviour
{
	public CollType collType;

	private void OnTriggerEnter2D(Collider2D other)
	{
		int t = (int)collType;
		List<Xlsx_Event> list = (from e in FrameWork.Tool.CheckList(Xlsx_Event_Query.data)
			where e.Type == t
			select e).ToList();
		int curRound = SingletonAsMono<GameDataMrg>.Instance.CurRound;
		foreach (Xlsx_Event item in list)
		{
			if (item.Round.Length != 0 && curRound >= item.Round[0] && curRound <= item.Round[1] && SingletonAsMono<GameDataMrg>.Instance.GetPropertyAsEvent(item.VideoAsset) < item.Count)
			{
				VideoGraph videoGraph = ABMrg.Load<VideoGraph>(item.VideoAsset);
				if (videoGraph.GetEvenChapterNode().propertyType.IsSuc())
				{
					SingletonAsMono<GameDataMrg>.Instance.CurEventKey = item.Key;
					SingletonAsMono<GameDataMrg>.Instance.CurVideoId = videoGraph.GetEvenFistNode().uniqueID;
					LoadMrg.Load(Scenes.Game);
					break;
				}
			}
		}
	}
}
