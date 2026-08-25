using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 8)]
[CallbackIdentity(3417)]
public struct DeleteItemResult_t
{
	public const int k_iCallback = 3417;

	public EResult m_eResult;

	public PublishedFileId_t m_nPublishedFileId;
}
