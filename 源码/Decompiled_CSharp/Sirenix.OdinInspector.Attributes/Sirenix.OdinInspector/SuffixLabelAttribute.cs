using System;
using System.Diagnostics;

namespace Sirenix.OdinInspector;

[Conditional("UNITY_EDITOR")]
[AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
public sealed class SuffixLabelAttribute : Attribute
{
	public string Label;

	public bool Overlay;

	public string IconColor;

	private SdfIconType icon;

	[OdinDesignerBinding(new string[] { "icon", "HasDefinedIcon" })]
	[ShowInInspector]
	public SdfIconType Icon
	{
		get
		{
			return icon;
		}
		set
		{
			icon = value;
			HasDefinedIcon = true;
		}
	}

	public bool HasDefinedIcon { get; private set; }

	public SuffixLabelAttribute(string label, bool overlay = false)
	{
		Label = label;
		Overlay = overlay;
	}

	public SuffixLabelAttribute(string label, SdfIconType icon, bool overlay = false)
	{
		Label = label;
		Icon = icon;
		Overlay = overlay;
	}

	public SuffixLabelAttribute(SdfIconType icon)
	{
		Icon = icon;
	}
}
