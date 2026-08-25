using System.Collections;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Script.Mrg;

public class InfoTopTipsMrg : SingletonAsMono<InfoTopTipsMrg>
{
	private Queue<string> _messageQueue = new Queue<string>();

	protected override void Awake()
	{
		base.Awake();
		StartCoroutine(UpdateInfoText());
	}

	public void Add(string message)
	{
		_messageQueue.Enqueue(message);
	}

	private IEnumerator UpdateInfoText()
	{
		while (true)
		{
			if (_messageQueue.Count > 0)
			{
				UiManager.ShowTopTips(_messageQueue.Dequeue());
				yield return new WaitForSeconds(2f);
			}
			else
			{
				yield return null;
			}
		}
	}
}
