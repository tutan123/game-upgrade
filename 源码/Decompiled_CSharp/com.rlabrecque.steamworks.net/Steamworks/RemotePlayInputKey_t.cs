using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
public struct RemotePlayInputKey_t
{
	public int m_eScancode;

	public uint m_unModifiers;

	public uint m_unKeycode;
}
