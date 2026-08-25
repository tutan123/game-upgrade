using System;
using System.Collections.Generic;
using DG.Tweening;
using FrameWork;
using Script.Mrg;
using Script.Scene;
using Script.UiTool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using XNode;
using Xlsx;

namespace Script.Tool;

public class VideoButton : MonoBehaviour
{
	public BT bt;

	public TMP_Text text;

	public TMP_Text addText;

	public CanvasGroup canvasGroup;

	public Shank shank;

	private VideoNode _videoNode;

	private ButtonNode _buttonNode;

	private Tweener _tween;

	private VideoNode _lastVideoNode;

	private bool _isCanClick;

	public Image[] icon;

	public Image[] tmpIcon;

	public Material clickedMaterial;

	public Material selectedMaterial;

	public Material normalMaterial;

	public TMP_Text[] showTips;

	public GameObject[] showGo;

	public GameObject tips;

	public GameObject roleGo;

	public Image roleImage;

	private VideoItem _videoItem;

	private Action _clickAction;

	private VideoIf _checkVideoIf;

	public void SetVideoItem(VideoItem videoItem)
	{
		_videoItem = videoItem;
	}

	public void Init(string subtitle, float size, VideoNode lastVideoNode, VideoNode videoNode, ButtonNode buttonNode, bool isDoFade = true, Action action = null)
	{
		_clickAction = action;
		if ((bool)bt)
		{
			bt.interactable = true;
		}
		_checkVideoIf = null;
		_isCanClick = true;
		_tween?.Kill();
		_tween = null;
		this.text.text = subtitle;
		this.text.fontSize = size;
		_lastVideoNode = lastVideoNode;
		_videoNode = videoNode;
		_buttonNode = buttonNode;
		if (isDoFade)
		{
			canvasGroup.alpha = 0f;
			_tween = canvasGroup.DOFade(1f, 1f);
		}
		else
		{
			canvasGroup.alpha = 1f;
		}
		for (int i = 0; i < showTips.Length; i++)
		{
			showTips[i].text = LanguageMrg.GetText(_buttonNode.btnName);
		}
		for (int j = 0; j < showGo.Length; j++)
		{
			showGo[j].SetActiveAsCheck(active: false);
		}
		Exit();
		string text = "";
		VideoNode nextVideoNode = videoNode.GetNextVideoNode();
		if ((bool)buttonNode.btnSprite)
		{
			for (int k = 0; k < tmpIcon.Length; k++)
			{
				tmpIcon[k].sprite = buttonNode.btnSprite;
				tmpIcon[k].SetActive(active: true);
			}
			Color color = Color.white;
			if (nextVideoNode.isToNextGroup && nextVideoNode.VideoGroupData != null)
			{
				Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(nextVideoNode.VideoGroupData.xlsxEventKey);
				color = ((xlsx_Event != null && xlsx_Event.AutoSave == 1) ? new Color(1f, 0.8f, 0f) : Color.white);
			}
			for (int l = 0; l < icon.Length; l++)
			{
				icon[l].SetActive(active: false);
				icon[l].color = color;
			}
		}
		else
		{
			for (int m = 0; m < tmpIcon.Length; m++)
			{
				tmpIcon[m].SetActive(active: false);
			}
			Color color2 = Color.white;
			if (nextVideoNode.isToNextGroup && nextVideoNode.VideoGroupData != null)
			{
				Xlsx_Event xlsx_Event2 = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(nextVideoNode.VideoGroupData.xlsxEventKey);
				color2 = ((xlsx_Event2 != null && xlsx_Event2.AutoSave == 1) ? new Color(1f, 0.8f, 0f) : Color.white);
			}
			for (int n = 0; n < icon.Length; n++)
			{
				icon[n].SetActive(active: true);
				icon[n].color = color2;
			}
		}
		int num = 0;
		int num2 = 0;
		if (_videoNode.PropertyData.Count > 0)
		{
			for (int num3 = 0; num3 < _videoNode.PropertyData.Count; num3++)
			{
				if (_videoNode.PropertyData[num3].PropertyType == PropertyType.Property && _videoNode.PropertyData[num3].propertyTypeValue == PropertyTypeValue.Execution)
				{
					num2 += _videoNode.PropertyData[num3].PropertyValue;
				}
				if (_videoNode.PropertyData[num3].PropertyValue < 0 && _videoNode.PropertyData[num3].PropertyType == PropertyType.Property && _videoNode.PropertyData[num3].propertyTypeValue == PropertyTypeValue.Money && _videoNode.PropertyData[num3].PropertyValue < 0)
				{
					num += _videoNode.PropertyData[num3].PropertyValue;
				}
			}
		}
		if (_videoNode.VideoEndPropertyData.Count > 0)
		{
			for (int num4 = 0; num4 < _videoNode.VideoEndPropertyData.Count; num4++)
			{
				if (_videoNode.VideoEndPropertyData[num4].PropertyType == PropertyType.Property && _videoNode.VideoEndPropertyData[num4].propertyTypeValue == PropertyTypeValue.Execution)
				{
					num2 += _videoNode.VideoEndPropertyData[num4].PropertyValue;
				}
				if (_videoNode.VideoEndPropertyData[num4].PropertyValue < 0 && _videoNode.VideoEndPropertyData[num4].PropertyType == PropertyType.Property && _videoNode.VideoEndPropertyData[num4].propertyTypeValue == PropertyTypeValue.Money)
				{
					num += _videoNode.VideoEndPropertyData[num4].PropertyValue;
				}
			}
		}
		if (_videoNode.isPlayerEndPlayerNext)
		{
			List<NodePort> connections = _videoNode.GetOutputPort("nextVideoNode").GetConnections();
			if (_videoNode.isNextCheckShowBtnText)
			{
				List<VideoNode> allVideoNodesOutNor = _videoNode.GetAllVideoNodesOutNor("nextVideoNode");
				if (allVideoNodesOutNor.Count > 0)
				{
					VideoNode videoNode2 = allVideoNodesOutNor[0];
					if (videoNode2.PropertyData.Count > 0)
					{
						for (int num5 = 0; num5 < videoNode2.PropertyData.Count; num5++)
						{
							if (videoNode2.PropertyData[num5].PropertyType == PropertyType.Property && videoNode2.PropertyData[num5].propertyTypeValue == PropertyTypeValue.Execution)
							{
								num2 += videoNode2.PropertyData[num5].PropertyValue;
							}
							if (videoNode2.PropertyData[num5].PropertyValue < 0 && videoNode2.PropertyData[num5].PropertyType == PropertyType.Property && videoNode2.PropertyData[num5].propertyTypeValue == PropertyTypeValue.Money)
							{
								num += videoNode2.PropertyData[num5].PropertyValue;
							}
						}
					}
					if (videoNode2.VideoEndPropertyData.Count > 0)
					{
						for (int num6 = 0; num6 < videoNode2.VideoEndPropertyData.Count; num6++)
						{
							if (videoNode2.VideoEndPropertyData[num6].PropertyType == PropertyType.Property && videoNode2.VideoEndPropertyData[num6].propertyTypeValue == PropertyTypeValue.Execution)
							{
								num2 += videoNode2.VideoEndPropertyData[num6].PropertyValue;
							}
							if (videoNode2.VideoEndPropertyData[num6].PropertyValue < 0 && videoNode2.VideoEndPropertyData[num6].PropertyType == PropertyType.Property && videoNode2.VideoEndPropertyData[num6].propertyTypeValue == PropertyTypeValue.Money)
							{
								num += videoNode2.VideoEndPropertyData[num6].PropertyValue;
							}
						}
					}
				}
			}
			if (connections != null && connections.Count > 0)
			{
				for (int num7 = 0; num7 < connections.Count; num7++)
				{
					if (!(connections[num7].node is VideoIf))
					{
						continue;
					}
					VideoIf videoIf = connections[num7].node as VideoIf;
					if (!videoIf)
					{
						continue;
					}
					for (int num8 = 0; num8 < videoIf.properties.Count; num8++)
					{
						if (videoIf.properties[num8].PropertyType == PropertyType.Property && videoIf.properties[num8].propertyTypeValue == PropertyTypeValue.Charm)
						{
							text += $"<sprite name=MeiLi>{PropertyTypeValue.Charm.GetName()}{videoIf.properties[num8].PropertyValue + 1}\n";
						}
						if (videoIf.properties[num8].PropertyType == PropertyType.Property && videoIf.properties[num8].propertyTypeValue == PropertyTypeValue.Stamina)
						{
							text += $"<sprite name=TiNeng>{PropertyTypeValue.Stamina.GetName()}{videoIf.properties[num8].PropertyValue + 1}\n";
						}
						if (videoIf.properties[num8].PropertyType == PropertyType.Property && videoIf.properties[num8].propertyTypeValue == PropertyTypeValue.Wisdom)
						{
							text += $"<sprite name=ZhiHui>{PropertyTypeValue.Wisdom.GetName()}{videoIf.properties[num8].PropertyValue + 1}\n";
						}
						if (videoIf.properties[num8].PropertyType == PropertyType.Property && videoIf.properties[num8].propertyTypeValue == PropertyTypeValue.CapacityForLiquor)
						{
							text += $"<sprite name=JiuLian>{PropertyTypeValue.CapacityForLiquor.GetName()}{videoIf.properties[num8].PropertyValue + 1}\n";
						}
					}
					_checkVideoIf = videoIf;
					break;
				}
			}
		}
		if (buttonNode.isHasUseProperty)
		{
			for (int num9 = 0; num9 < buttonNode.usePropertyDatas.Count; num9++)
			{
				PropertyData propertyData = buttonNode.usePropertyDatas[num9];
				if (propertyData.PropertyType == PropertyType.Property && propertyData.propertyTypeValue == PropertyTypeValue.Charm)
				{
					text += $"<sprite name=MeiLi>{PropertyTypeValue.Charm.GetName()}{propertyData.PropertyValue + 1}\n";
				}
				if (propertyData.PropertyType == PropertyType.Property && propertyData.propertyTypeValue == PropertyTypeValue.Stamina)
				{
					text += $"<sprite name=TiNeng>{PropertyTypeValue.Stamina.GetName()}{propertyData.PropertyValue + 1}\n";
				}
				if (propertyData.PropertyType == PropertyType.Property && propertyData.propertyTypeValue == PropertyTypeValue.Wisdom)
				{
					text += $"<sprite name=ZhiHui>{PropertyTypeValue.Wisdom.GetName()}{propertyData.PropertyValue + 1}\n";
				}
				if (propertyData.PropertyType == PropertyType.Property && propertyData.propertyTypeValue == PropertyTypeValue.CapacityForLiquor)
				{
					text += $"<sprite name=JiuLian>{PropertyTypeValue.CapacityForLiquor.GetName()}{propertyData.PropertyValue + 1}\n";
				}
			}
		}
		if (buttonNode.isShowResetXdl)
		{
			num2 += SingletonAsMono<GameDataMrg>.Instance.GetProperty("XinDonLiHuiFu", "Property", 0L);
		}
		if (num2 != 0)
		{
			string arg = ((num2 > 0) ? "+" : "");
			text = text + $"<sprite name=TiLi>{arg}{num2}" + "\n";
		}
		if (num < 0)
		{
			text = text + $"<sprite name=Qian>{num}" + "\n";
		}
		if (buttonNode.isJoinCombat)
		{
			text = text + LanguageMrg.GetText("A5331") + "\n";
		}
		if (roleGo != null)
		{
			roleGo.SetActiveAsCheck(active: false);
		}
		VideoGraph[] autoGroup;
		bool flag = nextVideoNode.IsHasAutoEventSAsNotCheck(out autoGroup);
		if ((bool)buttonNode.btnSprite)
		{
			for (int num10 = 0; num10 < tmpIcon.Length; num10++)
			{
				tmpIcon[num10].sprite = buttonNode.btnSprite;
				tmpIcon[num10].SetActive(active: true);
			}
			Color color3 = (flag ? new Color(1f, 0.8f, 0f) : Color.white);
			for (int num11 = 0; num11 < icon.Length; num11++)
			{
				icon[num11].SetActive(active: false);
				icon[num11].color = color3;
			}
		}
		else
		{
			Color color4 = (flag ? new Color(1f, 0.8f, 0f) : Color.white);
			for (int num12 = 0; num12 < icon.Length; num12++)
			{
				icon[num12].SetActive(active: true);
				icon[num12].color = color4;
			}
		}
		if ((bool)tips)
		{
			if (flag)
			{
				for (int num13 = 0; num13 < autoGroup.Length; num13++)
				{
					Xlsx_Event xlsx_Event3 = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(autoGroup[num13].xlsxEventKey);
					if (xlsx_Event3 != null)
					{
						tips.SetActiveAsCheck(xlsx_Event3.AutoSave == 1);
						break;
					}
					tips.SetActiveAsCheck(active: false);
				}
			}
			else
			{
				tips.SetActiveAsCheck(active: false);
			}
		}
		if (roleGo != null && flag)
		{
			for (int num14 = 0; num14 < autoGroup.Length; num14++)
			{
				Xlsx_Event xlsx_Event4 = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(autoGroup[num14].xlsxEventKey);
				if (xlsx_Event4 != null && xlsx_Event4.AutoSave == 1)
				{
					roleGo.SetActiveAsCheck(!string.IsNullOrEmpty(xlsx_Event4.RoleIcon));
					if (!string.IsNullOrEmpty(xlsx_Event4.RoleIcon))
					{
						roleImage.sprite = ABMrg.Load<Sprite>(xlsx_Event4.RoleIcon);
						break;
					}
				}
			}
		}
		if ((bool)addText)
		{
			addText.text = text;
		}
		if ((bool)shank)
		{
			shank.InitPos(base.transform.localPosition);
		}
	}

