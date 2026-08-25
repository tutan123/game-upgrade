using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using FrameWork;
using FrameWork.ChatTool;
using FrameWork.Data;
using LitJson;
using Script.UiTool;
using UnityEngine;
using Xlsx;

namespace Script.Mrg;

public class GameDataMrg : SingletonAsMono<GameDataMrg>
{
	private SavedData _saveData = new SavedData();

	private List<string> _saveKeys = new List<string>();

	private List<string> _autoSaveKeys = new List<string>();

	private string _saveKeyStr = "_saveKeyStr";

	private string _globalUnLockSaveKeyStr = "_globalUnLockSaveKeyStr";

	private string _unLockSaveKeyStr = "_unLockSaveKeyStr";

	private string _roundUnLockDicStr = "_roundUnLockDicStr";

	private string _autoSaveStr = "_autoSaveStr";

	private int _saveMaxCount = 20;

	private int _autoSaveMaxCount = 6;

	public string xlsxMiNiGameLevelKey = "A1";

	public Xlsx_WineList_Key xlsxWineListKey;

	public bool isRed;

	private List<string> _globalUnLoadKeys = new List<string>();

	private List<string> _unLockKeys = new List<string>();

	private Dictionary<string, List<string>> _roundUnLockDic = new Dictionary<string, List<string>>();

	private string _debugPath = Application.streamingAssetsPath + "/awdacwagvlknlkanwkldnaklwndkjankjfnjiknaklzx.txt";

	public bool isHasDebug;

	public int SaveMaxCount => _saveMaxCount;

	public int AutoSaveMaxMaxCount => _autoSaveMaxCount;

	public int SaveCount => _saveKeys.Count;

	public int AutoSaveCount => _autoSaveKeys.Count;

	public int CurRound
	{
		get
		{
			return _saveData.Round;
		}
		set
		{
			_saveData.Round = value;
		}
	}

	public bool Is2D
	{
		get
		{
			return _saveData.Data.GetValueOrDefault("Is2D", true.ToString()).ToBool();
		}
		set
		{
			_saveData.Data["Is2D"] = value.ToString();
		}
	}

	public string CurEventKey
	{
		get
		{
			string valueOrDefault = _saveData.Data.GetValueOrDefault("CurEventKey", Xlsx_Event_Query.data[0].Key);
			if (string.IsNullOrEmpty(valueOrDefault))
			{
				return Xlsx_Event_Query.data[0].Key;
			}
			return valueOrDefault;
		}
		set
		{
			if (_saveData.Data.ContainsKey("CurEventKey"))
			{
				_saveData.Data["CurEventKey"] = value;
			}
			else
			{
				_saveData.Data.TryAdd("CurEventKey", value);
			}
		}
	}

	public string CurVideoId
	{
		get
		{
			return _saveData.Data.GetValueOrDefault("CurVideoId", "");
		}
		set
		{
			if (_saveData.Data.ContainsKey("CurVideoId"))
			{
				_saveData.Data["CurVideoId"] = value;
			}
			_saveData.Data.TryAdd("CurVideoId", value);
		}
	}

	public Vector3 Video360Rotation
	{
		get
		{
			string valueOrDefault = _saveData.Data.GetValueOrDefault("Video360Rotation", "");
			if (string.IsNullOrEmpty(valueOrDefault))
			{
				return Vector3.zero;
			}
			List<float> list = (from s in valueOrDefault.Split(";")
				select float.Parse(s, CultureInfo.InvariantCulture)).ToList();
			return new Vector3(list[0], list[1], list[2]);
		}
		set
		{
			_saveData.Data["Video360Rotation"] = $"{value.x};{value.y};{value.z}";
		}
	}

	public Vector3 MapLoc
	{
		get
		{
			string valueOrDefault = _saveData.Data.GetValueOrDefault("MapLoc", "");
			if (string.IsNullOrEmpty(valueOrDefault))
			{
				return new Vector3(75f, 1.15f, 114f);
			}
			List<float> list = (from s in valueOrDefault.Split(";")
				select float.Parse(s, CultureInfo.InvariantCulture)).ToList();
			return new Vector3(list[0], Mathf.Max(list[1], 1.15f), list[2]);
		}
		set
		{
			_saveData.Data["MapLoc"] = $"{value.x};{Mathf.Max(1.15f, value.y)};{value.z}";
		}
	}

