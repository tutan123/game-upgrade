using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(4701)]
public struct SteamInventoryFullUpdate_t
{
	public const int k_iCallback = 4701;

	public SteamInventoryResult_t m_handle;
}
