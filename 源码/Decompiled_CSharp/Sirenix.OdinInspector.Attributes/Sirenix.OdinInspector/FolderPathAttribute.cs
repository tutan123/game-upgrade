using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
public sealed class FolderPathAttribute : Attribute
{
	public bool AbsolutePath;

	public string ParentFolder;

	[Obsolete("Use RequireExistingPath instead.", true)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool RequireValidPath;

	public bool RequireExistingPath;

	public bool UseBackslashes;
}
