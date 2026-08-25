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

[ActorInfo("", "PhoneWindows")]
[UiMode(Mode.Normal, true)]
public class PhoneWindows : UiActor
{
	private bool _isChat;

	private Action _action;

	private Tweener _moveTweener;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformMessage;

	public CanvasRenderer CanvasRendererMessage;

	public Image ImageMessage;

	public AddScripts AddScriptsMessage;

	public RectTransform RectTransformMsgContent;

	public VerticalLayoutGroup VerticalLayoutGroupMsgContent;

	public ContentSizeFitter ContentSizeFitterMsgContent;

	public AddScripts AddScriptsMsgContent;

	public RectTransform RectTransformChat;

	public CanvasRenderer CanvasRendererChat;

	public Image ImageChat;

	public AddScripts AddScriptsChat;

	public RectTransform RectTransformChatName;

	public CanvasRenderer CanvasRendererChatName;

	public TextMeshProUGUI TextMeshProUGUIChatName;

	public AddScripts AddScriptsChatName;

	public RectTransform RectTransformBack;

	public CanvasRenderer CanvasRendererBack;

	public Image ImageBack;

	public BT BTBack;

	public AddScripts AddScriptsBack;

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

	public VerticalLayoutGroup VerticalLayoutGroupSelectContent;

	public AddScripts AddScriptsSelectContent;

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

