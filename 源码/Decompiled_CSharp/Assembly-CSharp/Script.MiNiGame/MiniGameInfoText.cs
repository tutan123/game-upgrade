using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.MiNiGame;

public class MiniGameInfoText : MonoBehaviour
{
	public TMP_Text infoText;

	public void Init(string xlsxMiniGameItemKey)
	{
		Xlsx_MiNiGameItem xlsx_MiNiGameItem = Xlsx_MiNiGameItem_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxMiniGameItemKey);
		infoText.text = LanguageMrg.GetText(xlsx_MiNiGameItem.TouchDesc);
	}
}
