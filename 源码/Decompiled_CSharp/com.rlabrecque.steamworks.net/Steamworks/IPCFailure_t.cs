using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(117)]
public struct IPCFailure_t
{
	public const int k_iCallback = 117;

	public byte m_eFailureType;
}
