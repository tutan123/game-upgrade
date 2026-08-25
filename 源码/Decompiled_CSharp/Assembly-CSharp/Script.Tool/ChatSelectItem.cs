using System;
using FrameWork;
using FrameWork.ChatTool;
using Script.Mrg;
using TMPro;
using UnityEngine;

namespace Script.Tool;

public class ChatSelectItem : MonoBehaviour
{
	public TMP_Text text;

	private ChatNode _lastNode;

	private ChatNode _chatNode;

	private Action _clickAction;

	public void Init(ChatNode lastNode, ChatNode chatNode, Action action = null)
	{
		_lastNode = lastNode;
		_chatNode = chatNode;
		text.text = LanguageMrg.GetText(chatNode.selectName);
		_clickAction = action;
	}

	public void OnClick()
	{
		if (!_chatNode.fistPropertyDatas.IsSuc())
		{
			UiManager.ShowTips(LanguageMrg.GetText(_chatNode.languageKey));
			return;
		}
		for (int i = 0; i < _chatNode.propertyDatas.Count; i++)
		{
			_chatNode.propertyDatas[i].AddTypeValueAsShowTips(isCheck: false);
		}
		for (int j = 0; j < _chatNode.taskData.Count; j++)
		{
			FrameWork.Tool.SetTask(_chatNode.taskData[j]);
		}
		if (!string.IsNullOrEmpty(_chatNode.audioPath.path))
		{
			AudioClip audioClipPath = FrameWork.Tool.GetAudioClipPath(_chatNode.audioPath.path);
			SingletonAsMono<AudioMrg>.Instance.Play(audioClipPath);
		}
		SingletonAsMono<GameDataMrg>.Instance.SetMsgSelectId(_lastNode.uniqueID, _chatNode.uniqueID);
		_clickAction?.Invoke();
	}
}
