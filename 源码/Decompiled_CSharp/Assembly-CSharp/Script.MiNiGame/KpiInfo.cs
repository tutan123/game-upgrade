using System.Collections.Generic;
using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;

namespace Script.MiNiGame;

public class KpiInfo : MonoBehaviour
{
	public TMP_Text curKpi;

	public TMP_Text faFanTime;

	public bool isAutoSetValue = true;

	public List<int> times = new List<int> { 8, 23, 33 };

	private void OnEnable()
	{
		if (!isAutoSetValue)
		{
			return;
		}
		curKpi.text = string.Format(LanguageMrg.GetText("A5488"), SingletonAsMono<GameDataMrg>.Instance.GetProperty("KPI", "Property", 0L).ToString().FormatStringNumber());
		int num = times[0];
		for (int i = 0; i < times.Count; i++)
		{
			if (times[i] >= SingletonAsMono<GameDataMrg>.Instance.CurRound)
			{
				num = times[i];
				break;
			}
		}
		faFanTime.text = string.Format(LanguageMrg.GetText("A5489"), num - SingletonAsMono<GameDataMrg>.Instance.CurRound);
	}

	public void Init(string text)
	{
		curKpi.text = text;
		int num = times[0];
		for (int i = 0; i < times.Count; i++)
		{
			if (times[i] >= SingletonAsMono<GameDataMrg>.Instance.CurRound)
			{
				num = times[i];
				break;
			}
		}
		faFanTime.text = string.Format(LanguageMrg.GetText("A5489"), num - SingletonAsMono<GameDataMrg>.Instance.CurRound);
	}
}
