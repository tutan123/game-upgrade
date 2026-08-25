using System.Runtime.InteropServices;

namespace Steamworks;

[StructLayout(0, Pack = 1)]
public struct InputAnalogActionData_t
{
	public EInputSourceMode eMode;

	public float x;

	public float y;

	public byte bActive;
}
