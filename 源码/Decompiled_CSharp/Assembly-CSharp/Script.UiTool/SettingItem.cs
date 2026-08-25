using System;
using System.Collections.Generic;
using FrameWork;
using FrameWork.Data;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace Script.UiTool;

public class SettingItem : MonoBehaviour
{
	public SettingType settingType;

	public TMP_Text typeName;

	public TMP_Text valueName;

	public RectTransform volumeGroup;

	public Slider slider;

	public Image openIcon;

	private void Start()
	{
		if (settingType == SettingType.HighMode)
		{
			base.gameObject.SetActiveAsCheck(SdkMrg.IsDown4KDlc());
		}
	}

	private void OnEnable()
	{
		Init();
		EventManager.AddListener(MessageType.Game, GameMessageType.SetLanguage, SetLanguage);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.SetLanguage, SetLanguage);
	}

	private void SetLanguage(List<object> objects)
	{
		Init();
	}

	private void Init()
	{
		switch (settingType)
		{
		case SettingType.Sound:
			typeName.text = LanguageMrg.GetText("A96");
			slider.value = GameData.GetOpenAsNum(settingType.ToString());
			valueName.text = ((int)(GameData.GetOpenAsNum(settingType.ToString()) * 100f)).ToString();
			break;
		case SettingType.Subtitle:
			typeName.text = LanguageMrg.GetText("A100");
			openIcon.SetActive(GameData.IsOpen(settingType.ToString()));
			break;
		case SettingType.AnchorMode:
			typeName.text = LanguageMrg.GetText("A102");
			openIcon.SetActive(GameData.IsOpen(settingType.ToString()));
			break;
		case SettingType.RoleVolume:
			typeName.text = LanguageMrg.GetText("A99");
			slider.value = GameData.GetOpenAsNum(settingType.ToString());
			valueName.text = ((int)(GameData.GetOpenAsNum(settingType.ToString()) * 100f)).ToString();
			break;
		case SettingType.DisplayMode:
			typeName.text = LanguageMrg.GetText("A98");
			valueName.text = (GameData.IsOpen(settingType.ToString()) ? LanguageMrg.GetText("A105") : LanguageMrg.GetText("A106"));
			break;
		case SettingType.Language:
			typeName.text = LanguageMrg.GetText("A101");
			valueName.text = GameData.Language.GetLanguage();
			break;
		case SettingType.Quality:
			typeName.text = LanguageMrg.GetText("A1698");
			valueName.text = GameData.GetQualityText();
			break;
		case SettingType.Volume:
			typeName.text = LanguageMrg.GetText("A97");
			slider.value = GameData.GetOpenAsNum(settingType.ToString());
			valueName.text = ((int)(GameData.GetOpenAsNum(settingType.ToString()) * 100f)).ToString();
			break;
		case SettingType.HighMode:
			typeName.text = LanguageMrg.GetText("A1276");
			openIcon.SetActive(GameData.IsOpen(settingType.ToString(), def: false));
			break;
		}
	}

	public void SetValue(bool value)
	{
		switch (settingType)
		{
		case SettingType.Subtitle:
		case SettingType.AnchorMode:
		case SettingType.HighMode:
			GameData.SetOpen(settingType.ToString(), !GameData.IsOpen(settingType.ToString()));
			break;
		case SettingType.DisplayMode:
			GameData.SetOpen(settingType.ToString(), !GameData.IsOpen(settingType.ToString()));
			EventManager.DispatchEvent(MessageType.Game, GameMessageType.SetDisplayMode);
			GameData.SetDisPlay();
			break;
		case SettingType.Language:
			GameData.Language = (value ? Enum.Parse<Xlsx_Language_Type>(FrameWork.Tool.GetNextLanguage()) : Enum.Parse<Xlsx_Language_Type>(FrameWork.Tool.GetLastLanguage()));
			EventManager.DispatchEvent(MessageType.Game, GameMessageType.SetLanguage);
			break;
		case SettingType.Quality:
		{
			float openAsNum = GameData.GetOpenAsNum(settingType.ToString(), 3f);
			openAsNum = ((!value) ? (openAsNum - 1f) : (openAsNum + 1f));
			GameData.SetOpenAsNum(settingType.ToString(), Mathf.Clamp(openAsNum, 1f, 3f));
			GameData.SetQualityApply();
			break;
		}
		}
		Init();
	}

	public void SetValueAsNum(float s)
	{
		switch (settingType)
		{
		case SettingType.Volume:
			GameData.SetOpenAsNum(settingType.ToString(), s);
			valueName.text = ((int)(s * 100f)).ToString();
			EventManager.DispatchEvent(MessageType.Game, GameMessageType.ChangeVolume);
			break;
		case SettingType.Sound:
		case SettingType.RoleVolume:
			GameData.SetOpenAsNum(settingType.ToString(), s);
			valueName.text = ((int)(s * 100f)).ToString();
			break;
		case SettingType.DisplayMode:
			break;
		}
	}
}
