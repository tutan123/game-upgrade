namespace RenderHeads.Media.AVProVideo;

public class Variant
{
	private int m_iId = -1;

	private int m_iWidth;

	private int m_iHeight;

	private int m_iPeakDataRate;

	private int m_iAverageDataRate;

	private CodecType m_VideoCodecType;

	private float m_fFrameRate;

	private VideoRange m_eVideoRange;

	private CodecType m_AudioCodecType;

	private VariantFlags m_Flags;

	private static Variant s_Auto = new Variant(-1, 0, 0, 0);

	public int Id => m_iId;

	public int Width => m_iWidth;

	public int Height => m_iHeight;

	public int PeakDataRate => m_iPeakDataRate;

	public int AverageDataRate => m_iAverageDataRate;

	public float FrameRate => m_fFrameRate;

	public VideoRange VideoRange => m_eVideoRange;

	public CodecType VideoCodecType => m_VideoCodecType;

	public bool IsUnsupported => (m_Flags & VariantFlags.Unsupported) == VariantFlags.Unsupported;

	public string VideoCodecName
	{
		get
		{
			switch (m_VideoCodecType)
			{
			case CodecType.avc1:
			case CodecType.avc3:
				return "H264";
			case CodecType.dvh1:
			case CodecType.dvhe:
				return "Dolby Vision";
			case CodecType.hev1:
			case CodecType.hvc1:
				return "HEVC";
			case CodecType.mjpg:
				return "MJPEG";
			default:
				return "";
			}
		}
	}

	public CodecType AudioCodecType => m_AudioCodecType;

	public string AudioCodecName => m_AudioCodecType switch
	{
		CodecType.ac_3 => "AC-3", 
		CodecType.alac => "Apple Lossless", 
		CodecType.ec_3 => "EC-3", 
		CodecType.fLaC => "FLAC", 
		CodecType.mp4a => "AAC", 
		_ => "", 
	};

	public static Variant Auto => s_Auto;

	public Variant(int iId, int iWidth, int iHeight, int iPeakDataRate, int iAverageDataRate = 0, CodecType videoCodecType = CodecType.unknown, float fFrameRate = 0f, VideoRange eVideoRange = VideoRange.SDR, CodecType audioCodecType = CodecType.unknown, VariantFlags flags = (VariantFlags)0)
	{
		m_iId = iId;
		m_iWidth = iWidth;
		m_iHeight = iHeight;
		m_iPeakDataRate = iPeakDataRate;
		m_iAverageDataRate = iAverageDataRate;
		m_VideoCodecType = videoCodecType;
		m_fFrameRate = fFrameRate;
		m_eVideoRange = eVideoRange;
		m_AudioCodecType = audioCodecType;
		m_Flags = flags;
	}
}
