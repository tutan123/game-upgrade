using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[AttributeUsage(AttributeTargets.All)]
[DontApplyToListElements]
[Conditional("UNITY_EDITOR")]
[Obsolete("Use [HideIn(PrefabKind.PrefabInstance)] instead.", false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public class HideInPrefabInstancesAttribute : Attribute
{
}
