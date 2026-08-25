using System;

namespace RenderHeads.Media.AVProVideo;

[Serializable]
public struct HttpHeader
{
	public string name;

	public string value;

	public HttpHeader(string name, string value)
	{
		this.name = name;
		this.value = value;
	}

	public bool IsComplete()
	{
		if (!string.IsNullOrEmpty(name))
		{
			return !string.IsNullOrEmpty(value);
		}
		return false;
	}

	public string ToValidatedString()
	{
		string result = null;
		if (IsComplete() && IsValid())
		{
			result = $"{name}:{value}\r\n";
		}
		return result;
	}

	public static bool IsValid(string text)
	{
		if (!string.IsNullOrEmpty(text))
		{
			if (!IsAscii(text))
			{
				return false;
			}
			if (text.Contains("\r") || text.Contains("\n"))
			{
				return false;
			}
		}
		return true;
	}

	private static bool IsAscii(string text)
	{
		for (int i = 0; i < text.Length; i++)
		{
			if (text[i] >= '\u0080')
			{
				return false;
			}
		}
		return true;
	}

	private bool IsValid()
	{
		if (!IsValid(name) || !IsValid(value))
		{
			return false;
		}
		return true;
	}
}
