using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8, Size = 1)]
[CallbackIdentity(736)]
public struct AppResumingFromSuspend_t
{
	public const int k_iCallback = 736;
}
