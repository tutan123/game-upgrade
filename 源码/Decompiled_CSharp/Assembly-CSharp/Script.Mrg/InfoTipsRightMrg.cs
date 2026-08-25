using System.Collections;
using System.Collections.Generic;
using FrameWork;

namespace Script.Mrg;

public class InfoTipsRightMrg : SingletonAsMono<InfoTipsRightMrg>
{
	private Queue<TaskTipsData> _messageQueue = new Queue<TaskTipsData>();

	protected override void Awake()
	{
		base.Awake();
		StartCoroutine(UpdateInfoText());
	}

	public void Add(TaskTipsData message)
	{
		_messageQueue.Enqueue(message);
	}

	private IEnumerator UpdateInfoText()
	{
		while (true)
		{
			if (_messageQueue.Count > 0)
			{
				UiManager.ShowInfoTipsRight(_messageQueue.Dequeue());
			}
			else
			{
				yield return null;
			}
		}
	}
}