	public void Enter()
	{
		if ((bool)normalMaterial && (bool)clickedMaterial && (bool)selectedMaterial)
		{
			text.fontMaterial = selectedMaterial;
			if ((bool)addText)
			{
				addText.fontMaterial = selectedMaterial;
			}
		}
	}

	public void Exit()
	{
		if (!normalMaterial || !clickedMaterial || !selectedMaterial)
		{
			return;
		}
		if (!_buttonNode.btnIsNor && SingletonAsMono<GameDataMrg>.Instance.IsUnLockKey(_videoNode.uniqueID))
		{
			text.fontMaterial = clickedMaterial;
			if ((bool)addText)
			{
				addText.fontMaterial = clickedMaterial;
			}
		}
		else
		{
			text.fontMaterial = normalMaterial;
			if ((bool)addText)
			{
				addText.fontMaterial = normalMaterial;
			}
		}
	}

	public void RemoveBtn()
	{
		_isCanClick = false;
		_tween?.Kill();
		_tween = canvasGroup.DOFade(0f, 0.5f);
		Tweener tween = _tween;
		tween.onComplete = (TweenCallback)Delegate.Combine(tween.onComplete, (TweenCallback)delegate
		{
			base.gameObject.SetActive(value: false);
		});
	}

	public ButtonNode GetButtonNode()
	{
		return _buttonNode;
	}

