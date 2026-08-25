using System;
using System.Collections.Generic;
using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "LevelTargetWindows")]
public class LevelTargetWindows : UiActor
{
	private Action _action;

	public RectTransform RectTransformTargetText;

	public CanvasRenderer CanvasRendererTargetText;

	public TextMeshProUGUI TextMeshProUGUITargetText;

	public AddScripts AddScriptsTargetText;

	public override void Awake()
	{
		base.Awake();
		RectTransformTargetText = GetGameObject().transform.Find("View/TargetText/").GetComponent<RectTransform>();
		CanvasRendererTargetText = GetGameObject().transform.Find("View/TargetText/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITargetText = GetGameObject().transform.Find("View/TargetText/").GetComponent<TextMeshProUGUI>();
		AddScriptsTargetText = GetGameObject().transform.Find("View/TargetText/").GetComponent<AddScripts>();
	}

	public LevelTargetWindows(Transform trans)
		: base(trans)
	{
	}

	public LevelTargetWindows()
	{
	}

	public void Init(Action action)
	{
		Xlsx_MiNiGameLevel xlsx_MiNiGameLevel = Xlsx_MiNiGameLevel_Query.XlsxDataAsOneKey.ByKeyGetValue(SingletonAsMono<GameDataMrg>.Instance.xlsxMiNiGameLevelKey);
		if (xlsx_MiNiGameLevel.Target == -1)
		{
			TextMeshProUGUITargetText.text = LanguageMrg.GetText("A5493");
		}
		else
		{
			TextMeshProUGUITargetText.text = string.Format(LanguageMrg.GetText("A1309"), xlsx_MiNiGameLevel.Target);
		}
		_action = action;
		Timer.DelayCall(2f, delegate
		{
			try
			{
				CloseUi();
				_action?.Invoke();
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
			}
		});
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
			_action = null;
		}
	}
}
