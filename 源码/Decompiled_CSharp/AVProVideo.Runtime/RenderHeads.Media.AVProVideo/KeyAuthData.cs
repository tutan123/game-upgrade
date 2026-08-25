using System;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

[Serializable]
public class KeyAuthData : ISerializationCallbackReceiver
{
	[SerializeField]
	public string keyServerToken;

	[Multiline]
	[SerializeField]
	public string overrideDecryptionKeyBase64;

	private byte[] _overrideDecryptionKey;

	public byte[] overrideDecryptionKey
	{
		get
		{
			return _overrideDecryptionKey;
		}
		set
		{
			_overrideDecryptionKey = value;
			if (value == null)
			{
				overrideDecryptionKeyBase64 = "";
			}
			else
			{
				overrideDecryptionKeyBase64 = Convert.ToBase64String(_overrideDecryptionKey);
			}
		}
	}

	public bool IsModified()
	{
		if (string.IsNullOrEmpty(keyServerToken))
		{
			return !string.IsNullOrEmpty(overrideDecryptionKeyBase64);
		}
		return true;
	}

	public void OnBeforeSerialize()
	{
	}

	public void OnAfterDeserialize()
	{
		if (!string.IsNullOrEmpty(overrideDecryptionKeyBase64))
		{
			try
			{
				_overrideDecryptionKey = Convert.FromBase64String(overrideDecryptionKeyBase64);
				return;
			}
			catch (Exception arg)
			{
				Debug.LogWarning($"Failed to decode overrideDecryptionKeyBase64, error: {arg}");
				return;
			}
		}
		_overrideDecryptionKey = null;
	}
}
