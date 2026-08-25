using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(4510)]
public struct HTML_CanGoBackAndForward_t
{
	public const int k_iCallback = 4510;

	public HHTMLBrowser unBrowserHandle;

	[MarshalAs(UnmanagedType.I1)]
	public bool bCanGoBack;

	[MarshalAs(UnmanagedType.I1)]
	public bool bCanGoForward;
}
