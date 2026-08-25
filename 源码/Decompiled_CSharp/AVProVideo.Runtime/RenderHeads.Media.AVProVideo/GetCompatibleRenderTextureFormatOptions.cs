using System;

namespace RenderHeads.Media.AVProVideo;

[Flags]
public enum GetCompatibleRenderTextureFormatOptions
{
	Default = 0,
	ForResolve = 1,
	RequiresAlpha = 2
}
