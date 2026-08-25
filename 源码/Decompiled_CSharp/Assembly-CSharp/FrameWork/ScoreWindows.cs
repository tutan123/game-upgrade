using System.Collections.Generic;
using Script.UiTool;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "ScoreWindows")]
public class ScoreWindows : UiActor
{
	public RectTransform RectTransformRoleView;

	public AddScripts AddScriptsRoleView;

	public Animator AnimatorRoleView;

	public CanvasGroup CanvasGroupRoleView;

	public RectTransform RectTransformRoleList;

	public GridLayoutGroup GridLayoutGroupRoleList;

	public AddScripts AddScriptsRoleList;

	public override void Awake()
	{
		base.Awake();
		RectTransformRoleView = GetGameObject().transform.Find("View/RoleView/").GetComponent<RectTransform>();
		AddScriptsRoleView = GetGameObject().transform.Find("View/RoleView/").GetComponent<AddScripts>();
		AnimatorRoleView = GetGameObject().transform.Find("View/RoleView/").GetComponent<Animator>();
		CanvasGroupRoleView = GetGameObject().transform.Find("View/RoleView/").GetComponent<CanvasGroup>();
		RectTransformRoleList = GetGameObject().transform.Find("View/RoleView/RoleList/").GetComponent<RectTransform>();
		GridLayoutGroupRoleList = GetGameObject().transform.Find("View/RoleView/RoleList/").GetComponent<GridLayoutGroup>();
		AddScriptsRoleList = GetGameObject().transform.Find("View/RoleView/RoleList/").GetComponent<AddScripts>();
	}

	public ScoreWindows(Transform trans)
		: base(trans)
	{
	}

	public ScoreWindows()
	{
	}

	public void Init(int type = 1)
	{
		for (int i = 0; i < RectTransformRoleList.childCount; i++)
		{
			RectTransformRoleList.GetChild(i).GetComponent<ScoreRoleItem>().Init(type);
		}
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}
}
