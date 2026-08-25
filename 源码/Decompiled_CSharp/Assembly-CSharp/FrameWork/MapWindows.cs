using System;
using System.Collections.Generic;
using Script.Mrg;
using Script.Scene;
using Script.Tool;
using Script.UiTool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Xlsx;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "MapWindows")]
public class MapWindows : UiActor
{
	private int _p = 25;

	private List<Transform> _list = new List<Transform>();

	public RectTransform RectTransformRawImage;

	public CanvasRenderer CanvasRendererRawImage;

	public RawImage RawImageRawImage;

	public AddScripts AddScriptsRawImage;

	public RectTransform RectTransformRiMap;

	public CanvasRenderer CanvasRendererRiMap;

	public Image ImageRiMap;

	public AddScripts AddScriptsRiMap;

	public RectTransform RectTransformYeMap;

	public CanvasRenderer CanvasRendererYeMap;

	public Image ImageYeMap;

	public AddScripts AddScriptsYeMap;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformPointGroup;

	public AddScripts AddScriptsPointGroup;

	public RectTransform RectTransformPlayPos;

	public AddScripts AddScriptsPlayPos;

	public RectTransform RectTransformMeIcon;

	public CanvasRenderer CanvasRendererMeIcon;

	public Image ImageMeIcon;

	public AddScripts AddScriptsMeIcon;

