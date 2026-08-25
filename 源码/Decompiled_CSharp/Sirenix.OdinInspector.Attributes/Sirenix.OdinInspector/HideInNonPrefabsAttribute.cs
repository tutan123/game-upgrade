using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[EditorBrowsable(EditorBrowsableState.Never)]
[Obsolete("Use [HideIn(PrefabKind.NonPrefabInstance)] instead.", false)]
[DontApplyToListElements]
[AttributeUsage(AttributeTargets.All)]
[Conditional("UNITY_EDITOR")]
public class HideInNonPrefabsAttribute : Attribute
{
}
