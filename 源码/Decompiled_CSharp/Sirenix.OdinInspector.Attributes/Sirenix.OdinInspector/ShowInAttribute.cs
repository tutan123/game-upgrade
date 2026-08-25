using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All)]
public class ShowInAttribute : Attribute
{
	public PrefabKind PrefabKind;

	public ShowInAttribute(PrefabKind prefabKind)
	{
		PrefabKind = prefabKind;
	}
}