	public VideoNode GetVideoNode()
	{
		return _videoNode;
	}

	public void OnEnter()
	{
		_ = SingletonAsMono<GlobalMrg>.Instance.videoState;
		_ = 2;
	}

	public void OnExit()
	{
		_ = SingletonAsMono<GlobalMrg>.Instance.videoState;
		_ = 2;
	}

	public void Click()
	{
		if (!_isCanClick)
		{
			return;
		}
		if (_buttonNode.isClickSureTips)
		{
			UiManager.OpenUi<SureWindows>().Init(LanguageMrg.GetText(_buttonNode.btnSureTips), ClickSure);
		}
		else if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.Video360Scene)
		{
			int num = 0;
			VideoNode nextVideoNode = _videoNode.GetNextVideoNode();
			for (int i = 0; i < nextVideoNode.PropertyData.Count; i++)
			{
				if (nextVideoNode.PropertyData[i].PropertyValue < 0 && nextVideoNode.PropertyData[i].PropertyType == PropertyType.Property && nextVideoNode.PropertyData[i].propertyTypeValue == PropertyTypeValue.Execution)
				{
					num = nextVideoNode.PropertyData[i].PropertyValue;
				}
			}
			for (int j = 0; j < nextVideoNode.VideoEndPropertyData.Count; j++)
			{
				if (nextVideoNode.VideoEndPropertyData[j].PropertyValue < 0 && nextVideoNode.VideoEndPropertyData[j].PropertyType == PropertyType.Property && nextVideoNode.VideoEndPropertyData[j].propertyTypeValue == PropertyTypeValue.Execution)
				{
					num = nextVideoNode.VideoEndPropertyData[j].PropertyValue;
				}
			}
			if (num < 0)
			{
				UiManager.OpenUi<SureWindows>().Init(string.Format(LanguageMrg.GetText("A5313"), Mathf.Abs(num)), ClickSure);
			}
			else
			{
				ClickSure();
			}
		}
		else
		{
			ClickSure();
		}
	}

	private void ClickSure()
	{
		if (_buttonNode.isOpenUiBtn)
		{
			UiManager.OpenUi(GetType().Assembly.GetType("FrameWork" + _buttonNode.uiName));
			return;
		}
		if (_buttonNode.isCheckVideoExists && !_buttonNode.videoUnlockDatas.IsUnLockAllVideo())
		{
			UiManager.ShowTips(LanguageMrg.GetText("A4633"));
			return;
		}
		if (_buttonNode.isHasUseProperty && !_buttonNode.usePropertyDatas.IsSuc())
		{
			if ((bool)CameraShakeMrg.Instance)
			{
				CameraShakeMrg.Instance.TriggerShake(0.2f, 40f, 5f);
			}
			UiManager.ShowTips(LanguageMrg.GetText(_buttonNode.usePropertyName));
			return;
		}
		if (_buttonNode.isHasUsePropertyOr && !_buttonNode.usePropertyDatasOr.IsSucOr())
		{
			UiManager.ShowTips(LanguageMrg.GetText(_buttonNode.usePropertyNameOr));
			return;
		}
		if (_buttonNode.isSetProperty)
		{
			for (int i = 0; i < _buttonNode.propertyDatas.Count; i++)
			{
				_buttonNode.propertyDatas[i].AddTypeValueAsShowTips();
			}
		}
		if (_buttonNode.isClickExEvent)
		{
			bool flag = false;
			for (int j = 0; j < _buttonNode.eventDatas.Count; j++)
			{
				List<object> eventMsg = EventManager.GetEventMsg();
				eventMsg.Add(_buttonNode.eventDatas[j].eventValue);
				EventManager.DispatchEvent(MessageType.Video, _buttonNode.eventDatas[j].eventName, eventMsg);
				if (_buttonNode.eventDatas[j].isReturn && !flag)
				{
					flag = true;
				}
			}
			if (flag)
			{
				return;
			}
		}
		if ((bool)bt)
		{
			bt.interactable = false;
		}
		try
		{
			VideoNode nextVideoNode = _videoNode.GetNextVideoNode();
			if (nextVideoNode.isToNextGroup && nextVideoNode.VideoGroupData != null)
			{
				string xlsxEventKey = nextVideoNode.VideoGroupData.xlsxEventKey;
				Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxEventKey);
				if (xlsx_Event != null && SingletonAsMono<GameDataMrg>.Instance.IsCanAutoSave(xlsx_Event.Key) && xlsx_Event.Type == 0 && xlsx_Event.AutoSave == 1 && xlsx_Event.NotAutoSave != 1)
				{
					SingletonAsMono<GameDataMrg>.Instance.SetCanAutoSave(xlsx_Event.Key, value: false);
					SingletonAsMono<GameDataMrg>.Instance.AutoSave(LanguageMrg.GetText("A434"));
				}
			}
		}
		catch (Exception ex)
		{
			MyLog.LogWarning(ex.Message);
		}
		_clickAction?.Invoke();
		if (SingletonAsMono<GlobalMrg>.Instance.videoState == VideoState.GameScene)
		{
			SingletonAsMono<GameDataMrg>.Instance.AddUnLockKey(_lastVideoNode.uniqueID);
			SingletonAsMono<GameDataMrg>.Instance.AddUnLockKey(_lastVideoNode.videoPath);
			_videoItem.RemoveBtn();
			if (_videoNode.isClickBtnPlayAudio)
			{
				if ((bool)_videoNode.clickAudio)
				{
					SingletonAsMono<AudioMrg>.Instance.Play(_videoNode.clickAudio);
					GameScene.Instance.PlayText(LanguageMrg.GetText(_buttonNode.btnName), _videoNode.clickAudio.length + 1f);
				}
				if (_videoNode.isClickBtnPlayVideo)
				{
					_videoItem.ToNextVideoNode(_videoNode);
					return;
				}
				for (int k = 0; k < _videoNode.VideoEndPropertyData.Count; k++)
				{
					_videoNode.VideoEndPropertyData[k].AddTypeValueAsShowTips(isCheck: false);
				}
			}
			else
			{
				_videoItem.ToNextVideoNode(_videoNode);
			}
		}
		else if (!Video360Scene.Instance.isLockClick)
		{
			SingletonAsMono<GameDataMrg>.Instance.CurEventKey = ((VideoGraph)_videoNode.graph).xlsxEventKey.ToString();
			SingletonAsMono<GameDataMrg>.Instance.CurVideoId = _videoNode.uniqueID;
			if (_videoNode.is360Video)
			{
				Video360Scene.Instance.Init();
				return;
			}
			Video360Scene.Instance.SaveVideoPos();
			GameScene.Instance.gameCanvas.SetActive(value: true);
			Video360Scene.Instance.HideAllBtn();
			GameScene.Instance.PlayVideo(_videoNode, isShowRaw: true, isCanOnlyVideoLoad: true);
		}
	}
}
