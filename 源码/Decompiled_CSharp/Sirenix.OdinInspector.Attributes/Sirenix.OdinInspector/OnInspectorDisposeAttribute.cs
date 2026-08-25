using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[HideInTables]
[IncludeMyAttributes]
[DontApplyToListElements]
[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public class OnInspectorDisposeAttribute : ShowInInspectorAttribute
{
	public string Action;

	public OnInspectorDisposeAttribute()
	{
	}

	public OnInspectorDisposeAttribute(string action)
	{
		Action = action;
	}
}
