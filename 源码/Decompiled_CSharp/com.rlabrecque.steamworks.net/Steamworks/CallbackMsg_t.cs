using System;
using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
public struct CallbackMsg_t
{
	public int m_hSteamUser;

	public int m_iCallback;

	public IntPtr m_pubParam;

	public int m_cubParam;
}
