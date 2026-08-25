using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(4605)]
public struct BroadcastUploadStop_t
{
	public const int k_iCallback = 4605;

	public EBroadcastUploadResult m_eResult;
}