	public bool IsMorning
	{
		get
		{
			return _saveData.Data.GetValueOrDefault("IsMorning", true.ToString()).ToBool();
		}
		set
		{
			if (_saveData.Data.ContainsKey("IsMorning"))
			{
				_saveData.Data["IsMorning"] = value.ToString();
			}
			_saveData.Data.TryAdd("IsMorning", value.ToString());
		}
	}

	public int PlayerJiuLian => GetProperty("CapacityForLiquor", "Property", 0L);

	public int TmpPlayerJiuLian
	{
		get
		{
			return GetProperty("TmpJiuLian", "Property", 0L);
		}
		set
		{
			SetProperty("TmpJiuLian", value, "Property");
		}
	}

	public int TargetJiuLian
	{
		get
		{
			return GetProperty("TargetJiuLian", "Property", 0L);
		}
		set
		{
			SetProperty("TargetJiuLian", value, "Property");
		}
	}

	public int TmpTargetJiuLian
	{
		get
		{
			return GetProperty("TmpTargetJiuLi", "Property", 0L);
		}
		set
		{
			SetProperty("TmpTargetJiuLi", value, "Property");
		}
	}

	public int PlayerHp => GetProperty("Stamina", "Property", 0L);

	public int TmpPlayerHp
	{
		get
		{
			return GetProperty("TmpPlayerHp", "Property", 0L);
		}
		set
		{
			SetProperty("TmpPlayerHp", value, "Property");
		}
	}

	public int EnemyHp
	{
		get
		{
			return GetProperty("EnemyHp", "Property", 0L);
		}
		set
		{
			SetProperty("EnemyHp", value, "Property");
		}
	}

	public int EnemyHpMax
	{
		get
		{
			return GetProperty("EnemyHpMax", "Property", 0L);
		}
		set
		{
			SetProperty("EnemyHpMax", value, "Property");
		}
	}

	public bool IsYaLiMan => GetProperty("Pressure", "Property", 0L) >= GetProperty("YaLiShangXian", "Property", 0L);

	public int Coin => GetProperty("Money", "Property", 0L);

	public long SaveTime => _saveData.Time.ToLong();

	public string SaveName => _saveData.Key;

	public List<string> GetSaveKeys()
	{
		return _saveKeys;
	}

	public List<string> GetAutoSaveKeys()
	{
		return _autoSaveKeys;
	}

	public SavedData GetData()
	{
		return _saveData;
	}

	protected override void Awake()
	{
		base.Awake();
		isHasDebug = File.Exists(_debugPath);
		_saveKeys = JsonMapper.ToObject<List<string>>(GameData.GetValue(_saveKeyStr, "[]"));
		_autoSaveKeys = JsonMapper.ToObject<List<string>>(GameData.GetValue(_autoSaveStr, "[]"));
		_globalUnLoadKeys = JsonMapper.ToObject<List<string>>(GameData.GetValue(_globalUnLockSaveKeyStr, "[]"));
		LoadSaveData();
	}

	public bool IsUnLockPoint(string key)
	{
		return GetProperty("IsUnLockPoint_" + key, "Point", 0L) > 0;
	}

