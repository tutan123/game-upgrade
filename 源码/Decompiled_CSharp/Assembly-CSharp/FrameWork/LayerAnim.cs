using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace FrameWork;

public class LayerAnim
{
	private PlayableGraph _playableGraph;

	private int lastMixProt = -1;

	private int lastLayerProt = -1;

	private int curMixProt = -1;

	private int curLayerProt = -1;

	private float _lefpSpeed = 1f;

	private AnimationMixerPlayable _mixerPlayable;

	private ConcurrentDictionary<string, int> _layerAnims;

	private ConcurrentDictionary<int, AnimationClipPlayable> _animationClipPlayables;

	private List<float> _animLenghts;

	private StringBuilder _curAnim;

	private float _curAnimPlayLenght;

	private AnimationClipPlayable _curPlayable;

	private int _animIndex;

	private AnimationClipPlayable _clipPlayable;

	private int _prot = -1;

	private int _port;

	private float _mixlerp = 1f;

	public LayerAnim()
	{
		_curAnim = new StringBuilder();
		_layerAnims = new ConcurrentDictionary<string, int>();
		_animLenghts = new List<float>();
		_animationClipPlayables = new ConcurrentDictionary<int, AnimationClipPlayable>();
	}

	public AnimationMixerPlayable GetMixer()
	{
		return _mixerPlayable;
	}

	public void Init(AnimLayerData animLayerData, PlayableGraph playableGraph, Action<string, int> init, Action<AnimationMixerPlayable> end)
	{
		_playableGraph = playableGraph;
		_mixerPlayable = AnimationMixerPlayable.Create(_playableGraph, animLayerData.AnimData.Count);
		for (int i = 0; i < animLayerData.AnimData.Count; i++)
		{
			AnimationClip animationClip = animLayerData.AnimData[i];
			_animLenghts.Add(animationClip.length);
			AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(_playableGraph, animationClip);
			_animationClipPlayables.TryAdd(i, animationClipPlayable);
			init?.Invoke(animationClip.name, i);
			_layerAnims.TryAdd(animationClip.name, i);
			_playableGraph.Connect(animationClipPlayable, 0, _mixerPlayable, i);
			if (i == 0)
			{
				_mixerPlayable.SetInputWeight(0, 1f);
			}
		}
		end?.Invoke(_mixerPlayable);
		if (_mixerPlayable.GetInputCount() > 0)
		{
			_mixerPlayable.SetInputWeight(0, 1f);
			lastMixProt = 0;
			curMixProt = 0;
			_mixlerp = 1f;
		}
		_curAnimPlayLenght = 0f;
	}

	public void Init(PlayableGraph playableGraph)
	{
		_playableGraph = playableGraph;
		_mixerPlayable = AnimationMixerPlayable.Create(_playableGraph, 1);
		_curAnimPlayLenght = 0f;
	}

	public void SetSpeed(float speed)
	{
		_mixerPlayable.SetSpeed(speed);
	}

	public void Update()
	{
		LerpAnim();
	}

	private void SetAnim(int port, float lerpSpeed = 1f, bool isSetTime = false)
	{
		if (!_animationClipPlayables.TryGetValue(port, out _clipPlayable))
		{
			return;
		}
		if (isSetTime)
		{
			_clipPlayable.SetTime(0.0);
		}
		_curAnimPlayLenght = 0f;
		if (_mixlerp >= 1f)
		{
			_mixlerp = 0f;
			_lefpSpeed = lerpSpeed;
			if (curMixProt != -1)
			{
				lastMixProt = curMixProt;
			}
			else
			{
				_mixlerp = 1f;
			}
			curMixProt = port;
		}
		else
		{
			_prot = lastMixProt;
			_curAnimPlayLenght = _animLenghts[port] * _mixlerp;
			_lefpSpeed = lerpSpeed;
			lastMixProt = curMixProt;
			curMixProt = port;
			_mixlerp = 1f - _mixlerp;
			if (_prot != -1 && _prot != port && _mixlerp != 1f)
			{
				_mixerPlayable.SetInputWeight(_prot, 0f);
			}
		}
		if (lerpSpeed == -1f)
		{
			_mixlerp = 1f;
			_lefpSpeed = 1f;
			_clipPlayable.SetTime(0.0);
		}
	}

	public void SetAnim(AnimationClip animationClip, float lerpSpeed = 1f, bool isSetTime = false)
	{
		int inputCount = _mixerPlayable.GetInputCount();
		if (_layerAnims.TryAdd(animationClip.name, _layerAnims.Count))
		{
			_animLenghts.Add(animationClip.length);
			AnimationClipPlayable animationClipPlayable = AnimationClipPlayable.Create(_playableGraph, animationClip);
			_playableGraph.Connect(animationClipPlayable, 0, _mixerPlayable, _layerAnims.Count - 1);
			_mixerPlayable.SetInputCount(inputCount + 1);
			_curPlayable = animationClipPlayable;
			_animationClipPlayables.TryAdd(_layerAnims.Count - 1, animationClipPlayable);
			SetAnim(_layerAnims.Count - 1, lerpSpeed, isSetTime);
		}
		else
		{
			SetAnim(_layerAnims[animationClip.name], lerpSpeed, isSetTime);
		}
	}

	public void SetAnim(string animName, float lerpSpeed = 1f, bool isSetTime = false)
	{
		if (_layerAnims.TryGetValue(animName, out _port))
		{
			SetAnim(_port, lerpSpeed, isSetTime);
			_curAnim.Clear();
			_curAnim.Append(animName);
		}
	}

	private void LerpAnim()
	{
		_mixlerp += Time.deltaTime * _lefpSpeed;
		_mixlerp = Mathf.Clamp(_mixlerp, 0f, 1f);
		_curAnimPlayLenght += Time.deltaTime;
		if (lastMixProt != -1)
		{
			_mixerPlayable.SetInputWeight(lastMixProt, 1f - _mixlerp);
		}
		if (curMixProt != -1)
		{
			_mixerPlayable.SetInputWeight(curMixProt, _mixlerp);
		}
	}

	public float GetLerp()
	{
		return _mixlerp;
	}

	public bool IsGreater(float value)
	{
		if (_animationClipPlayables.TryGetValue(curMixProt, out var _))
		{
			return _curAnimPlayLenght >= value;
		}
		return true;
	}

	public float GetCurAnimPlayLenght()
	{
		return _curAnimPlayLenght;
	}
}
