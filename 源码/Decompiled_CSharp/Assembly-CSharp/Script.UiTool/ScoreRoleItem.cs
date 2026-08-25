using System.Collections.Generic;
using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UiTool;

public class ScoreRoleItem : MonoBehaviour
{
	public GameObject showGo;

	public GameObject xinDonGGo;

	public GameObject xinSuiGo;

	public Image xinImg;

	public TMP_Text progressText;

	public PropertyTypeValue propertyTypeValue;

	public TMP_Text hgdText;

	public List<VideoUnlockData> unlockDatas;

	public List<VideoUnlockData> loseDatas;

	public float favorability;

	public void Init(int type)
	{
		float num = 0f;
		float a = unlockDatas.Count;
		for (int i = 0; i < unlockDatas.Count; i++)
		{
			if (SingletonAsMono<GameDataMrg>.Instance.IsUnLockKey(unlockDatas[i].unlockVideoPath))
			{
				num += 1f;
			}
		}
		showGo.SetActiveAsCheck(num > 0f);
		float num2 = 0f;
		for (int j = 0; j < loseDatas.Count; j++)
		{
			if (SingletonAsMono<GameDataMrg>.Instance.IsUnLockKey(loseDatas[j].unlockVideoPath))
			{
				num2 += 1f;
			}
		}
		xinDonGGo.SetActiveAsCheck(num2 == 0f);
		xinSuiGo.SetActiveAsCheck(num2 > 0f);
		float num3 = (float)SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyTypeValue.ToString(), "Property", 0L) / favorability;
		xinImg.fillAmount = num3;
		hgdText.text = (int)(num3 * 100f) + "%";
		float num4 = num / Mathf.Max(a, 0.1f);
		num4 *= 100f;
		num4 = (int)num4;
		progressText.text = string.Format(LanguageMrg.GetText("A2488"), num4);
	}
}
