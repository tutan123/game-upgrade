using System.Collections.Generic;
using FrameWork;
using FrameWork.ChatTool;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.UiTool;

public class ChatItem : MonoBehaviour
{
	public GameObject left;

	public GameObject right;

	public Image leftIcon;

	public TMP_Text leftText;

	public Image leftImg;

	public Image leftBg;

	public Image leftVideoBg;

	public CheckTextWidth leftCheckTextWidth;

	public Image rightIcon;

	public TMP_Text rightText;

	public Image rightImg;

	public Image rightBg;

	public Image rightVideoBg;

	public CheckTextWidth rightCheckTextWidth;

	private Xlsx_Message _xlsxMessage;

	private ChatNode _chatNode;

	private Xlsx_Message _selfMsg;

	public List<RectTransform> updateRect;

	public GameObject br;

	private ContentSizeFitter _contentSizeFitter;

	public void Init(Xlsx_Message xlsxMessage, ChatNode chatNode)
	{
		_xlsxMessage = xlsxMessage;
		_chatNode = chatNode;
		_selfMsg = Xlsx_Message_Query.data[0];
		left.SetActiveAsCheck(active: false);
		right.SetActiveAsCheck(active: false);
		leftText.SetActive(active: false);
		leftImg.SetActive(active: false);
		leftVideoBg.SetActive(active: false);
		leftBg.enabled = true;
		rightText.SetActive(active: false);
		rightImg.SetActive(active: false);
		rightVideoBg.SetActive(active: false);
		rightBg.enabled = true;
		leftIcon.sprite = (string.IsNullOrEmpty(xlsxMessage.Icon) ? null : ABMrg.Load<Sprite>(xlsxMessage.Icon));
		rightIcon.sprite = (string.IsNullOrEmpty(_selfMsg.Icon) ? null : ABMrg.Load<Sprite>(_selfMsg.Icon));
		br.SetActiveAsCheck(chatNode.isEnd);
		if ((bool)chatNode.icon)
		{
			leftIcon.sprite = chatNode.icon;
		}
		if (chatNode.mType == MType.Text)
		{
			if (chatNode.isSelfMsg)
			{
				rightText.SetActive(active: true);
				right.SetActiveAsCheck(active: true);
				rightText.text = LanguageMrg.GetText(chatNode.msg);
				rightCheckTextWidth.UpdateTextSize();
			}
			else
			{
				leftText.SetActive(active: true);
				left.SetActiveAsCheck(active: true);
				leftText.text = LanguageMrg.GetText(chatNode.msg);
				leftCheckTextWidth.UpdateTextSize();
			}
		}
		else if (chatNode.mType == MType.Video)
		{
			if (chatNode.isSelfMsg)
			{
				rightBg.enabled = false;
				right.SetActiveAsCheck(active: true);
				rightVideoBg.SetActive(active: true);
				rightVideoBg.sprite = chatNode.fenMian;
				int num = 150;
				int num2 = 150;
				if ((bool)chatNode.fenMian)
				{
					num = chatNode.fenMian.texture.width;
					num2 = chatNode.fenMian.texture.height;
				}
				float num3 = 1f;
				if ((float)num > 150f)
				{
					num3 = (float)num / 150f;
				}
				rightVideoBg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, num), (float)num2 / num3);
				rightBg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, num), (float)num2 / num3);
			}
			else
			{
				leftBg.enabled = false;
				left.SetActiveAsCheck(active: true);
				leftVideoBg.SetActive(active: true);
				leftVideoBg.sprite = chatNode.fenMian;
				int num4 = 150;
				int num5 = 150;
				if ((bool)chatNode.fenMian)
				{
					num4 = chatNode.fenMian.texture.width;
					num5 = chatNode.fenMian.texture.height;
				}
				float num6 = 1f;
				if ((float)num4 > 150f)
				{
					num6 = (float)num4 / 150f;
				}
				leftVideoBg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, num4), (float)num5 / num6);
				leftBg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, num4), (float)num5 / num6);
			}
		}
		else if (chatNode.isSelfMsg)
		{
			rightBg.enabled = false;
			rightImg.SetActive(active: true);
			right.SetActiveAsCheck(active: true);
			rightImg.sprite = chatNode.img;
			int width = chatNode.img.texture.width;
			int height = chatNode.img.texture.height;
			float num7 = 1f;
			if ((float)width > 150f)
			{
				num7 = (float)width / 150f;
			}
			rightImg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, width), (float)height / num7);
			rightBg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, width), (float)height / num7);
		}
		else
		{
			leftBg.enabled = false;
			leftImg.SetActive(active: true);
			left.SetActiveAsCheck(active: true);
			leftImg.sprite = chatNode.img;
			int width2 = chatNode.img.texture.width;
			int height2 = chatNode.img.texture.height;
			float num8 = 1f;
			if ((float)width2 > 150f)
			{
				num8 = (float)width2 / 150f;
			}
			leftImg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, width2), (float)height2 / num8);
			leftBg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, width2), (float)height2 / num8);
		}
		for (int i = 0; i < updateRect.Count; i++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(updateRect[i]);
		}
		for (int j = 0; j < _chatNode.taskData.Count; j++)
		{
			if (SingletonAsMono<GameDataMrg>.Instance.GetChatCount(chatNode.uniqueID) <= 0)
			{
				FrameWork.Tool.SetTask(chatNode.taskData[j]);
				SingletonAsMono<GameDataMrg>.Instance.AddChatCount(chatNode.uniqueID);
			}
		}
	}

	public void PlayVideo()
	{
		if (_chatNode.mType == MType.Video)
		{
			UiManager.OpenUi<PlayVideoWindows>().Init(_chatNode.videoClipPath);
		}
	}

	public void OpenImg()
	{
		if (_chatNode.isSelfMsg)
		{
			UiManager.OpenUi<ImgShowWindows>().Init(rightImg.sprite);
		}
		else
		{
			UiManager.OpenUi<ImgShowWindows>().Init(leftImg.sprite);
		}
	}
}
