using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DG.Tweening;
using FrameWork.ChatTool;
using Script.Audio;
using Script.Mrg;
using Script.Tool;
using Script.UiTool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "EmailWindows")]
public class EmailWindows : UiActor
{
	private bool _isChat;

	private Tweener _moveTweener;

	public RectTransform RectTransformChat;

	public CanvasRenderer CanvasRendererChat;

	public AddScripts AddScriptsChat;

	public RectTransform RectTransformChatName;

	public CanvasRenderer CanvasRendererChatName;

	public TextMeshProUGUI TextMeshProUGUIChatName;

	public AddScripts AddScriptsChatName;

	public RectTransform RectTransformTitle2;

	public CanvasRenderer CanvasRendererTitle2;

	public TextMeshProUGUI TextMeshProUGUITitle2;

	public AddScripts AddScriptsTitle2;

	public RectTransform RectTransformScrollView;

	public CanvasRenderer CanvasRendererScrollView;

	public Image ImageScrollView;

	public ScrollRect ScrollRectScrollView;

	public AddScripts AddScriptsScrollView;

	public RectTransform RectTransformChatContent;

	public ContentSizeFitter ContentSizeFitterChatContent;

	public VerticalLayoutGroup VerticalLayoutGroupChatContent;

	public AddScripts AddScriptsChatContent;

	public RectTransform RectTransformSelectContent;

	public ContentSizeFitter ContentSizeFitterSelectContent;

	public AddScripts AddScriptsSelectContent;

	public HorizontalLayoutGroup HorizontalLayoutGroupSelectContent;

	public RectTransform RectTransformXinBg;

	public CanvasRenderer CanvasRendererXinBg;

	public Image ImageXinBg;

	public AddScripts AddScriptsXinBg;

	public RectTransform RectTransformXinAg;

	public CanvasRenderer CanvasRendererXinAg;

	public Image ImageXinAg;

	public AddScripts AddScriptsXinAg;

	public RectTransform RectTransformXinZhi;

	public CanvasRenderer CanvasRendererXinZhi;

	public TextMeshProUGUI TextMeshProUGUIXinZhi;

	public AddScripts AddScriptsXinZhi;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformCloseText;

	public CanvasRenderer CanvasRendererCloseText;

	public TextMeshProUGUI TextMeshProUGUICloseText;

	public AddScripts AddScriptsCloseText;

