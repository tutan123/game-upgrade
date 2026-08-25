using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(3418)]
public struct UserSubscribedItemsListChanged_t
{
	public const int k_iCallback = 3418;

	public AppId_t m_nAppID;
}
