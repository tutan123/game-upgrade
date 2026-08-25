using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8, Size = 1)]
[CallbackIdentity(4001)]
public struct PlaybackStatusHasChanged_t
{
	public const int k_iCallback = 4001;
}
