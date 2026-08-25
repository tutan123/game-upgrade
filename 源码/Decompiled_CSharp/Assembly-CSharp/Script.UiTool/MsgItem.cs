using System.Collections.Generic;
using System.Linq;
using FrameWork;
using FrameWork.ChatTool;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.UiTool;

public class MsgItem : MonoBehaviour
{
	public Image icon;

	public TMP_Text roleName;

	public TMP_Text info;

	public Image red;

	private Xlsx_Message _xlsxMessage;

	private List<ChatNode> _chatNodes = new List<ChatNode>();

	public void Init(Xlsx_Message xlsxMessage)
	{
		icon.sprite = (string.IsNullOrEmpty(xlsxMessage.Icon) ? null : ABMrg.Load<Sprite>(xlsxMessage.Icon));
		_xlsxMessage = xlsxMessage;
		roleName.text = LanguageMrg.GetText(xlsxMessage.Name);
		List<MessageData> chat = SingletonAsMono<GameDataMrg>.Instance.GetChat(xlsxMessage.Key);
		_chatNodes.Clear();
		red.SetActive(SingletonAsMono<GameDataMrg>.Instance.IsShowChatRed(xlsxMessage.Key));
		MessageData messageData = null;
		for (int i = 0; i < chat.Count; i++)
		{
			_chatNodes.AddRange(chat[i].GetAllChat());
			if (chat[i].msgType == ChatMsgType.UnRead)
			{
				messageData = chat[i];
				break;
			}
		}
		if (messageData == null)
		{
			if (_chatNodes.Count != 0)
			{
				ChatNode chatNode = _chatNodes.Last();
				if (chatNode.mType == MType.Image)
				{
					info.text = LanguageMrg.GetText("A2477");
				}
				else if (chatNode.mType == MType.Video)
				{
					info.text = LanguageMrg.GetText("A2653");
				}
				else
				{
					info.text = LanguageMrg.GetText(chatNode.msg);
				}
			}
			return;
		}
		List<ChatNode> allChat = messageData.GetAllChat();
		if (allChat.Count == 0)
		{
			return;
		}
		if (messageData.msgType == ChatMsgType.UnRead)
		{
			ChatNode chatNode2 = allChat[0];
			if (chatNode2.mType == MType.Image)
			{
				info.text = LanguageMrg.GetText("A2477");
			}
			else if (chatNode2.mType == MType.Video)
			{
				info.text = LanguageMrg.GetText("A2653");
			}
			else
			{
				info.text = LanguageMrg.GetText(chatNode2.msg);
			}
		}
		else
		{
			ChatNode chatNode3 = allChat.Last();
			if (chatNode3.mType == MType.Image)
			{
				info.text = LanguageMrg.GetText("A2477");
			}
			else if (chatNode3.mType == MType.Video)
			{
				info.text = LanguageMrg.GetText("A2653");
			}
			else
			{
				info.text = LanguageMrg.GetText(chatNode3.msg);
			}
		}
	}

	private void OnEnable()
	{
		EventManager.AddListener(MessageType.Game, GameMessageType.UpdateChatRed, UpdateRed);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.UpdateChatRed, UpdateRed);
	}

	private void UpdateRed(List<object> objects)
	{
		red.SetActive(SingletonAsMono<GameDataMrg>.Instance.IsShowChatRed(_xlsxMessage.Key));
	}

	public void OnClick()
	{
		UiManager.GetUi<MainWindows>().OpenMessage().InitChat(_xlsxMessage);
	}
}
