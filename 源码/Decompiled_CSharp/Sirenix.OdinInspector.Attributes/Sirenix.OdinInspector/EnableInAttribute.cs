using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All)]
[DontApplyToListElements]
public class EnableInAttribute : Attribute
{
	public PrefabKind PrefabKind;

	public EnableInAttribute(PrefabKind prefabKind)
	{
		PrefabKind = prefabKind;
	}
}
