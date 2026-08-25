using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(4501)]
public struct HTML_BrowserReady_t
{
	public const int k_iCallback = 4501;

	public HHTMLBrowser unBrowserHandle;
}
