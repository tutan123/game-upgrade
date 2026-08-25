using System.Collections;
using FrameWork;
using FrameWork.Data;
using UnityEngine;

namespace Script.Mrg;

public class AudioMrg : SingletonAsMono<AudioMrg>
{
	private ObjectPool<AudioSource> _audioSourcePool = new ObjectPool<AudioSource>(() => new GameObject("AudioSource").AddComponent<AudioSource>());

	public AudioSource Play(AudioClip audioClip, bool loop = false)
	{
		if (audioClip == null)
		{
			return null;
		}
		AudioSource source = _audioSourcePool.DeQueue();
		SingletonAsMono<FrameWork.Mono>.Instance.StartCoroutine(PlayAudio());
		return source;
		IEnumerator PlayAudio()
		{
			source.transform.SetParent(base.transform);
			source.transform.localPosition = Vector3.zero;
			source.gameObject.SetActiveAsCheck(active: true);
			source.clip = audioClip;
			source.volume = GameData.GetOpenAsNum("Sound");
			source.playOnAwake = false;
			source.loop = loop;
			source.PlayScheduled(AudioSettings.dspTime);
			if (!loop)
			{
				yield return new WaitForSeconds(audioClip.length);
				_audioSourcePool.EnQueue(source);
				source.gameObject.SetActiveAsCheck(active: false);
			}
			yield return null;
		}
	}

	public void Enqueue(AudioSource audioSource)
	{
		if (!(audioSource == null))
		{
			_audioSourcePool.EnQueue(audioSource);
			audioSource.gameObject.SetActiveAsCheck(active: false);
		}
	}
}
