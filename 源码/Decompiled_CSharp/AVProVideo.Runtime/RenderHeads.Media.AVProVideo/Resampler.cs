using System;
using System.Collections.Generic;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public class Resampler
{
	private class TimestampedRenderTexture
	{
		public RenderTexture texture;

		public long timestamp;

		public bool used;
	}

	public enum ResampleMode
	{
		POINT,
		LINEAR
	}

	private List<TimestampedRenderTexture[]> _buffer = new List<TimestampedRenderTexture[]>();

	private MediaPlayer _mediaPlayer;

	private RenderTexture[] _outputTexture;

	private int _start;

	private int _end;

	private int _bufferSize;

	private long _baseTimestamp;

	private float _elapsedTimeSinceBase;

	private Material _blendMat;

	private ResampleMode _resampleMode;

	private string _name = "";

	private long _lastTimeStamp = -1L;

	private int _droppedFrames;

	private long _lastDisplayedTimestamp;

	private int _frameDisplayedTimer;

	private long _currentDisplayedTimestamp;

	private const string ShaderPropT = "_t";

	private const string ShaderPropAftertex = "_AfterTex";

	private int _propAfterTex;

	private int _propT;

	private float _videoFrameRate;

	public int DroppedFrames => _droppedFrames;

	public int FrameDisplayedTimer => _frameDisplayedTimer;

	public long BaseTimestamp
	{
		get
		{
			return _baseTimestamp;
		}
		set
		{
			_baseTimestamp = value;
		}
	}

	public float ElapsedTimeSinceBase
	{
		get
		{
			return _elapsedTimeSinceBase;
		}
		set
		{
			_elapsedTimeSinceBase = value;
		}
	}

	public float LastT { get; private set; }

	public long TextureTimeStamp { get; private set; }

	public Texture[] OutputTexture => _outputTexture;

	public void OnVideoEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	{
		switch (et)
		{
		case MediaPlayerEvent.EventType.MetaDataReady:
			_videoFrameRate = mp.Info.GetVideoFrameRate();
			_elapsedTimeSinceBase = 0f;
			if (_videoFrameRate > 0f)
			{
				_elapsedTimeSinceBase = (float)_bufferSize / _videoFrameRate;
			}
			break;
		case MediaPlayerEvent.EventType.Closing:
			Reset();
			break;
		}
	}

	public Resampler(MediaPlayer player, string name, int bufferSize = 2, ResampleMode resampleMode = ResampleMode.LINEAR)
	{
		_bufferSize = Mathf.Max(2, bufferSize);
		player.Events.AddListener(OnVideoEvent);
		_mediaPlayer = player;
		Shader shader = Shader.Find("AVProVideo/Internal/BlendFrames");
		if (shader != null)
		{
			_blendMat = new Material(shader);
			_propT = Shader.PropertyToID("_t");
			_propAfterTex = Shader.PropertyToID("_AfterTex");
		}
		else
		{
			Debug.LogError("[AVProVideo] Failed to find BlendFrames shader");
		}
		_resampleMode = resampleMode;
		_name = name;
		Debug.Log("[AVProVideo] Resampler " + _name + " started");
	}

	public void Reset()
	{
		_lastTimeStamp = -1L;
		_baseTimestamp = 0L;
		InvalidateBuffer();
	}

	public void Release()
	{
		ReleaseRenderTextures();
		if (_blendMat != null)
		{
			if (Application.isPlaying)
			{
				UnityEngine.Object.Destroy(_blendMat);
			}
			else
			{
				UnityEngine.Object.DestroyImmediate(_blendMat);
			}
		}
	}

	private void ReleaseRenderTextures()
	{
		for (int i = 0; i < _buffer.Count; i++)
		{
			for (int j = 0; j < _buffer[i].Length; j++)
			{
				if (_buffer[i][j].texture != null)
				{
					RenderTexture.ReleaseTemporary(_buffer[i][j].texture);
					_buffer[i][j].texture = null;
				}
			}
			if (_outputTexture != null && _outputTexture[i] != null)
			{
				RenderTexture.ReleaseTemporary(_outputTexture[i]);
			}
		}
		_outputTexture = null;
	}

	private void ConstructRenderTextures()
	{
		ReleaseRenderTextures();
		_buffer.Clear();
		_outputTexture = new RenderTexture[_mediaPlayer.TextureProducer.GetTextureCount()];
		for (int i = 0; i < _mediaPlayer.TextureProducer.GetTextureCount(); i++)
		{
			Texture texture = _mediaPlayer.TextureProducer.GetTexture(i);
			_buffer.Add(new TimestampedRenderTexture[_bufferSize]);
			for (int j = 0; j < _bufferSize; j++)
			{
				_buffer[i][j] = new TimestampedRenderTexture();
			}
			for (int k = 0; k < _buffer[i].Length; k++)
			{
				_buffer[i][k].texture = RenderTexture.GetTemporary(texture.width, texture.height, 0);
				_buffer[i][k].timestamp = 0L;
				_buffer[i][k].used = false;
			}
			_outputTexture[i] = RenderTexture.GetTemporary(texture.width, texture.height, 0);
			_outputTexture[i].filterMode = texture.filterMode;
			_outputTexture[i].wrapMode = texture.wrapMode;
			_outputTexture[i].anisoLevel = texture.anisoLevel;
		}
	}

	private bool CheckRenderTexturesValid()
	{
		for (int i = 0; i < _mediaPlayer.TextureProducer.GetTextureCount(); i++)
		{
			Texture texture = _mediaPlayer.TextureProducer.GetTexture(i);
			for (int j = 0; j < _buffer.Count; j++)
			{
				if (_buffer[i][j].texture == null || _buffer[i][j].texture.width != texture.width || _buffer[i][j].texture.height != texture.height)
				{
					return false;
				}
			}
			if (_outputTexture == null || _outputTexture[i] == null || _outputTexture[i].width != texture.width || _outputTexture[i].height != texture.height)
			{
				return false;
			}
		}
		return true;
	}

	private int FindBeforeFrameIndex(int frameIdx)
	{
		if (frameIdx >= _buffer.Count)
		{
			return -1;
		}
		int num = -1;
		float num2 = float.MaxValue;
		int num3 = -1;
		float num4 = float.MaxValue;
		for (int i = 0; i < _buffer[frameIdx].Length; i++)
		{
			if (_buffer[frameIdx][i].used)
			{
				float num5 = (float)(_buffer[frameIdx][i].timestamp - _baseTimestamp) / 10000000f;
				if (num5 < num4)
				{
					num3 = i;
					num4 = num5;
				}
				float num6 = _elapsedTimeSinceBase - num5;
				if (num6 >= 0f && num6 < num2)
				{
					num2 = num6;
					num = i;
				}
			}
		}
		if (num < 0)
		{
			if (num3 < 0)
			{
				return -1;
			}
			return num3;
		}
		return num;
	}

	private int FindClosestFrame(int frameIdx)
	{
		if (frameIdx >= _buffer.Count)
		{
			return -1;
		}
		int result = -1;
		float num = float.MaxValue;
		for (int i = 0; i < _buffer[frameIdx].Length; i++)
		{
			if (_buffer[frameIdx][i].used)
			{
				float num2 = (float)(_buffer[frameIdx][i].timestamp - _baseTimestamp) / 10000000f;
				float num3 = Mathf.Abs(_elapsedTimeSinceBase - num2);
				if (num3 < num)
				{
					result = i;
					num = num3;
				}
			}
		}
		return result;
	}

	private void PointUpdate()
	{
		for (int i = 0; i < _buffer.Count; i++)
		{
			int num = FindClosestFrame(i);
			if (num >= 0)
			{
				_outputTexture[i].DiscardContents();
				Graphics.Blit(_buffer[i][num].texture, _outputTexture[i]);
				TextureTimeStamp = (_currentDisplayedTimestamp = _buffer[i][num].timestamp);
			}
		}
	}

	private void SampleFrame(int frameIdx, int bufferIdx)
	{
		_outputTexture[bufferIdx].DiscardContents();
		Graphics.Blit(_buffer[bufferIdx][frameIdx].texture, _outputTexture[bufferIdx]);
		TextureTimeStamp = (_currentDisplayedTimestamp = _buffer[bufferIdx][frameIdx].timestamp);
	}

	private void SampleFrames(int bufferIdx, int frameIdx1, int frameIdx2, float t)
	{
		_blendMat.SetFloat(_propT, t);
		_blendMat.SetTexture(_propAfterTex, _buffer[bufferIdx][frameIdx2].texture);
		_outputTexture[bufferIdx].DiscardContents();
		Graphics.Blit(_buffer[bufferIdx][frameIdx1].texture, _outputTexture[bufferIdx], _blendMat);
		TextureTimeStamp = (long)Mathf.Lerp(_buffer[bufferIdx][frameIdx1].timestamp, _buffer[bufferIdx][frameIdx2].timestamp, t);
		_currentDisplayedTimestamp = _buffer[bufferIdx][frameIdx1].timestamp;
	}

	private void LinearUpdate()
	{
		for (int i = 0; i < _buffer.Count; i++)
		{
			int num = FindBeforeFrameIndex(i);
			if (num < 0)
			{
				continue;
			}
			float num2 = (float)(_buffer[i][num].timestamp - _baseTimestamp) / 10000000f;
			if (num2 > _elapsedTimeSinceBase)
			{
				SampleFrame(num, i);
				LastT = -1f;
				continue;
			}
			int num3 = (num + 1) % _buffer[i].Length;
			float num4 = (float)(_buffer[i][num3].timestamp - _baseTimestamp) / 10000000f;
			if (num4 < num2)
			{
				SampleFrame(num, i);
				LastT = 2f;
				continue;
			}
			float num5 = num4 - num2;
			float num6 = (_elapsedTimeSinceBase - num2) / num5;
			SampleFrames(i, num, num3, num6);
			LastT = num6;
		}
	}

	private void InvalidateBuffer()
	{
		_elapsedTimeSinceBase = (float)(_bufferSize / 2) / _videoFrameRate;
		for (int i = 0; i < _buffer.Count; i++)
		{
			for (int j = 0; j < _buffer[i].Length; j++)
			{
				_buffer[i][j].used = false;
			}
		}
		_start = (_end = 0);
	}

	private float GuessFrameRate()
	{
		int num = 0;
		long num2 = 0L;
		for (int i = 0; i < _buffer[0].Length; i++)
		{
			if (!_buffer[0][i].used)
			{
				continue;
			}
			long num3 = long.MaxValue;
			for (int j = i + 1; j < _buffer[0].Length; j++)
			{
				if (_buffer[0][j].used)
				{
					long num4 = Math.Abs(_buffer[0][i].timestamp - _buffer[0][j].timestamp);
					if (num4 < num3)
					{
						num3 = num4;
					}
				}
			}
			if (num3 != long.MaxValue)
			{
				num2 += num3;
				num++;
			}
		}
		if (num > 1)
		{
			num2 /= num;
		}
		return 10000000f / (float)num2;
	}

	public void Update()
	{
		if (_mediaPlayer.TextureProducer == null || _mediaPlayer.TextureProducer == null || _mediaPlayer.TextureProducer.GetTexture() == null)
		{
			return;
		}
		if (!CheckRenderTexturesValid())
		{
			ConstructRenderTextures();
		}
		long textureTimeStamp = _mediaPlayer.TextureProducer.GetTextureTimeStamp();
		if (textureTimeStamp != _lastTimeStamp)
		{
			float num = Mathf.Abs(textureTimeStamp - _lastTimeStamp);
			float num2 = 10000000f / _videoFrameRate;
			if (num > num2 * 1.1f && num < num2 * 3.1f)
			{
				_droppedFrames += (int)((double)((num - num2) / num2) + 0.5);
			}
			_lastTimeStamp = textureTimeStamp;
		}
		long textureTimeStamp2 = _mediaPlayer.TextureProducer.GetTextureTimeStamp();
		bool flag = !_mediaPlayer.Control.IsSeeking();
		if (_start != _end || _buffer[0][_end].used)
		{
			int num3 = (_end + _buffer[0].Length - 1) % _buffer[0].Length;
			if (textureTimeStamp2 == _buffer[0][num3].timestamp)
			{
				flag = false;
			}
		}
		bool flag2 = _start != _end || !_buffer[0][_end].used;
		if (flag)
		{
			if (_start == _end && !_buffer[0][_end].used)
			{
				_baseTimestamp = textureTimeStamp2;
			}
			if (_end == _start && _buffer[0][_end].used)
			{
				_start = (_start + 1) % _buffer[0].Length;
			}
			for (int i = 0; i < _mediaPlayer.TextureProducer.GetTextureCount(); i++)
			{
				Texture texture = _mediaPlayer.TextureProducer.GetTexture(i);
				_buffer[i][_end].texture.DiscardContents();
				Graphics.Blit(texture, _buffer[i][_end].texture);
				_buffer[i][_end].timestamp = textureTimeStamp2;
				_buffer[i][_end].used = true;
			}
			_end = (_end + 1) % _buffer[0].Length;
		}
		bool flag3 = _start != _end || !_buffer[0][_end].used;
		if (flag3)
		{
			for (int j = 0; j < _buffer.Count; j++)
			{
				_outputTexture[j].DiscardContents();
				Graphics.Blit(_buffer[j][_start].texture, _outputTexture[j]);
				_currentDisplayedTimestamp = _buffer[j][_start].timestamp;
			}
		}
		else if (flag2 && _videoFrameRate <= 0f)
		{
			_videoFrameRate = GuessFrameRate();
			_elapsedTimeSinceBase = (float)(_bufferSize / 2) / _videoFrameRate;
		}
		if (_mediaPlayer.Control.IsPaused())
		{
			InvalidateBuffer();
		}
		if (!flag3 && _mediaPlayer.Control.IsPlaying() && !_mediaPlayer.Control.IsFinished())
		{
			long num4 = _buffer[0][(_start + _bufferSize / 2) % _bufferSize].timestamp - _baseTimestamp;
			double num5 = Mathf.Abs((float)((double)_elapsedTimeSinceBase * 10000000.0) - (float)num4);
			double num6 = (float)(_buffer[0].Length / 2) / _videoFrameRate * 10000000f;
			if (num5 > num6)
			{
				_elapsedTimeSinceBase = (float)num4 / 10000000f;
			}
			if (_resampleMode == ResampleMode.POINT)
			{
				PointUpdate();
			}
			else if (_resampleMode == ResampleMode.LINEAR)
			{
				LinearUpdate();
			}
			_elapsedTimeSinceBase += Time.unscaledDeltaTime;
		}
	}

	public void UpdateTimestamp()
	{
		if (_lastDisplayedTimestamp != _currentDisplayedTimestamp)
		{
			_lastDisplayedTimestamp = _currentDisplayedTimestamp;
			_frameDisplayedTimer = 0;
		}
		_frameDisplayedTimer++;
	}
}
