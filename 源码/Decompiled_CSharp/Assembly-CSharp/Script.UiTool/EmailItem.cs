using System.Collections.Generic;
using FrameWork;
using FrameWork.ChatTool;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.UiTool;

public class EmailItem : MonoBehaviour
{
	public GameObject left;

	public TMP_Text leftText;

	public Image leftImg;

	public Image leftBg;

	public Image leftVideoBg;

	public CheckTextWidth leftCheckTextWidth;

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
		leftText.SetActive(active: false);
		leftImg.SetActive(active: false);
		leftVideoBg.SetActive(active: false);
		leftBg.enabled = false;
		br.SetActiveAsCheck(chatNode.isEnd);
		if (chatNode.mType == MType.Text)
		{
			leftText.SetActive(active: true);
			left.SetActiveAsCheck(active: true);
			leftText.text = LanguageMrg.GetText(chatNode.msg);
			leftCheckTextWidth.UpdateTextSize();
		}
		else if (chatNode.mType == MType.Video)
		{
			leftBg.enabled = false;
			left.SetActiveAsCheck(active: true);
			leftVideoBg.SetActive(active: true);
			leftVideoBg.sprite = chatNode.fenMian;
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
			leftVideoBg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, num), (float)num2 / num3);
			leftBg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, num), (float)num2 / num3);
		}
		else
		{
			leftBg.enabled = false;
			leftImg.SetActive(active: true);
			left.SetActiveAsCheck(active: true);
			leftImg.sprite = chatNode.img;
			int width = chatNode.img.texture.width;
			int height = chatNode.img.texture.height;
			float num4 = 1f;
			if ((float)width > 150f)
			{
				num4 = (float)width / 150f;
			}
			leftImg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, width), (float)height / num4);
			leftBg.rectTransform.sizeDelta = new Vector2(Mathf.Min(150f, width), (float)height / num4);
		}
		for (int i = 0; i < updateRect.Count; i++)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(updateRect[i]);
		}
		for (int j = 0; j < _chatNode.taskData.Count; j++)
		{
			FrameWork.Tool.SetTask(chatNode.taskData[j]);
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
		UiManager.OpenUi<ImgShowWindows>().Init(leftImg.sprite);
	}
}
