using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Obsolete("Use [HideIn(PrefabKind.PrefabAsset | PrefabKind.PrefabInstance)] instead.", false)]
[DontApplyToListElements]
[AttributeUsage(AttributeTargets.All)]
[Conditional("UNITY_EDITOR")]
[EditorBrowsable(EditorBrowsableState.Never)]
public class HideInPrefabsAttribute : Attribute
{
}
