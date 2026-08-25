using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(5302)]
public struct CreateBeaconCallback_t
{
	public const int k_iCallback = 5302;

	public EResult m_eResult;

	public PartyBeaconID_t m_ulBeaconID;
}
