using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
[DontApplyToListElements]
public sealed class CustomContextMenuAttribute : Attribute
{
	public string MenuItem;

	public string Action;

	[Obsolete("Use the Action member instead.", false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public string MethodName
	{
		get
		{
			return Action;
		}
		set
		{
			Action = value;
		}
	}

	public CustomContextMenuAttribute(string menuItem, string action)
	{
		MenuItem = menuItem;
		Action = action;
	}
}
