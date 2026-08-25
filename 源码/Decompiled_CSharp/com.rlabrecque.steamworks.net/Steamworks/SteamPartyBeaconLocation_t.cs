using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
public struct SteamPartyBeaconLocation_t
{
	public ESteamPartyBeaconLocationType m_eType;

	public ulong m_ulLocationID;
}
