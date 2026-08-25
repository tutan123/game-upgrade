using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(1252)]
public struct SteamNetworkingMessagesSessionFailed_t
{
	public const int k_iCallback = 1252;

	public SteamNetConnectionInfo_t m_info;
}
