using System;

namespace FrameWork;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class UiModeAttribute : Attribute
{
	public Mode Mode;

	public bool isCanRemove;

	public UiModeAttribute(Mode mode, bool isCan = true)
	{
		Mode = mode;
		isCanRemove = isCan;
	}
}
