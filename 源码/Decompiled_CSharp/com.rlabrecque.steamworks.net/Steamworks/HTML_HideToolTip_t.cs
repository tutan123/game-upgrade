using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(4526)]
public struct HTML_HideToolTip_t
{
	public const int k_iCallback = 4526;

	public HHTMLBrowser unBrowserHandle;
}