	public override void Awake()
	{
		base.Awake();
		RectTransformClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<AddScripts>();
		RectTransformMessage = GetGameObject().transform.Find("View/Message/").GetComponent<RectTransform>();
		CanvasRendererMessage = GetGameObject().transform.Find("View/Message/").GetComponent<CanvasRenderer>();
		ImageMessage = GetGameObject().transform.Find("View/Message/").GetComponent<Image>();
		AddScriptsMessage = GetGameObject().transform.Find("View/Message/").GetComponent<AddScripts>();
		RectTransformMsgContent = GetGameObject().transform.Find("View/Message/Scroll View/Viewport/MsgContent/").GetComponent<RectTransform>();
		VerticalLayoutGroupMsgContent = GetGameObject().transform.Find("View/Message/Scroll View/Viewport/MsgContent/").GetComponent<VerticalLayoutGroup>();
		ContentSizeFitterMsgContent = GetGameObject().transform.Find("View/Message/Scroll View/Viewport/MsgContent/").GetComponent<ContentSizeFitter>();
		AddScriptsMsgContent = GetGameObject().transform.Find("View/Message/Scroll View/Viewport/MsgContent/").GetComponent<AddScripts>();
		RectTransformChat = GetGameObject().transform.Find("View/Chat/").GetComponent<RectTransform>();
		CanvasRendererChat = GetGameObject().transform.Find("View/Chat/").GetComponent<CanvasRenderer>();
		ImageChat = GetGameObject().transform.Find("View/Chat/").GetComponent<Image>();
		AddScriptsChat = GetGameObject().transform.Find("View/Chat/").GetComponent<AddScripts>();
		RectTransformChatName = GetGameObject().transform.Find("View/Chat/Top/ChatName/").GetComponent<RectTransform>();
		CanvasRendererChatName = GetGameObject().transform.Find("View/Chat/Top/ChatName/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIChatName = GetGameObject().transform.Find("View/Chat/Top/ChatName/").GetComponent<TextMeshProUGUI>();
		AddScriptsChatName = GetGameObject().transform.Find("View/Chat/Top/ChatName/").GetComponent<AddScripts>();
		RectTransformBack = GetGameObject().transform.Find("View/Chat/Top/Back/").GetComponent<RectTransform>();
		CanvasRendererBack = GetGameObject().transform.Find("View/Chat/Top/Back/").GetComponent<CanvasRenderer>();
		ImageBack = GetGameObject().transform.Find("View/Chat/Top/Back/").GetComponent<Image>();
		BTBack = GetGameObject().transform.Find("View/Chat/Top/Back/").GetComponent<BT>();
		AddScriptsBack = GetGameObject().transform.Find("View/Chat/Top/Back/").GetComponent<AddScripts>();
		RectTransformScrollView = GetGameObject().transform.Find("View/Chat/Scroll View/").GetComponent<RectTransform>();
		CanvasRendererScrollView = GetGameObject().transform.Find("View/Chat/Scroll View/").GetComponent<CanvasRenderer>();
		ImageScrollView = GetGameObject().transform.Find("View/Chat/Scroll View/").GetComponent<Image>();
		ScrollRectScrollView = GetGameObject().transform.Find("View/Chat/Scroll View/").GetComponent<ScrollRect>();
		AddScriptsScrollView = GetGameObject().transform.Find("View/Chat/Scroll View/").GetComponent<AddScripts>();
		RectTransformChatContent = GetGameObject().transform.Find("View/Chat/Scroll View/Viewport/ChatContent/").GetComponent<RectTransform>();
		ContentSizeFitterChatContent = GetGameObject().transform.Find("View/Chat/Scroll View/Viewport/ChatContent/").GetComponent<ContentSizeFitter>();
		VerticalLayoutGroupChatContent = GetGameObject().transform.Find("View/Chat/Scroll View/Viewport/ChatContent/").GetComponent<VerticalLayoutGroup>();
		AddScriptsChatContent = GetGameObject().transform.Find("View/Chat/Scroll View/Viewport/ChatContent/").GetComponent<AddScripts>();
		RectTransformSelectContent = GetGameObject().transform.Find("View/Chat/SelectChat/Scroll View/Viewport/SelectContent/").GetComponent<RectTransform>();
		ContentSizeFitterSelectContent = GetGameObject().transform.Find("View/Chat/SelectChat/Scroll View/Viewport/SelectContent/").GetComponent<ContentSizeFitter>();
		VerticalLayoutGroupSelectContent = GetGameObject().transform.Find("View/Chat/SelectChat/Scroll View/Viewport/SelectContent/").GetComponent<VerticalLayoutGroup>();
		AddScriptsSelectContent = GetGameObject().transform.Find("View/Chat/SelectChat/Scroll View/Viewport/SelectContent/").GetComponent<AddScripts>();
		RectTransformXinBg = GetGameObject().transform.Find("View/Chat/XinBg/").GetComponent<RectTransform>();
		CanvasRendererXinBg = GetGameObject().transform.Find("View/Chat/XinBg/").GetComponent<CanvasRenderer>();
		ImageXinBg = GetGameObject().transform.Find("View/Chat/XinBg/").GetComponent<Image>();
		AddScriptsXinBg = GetGameObject().transform.Find("View/Chat/XinBg/").GetComponent<AddScripts>();
		RectTransformXinAg = GetGameObject().transform.Find("View/Chat/XinBg/XinAg/").GetComponent<RectTransform>();
		CanvasRendererXinAg = GetGameObject().transform.Find("View/Chat/XinBg/XinAg/").GetComponent<CanvasRenderer>();
		ImageXinAg = GetGameObject().transform.Find("View/Chat/XinBg/XinAg/").GetComponent<Image>();
		AddScriptsXinAg = GetGameObject().transform.Find("View/Chat/XinBg/XinAg/").GetComponent<AddScripts>();
		RectTransformXinZhi = GetGameObject().transform.Find("View/Chat/XinBg/XinZhi/").GetComponent<RectTransform>();
		CanvasRendererXinZhi = GetGameObject().transform.Find("View/Chat/XinBg/XinZhi/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIXinZhi = GetGameObject().transform.Find("View/Chat/XinBg/XinZhi/").GetComponent<TextMeshProUGUI>();
		AddScriptsXinZhi = GetGameObject().transform.Find("View/Chat/XinBg/XinZhi/").GetComponent<AddScripts>();
	}

	public PhoneWindows(Transform trans)
		: base(trans)
	{
	}

	public PhoneWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(Close);
		BTBack.onClick.AddListener(Back);
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		SingletonAsMono<GameDataMrg>.Instance.AddProperty("手机", 1L, "Value");
		RectTransformMessage.SetActive(active: true);
		RectTransformChat.SetActive(active: false);
		_action = null;
		InitMessage();
		base.transform.SetParent(UiManager.GetTransform(new UiModeAttribute(Mode.Normal)));
	}

	public void Init(Action action)
	{
		_action = action;
	}

	public void InitNotBack()
	{
		base.transform.SetParent(UiManager.GetTransform(new UiModeAttribute(Mode.MainItem)));
		BTClose.SetActive(active: false);
	}

	public void InitMessage()
	{
		List<Xlsx_Message> allRole = Xlsx_Message_Query.data.Where((Xlsx_Message message) => message.Type == 0 && SingletonAsMono<GameDataMrg>.Instance.GetChat(message.Key).Count > 0).ToList();
		allRole = (from m in allRole
			orderby SingletonAsMono<GameDataMrg>.Instance.IsShowChatRed(m.Key) descending, SingletonAsMono<GameDataMrg>.Instance.GetChatNewTime(m.Key) descending
			select m).ToList();
		RectTransformMsgContent.TranFor(allRole.Count, RectTransformMsgContent.GetChild(0), delegate(int i, GameObject o)
		{
			o.GetComponent<MsgItem>().Init(allRole[i]);
		});
	}

	public void InitChat(Xlsx_Message xlsxMessage)
	{
		TextMeshProUGUIChatName.text = LanguageMrg.GetText(xlsxMessage.Name);
		Tool.HideAllChild(RectTransformSelectContent);
		_isChat = true;
		RectTransformMessage.SetActive(active: false);
		RectTransformChat.SetActive(active: true);
		SingletonAsMono<GameDataMrg>.Instance.SetShowChatRed(xlsxMessage.Key, show: false);
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.UpdateChatRed);
		List<MessageData> chat = SingletonAsMono<GameDataMrg>.Instance.GetChat(xlsxMessage.Key);
		List<ChatNode> readChatNodes = new List<ChatNode>();
		List<MessageData> list = new List<MessageData>();
		for (int j = 0; j < chat.Count; j++)
		{
			if (chat[j].msgType == ChatMsgType.Read)
			{
				readChatNodes.AddRange(chat[j].GetAllChat());
			}
			else
			{
				list.Add(chat[j]);
			}
		}
		RectTransformChatContent.TranFor(readChatNodes.Count, RectTransformChatContent.GetChild(0), delegate(int i, GameObject o)
		{
			o.GetComponent<ChatItem>().Init(xlsxMessage, readChatNodes[i]);
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
				_isChat = false;
				return;
			}
			item = allMessage[index];
			ChatNode fistNode = item.assetName.GetFistNode();
			InitItem(fistNode, isDelay: false);
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
			transform.GetComponent<ChatItem>().Init(xlsxMessage, chatNode);
			childIndex++;
			if (chatNode.isHasBtn)
			{
				List<ChatNode> allSelect = chatNode.GetAllChatSelect();
				RectTransformSelectContent.TranFor(allSelect.Count, RectTransformSelectContent.GetChild(0), delegate(int i, GameObject o)
				{
					o.GetComponent<ChatSelectItem>().Init(chatNode, allSelect[i], delegate
					{
						InitItem(allSelect[i], isDelay: false);
						Tool.HideAllChild(RectTransformSelectContent);
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
		if (_isChat || SingletonAsMono<GameDataMrg>.Instance.IsShowPhoneRed())
		{
			UiManager.ShowTips(LanguageMrg.GetText("A831"));
			return;
		}
		_action?.Invoke();
		CloseUi();
	}

	public bool IsCanClose()
	{
		if (_isChat || SingletonAsMono<GameDataMrg>.Instance.IsShowPhoneRed())
		{
			return false;
		}
		return true;
	}

	private void Back()
	{
		if (!_isChat)
		{
			RectTransformMessage.SetActive(active: true);
			RectTransformChat.SetActive(active: false);
			InitMessage();
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
