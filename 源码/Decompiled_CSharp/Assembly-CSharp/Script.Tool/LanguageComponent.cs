using System.Collections.Generic;
using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using Xlsx;

namespace Script.Tool;

public class LanguageComponent : MonoBehaviour
{
	public Xlsx_Language_Key key;

	private TMP_Text _text;

	private void Awake()
	{
		_text = GetComponent<TMP_Text>();
	}

	private void Start()
	{
		Init();
	}

	private void OnEnable()
	{
		EventManager.AddListener(MessageType.Game, GameMessageType.SetLanguage, SetLanguage);
		Init();
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.SetLanguage, SetLanguage);
	}

	private void Init()
	{
		_text.text = LanguageMrg.GetText(key);
	}

	private void SetLanguage(List<object> objects)
	{
		Init();
	}
}
