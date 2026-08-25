using System;

namespace RenderHeads.Media.AVProVideo;

public class AuthData
{
	public string URL { get; set; }

	public string Token { get; set; }

	public byte[] KeyBytes { get; set; }

	public string KeyBase64
	{
		get
		{
			if (KeyBytes != null)
			{
				return Convert.ToBase64String(KeyBytes);
			}
			return string.Empty;
		}
		set
		{
			if (value != null)
			{
				KeyBytes = Convert.FromBase64String(value);
			}
			else
			{
				KeyBytes = null;
			}
		}
	}

	public AuthData()
	{
		Clear();
	}

	public void Clear()
	{
		URL = string.Empty;
		Token = string.Empty;
		KeyBytes = null;
	}
}
