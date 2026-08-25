using System.Linq;
using FrameWork;
using Script.Mrg;
using Script.Tool;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.UiTool;

public class Img360BtnInfo : MonoBehaviour
{
	public TMP_Text info;

	public TMP_Text useText;

	public TMP_Text timeText;

	public void Init(VideoButton videoButton)
	{
		if (videoButton.GetVideoNode().GetNextVideoNode().IsHasAutoEventSAsNotCheck(out var autoGroup))
		{
			for (int i = 0; i < autoGroup.Length; i++)
			{
				useText.text = autoGroup[i].GetPropertyDataStrAsVideoGroup(videoButton);
				Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(autoGroup[i].xlsxEventKey);
				if (xlsx_Event != null && xlsx_Event.IsShowRound == 1 && xlsx_Event.Round.Length != 0)
				{
					info.text = LanguageMrg.GetText(xlsx_Event.ShowText);
					int num = xlsx_Event.Round.Last() - SingletonAsMono<GameDataMrg>.Instance.CurRound;
					if (num > 0)
					{
						timeText.text = "<sprite name=TimeOut>" + (num + 1);
						break;
					}
				}
			}
		}
		else
		{
			info.text = LanguageMrg.GetText(videoButton.GetButtonNode().btnName);
			useText.text = "";
			timeText.text = "";
		}
	}
}
