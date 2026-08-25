using System;
using System.Collections.Generic;

namespace RenderHeads.Media.AVProVideo;

public class AudioOutputManager
{
	protected class PlayerInstance
	{
		public HashSet<AudioOutput> outputs;

		public float[] pcmData;

		public bool isPcmDataReady;
	}

	private static AudioOutputManager _instance;

	private Dictionary<int, PlayerInstance> _instances;

	public static AudioOutputManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new AudioOutputManager();
			}
			return _instance;
		}
	}

	private AudioOutputManager()
	{
		_instances = new Dictionary<int, PlayerInstance>();
	}

	public void AddPlayerInstance(int mediaPlayerInstanceID)
	{
		_instances[mediaPlayerInstanceID] = new PlayerInstance
		{
			outputs = new HashSet<AudioOutput>(),
			pcmData = null
		};
	}

	public void RemovePlayerInstance(int mediaPlayerInstanceID)
	{
		if (_instances.ContainsKey(mediaPlayerInstanceID))
		{
			_instances.Remove(mediaPlayerInstanceID);
		}
	}

	public void RequestAudio(AudioOutput outputComponent, MediaPlayer mediaPlayer, int mediaPlayerInstanceID, float[] audioData, int audioChannelCount, int channelMask, AudioOutput.AudioOutputMode audioOutputMode, bool supportPositionalAudio)
	{
		if (mediaPlayer == null || mediaPlayer.Control == null)
		{
			if (supportPositionalAudio)
			{
				ZeroAudio(audioData, 0);
			}
			return;
		}
		int audioChannelCount2 = mediaPlayer.Control.GetAudioChannelCount();
		if (audioChannelCount2 <= 0)
		{
			if (supportPositionalAudio)
			{
				ZeroAudio(audioData, 0);
			}
			return;
		}
		PlayerInstance value = null;
		if (!_instances.TryGetValue(mediaPlayerInstanceID, out value))
		{
			Dictionary<int, PlayerInstance> instances = _instances;
			PlayerInstance obj = new PlayerInstance
			{
				outputs = new HashSet<AudioOutput>(),
				pcmData = null
			};
			PlayerInstance playerInstance = obj;
			instances[mediaPlayerInstanceID] = obj;
			value = playerInstance;
		}
		if (value.outputs.Count == 0 || value.outputs.Contains(outputComponent) || value.pcmData == null)
		{
			value.outputs.Clear();
			int num = audioData.Length * audioChannelCount2 / audioChannelCount;
			if (value.pcmData == null || num != value.pcmData.Length)
			{
				value.pcmData = new float[num];
			}
			value.isPcmDataReady = GrabAudio(mediaPlayer, value.pcmData, audioChannelCount2);
			value.outputs.Add(outputComponent);
		}
		if (value.isPcmDataReady)
		{
			int num2 = Math.Min(audioData.Length / audioChannelCount, value.pcmData.Length / audioChannelCount2);
			int num3 = 0;
			int num4 = 0;
			switch (audioOutputMode)
			{
			case AudioOutput.AudioOutputMode.MultipleChannels:
			{
				int num6 = Math.Min(audioChannelCount2, audioChannelCount);
				if (!supportPositionalAudio)
				{
					for (int n = 0; n < num2; n++)
					{
						for (int num7 = 0; num7 < num6; num7++)
						{
							if (((1 << num7) & channelMask) > 0)
							{
								audioData[num4 + num7] = value.pcmData[num3 + num7];
							}
						}
						num3 += audioChannelCount2;
						num4 += audioChannelCount;
					}
					break;
				}
				for (int num8 = 0; num8 < num2; num8++)
				{
					for (int num9 = 0; num9 < num6; num9++)
					{
						if (((1 << num9) & channelMask) > 0)
						{
							audioData[num4 + num9] *= value.pcmData[num3 + num9];
						}
					}
					num3 += audioChannelCount2;
					num4 += audioChannelCount;
				}
				break;
			}
			case AudioOutput.AudioOutputMode.OneToAllChannels:
			{
				int num5 = 0;
				for (int i = 0; i < 8; i++)
				{
					if ((channelMask & (1 << i)) > 0)
					{
						num5 = i;
						break;
					}
				}
				if (num5 >= audioChannelCount2)
				{
					break;
				}
				if (!supportPositionalAudio)
				{
					for (int j = 0; j < num2; j++)
					{
						for (int k = 0; k < audioChannelCount; k++)
						{
							audioData[num4 + k] = value.pcmData[num3 + num5];
						}
						num3 += audioChannelCount2;
						num4 += audioChannelCount;
					}
					break;
				}
				for (int l = 0; l < num2; l++)
				{
					for (int m = 0; m < audioChannelCount; m++)
					{
						audioData[num4 + m] *= value.pcmData[num3 + num5];
					}
					num3 += audioChannelCount2;
					num4 += audioChannelCount;
				}
				break;
			}
			}
			if (supportPositionalAudio && num4 != audioData.Length)
			{
				ZeroAudio(audioData, num4);
			}
		}
		else if (supportPositionalAudio)
		{
			ZeroAudio(audioData, 0);
		}
	}

	private void ZeroAudio(float[] audioData, int startPosition)
	{
		for (int i = startPosition; i < audioData.Length; i++)
		{
			audioData[i] = 0f;
		}
	}

	private bool GrabAudio(MediaPlayer player, float[] audioData, int channelCount)
	{
		return player.Control.GrabAudio(audioData, audioData.Length, channelCount) != 0;
	}
}
