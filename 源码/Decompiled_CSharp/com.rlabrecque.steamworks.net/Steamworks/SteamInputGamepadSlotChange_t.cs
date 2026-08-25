using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(2804)]
public struct SteamInputGamepadSlotChange_t
{
	public const int k_iCallback = 2804;

	public AppId_t m_unAppID;

	public InputHandle_t m_ulDeviceHandle;

	public ESteamInputType m_eDeviceType;

	public int m_nOldGamepadSlot;

	public int m_nNewGamepadSlot;
}
