using System.Collections;
using System.Collections.Generic;
using FrameWork;
using FrameWork.Data;
using UnityEngine;
using Xlsx;

namespace Script.Mrg;

public class EventContentMrg : SingletonAsMono<EventContentMrg>
{
	protected override void Awake()
	{
		base.Awake();
	}

	private void Update()
	{
		CheckKeyDown();
	}

	private void CheckKeyDown()
	{
		if (Input.GetKeyDown(KeyCode.Escape) && !UiManager.IsShowUi<EscWindows>() && SingletonAsMono<GlobalMrg>.Instance.videoState != 0)
		{
			UiManager.OpenUi<EscWindows>();
		}
		else if (UiManager.IsShowUi<EscWindows>() && Input.GetKeyDown(KeyCode.Escape) && SingletonAsMono<GlobalMrg>.Instance.videoState != 0)
		{
			if (UiManager.IsEscWindowsLast())
			{
				UiManager.HideAllUi();
				UiManager.OpenUi<EscWindows>();
			}
			else if (UiManager.IsEscWindowsFistHasUi())
			{
				UiManager.GetUi<EscWindows>().Continue();
				UiManager.HideAllUi();
			}
			else
			{
				UiManager.GetUi<EscWindows>().Continue();
			}
		}
		else if (Input.GetKeyDown(KeyCode.Tab) && !UiManager.IsShowUi<EscWindows>() && SingletonAsMono<GlobalMrg>.Instance.videoState != 0)
		{
			UiManager.OpenUi<EscWindows>();
		}
		else if (UiManager.IsShowUi<EscWindows>() && Input.GetKeyDown(KeyCode.Tab) && SingletonAsMono<GlobalMrg>.Instance.videoState != 0)
		{
			if (UiManager.IsEscWindowsLast())
			{
				UiManager.HideAllUi();
				UiManager.OpenUi<EscWindows>();
			}
			else if (UiManager.IsEscWindowsFistHasUi())
			{
				UiManager.HideAllUi();
				UiManager.GetUi<EscWindows>().Continue();
			}
			else
			{
				UiManager.GetUi<EscWindows>().Continue();
			}
		}
		if (UiManager.IsOpenUi<InputWindows>())
		{
			return;
		}
		if (Input.GetKeyDown(KeyCode.L))
		{
			UiManager.OpenUi<LoadSaveWindows>().InitType();
		}
		if (SingletonAsMono<GlobalMrg>.Instance.videoState != 0)
		{
			if (Input.GetKeyDown(KeyCode.U))
			{
				UiManager.OpenUi<GameWindows>();
			}
			VideoState videoState = SingletonAsMono<GlobalMrg>.Instance.videoState;
			if ((videoState == VideoState.Map || videoState == VideoState.Video360Scene) && Input.GetKeyDown(KeyCode.R))
			{
				UiManager.OpenUi<MainWindows>().OpenSave();
			}
			if (Input.GetKeyDown(KeyCode.I))
			{
				UiManager.OpenUi<MainWindows>().OpenEquip();
			}
			if (Input.GetKeyDown(KeyCode.P))
			{
				UiManager.OpenUi<MainWindows>().OpenMessage();
			}
			if (Input.GetKeyDown(KeyCode.J))
			{
				UiManager.OpenUi<MainWindows>().OpenTask();
			}
		}
	}

