using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
public struct FriendGameInfo_t
{
	public CGameID m_gameID;

	public uint m_unGameIP;

	public ushort m_usGamePort;

	public ushort m_usQueryPort;

	public CSteamID m_steamIDLobby;
}
