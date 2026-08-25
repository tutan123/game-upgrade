using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
[Conditional("UNITY_EDITOR")]
[DontApplyToListElements]
public sealed class OnCollectionChangedAttribute : Attribute
{
	public string Before;

	public string After;

	public OnCollectionChangedAttribute()
	{
	}

	public OnCollectionChangedAttribute(string after)
	{
		After = after;
	}

	public OnCollectionChangedAttribute(string before, string after)
	{
		Before = before;
		After = after;
	}
}
