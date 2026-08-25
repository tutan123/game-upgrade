using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

[Serializable]
public class HttpHeaderData : IEnumerable
{
	[SerializeField]
	private List<HttpHeader> httpHeaders = new List<HttpHeader>();

	public HttpHeader this[int index] => httpHeaders[index];

	public IEnumerator GetEnumerator()
	{
		return httpHeaders.GetEnumerator();
	}

	public void Clear()
	{
		httpHeaders.Clear();
	}

	public void Add(string name, string value)
	{
		httpHeaders.Add(new HttpHeader(name, value));
	}

	public bool IsModified()
	{
		if (httpHeaders != null)
		{
			return httpHeaders.Count > 0;
		}
		return false;
	}

	public string ToValidatedString()
	{
		string text = string.Empty;
		foreach (HttpHeader httpHeader in httpHeaders)
		{
			if (httpHeader.IsComplete())
			{
				string text2 = httpHeader.ToValidatedString();
				if (!string.IsNullOrEmpty(text2))
				{
					text += text2;
				}
				else
				{
					Debug.LogWarning("[AVProVideo] Custom HTTP header field ignored due to invalid format");
				}
			}
		}
		return text;
	}
}
