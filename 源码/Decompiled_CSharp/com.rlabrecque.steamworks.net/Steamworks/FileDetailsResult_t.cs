using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(1023)]
public struct FileDetailsResult_t
{
	public const int k_iCallback = 1023;

	public EResult m_eResult;

	public ulong m_ulFileSize;

	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
	public byte[] m_FileSHA;

	public uint m_unFlags;
}
