using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(4604)]
public struct BroadcastUploadStart_t
{
	public const int k_iCallback = 4604;

	[MarshalAs(UnmanagedType.I1)]
	public bool m_bIsRTMP;
}
