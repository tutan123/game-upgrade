using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[IncludeMyAttributes]
[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
[DontApplyToListElements]
[HideInTables]
public sealed class OnStateUpdateAttribute : Attribute
{
	public string Action;

	public OnStateUpdateAttribute(string action)
	{
		Action = action;
	}
}
