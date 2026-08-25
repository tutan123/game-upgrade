using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8, Size = 1)]
[CallbackIdentity(4702)]
public struct SteamInventoryDefinitionUpdate_t
{
	public const int k_iCallback = 4702;
}
