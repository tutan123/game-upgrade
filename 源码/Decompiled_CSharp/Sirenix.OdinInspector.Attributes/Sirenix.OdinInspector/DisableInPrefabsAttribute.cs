using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Obsolete("Use [DisableIn(PrefabKind.PrefabAsset | PrefabKind.PrefabInstance)] instead.", false)]
[AttributeUsage(AttributeTargets.All)]
[Conditional("UNITY_EDITOR")]
[DontApplyToListElements]
[EditorBrowsable(EditorBrowsableState.Never)]
public class DisableInPrefabsAttribute : Attribute
{
}