	public override void Awake()
	{
		base.Awake();
		RectTransformChat = GetGameObject().transform.Find("Vieww/Chat/").GetComponent<RectTransform>();
		CanvasRendererChat = GetGameObject().transform.Find("Vieww/Chat/").GetComponent<CanvasRenderer>();
		AddScriptsChat = GetGameObject().transform.Find("Vieww/Chat/").GetComponent<AddScripts>();
		RectTransformChatName = GetGameObject().transform.Find("Vieww/Chat/Top/ChatName/").GetComponent<RectTransform>();
		CanvasRendererChatName = GetGameObject().transform.Find("Vieww/Chat/Top/ChatName/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIChatName = GetGameObject().transform.Find("Vieww/Chat/Top/ChatName/").GetComponent<TextMeshProUGUI>();
		AddScriptsChatName = GetGameObject().transform.Find("Vieww/Chat/Top/ChatName/").GetComponent<AddScripts>();
		RectTransformTitle2 = GetGameObject().transform.Find("Vieww/Chat/Top/Title2/").GetComponent<RectTransform>();
		CanvasRendererTitle2 = GetGameObject().transform.Find("Vieww/Chat/Top/Title2/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITitle2 = GetGameObject().transform.Find("Vieww/Chat/Top/Title2/").GetComponent<TextMeshProUGUI>();
		AddScriptsTitle2 = GetGameObject().transform.Find("Vieww/Chat/Top/Title2/").GetComponent<AddScripts>();
		RectTransformScrollView = GetGameObject().transform.Find("Vieww/Chat/Scroll View/").GetComponent<RectTransform>();
		CanvasRendererScrollView = GetGameObject().transform.Find("Vieww/Chat/Scroll View/").GetComponent<CanvasRenderer>();
		ImageScrollView = GetGameObject().transform.Find("Vieww/Chat/Scroll View/").GetComponent<Image>();
		ScrollRectScrollView = GetGameObject().transform.Find("Vieww/Chat/Scroll View/").GetComponent<ScrollRect>();
		AddScriptsScrollView = GetGameObject().transform.Find("Vieww/Chat/Scroll View/").GetComponent<AddScripts>();
		RectTransformChatContent = GetGameObject().transform.Find("Vieww/Chat/Scroll View/Viewport/ChatContent/").GetComponent<RectTransform>();
		ContentSizeFitterChatContent = GetGameObject().transform.Find("Vieww/Chat/Scroll View/Viewport/ChatContent/").GetComponent<ContentSizeFitter>();
		VerticalLayoutGroupChatContent = GetGameObject().transform.Find("Vieww/Chat/Scroll View/Viewport/ChatContent/").GetComponent<VerticalLayoutGroup>();
		AddScriptsChatContent = GetGameObject().transform.Find("Vieww/Chat/Scroll View/Viewport/ChatContent/").GetComponent<AddScripts>();
		RectTransformSelectContent = GetGameObject().transform.Find("Vieww/Chat/SelectChat/Scroll View/Viewport/SelectContent/").GetComponent<RectTransform>();
		ContentSizeFitterSelectContent = GetGameObject().transform.Find("Vieww/Chat/SelectChat/Scroll View/Viewport/SelectContent/").GetComponent<ContentSizeFitter>();
		AddScriptsSelectContent = GetGameObject().transform.Find("Vieww/Chat/SelectChat/Scroll View/Viewport/SelectContent/").GetComponent<AddScripts>();
		HorizontalLayoutGroupSelectContent = GetGameObject().transform.Find("Vieww/Chat/SelectChat/Scroll View/Viewport/SelectContent/").GetComponent<HorizontalLayoutGroup>();
		RectTransformXinBg = GetGameObject().transform.Find("Vieww/Chat/XinBg/").GetComponent<RectTransform>();
		CanvasRendererXinBg = GetGameObject().transform.Find("Vieww/Chat/XinBg/").GetComponent<CanvasRenderer>();
		ImageXinBg = GetGameObject().transform.Find("Vieww/Chat/XinBg/").GetComponent<Image>();
		AddScriptsXinBg = GetGameObject().transform.Find("Vieww/Chat/XinBg/").GetComponent<AddScripts>();
		RectTransformXinAg = GetGameObject().transform.Find("Vieww/Chat/XinBg/XinAg/").GetComponent<RectTransform>();
		CanvasRendererXinAg = GetGameObject().transform.Find("Vieww/Chat/XinBg/XinAg/").GetComponent<CanvasRenderer>();
		ImageXinAg = GetGameObject().transform.Find("Vieww/Chat/XinBg/XinAg/").GetComponent<Image>();
		AddScriptsXinAg = GetGameObject().transform.Find("Vieww/Chat/XinBg/XinAg/").GetComponent<AddScripts>();
		RectTransformXinZhi = GetGameObject().transform.Find("Vieww/Chat/XinBg/XinZhi/").GetComponent<RectTransform>();
		CanvasRendererXinZhi = GetGameObject().transform.Find("Vieww/Chat/XinBg/XinZhi/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIXinZhi = GetGameObject().transform.Find("Vieww/Chat/XinBg/XinZhi/").GetComponent<TextMeshProUGUI>();
		AddScriptsXinZhi = GetGameObject().transform.Find("Vieww/Chat/XinBg/XinZhi/").GetComponent<AddScripts>();
		RectTransformClose = GetGameObject().transform.Find("Vieww/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("Vieww/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("Vieww/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("Vieww/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("Vieww/Close/").GetComponent<AddScripts>();
		RectTransformCloseText = GetGameObject().transform.Find("Vieww/Close/CloseText/").GetComponent<RectTransform>();
		CanvasRendererCloseText = GetGameObject().transform.Find("Vieww/Close/CloseText/").GetComponent<CanvasRenderer>();
		TextMeshProUGUICloseText = GetGameObject().transform.Find("Vieww/Close/CloseText/").GetComponent<TextMeshProUGUI>();
		AddScriptsCloseText = GetGameObject().transform.Find("Vieww/Close/CloseText/").GetComponent<AddScripts>();
	}

	public EmailWindows(Transform trans)
		: base(trans)
	{
	}

	public EmailWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(Close);
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		List<Xlsx_Message> list = Xlsx_Message_Query.data.Where((Xlsx_Message message) => message.Type == 1).ToList();
		BTClose.SetActive(active: false);
		if (list.Count > 0)
		{
			InitChat(list[0]);
		}
	}

	public void InitChat(Xlsx_Message xlsxMessage)
	{
		TextMeshProUGUIChatName.text = LanguageMrg.GetText(xlsxMessage.Name);
		Tool.HideAllChild(RectTransformSelectContent);
		_isChat = true;
		SingletonAsMono<GameDataMrg>.Instance.SetShowChatRed(xlsxMessage.Key, show: false);
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.UpdateChatRed);
		List<MessageData> chat = SingletonAsMono<GameDataMrg>.Instance.GetChat(xlsxMessage.Key);
		if (chat.Count == 0)
		{
			return;
		}
		List<ChatNode> readChatNodes = new List<ChatNode>();
		List<MessageData> list = new List<MessageData>();
		MessageData messageData = chat.Last();
		ChatGraph chatGraph = ABMrg.Load<ChatGraph>(messageData.assetName);
		StartChat startChat = null;
		for (int j = 0; j < chatGraph.nodes.Count; j++)
		{
			if (chatGraph.nodes[j] is StartChat)
			{
				startChat = chatGraph.nodes[j] as StartChat;
				break;
			}
		}
		if (startChat != null)
		{
			TextMeshProUGUITitle2.text = string.Format(LanguageMrg.GetText("A4375"), LanguageMrg.GetText(startChat.xlsxLanguageKey));
		}
		if (messageData.msgType == ChatMsgType.Read)
		{
			readChatNodes.AddRange(messageData.GetAllChat());
			TextMeshProUGUICloseText.text = LanguageMrg.GetText("A5495");
		}
		else
		{
			list.Add(messageData);
			TextMeshProUGUICloseText.text = LanguageMrg.GetText("A5494");
		}
		RectTransformChatContent.TranFor(readChatNodes.Count, RectTransformChatContent.GetChild(0), delegate(int i, GameObject o)
		{
			o.GetComponent<EmailItem>().Init(xlsxMessage, readChatNodes[i]);
		});
		RectTransformXinBg.SetActive(!string.IsNullOrEmpty(xlsxMessage.PropertyName));
		if (!string.IsNullOrEmpty(xlsxMessage.PropertyName))
		{
			float b = (float)SingletonAsMono<GameDataMrg>.Instance.GetProperty(xlsxMessage.PropertyName, "Property", 0L) / (float)xlsxMessage.PropertyMaxValue;
			b = Mathf.Min(1f, b);
			TextMeshProUGUIXinZhi.text = (int)(b * 100f) + "%";
		}
		_moveTweener?.Kill();
		_moveTweener = ScrollRectScrollView.DOVerticalNormalizedPos(0f, 0.3f);
		InitChatItem(xlsxMessage, list, 0, readChatNodes.Count);
	}

	private async void InitChatItem(Xlsx_Message xlsxMessage, List<MessageData> allMessage, int index, int childIndex)
	{
		MessageData item;
		try
		{
			if (index >= allMessage.Count)
			{
				BTClose.SetActive(active: true);
				_isChat = false;
			}
			else
			{
				item = allMessage[index];
				ChatNode fistNode = item.assetName.GetFistNode();
				InitItem(fistNode, isDelay: false);
			}
		}
		catch (Exception ex)
		{
			MyLog.LogError(ex.Message);
		}
		async void InitItem(ChatNode chatNode, bool isDelay)
		{
			if (isDelay)
			{
				await Task.Delay((int)(chatNode.delay * 1000f));
			}
			if (chatNode.delay > 0f)
			{
				SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.duanXin);
			}
			Transform transform = ((RectTransformChatContent.childCount <= childIndex) ? UnityEngine.Object.Instantiate(RectTransformChatContent.GetChild(0), RectTransformChatContent) : RectTransformChatContent.GetChild(childIndex));
			transform.SetActive(active: true);
			transform.GetComponent<EmailItem>().Init(xlsxMessage, chatNode);
			childIndex++;
			if (chatNode.isHasBtn)
			{
				List<ChatNode> allSelect = chatNode.GetAllChatSelect();
				RectTransformSelectContent.TranFor(allSelect.Count, RectTransformSelectContent.GetChild(0), delegate(int i, GameObject o)
				{
					o.GetComponent<ChatSelectItem>().Init(chatNode, allSelect[i], delegate
					{
						Tool.HideAllChild(RectTransformSelectContent);
						InitItem(allSelect[i], isDelay: false);
					});
				});
			}
			else
			{
				ChatNode getOutChatNode = chatNode.GetGetOutChatNode();
				if ((bool)getOutChatNode)
				{
					InitItem(getOutChatNode, isDelay: true);
				}
				else
				{
					index++;
					Tool.HideAllChild(RectTransformSelectContent);
					SingletonAsMono<GameDataMrg>.Instance.SetRead(xlsxMessage.Key, item.assetName);
					InitChatItem(xlsxMessage, allMessage, index, childIndex);
				}
			}
			_moveTweener?.Kill();
			_moveTweener = ScrollRectScrollView.DOVerticalNormalizedPos(0f, 0.15f);
		}
	}

	private void Close()
	{
		if (_isChat)
		{
			UiManager.ShowTips(LanguageMrg.GetText("A831"));
		}
		else
		{
			CloseUi();
		}
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}
}
