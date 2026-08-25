using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class ShowOdinSerializedPropertiesInInspectorAttribute : Attribute
{
}
