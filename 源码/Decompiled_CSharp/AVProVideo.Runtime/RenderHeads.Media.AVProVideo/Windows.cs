namespace RenderHeads.Media.AVProVideo;

public static class Windows
{
	public enum VideoApi
	{
		MediaFoundation,
		DirectShow,
		WinRT
	}

	public enum AudioOutput
	{
		System,
		Unity,
		FacebookAudio360,
		None
	}

	public const string AudioDeviceOutputName_Vive = "HTC VIVE USB Audio";

	public const string AudioDeviceOutputName_Rift = "Headphones (Rift Audio)";
}
