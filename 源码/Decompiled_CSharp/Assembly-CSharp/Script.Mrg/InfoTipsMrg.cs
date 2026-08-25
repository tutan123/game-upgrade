using System.Collections;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Script.Mrg;

public class InfoTipsMrg : SingletonAsMono<InfoTipsMrg>
{
	private Queue<InfoTipsData> _messageQueue = new Queue<InfoTipsData>();

	protected override void Awake()
	{
		base.Awake();
		StartCoroutine(UpdateInfoText());
	}

	public void Add(string message, bool isAdd = false, string type = "nor")
	{
		_messageQueue.Enqueue(new InfoTipsData
		{
			Info = message,
			IsAdd = isAdd,
			Type = type
		});
	}

	private IEnumerator UpdateInfoText()
	{
		while (true)
		{
			if (_messageQueue.Count > 0)
			{
				InfoTipsData infoTipsData = _messageQueue.Dequeue();
				UiManager.ShowInfoTips(infoTipsData.Info, infoTipsData.IsAdd, infoTipsData.Type);
				yield return new WaitForSeconds(0.5f);
			}
			else
			{
				yield return null;
			}
		}
	}
}
