using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

[AddComponentMenu("AVPro Video/Audio Channel Mixer", 401)]
[HelpURL("https://www.renderheads.com/products/avpro-video/")]
public class AudioChannelMixer : MonoBehaviour
{
	private const int MaxChannels = 8;

	[Range(0f, 1f)]
	[SerializeField]
	private float[] _channels;

	public float[] Channel
	{
		get
		{
			return _channels;
		}
		set
		{
			_channels = value;
		}
	}

	private void Reset()
	{
		_channels = new float[8];
		for (int i = 0; i < 8; i++)
		{
			_channels[i] = 1f;
		}
	}

	private void ChangeChannelCount(int numChannels)
	{
		float[] array = new float[numChannels];
		if (_channels != null && _channels.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (i < _channels.Length)
				{
					array[i] = _channels[i];
				}
				else
				{
					array[i] = 1f;
				}
			}
		}
		else
		{
			for (int j = 0; j < numChannels; j++)
			{
				array[j] = 1f;
			}
		}
		_channels = array;
	}

	private void OnAudioFilterRead(float[] data, int channels)
	{
		if (channels != _channels.Length)
		{
			ChangeChannelCount(channels);
		}
		int num = 0;
		int num2 = data.Length / channels;
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < channels; j++)
			{
				data[num] *= _channels[j];
				num++;
			}
		}
	}
}
