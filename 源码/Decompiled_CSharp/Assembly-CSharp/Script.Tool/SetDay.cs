using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;

namespace Script.Tool;

public class SetDay : MonoBehaviour
{
	public TMP_Text day;

	private void OnEnable()
	{
		day.text = string.Format(LanguageMrg.GetText("A1585"), SingletonAsMono<GameDataMrg>.Instance.CurRound);
	}
}
