using System.Linq;
using FrameWork;
using Script.Mrg;
using Script.Scene;
using Script.Tool;
using UnityEngine;
using Xlsx;

namespace Script.UiTool;

public class Set360Info : MonoBehaviour
{
	public VideoButton videoButton;

	public void SetInfo()
	{
		if (!(videoButton != null))
		{
			return;
		}
		string use = "";
		string time = "";
		string text = "";
		if (videoButton.GetVideoNode().GetNextVideoNode().IsHasAutoEventSAsNotCheck(out var autoGroup))
		{
			for (int i = 0; i < autoGroup.Length; i++)
			{
				time = autoGroup[i].GetPropertyDataStrAsVideoGroup(videoButton);
				Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(autoGroup[i].xlsxEventKey);
				if (xlsx_Event != null && xlsx_Event.IsShowRound == 1 && xlsx_Event.Round.Length != 0)
				{
					text = LanguageMrg.GetText(xlsx_Event.ShowText);
					int num = xlsx_Event.Round.Last() - SingletonAsMono<GameDataMrg>.Instance.CurRound;
					if (num > 0)
					{
						use = "<sprite name=TimeOut>" + (num + 1);
						break;
					}
				}
			}
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			text = LanguageMrg.GetText(videoButton.GetButtonNode().btnName);
		}
		Video360Scene.Instance.SetShowText(text, use, time);
	}
}
