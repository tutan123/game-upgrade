using FrameWork;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.UiTool;

public class ChaperItem : MonoBehaviour
{
	public TMP_Text title;

	public ChaperInfo chaperInfo;

	public Image icon;

	private Xlsx_Chapter _xlsxChapter;

	public void Init(Xlsx_Chapter xlsxChapter)
	{
		_xlsxChapter = xlsxChapter;
	}

	public void Enter()
	{
		if (!string.IsNullOrEmpty(_xlsxChapter.RoleKey))
		{
			Xlsx_Role xlsxRole = Xlsx_Role_Query.XlsxDataAsOneKey.ByKeyGetValue(_xlsxChapter.RoleKey);
			chaperInfo.Init(xlsxRole);
			chaperInfo.SetActive(active: true);
		}
		else
		{
			chaperInfo.SetActive(active: false);
		}
	}
}
