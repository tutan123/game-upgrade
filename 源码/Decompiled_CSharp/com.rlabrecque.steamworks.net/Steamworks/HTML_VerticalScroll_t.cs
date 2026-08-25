using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(4512)]
public struct HTML_VerticalScroll_t
{
	public const int k_iCallback = 4512;

	public HHTMLBrowser unBrowserHandle;

	public uint unScrollMax;

	public uint unScrollCurrent;

	public float flPageScale;

	[MarshalAs(UnmanagedType.I1)]
	public bool bVisible;

	public uint unPageSize;
}
