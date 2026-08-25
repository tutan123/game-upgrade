using System;

namespace RenderHeads.Media.AVProVideo;

public enum StereoPacking
{
	Unknown = -1,
	Monoscopic = 0,
	TopBottom = 1,
	LeftRight = 2,
	CustomUV = 3,
	RightLeft = 4,
	MultiviewLeftPrimary = 5,
	MultiviewRightPrimary = 6,
	[Obsolete]
	None = 0,
	[Obsolete]
	TwoTextures = 5
}
