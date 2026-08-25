using System;
using System.Collections.Generic;
using System.Linq;
using FrameWork;
using Script.Mrg;
using Script.Scene;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UiTool;

public class MapTool : MonoBehaviour
{
	public RectTransform content;

	public Image mapImg;

	public TMP_Text mapUnlockText;

	private int[] _weekEnd = new int[4] { 4, 5, 11, 12 };

	private List<Map3DTriggerr> _list = new List<Map3DTriggerr>();

	private void OnEnable()
	{
		EventManager.AddListener(MessageType.Game, GameMessageType.UpdateMapUnLockPoint, UpdateMapUnLockPoint);
	}

	private void OnDisable()
	{
		EventManager.RemoveListener(MessageType.Game, GameMessageType.UpdateMapUnLockPoint, UpdateMapUnLockPoint);
	}

	private void UpdateMapUnLockPoint(List<object> list)
	{
		InitUnlockPointText();
	}

	public void Init()
	{
		_list.Clear();
		Transform pointGroup = Map3DScene.Instance.pointGroup;
		for (int j = 0; j < pointGroup.childCount; j++)
		{
			try
			{
				Map3DTriggerr component = pointGroup.GetChild(j).GetComponent<Map3DTriggerr>();
				if (component != null && component.gameObject.activeSelf && component.eventData != null && component.eventData.IsNotShowMap != 1 && (!FrameWork.Tool.GetMapLockPoint().Contains(component.collType) || SingletonAsMono<GameDataMrg>.Instance.IsUnLockPoint(component.collType.ToString())))
				{
					_list.Add(component);
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
			}
		}
		content.TranFor(_list.Count, content.GetChild(0), delegate(int i, GameObject o)
		{
			try
			{
				o.GetComponent<MapItem>().Init(_list[i], mapImg);
			}
			catch (Exception ex2)
			{
				Debug.LogError(ex2.Message);
			}
		});
		SetPos();
		InitUnlockPointText();
	}

	public void InitUnlockPointText()
	{
		List<Map3DTriggerr> list = new List<Map3DTriggerr>();
		CollType[] mapLockPoint = FrameWork.Tool.GetMapLockPoint();
		for (int i = 0; i < Map3DScene.Instance.pointGroup.childCount; i++)
		{
			try
			{
				if (Map3DScene.Instance.pointGroup.GetChild(i).gameObject.activeSelf)
				{
					Map3DTriggerr component = Map3DScene.Instance.pointGroup.GetChild(i).GetComponent<Map3DTriggerr>();
					if (mapLockPoint.Contains(component.collType))
					{
						list.Add(component);
					}
				}
			}
			catch (Exception ex)
			{
				Debug.LogError(ex.Message);
			}
		}
		float num = 0f;
		for (int j = 0; j < list.Count; j++)
		{
			try
			{
				if (!SingletonAsMono<GameDataMrg>.Instance.IsUnLockPoint(list[j].collType.ToString()))
				{
					num += 1f;
				}
			}
			catch (Exception ex2)
			{
				Debug.LogError(ex2.Message);
			}
		}
		mapUnlockText.text = string.Format(LanguageMrg.GetText("A6363"), num);
		mapUnlockText.SetActive(num > 0f);
	}

	private void SetPos()
	{
		List<Transform> list = new List<Transform>();
		for (int i = 0; i < content.childCount; i++)
		{
			list.Add(content.GetChild(i));
		}
		list.Sort((Transform a, Transform b) => -a.localPosition.y.CompareTo(b.localPosition.y));
		for (int j = 0; j < list.Count; j++)
		{
			list[j].SetSiblingIndex(j);
		}
	}

	public void ClickToMap()
	{
		SingletonAsMono<GameDataMrg>.Instance.Is2D = false;
		UiManager.GetUi<MainWindows>().ChangeMap();
	}
}
