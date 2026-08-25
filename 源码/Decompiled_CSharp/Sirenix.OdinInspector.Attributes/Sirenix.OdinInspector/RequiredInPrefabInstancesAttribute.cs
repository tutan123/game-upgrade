using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Obsolete("Use [RequiredIn(PrefabKind.PrefabInstance)] instead.", true)]
[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
[EditorBrowsable(EditorBrowsableState.Never)]
public sealed class RequiredInPrefabInstancesAttribute : Attribute
{
	public string ErrorMessage;

	public InfoMessageType MessageType;

	public RequiredInPrefabInstancesAttribute()
	{
		MessageType = InfoMessageType.Error;
	}

	public RequiredInPrefabInstancesAttribute(string errorMessage, InfoMessageType messageType)
	{
		ErrorMessage = errorMessage;
		MessageType = messageType;
	}

	public RequiredInPrefabInstancesAttribute(string errorMessage)
	{
		ErrorMessage = errorMessage;
		MessageType = InfoMessageType.Error;
	}

	public RequiredInPrefabInstancesAttribute(InfoMessageType messageType)
	{
		MessageType = messageType;
	}
}