	public override void Awake()
	{
		base.Awake();
		RectTransformRawImage = GetGameObject().transform.Find("View/RawImage/").GetComponent<RectTransform>();
		CanvasRendererRawImage = GetGameObject().transform.Find("View/RawImage/").GetComponent<CanvasRenderer>();
		RawImageRawImage = GetGameObject().transform.Find("View/RawImage/").GetComponent<RawImage>();
		AddScriptsRawImage = GetGameObject().transform.Find("View/RawImage/").GetComponent<AddScripts>();
		RectTransformRiMap = GetGameObject().transform.Find("View/RawImage/RiMap/").GetComponent<RectTransform>();
		CanvasRendererRiMap = GetGameObject().transform.Find("View/RawImage/RiMap/").GetComponent<CanvasRenderer>();
		ImageRiMap = GetGameObject().transform.Find("View/RawImage/RiMap/").GetComponent<Image>();
		AddScriptsRiMap = GetGameObject().transform.Find("View/RawImage/RiMap/").GetComponent<AddScripts>();
		RectTransformYeMap = GetGameObject().transform.Find("View/RawImage/YeMap/").GetComponent<RectTransform>();
		CanvasRendererYeMap = GetGameObject().transform.Find("View/RawImage/YeMap/").GetComponent<CanvasRenderer>();
		ImageYeMap = GetGameObject().transform.Find("View/RawImage/YeMap/").GetComponent<Image>();
		AddScriptsYeMap = GetGameObject().transform.Find("View/RawImage/YeMap/").GetComponent<AddScripts>();
		RectTransformClose = GetGameObject().transform.Find("View/RawImage/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("View/RawImage/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("View/RawImage/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("View/RawImage/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("View/RawImage/Close/").GetComponent<AddScripts>();
		RectTransformPointGroup = GetGameObject().transform.Find("View/RawImage/PointGroup/").GetComponent<RectTransform>();
		AddScriptsPointGroup = GetGameObject().transform.Find("View/RawImage/PointGroup/").GetComponent<AddScripts>();
		RectTransformPlayPos = GetGameObject().transform.Find("View/RawImage/PlayPos/").GetComponent<RectTransform>();
		AddScriptsPlayPos = GetGameObject().transform.Find("View/RawImage/PlayPos/").GetComponent<AddScripts>();
		RectTransformMeIcon = GetGameObject().transform.Find("View/RawImage/PlayPos/MeIcon/").GetComponent<RectTransform>();
		CanvasRendererMeIcon = GetGameObject().transform.Find("View/RawImage/PlayPos/MeIcon/").GetComponent<CanvasRenderer>();
		ImageMeIcon = GetGameObject().transform.Find("View/RawImage/PlayPos/MeIcon/").GetComponent<Image>();
		AddScriptsMeIcon = GetGameObject().transform.Find("View/RawImage/PlayPos/MeIcon/").GetComponent<AddScripts>();
	}

	public MapWindows(Transform trans)
		: base(trans)
	{
	}

	public MapWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(CloseUi);
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		if (Map3DScene.Instance != null)
		{
			UpdateMapIcon();
		}
		else
		{
			Tool.HideAllChild(RectTransformPointGroup);
		}
		RectTransformRiMap.SetActive(SingletonAsMono<GameDataMrg>.Instance.IsMorning);
		RectTransformYeMap.SetActive(!SingletonAsMono<GameDataMrg>.Instance.IsMorning);
	}

	private void UpdateMapIcon()
	{
		_list.Clear();
		Transform pointGroup = Map3DScene.Instance.pointGroup;
		for (int j = 0; j < pointGroup.childCount; j++)
		{
			Map3DTriggerr component = pointGroup.GetChild(j).GetComponent<Map3DTriggerr>();
			if ((component != null && component.gameObject.activeSelf && component.eventData != null) || component.collType == CollType.ChongZhi)
			{
				_list.Add(pointGroup.GetChild(j));
			}
		}
		RectTransformPointGroup.TranFor(_list.Count, RectTransformPointGroup.GetChild(0), delegate(int i, GameObject o)
		{
			Vector3 vector = Map3DScene.Instance.mapCamera.WorldToViewportPoint(_list[i].position);
			float x = (vector.x - 0.5f) * RectTransformRawImage.rect.width;
			float y = (vector.y - 0.5f) * RectTransformRawImage.rect.height;
			o.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);
			o.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>()
				.text = LanguageMrg.GetText(_list[i].GetComponent<Map3DTriggerr>().languageComponent.key);
			string curZbKey = SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.ZaiJu);
			Xlsx_Item xlsxItem = null;
			if (!string.IsNullOrEmpty(curZbKey))
			{
				try
				{
					xlsxItem = Xlsx_Item_Query.XlsxDataAsOneKey.ByKeyGetValue(curZbKey);
				}
				catch (Exception ex)
				{
					Debug.LogWarning(ex.Message);
				}
			}
			Transform child = o.transform.GetChild(1).GetChild(0).GetChild(0);
			Map3DTriggerr map3DTriggerr = _list[i].GetComponent<Map3DTriggerr>();
			bool flag = false;
			if (map3DTriggerr.videoGraph != null)
			{
				List<VideoNode> allVideoNodesOutNor = map3DTriggerr.videoGraph.GetEvenFistNode().GetAllVideoNodesOutNor();
				for (int k = 0; k < allVideoNodesOutNor.Count; k++)
				{
					VideoNode nextVideoNode = allVideoNodesOutNor[k].GetNextVideoNode();
					if (nextVideoNode.isToNextGroup)
					{
						try
						{
							string xlsxEventKey = nextVideoNode.VideoGroupData.xlsxEventKey;
							Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxEventKey);
							if (xlsx_Event != null && SingletonAsMono<GameDataMrg>.Instance.IsCanAutoSave(xlsx_Event.Key) && xlsx_Event.Type == 0 && xlsx_Event.AutoSave == 1)
							{
								flag = true;
								break;
							}
						}
						catch (Exception ex2)
						{
							Debug.LogError(ex2.Message);
						}
					}
				}
			}
			child.SetActive(flag || (map3DTriggerr != null && map3DTriggerr.eventData != null && map3DTriggerr.eventData.AutoSave == 1));
			o.GetComponent<BT>().onClick.RemoveAllListeners();
			o.GetComponent<BT>().onClick.AddListener(delegate
			{
				if ((xlsxItem == null && map3DTriggerr.collType != CollType.ChongZhi) || (xlsxItem != null && xlsxItem.ZjType != 2 && map3DTriggerr.collType != CollType.ChongZhi))
				{
					UiManager.OpenUi<SureWindows>().Init(LanguageMrg.GetText("A1671"), delegate
					{
						if (SingletonAsMono<GameDataMrg>.Instance.GetProperty("Money", "Property", 0L) >= _p)
						{
							Map3DScene.Instance.player.transform.position = _list[i].position;
							Map3DScene.Instance.player.Warp(_list[i].position);
							new PropertyData
							{
								PropertyType = PropertyType.Property,
								propertyTypeValue = PropertyTypeValue.Money,
								PropertyValue = -_p
							}.AddTypeValueAsShowTips();
							CloseUi();
						}
						else
						{
							UiManager.ShowTips(LanguageMrg.GetText("A1673"));
						}
					});
				}
				else
				{
					string title = ((map3DTriggerr.collType == CollType.ChongZhi) ? LanguageMrg.GetText("A1680") : LanguageMrg.GetText("A1672"));
					UiManager.OpenUi<SureWindows>().Init(title, delegate
					{
						Map3DScene.Instance.player.transform.position = _list[i].position;
						Map3DScene.Instance.player.Warp(_list[i].position);
						CloseUi();
					});
				}
			});
		});
		InitPlayerLoc();
	}

	private void InitPlayerLoc()
	{
		Vector3 vector = Map3DScene.Instance.mapCamera.WorldToViewportPoint(Map3DScene.Instance.player.transform.position);
		float x = (vector.x - 0.5f) * RectTransformRawImage.rect.width;
		float y = (vector.y - 0.5f) * RectTransformRawImage.rect.height;
		RectTransformPlayPos.anchoredPosition = new Vector2(x, y);
		float y2 = Map3DScene.Instance.player.transform.eulerAngles.y;
		Debug.Log(y2);
		RectTransformMeIcon.rotation = Quaternion.Euler(0f, 0f, 0f - y2);
	}
}
