using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.Tool;

public class ChaperInfo : MonoBehaviour
{
	public TMP_Text zhiYe;

	public TMP_Text chuSheng;

	public TMP_Text sanWei;

	public TMP_Text jianPin;

	private Xlsx_Role _xlsxRole;

	public void Init(Xlsx_Role xlsxRole)
	{
		_xlsxRole = xlsxRole;
		zhiYe.text = string.Format(LanguageMrg.GetText("A1319"), LanguageMrg.GetText(xlsxRole.ZhiYe));
		chuSheng.text = string.Format(LanguageMrg.GetText("A1320"), LanguageMrg.GetText(xlsxRole.ChuSheng));
		sanWei.text = string.Format(LanguageMrg.GetText("A1321"), LanguageMrg.GetText(xlsxRole.SanWei));
		jianPin.text = string.Format(LanguageMrg.GetText("A1321"), LanguageMrg.GetText(xlsxRole.JianPin));
	}
}
