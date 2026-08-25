using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(3411)]
public struct StopPlaytimeTrackingResult_t
{
	public const int k_iCallback = 3411;

	public EResult m_eResult;
}