	public void UnLockPoint(string key)
	{
		SetProperty("IsUnLockPoint_" + key, 1, "Point");
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.UpdateMapUnLockPoint);
	}

	public bool IsPassMiniGame(string key)
	{
		return GetProperty("Pass_" + key, "Nor", 0L) > 0;
	}

	public void PassMiniGame(string key)
	{
		AddProperty("Pass_" + key, 1L);
	}

	public void AddUnLockKey(string key)
	{
		if (!_globalUnLoadKeys.Contains(key))
		{
			_globalUnLoadKeys.Add(key);
		}
		if (!_unLockKeys.Contains(key))
		{
			_unLockKeys.Add(key);
		}
		if (_roundUnLockDic.ContainsKey(CurRound.ToString()))
		{
			_roundUnLockDic[CurRound.ToString()].Add(key);
			return;
		}
		_roundUnLockDic.TryAdd(CurRound.ToString(), new List<string> { key });
	}

	public bool IsUnLockKey(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return false;
		}
		return _unLockKeys.Contains(key);
	}

	public bool IsUnLockGlobalKey(string key)
	{
		if (isHasDebug)
		{
			return true;
		}
		if (string.IsNullOrEmpty(key))
		{
			return false;
		}
		return _globalUnLoadKeys.Contains(key);
	}

	public bool IsRoundUnLockKey(string key)
	{
		if (_roundUnLockDic.ContainsKey(CurRound.ToString()))
		{
			return _roundUnLockDic[CurRound.ToString()].Contains(key);
		}
		return false;
	}

	public VideoNode GetCurVideo()
	{
		VideoGraph videoGraph = ABMrg.Load<VideoGraph>(Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(CurEventKey).VideoAsset);
		string curVideoId = CurVideoId;
		if (string.IsNullOrEmpty(curVideoId))
		{
			return videoGraph.GetEvenFistNode();
		}
		for (int i = 0; i < videoGraph.nodes.Count; i++)
		{
			if (videoGraph.nodes[i] is VideoNode)
			{
				VideoNode videoNode = videoGraph.nodes[i] as VideoNode;
				if (videoNode.uniqueID == curVideoId)
				{
					return videoNode;
				}
			}
		}
		return null;
	}

	public string GetCurZbKey(ZbType zbType)
	{
		return _saveData.Data.GetValueOrDefault(zbType.ToString() + "_CurZbKey", "");
	}

	public void SetCurZbKey(ZbType zbType, string zbKey)
	{
		_saveData.Data[zbType.ToString() + "_CurZbKey"] = zbKey;
	}

	public long GetItemGetTime(string key)
	{
		string key2 = "GetItemTime_" + key;
		return _saveData.Data.GetValueOrDefault(key2, "0").ToLong();
	}

	public void SetItemGetTime(string key, long time)
	{
		string key2 = "GetItemTime_" + key;
		_saveData.Data[key2] = time.ToString();
	}

	public bool GetItemGetIsShowRed(string key)
	{
		string key2 = "GetItemGetIsShowRed_" + key;
		return _saveData.Data.GetValueOrDefault(key2, false.ToString()).ToBool();
	}

	public void SetItemShowRed(string key, bool value = true)
	{
		string key2 = "GetItemGetIsShowRed_" + key;
		_saveData.Data[key2] = value.ToString();
	}

	public bool IsCanAutoSave(string key)
	{
		return _saveData.Data.GetValueOrDefault("CanAutoSave_" + CurRound + "_" + key, true.ToString()).ToBool();
	}

	public void SetCanAutoSave(string key, bool value)
	{
		string key2 = "CanAutoSave_" + CurRound + "_" + key;
		_saveData.Data[key2] = value.ToString();
	}

	public int GetProperty(string propertyName, string type = "Nor", long defaultValue = 0L)
	{
		if (type == "Property" && propertyName == "IsMorning")
		{
			if (!IsMorning)
			{
				return 0;
			}
			return 1;
		}
		GetEquipAdd().TryGetValue(propertyName, out var value);
		int num = GetPropertyNorEquip(propertyName, type, defaultValue);
		if (type == "Property" && propertyName == "KPI")
		{
			num += 4000;
		}
		if (type == "Property" && propertyName == "CapacityForLiquor")
		{
			num += GetRoundProperty("CapacityForLiquor", "Property");
		}
		return num + value;
	}

	public void AddProperty(string propertyName, long count, string type = "Nor")
	{
		if (type == "Property" && propertyName == "TmpPlayerHp")
		{
			int tmpPlayerHp = TmpPlayerHp;
			int playerHp = PlayerHp;
			if (tmpPlayerHp + count > playerHp)
			{
				count = playerHp - tmpPlayerHp;
			}
		}
		if (type == "Item" && count > 0)
		{
			long time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			SetItemGetTime(propertyName, time);
			SetItemShowRed(propertyName);
		}
		if (type == "Property" && propertyName == "TSGSNFavorability")
		{
			propertyName = "XFPFavorability";
		}
		if (type == "Property" && propertyName == "XXSNFavorability")
		{
			propertyName = "XXGFavorability";
		}
		if (type == "Property" && propertyName == "WYFavorability")
		{
			propertyName = "LBNFavorability";
		}
		if (type == "Property" && propertyName == "LXYavorability")
		{
			propertyName = "XJMFavorability";
		}
		string key = "property_" + type + "_" + propertyName;
		long num = GetPropertyNorEquip(propertyName, type, 0L) + count;
		if (type == "Property" && propertyName == "JianJin")
		{
			key = "property_" + type + "_Money";
			num = GetPropertyNorEquip("Money", type, 0L) + count;
		}
		if (type == "Property" && propertyName == "GonShiZhiChang")
		{
			key = "property_" + type + "_Money";
			num = GetPropertyNorEquip("Money", type, 0L) + count;
		}
		if (type == "Property" && propertyName == "Execution")
		{
			num = Math.Min(num, GetProperty("XinDonLiShangXian", type, 0L));
			num = Math.Max(0L, num);
		}
		if (type == "Property" && propertyName == "Pressure")
		{
			num = Math.Max(0L, num);
		}
		if (type == "Property" && propertyName == "JiNengDian")
		{
			num = Math.Min(num, 12L);
		}
		if (type == "Property" && propertyName == "TmpJiNengDian")
		{
			num = Math.Min(num, 12L);
		}
		_saveData.Data[key] = num.ToString() ?? "";
		List<object> eventMsg = EventManager.GetEventMsg();
		eventMsg.Add(type);
		eventMsg.Add(propertyName);
		eventMsg.Add(count);
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.UpdateProperty, eventMsg);
		FrameWork.Tool.CheckAchievement();
	}

	public int GetPropertyNorEquip(string propertyName, string type = "Nor", long defaultValue = 0L)
	{
		if (type == "Property" && propertyName == "TSGSNFavorability")
		{
			propertyName = "XFPFavorability";
		}
		if (type == "Property" && propertyName == "XXSNFavorability")
		{
			propertyName = "XXGFavorability";
		}
		if (type == "Property" && propertyName == "WYFavorability")
		{
			propertyName = "LBNFavorability";
		}
		if (type == "Property" && propertyName == "LXYavorability")
		{
			propertyName = "XJMFavorability";
		}
		if (type == "Property" && propertyName == "GonShiZhiChang")
		{
			propertyName = "Money";
		}
		if (type == "Other" && propertyName == "Round")
		{
			return CurRound;
		}
		if (type == "Property" && propertyName == "XinDonLiShangXian")
		{
			defaultValue = 20L;
		}
		if (type == "Property" && propertyName == "Execution")
		{
			defaultValue = 10L;
		}
		if (type == "Property" && propertyName == "CapacityForLiquor")
		{
			defaultValue = 100L;
		}
		if (type == "Property" && propertyName == "XinDonLiHuiFu")
		{
			defaultValue = 15L;
		}
		if (type == "Property" && propertyName == "Stamina")
		{
			defaultValue = 5L;
		}
		if (type == "Property" && propertyName == "Wisdom")
		{
			defaultValue = 5L;
		}
		if (type == "Property" && propertyName == "YaLiShangXian")
		{
			defaultValue = 100L;
		}
		if (type == "Property" && propertyName == "Money")
		{
			defaultValue = 352L;
		}
		return _saveData.Data.GetValueOrDefault("property_" + type + "_" + propertyName, defaultValue.ToString() ?? "").ToInt();
	}

	public void SetProperty(string propertyName, int count, string type = "Nor")
	{
		string key = "property_" + type + "_" + propertyName;
		_saveData.Data[key] = count.ToString() ?? "";
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.UpdateProperty);
	}

	public int GetPropertyAsEvent(string propertyName)
	{
		return GetProperty(propertyName, "Event", 0L);
	}

	public void AddPropertyAsEvent(string propertyName)
	{
		AddProperty(propertyName, 1L, "Event");
	}

	public int GetRoundProperty(string propertyName, string type = "Nor", int defaultValue = 0)
	{
		return _saveData.Data.GetValueOrDefault($"property_{type}_{CurRound}_" + propertyName, defaultValue.ToString() ?? "").ToInt();
	}

	public void AddRoundProperty(string propertyName, int count, string type = "Nor")
	{
		string key = $"property_{type}_{CurRound}_" + propertyName;
		_saveData.Data[key] = (GetRoundProperty(propertyName, type) + count).ToString() ?? "";
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.UpdateProperty);
	}

	public void SetRoundProperty(string propertyName, int count, string type = "Nor")
	{
		string key = $"property_{type}_{CurRound}_" + propertyName;
		_saveData.Data[key] = count.ToString() ?? "";
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.UpdateProperty);
	}

	public int GetChatCount(string propertyName)
	{
		return GetProperty(propertyName, "ChatCount", 0L);
	}

	public void AddChatCount(string propertyName, int count = 1)
	{
		AddProperty(propertyName, count, "ChatCount");
	}

	public TaskType GetTaskState(string taskName)
	{
		return _saveData.Data.GetValueOrDefault("Task_" + taskName, "None").ToEnum<TaskType>();
	}

	public void SetTaskData(string taskName, TaskType type = TaskType.None)
	{
		string key = "Task_" + taskName;
		_saveData.Data[key] = type.ToString();
	}

	public void Save()
	{
		GameData.SetString(_saveKeyStr, JsonMapper.ToJson(_saveKeys));
		GameData.SetString(_autoSaveStr, JsonMapper.ToJson(_autoSaveKeys));
		GameData.SetString(_globalUnLockSaveKeyStr, JsonMapper.ToJson(_globalUnLoadKeys));
	}

	private void SaveUnLockKey()
	{
		_saveData.Data[_unLockSaveKeyStr] = JsonMapper.ToJson(_unLockKeys);
	}

	protected override string GetDataKey()
	{
		return base.GetDataKey();
	}

	public string NewSaved(string savedName = "存档")
	{
		SaveUnLockKey();
		string text = Guid.NewGuid().ToString();
		if (_saveKeys.Count >= _saveMaxCount)
		{
			List<SavedData> savedData = GetSavedData(_saveKeys);
			_saveKeys.Remove(savedData[0].Key);
			GameData.RemoveValue(GameData.GetDataKey(savedData[0].Key));
		}
		_saveKeys.Add(text);
		SavedData savedData2 = new SavedData();
		savedData2.Time = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
		savedData2.Key = text;
		savedData2.SavedName = savedName;
		savedData2.isNew = true;
		GameData.SetString(GameData.GetDataKey(text), JsonMapper.ToJson(savedData2));
		Save();
		return text;
	}

	public string CurNewSaved(string savedName = "存档")
	{
		SaveUnLockKey();
		string text = Guid.NewGuid().ToString();
		if (_saveKeys.Count >= _saveMaxCount)
		{
			List<SavedData> savedData = GetSavedData(_saveKeys);
			_saveKeys.Remove(savedData[0].Key);
			GameData.RemoveValue(GameData.GetDataKey(savedData[0].Key));
		}
		_saveKeys.Add(text);
		SavedData savedData2 = JsonMapper.ToObject<SavedData>(JsonMapper.ToJson(_saveData));
		savedData2.Time = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
		savedData2.Key = text;
		savedData2.SavedName = savedName;
		savedData2.isNew = true;
		GameData.SetString(GameData.GetDataKey(text), JsonMapper.ToJson(savedData2));
		Save();
		return text;
	}

	public string AutoSave(string saveName = "")
	{
		SaveUnLockKey();
		string text = Guid.NewGuid().ToString();
		List<SavedData> savedData = GetSavedData(_autoSaveKeys, isSort: true);
		if (savedData.Count >= _autoSaveMaxCount)
		{
			text = savedData.Last().Key;
		}
		if (!_autoSaveKeys.Contains(text))
		{
			_autoSaveKeys.Add(text);
		}
		SavedData savedData2 = JsonMapper.ToObject<SavedData>(JsonMapper.ToJson(_saveData));
		savedData2.Time = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
		savedData2.Key = text;
		savedData2.SavedName = saveName;
		savedData2.isNew = true;
		GameData.SetString(GameData.GetDataKey(text), JsonMapper.ToJson(savedData2));
		Save();
		return text;
	}

	public void NewSavedLoad(string savedName = "存档")
	{
		GameData.CurKey = Guid.NewGuid().ToString();
		LoadSaveData();
	}

	public void SetIsNew(bool isNew)
	{
		_saveData.isNew = isNew;
	}

	public void CoverSave(string key, string coverName)
	{
		if (string.IsNullOrEmpty(key))
		{
			NewSaved();
			return;
		}
		SaveUnLockKey();
		if (!string.IsNullOrEmpty(_saveData.Key))
		{
			SavedData savedData = JsonMapper.ToObject<SavedData>(JsonMapper.ToJson(_saveData));
			savedData.Time = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
			savedData.SavedName = coverName;
			savedData.Key = key;
			savedData.isNew = true;
			GameData.SetString(GameData.GetDataKey(key), JsonMapper.ToJson(savedData));
		}
		else
		{
			long num = DateTimeOffset.Now.ToUnixTimeMilliseconds();
			SavedData savedData2 = JsonMapper.ToObject<SavedData>(JsonMapper.ToJson(_saveData));
			savedData2.Time = num.ToString();
			savedData2.SavedName = coverName;
			savedData2.isNew = true;
			savedData2.Key = key;
			GameData.SetString(GameData.GetDataKey(key), JsonMapper.ToJson(savedData2));
		}
	}

	public void RemoveSave(string key)
	{
		SavedData savedData = new SavedData();
		savedData.Time = "1579746794";
		savedData.Key = key;
		savedData.SavedName = LanguageMrg.GetText("A429") + UnityEngine.Random.Range(1, 100);
		GameData.SetString(GameData.GetDataKey(key), JsonMapper.ToJson(savedData));
	}

	public void LoadSaveData()
	{
		string curDataKey = GameData.CurDataKey;
		_saveData = JsonMapper.ToObject<SavedData>(GameData.GetValue(curDataKey, "{}"));
		_saveData.isNew = false;
		_unLockKeys = JsonMapper.ToObject<List<string>>(_saveData.Data.GetValueOrDefault(_unLockSaveKeyStr, "[]"));
		_roundUnLockDic = JsonMapper.ToObject<Dictionary<string, List<string>>>(_saveData.Data.GetValueOrDefault(_roundUnLockDicStr, "{}"));
	}

	public List<SavedData> GetSavedData(List<string> keys, bool isSort = false)
	{
		List<SavedData> list = (from savedData in keys.Select((string s) => GameData.GetValue(GameData.GetDataKey(s), "")).Select(delegate(string s)
			{
				try
				{
					return JsonMapper.ToObject<SavedData>(s);
				}
				catch (Exception)
				{
					return (SavedData)null;
				}
			})
			where savedData != null && savedData.Key != null
			select savedData).ToList();
		if (isSort)
		{
			list.Sort((SavedData a, SavedData b) => -a.Time.ToLong().CompareTo(b.Time.ToLong()));
		}
		return list;
	}

	public bool IsShowChatRed(string key)
	{
		return _saveData.Data.GetValueOrDefault("ChatRed_" + key, false.ToString()).ToBool();
	}

	public void SetShowChatRed(string key, bool show)
	{
		if (IsShowChatRed(key) != show)
		{
			SetChatNewTime(key);
		}
		_saveData.Data["ChatRed_" + key] = show.ToString();
	}

	public bool IsShowPhoneRed()
	{
		return Xlsx_Message_Query.data.Where((Xlsx_Message message) => message.Type == 0).Any((Xlsx_Message message) => IsShowChatRed(message.Key));
	}

	public void AddChat(Xlsx_Message_Key xlsxMessageKey, string assetName, bool isAlwaysEx = false)
	{
		SetChatNewTime(xlsxMessageKey.ToString());
		List<string> list = JsonMapper.ToObject<List<string>>(_saveData.Data.GetValueOrDefault("Chat_" + xlsxMessageKey, "[]"));
		List<MessageData> list2 = JsonMapper.ToObject<List<MessageData>>(_saveData.Data.GetValueOrDefault("Chat_Data" + xlsxMessageKey, "[]"));
		if (isAlwaysEx || !list.Contains(assetName))
		{
			list.Add(assetName);
			list2.Add(new MessageData
			{
				assetName = assetName,
				msgType = ChatMsgType.UnRead
			});
			SetShowChatRed(xlsxMessageKey.ToString(), show: true);
			_saveData.Data["Chat_" + xlsxMessageKey] = JsonMapper.ToJson(list);
			_saveData.Data["Chat_Data" + xlsxMessageKey] = JsonMapper.ToJson(list2);
		}
	}

	public void SetChatNewTime(string xlsxMessageKey)
	{
		_saveData.Data["Chat_Time" + xlsxMessageKey] = DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString();
	}

	public long GetChatNewTime(string xlsxMessageKey)
	{
		return _saveData.Data.GetValueOrDefault("Chat_Time" + xlsxMessageKey, "0").ToLong();
	}

	public List<MessageData> GetChat(Xlsx_Message_Key xlsxMessageKey)
	{
		return GetChat(xlsxMessageKey.ToString());
	}

	public List<MessageData> GetChat(string xlsxMessageKey)
	{
		return JsonMapper.ToObject<List<MessageData>>(_saveData.Data.GetValueOrDefault("Chat_Data" + xlsxMessageKey, "[]"));
	}

	public void SetRead(Xlsx_Message_Key xlsxMessageKey, string assetName)
	{
		SetRead(xlsxMessageKey.ToString(), assetName);
	}

	public void SetRead(string xlsxMessageKey, string assetName)
	{
		List<MessageData> chat = GetChat(xlsxMessageKey);
		for (int i = 0; i < chat.Count; i++)
		{
			if (chat[i].assetName == assetName)
			{
				chat[i].msgType = ChatMsgType.Read;
			}
		}
		_saveData.Data["Chat_Data" + xlsxMessageKey] = JsonMapper.ToJson(chat);
	}

	public string GetMsgSelectId(string key)
	{
		return JsonMapper.ToObject<Dictionary<string, string>>(_saveData.Data.GetValueOrDefault("Chat_Select", "{}")).GetValueOrDefault(key, "");
	}

	public void SetMsgSelectId(string key, string id)
	{
		Dictionary<string, string> dictionary = JsonMapper.ToObject<Dictionary<string, string>>(_saveData.Data.GetValueOrDefault("Chat_Select", "{}"));
		dictionary[key] = id;
		_saveData.Data["Chat_Select"] = JsonMapper.ToJson(dictionary);
	}

	public Dictionary<string, int> GetEquipAdd()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (ZbType zbType = ZbType.TouBu; zbType <= ZbType.PeiShi3; zbType++)
		{
			string curZbKey = GetCurZbKey(zbType);
			if (string.IsNullOrEmpty(curZbKey))
			{
				continue;
			}
			Xlsx_Item xlsx_Item = Xlsx_Item_Query.XlsxDataAsOneKey.ByKeyGetValue(curZbKey);
			for (int i = 0; i < xlsx_Item.AddType.Length; i++)
			{
				if (dictionary.ContainsKey(xlsx_Item.AddType[i]))
				{
					dictionary[xlsx_Item.AddType[i]] += xlsx_Item.AddValue[i];
				}
				else
				{
					dictionary[xlsx_Item.AddType[i]] = xlsx_Item.AddValue[i];
				}
			}
		}
		return dictionary;
	}
}
