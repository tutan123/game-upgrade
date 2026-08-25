using System;
using System.Runtime.InteropServices;

namespace RenderHeads.Media.AVProVideo;

[StructLayout(0, Pack = 1)]
public struct TextureFrame
{
	internal IntPtr texturePointer;

	internal IntPtr auxTexturePointer;

	internal long timeStamp;

	internal uint frameCounter;

	internal uint writtenFrameCount;

	internal TextureFlags flags;

	internal IntPtr internalNativePointer;
}
