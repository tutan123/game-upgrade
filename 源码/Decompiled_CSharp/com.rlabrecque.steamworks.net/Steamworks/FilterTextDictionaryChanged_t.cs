using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(739)]
public struct FilterTextDictionaryChanged_t
{
	public const int k_iCallback = 739;

	public int m_eLanguage;
}