	public IEnumerator LoadEventContent()
	{
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenCombatWindow, OpenCombatWindow);
		EventManager.AddListener(MessageType.Video, VideoMessageType.CloseCombatWindow, CloseCombatWindow);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenSaveWindows, OpenSaveWindow);
		EventManager.AddListener(MessageType.Video, VideoMessageType.ClearCoin, ClearCoin);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenRoleInfoWindow, OpenRoleInfoWindow);
		EventManager.AddListener(MessageType.Video, VideoMessageType.SetDaytime, SetDaytime);
		EventManager.AddListener(MessageType.Video, VideoMessageType.SetNight, SetNight);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenSaveCoveWindows, OpenSaveCoveWindow);
		EventManager.AddListener(MessageType.Video, VideoMessageType.AutoSaveData, AutoSave);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenPhone, OpenPhone);
		EventManager.AddListener(MessageType.Video, VideoMessageType.CheckMsgRi, CheckMsgRi);
		EventManager.AddListener(MessageType.Video, VideoMessageType.CheckMsgYe, CheckMsgYe);
		EventManager.AddListener(MessageType.Video, VideoMessageType.ReSetXDL, ReSetXDL);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenWineList, OpenWineList);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenHeJiu, OpenHeJiuWindows);
		EventManager.AddListener(MessageType.Video, VideoMessageType.CloseHeJiu, CloseHeJiuWindows);
		EventManager.AddListener(MessageType.Video, VideoMessageType.TargetAddZuiJiu, TargetHeJiu);
		EventManager.AddListener(MessageType.Video, VideoMessageType.PlayerAddZuiJiu, SelfHeJiu);
		EventManager.AddListener(MessageType.Video, VideoMessageType.SetRed, SetRed);
		EventManager.AddListener(MessageType.Video, VideoMessageType.SetBack, SetBack);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenShop, OpenShop);
		EventManager.AddListener(MessageType.Video, VideoMessageType.GetCoin, GetCoin);
		EventManager.AddListener(MessageType.Video, VideoMessageType.Bei1000, GetCoin1000Bei);
		EventManager.AddListener(MessageType.Video, VideoMessageType.ShowJieDuan, ShowJieDuan);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenBaoKeMengUi, OpenBaoKeMenUi);
		EventManager.AddListener(MessageType.Video, VideoMessageType.CloseBaoKeMengUi, CloseBaoKeMenUi);
		EventManager.AddListener(MessageType.Video, VideoMessageType.ZhaoZonZhiChang, GetZhiChang);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenBgmWin, OpenBgmWindow);
		EventManager.AddListener(MessageType.Video, VideoMessageType.CloseBgmWin, CloseBgmWindow);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenBqWin, OpenBqWindow);
		EventManager.AddListener(MessageType.Video, VideoMessageType.CloseBqWin, CloseBqWindow);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenEmailWin, OpenEmailWindow);
		EventManager.AddListener(MessageType.Video, VideoMessageType.CloseEmailWin, CloseEmailWindow);
		EventManager.AddListener(MessageType.Video, VideoMessageType.OpenCombat2Win, OpenCombat2Window);
		EventManager.AddListener(MessageType.Video, VideoMessageType.CloseCombat2Win, CloseCombat2Window);
		EventManager.AddListener(MessageType.Video, VideoMessageType.PlayerAddZuiJiuNotBFB, SelfHeJiuNotBfb);
		yield return null;
	}

	private void OpenCombat2Window(List<object> objects)
	{
		UiManager.OpenUi<Combat2Windows>();
	}

	private void CloseCombat2Window(List<object> objects)
	{
		UiManager.HideUi<Combat2Windows>();
	}

	private void OpenEmailWindow(List<object> objects)
	{
		UiManager.OpenUi<EmailWindows>();
	}

	private void CloseEmailWindow(List<object> objects)
	{
		UiManager.HideUi<EmailWindows>();
	}

	private void OpenBqWindow(List<object> objects)
	{
		UiManager.OpenUi<BqWindows>();
	}

	private void CloseBqWindow(List<object> objects)
	{
		UiManager.HideUi<BqWindows>();
	}

	private void OpenBgmWindow(List<object> objects)
	{
		UiManager.OpenUi<BgmWindows>();
	}

	private void CloseBgmWindow(List<object> objects)
	{
		UiManager.HideUi<BgmWindows>();
	}

	private void GetZhiChang(List<object> objects)
	{
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty("Money", "Property", 0L);
		int num = property * 1000;
		num += 20000000;
		num += SingletonAsMono<GameDataMrg>.Instance.GetProperty("ZhaoZon", "Property", 0L) * 2000000;
		new PropertyData
		{
			PropertyType = PropertyType.Property,
			propertyTypeValue = PropertyTypeValue.Money,
			PropertyValue = num - property
		}.AddTypeValueAsShowTips(isCheck: false);
	}

	private void OpenBaoKeMenUi(List<object> objects)
	{
		UiManager.OpenUi<BaoKeMengWindows>();
	}

	private void CloseBaoKeMenUi(List<object> objects)
	{
		UiManager.HideUi<BaoKeMengWindows>();
	}

	private void ShowJieDuan(List<object> objects)
	{
		UiManager.OpenUi<MainWindows>().OpenRole();
	}

	private void GetCoin1000Bei(List<object> objects)
	{
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty("Money", "Property", 0L);
		int num = property * 1000;
		new PropertyData
		{
			PropertyType = PropertyType.Property,
			propertyTypeValue = PropertyTypeValue.Money,
			PropertyValue = num - property
		}.AddTypeValueAsShowTips(isCheck: false);
	}

	private void GetCoin(List<object> objects)
	{
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty("KPI", "Property", 0L);
		if (property > 0)
		{
			SingletonAsMono<GameDataMrg>.Instance.SetProperty("KPI", 0, "Property");
			new PropertyData
			{
				PropertyType = PropertyType.Property,
				propertyTypeValue = PropertyTypeValue.Money,
				PropertyValue = property
			}.AddTypeValueAsShowTips(isCheck: false);
		}
	}

	private void OpenShop(List<object> objects)
	{
		if (objects != null && objects.Count > 0)
		{
			string type = objects[0].ToString();
			UiManager.OpenUi<ShopWindows>().Init(type);
		}
	}

	private void SetRed(List<object> objects)
	{
		SingletonAsMono<GameDataMrg>.Instance.isRed = true;
	}

	private void SetBack(List<object> objects)
	{
		SingletonAsMono<GameDataMrg>.Instance.isRed = false;
	}

	private void TargetHeJiu(List<object> objects)
	{
		Xlsx_WineList xlsx_WineList = Xlsx_WineList_Query.XlsxDataAsOneKey.ByKeyGetValue(SingletonAsMono<GameDataMrg>.Instance.xlsxWineListKey.ToString());
		new PropertyData
		{
			propertyTypeValue = PropertyTypeValue.TmpTargetJiuLi,
			PropertyType = PropertyType.Property,
			PropertyValue = xlsx_WineList.ZJAdd
		}.AddTypeValueAsShowTips();
	}

	private void SelfHeJiu(List<object> objects)
	{
		Xlsx_WineList xlsx_WineList = Xlsx_WineList_Query.XlsxDataAsOneKey.ByKeyGetValue(SingletonAsMono<GameDataMrg>.Instance.xlsxWineListKey.ToString());
		new PropertyData
		{
			propertyTypeValue = PropertyTypeValue.TmpJiuLian,
			PropertyType = PropertyType.Property,
			PropertyValue = xlsx_WineList.ZJAdd
		}.AddTypeValue();
		List<PropertyData> list = new List<PropertyData>();
		if (xlsx_WineList.JLBFBAdd != 0)
		{
			int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty("CapacityForLiquor", "Property", 0L);
			int num = (int)((float)xlsx_WineList.JLBFBAdd / 100f * (float)property);
			if (num >= 1)
			{
				list.Add(new PropertyData
				{
					PropertyType = PropertyType.Property,
					propertyTypeValue = PropertyTypeValue.CapacityForLiquor,
					PropertyValue = num
				});
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].AddTypeValueAsShowTips(isCheck: false);
		}
	}

	private void SelfHeJiuNotBfb(List<object> objects)
	{
		Xlsx_WineList xlsx_WineList = Xlsx_WineList_Query.XlsxDataAsOneKey.ByKeyGetValue(SingletonAsMono<GameDataMrg>.Instance.xlsxWineListKey.ToString());
		new PropertyData
		{
			propertyTypeValue = PropertyTypeValue.TmpJiuLian,
			PropertyType = PropertyType.Property,
			PropertyValue = xlsx_WineList.ZJAdd
		}.AddTypeValueAsShowTips();
		List<PropertyData> list = new List<PropertyData>();
		if (xlsx_WineList.JLAdd != 0)
		{
			list.Add(new PropertyData
			{
				PropertyType = PropertyType.Property,
				propertyTypeValue = PropertyTypeValue.CapacityForLiquor,
				PropertyValue = xlsx_WineList.JLAdd
			});
		}
		for (int i = 0; i < list.Count; i++)
		{
			list[i].AddTypeValueAsShowTips(isCheck: false);
		}
	}

	private void OpenHeJiuWindows(List<object> objects)
	{
		UiManager.OpenUi<HeJiuWindows>();
	}

	private void CloseHeJiuWindows(List<object> objects)
	{
		if (UiManager.IsOpenUi<HeJiuWindows>())
		{
			UiManager.GetUi<HeJiuWindows>().CloseUi();
		}
	}

	private void OpenWineList(List<object> objects)
	{
		UiManager.OpenUi<WineListWindows>();
	}

	private void CheckMsgRi(List<object> objects)
	{
		MessageGroup messageGroup = ABMrg.Load<MessageGroup>("MessageRi");
		int curRound = SingletonAsMono<GameDataMrg>.Instance.CurRound;
		for (int i = 0; i < messageGroup.nodes.Count; i++)
		{
			if (messageGroup.nodes[i] is MessageNode)
			{
				MessageNode messageNode = messageGroup.nodes[i] as MessageNode;
				if (messageNode != null && curRound >= messageNode.minRound && curRound <= messageNode.maxRound && messageNode.chatGraph != null && messageNode.properties.IsSuc())
				{
					SingletonAsMono<GameDataMrg>.Instance.AddChat(messageNode.key, messageNode.chatGraph.name);
				}
			}
		}
	}

	private void CheckMsgYe(List<object> objects)
	{
		MessageGroup messageGroup = ABMrg.Load<MessageGroup>("MessageYe");
		int curRound = SingletonAsMono<GameDataMrg>.Instance.CurRound;
		for (int i = 0; i < messageGroup.nodes.Count; i++)
		{
			if (messageGroup.nodes[i] is MessageNode)
			{
				MessageNode messageNode = messageGroup.nodes[i] as MessageNode;
				if (messageNode != null && curRound >= messageNode.minRound && curRound <= messageNode.maxRound && messageNode.chatGraph != null && messageNode.properties.IsSuc())
				{
					SingletonAsMono<GameDataMrg>.Instance.AddChat(messageNode.key, messageNode.chatGraph.name);
				}
			}
		}
	}

	private void ReSetXDL(List<object> objects)
	{
		SingletonAsMono<GameDataMrg>.Instance.AddProperty("Execution", SingletonAsMono<GameDataMrg>.Instance.GetProperty("XinDonLiHuiFu", "Property", 0L), "Property");
	}

	private void AutoSave(List<object> objects)
	{
		SingletonAsMono<GameDataMrg>.Instance.AutoSave(LanguageMrg.GetText("A434"));
	}

	private void OpenPhone(List<object> objects)
	{
		string text = "";
		if (objects != null && objects.Count > 0)
		{
			text = objects[0].ToString();
		}
		if (string.IsNullOrEmpty(text))
		{
			UiManager.OpenUi<MainWindows>().OpenMessage();
		}
		else
		{
			UiManager.OpenUi<MainWindows>().OpenMessage().InitChat(Xlsx_Message_Query.XlsxDataAsOneKey.ByKeyGetValue(text));
		}
	}

	private void SetDaytime(List<object> objects)
	{
		SingletonAsMono<GameDataMrg>.Instance.IsMorning = true;
		EventManager.DispatchEvent(MessageType.Video, VideoMessageType.UpdateTime);
	}

	private void SetNight(List<object> objects)
	{
		SingletonAsMono<GameDataMrg>.Instance.IsMorning = false;
		EventManager.DispatchEvent(MessageType.Video, VideoMessageType.UpdateTime);
		if (FrameWork.Tool.IsCanShow("SetNight"))
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1588"));
		}
	}

	private void OpenRoleInfoWindow(List<object> objects)
	{
		UiManager.OpenUi<MainWindows>().OpenEquip();
	}

	private void OpenSaveCoveWindow(List<object> objects)
	{
		UiManager.OpenUi<MainWindows>().OpenSave();
	}

	private void ClearCoin(List<object> objects)
	{
		SingletonAsMono<GameDataMrg>.Instance.SetProperty("Money", 0, "Property");
		SingletonAsMono<InfoTipsMrg>.Instance.Add(LanguageMrg.GetText("A280"));
	}

	private void OpenCombatWindow(List<object> objects)
	{
		UiManager.OpenUi<CombatWindows>();
	}

	private void OpenSaveWindow(List<object> objects)
	{
		UiManager.OpenUi<LoadSaveWindows>().InitType();
	}

	private void CloseCombatWindow(List<object> objects)
	{
		UiManager.HideUi<CombatWindows>();
	}

	private void Start()
	{
		Physics.autoSyncTransforms = true;
		Application.runInBackground = true;
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			Physics.SyncTransforms();
		}
	}

	private void OnApplicationQuit()
	{
		SingletonAsMono<GameDataMrg>.Instance.Save();
		GameData.Save();
	}
}
