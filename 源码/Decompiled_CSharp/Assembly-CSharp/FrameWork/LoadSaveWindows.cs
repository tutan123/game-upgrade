using System;
using System.Collections;
using System.Collections.Generic;
using Script.Mrg;
using Script.Tool;
using Script.UiTool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "LoadSaveWindows")]
[UiMode(Mode.Normal, false)]
public class LoadSaveWindows : UiActor
{
	private Action _loadClose;

	private bool _isLoad;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformTitle;

	public CanvasRenderer CanvasRendererTitle;

	public TextMeshProUGUI TextMeshProUGUITitle;

	public AddScripts AddScriptsTitle;

	public RectTransform RectTransformAutoContent;

	public AddScripts AddScriptsAutoContent;

	public GridLayoutGroup GridLayoutGroupAutoContent;

	public ContentSizeFitter ContentSizeFitterAutoContent;

	public RectTransform RectTransformSaveContent;

	public AddScripts AddScriptsSaveContent;

	public GridLayoutGroup GridLayoutGroupSaveContent;

	public ContentSizeFitter ContentSizeFitterSaveContent;

	public CanvasRenderer CanvasRendererSaveContent;

	public RectTransform RectTransformLoad;

	public CanvasRenderer CanvasRendererLoad;

	public Image ImageLoad;

	public AddScripts AddScriptsLoad;

	public override void Awake()
	{
		base.Awake();
		RectTransformClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("Bg/Close/").GetComponent<AddScripts>();
		RectTransformTitle = GetGameObject().transform.Find("Bg/Title/").GetComponent<RectTransform>();
		CanvasRendererTitle = GetGameObject().transform.Find("Bg/Title/").GetComponent<CanvasRenderer>();
		TextMeshProUGUITitle = GetGameObject().transform.Find("Bg/Title/").GetComponent<TextMeshProUGUI>();
		AddScriptsTitle = GetGameObject().transform.Find("Bg/Title/").GetComponent<AddScripts>();
		RectTransformAutoContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/AutoScroll View/Viewport/AutoContent/").GetComponent<RectTransform>();
		AddScriptsAutoContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/AutoScroll View/Viewport/AutoContent/").GetComponent<AddScripts>();
		GridLayoutGroupAutoContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/AutoScroll View/Viewport/AutoContent/").GetComponent<GridLayoutGroup>();
		ContentSizeFitterAutoContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/AutoScroll View/Viewport/AutoContent/").GetComponent<ContentSizeFitter>();
		RectTransformSaveContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/SaveScroll View/SaveContent/").GetComponent<RectTransform>();
		AddScriptsSaveContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/SaveScroll View/SaveContent/").GetComponent<AddScripts>();
		GridLayoutGroupSaveContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/SaveScroll View/SaveContent/").GetComponent<GridLayoutGroup>();
		ContentSizeFitterSaveContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/SaveScroll View/SaveContent/").GetComponent<ContentSizeFitter>();
		CanvasRendererSaveContent = GetGameObject().transform.Find("View/Bg/Scroll View/Viewport/Content/SaveScroll View/SaveContent/").GetComponent<CanvasRenderer>();
		RectTransformLoad = GetGameObject().transform.Find("View/Load/").GetComponent<RectTransform>();
		CanvasRendererLoad = GetGameObject().transform.Find("View/Load/").GetComponent<CanvasRenderer>();
		ImageLoad = GetGameObject().transform.Find("View/Load/").GetComponent<Image>();
		AddScriptsLoad = GetGameObject().transform.Find("View/Load/").GetComponent<AddScripts>();
	}

	public LoadSaveWindows(Transform trans)
		: base(trans)
	{
	}

	public LoadSaveWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(CloseUi);
	}

	private void NewSave()
	{
		UiManager.OpenUi<InputWindows>().Init(LanguageMrg.GetText("A1592"), delegate(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				s = LanguageMrg.GetText("A434");
			}
			SingletonAsMono<GameDataMrg>.Instance.CurNewSaved(s);
			Init();
		});
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
	}

	public void InitType(bool isLoad = true, Action action = null, bool isInit = false)
	{
		SingletonAsMono<Mono>.Instance.StartCoroutine(CreateSaveData());
		IEnumerator CreateSaveData()
		{
			while (RectTransformAutoContent == null || RectTransformSaveContent == null || RectTransformLoad == null)
			{
				yield return null;
			}
			Tool.HideAllChild(RectTransformAutoContent);
			Tool.HideAllChild(RectTransformSaveContent);
			RectTransformLoad.SetActive(active: true);
			if (SingletonAsMono<GameDataMrg>.Instance.GetSaveKeys().Count == 0)
			{
				yield return new WaitForSeconds(0.3f);
				for (int num = SingletonAsMono<GameDataMrg>.Instance.SaveMaxCount; num > 0; num--)
				{
					SingletonAsMono<GameDataMrg>.Instance.NewSaved(LanguageMrg.GetText("A429"));
				}
			}
			yield return null;
			if (isLoad)
			{
				TextMeshProUGUITitle.text = LanguageMrg.GetText("A1369");
			}
			else
			{
				TextMeshProUGUITitle.text = LanguageMrg.GetText("A1368");
			}
			_isLoad = isLoad;
			_loadClose = action;
			yield return Init();
			RectTransformLoad.SetActive(active: false);
		}
	}

	public Coroutine Init()
	{
		return SingletonAsMono<Mono>.Instance.StartCoroutine(LoadSave());
		IEnumerator LoadSave()
		{
			List<string> autoSaveKeys = SingletonAsMono<GameDataMrg>.Instance.GetAutoSaveKeys();
			List<SavedData> autoSave = SingletonAsMono<GameDataMrg>.Instance.GetSavedData(autoSaveKeys);
			List<string> saveKeys = SingletonAsMono<GameDataMrg>.Instance.GetSaveKeys();
			List<SavedData> savedData = SingletonAsMono<GameDataMrg>.Instance.GetSavedData(saveKeys);
			string autoNewKey = GetNewKey(autoSave);
			RectTransformAutoContent.TranFor(autoSave.Count, RectTransformAutoContent.GetChild(0), delegate(int i, GameObject o)
			{
				o.GetComponent<SaveItem>().Init(autoSave[i], i, _isLoad, autoSave[i].Key == autoNewKey);
			});
			string newKey = GetNewKey(savedData);
			RectTransformSaveContent.TranFor(savedData.Count, RectTransformSaveContent.GetChild(0), delegate(int i, GameObject o)
			{
				o.GetComponent<SaveItem>().Init(savedData[i], i, _isLoad, savedData[i].Key == newKey);
			});
			yield return null;
		}
	}

	private string GetNewKey(List<SavedData> savedData)
	{
		string result = "";
		long num = 0L;
		for (int i = 0; i < savedData.Count; i++)
		{
			if (long.TryParse(savedData[i].Time, out var result2) && result2 > num)
			{
				result = savedData[i].Key;
				num = result2;
			}
		}
		return result;
	}

	public void LoadClose()
	{
		_loadClose?.Invoke();
		CloseUi();
	}

	public override void OnClose()
	{
		base.OnClose();
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (_isLoad)
		{
			RemoveUi(GetIndex());
		}
		else if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}
}
