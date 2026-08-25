using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Obsolete("Use [DisableIn(PrefabKind.PrefabAsset)] instead.", false)]
[Conditional("UNITY_EDITOR")]
[DontApplyToListElements]
[EditorBrowsable(EditorBrowsableState.Never)]
[AttributeUsage(AttributeTargets.All)]
public class DisableInPrefabAssetsAttribute : Attribute
{
}
