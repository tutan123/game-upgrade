using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPlaybackSpeedPage : MonoBehaviour
{
	private class CPlaybackSpeedSet
	{
		public GameObject m_LineGO;

		public string m_DisplayName = "";

		public float m_fRate = 1f;
	}

	[Header("Media Player")]
	[SerializeField]
	private MediaPlayer _MediaPlayer;

	[Header("Options Menu")]
	[SerializeField]
	private OptionsMenu _OptionsMenu;

	[SerializeField]
	[Header("Content")]
	private Transform _Content;

	[SerializeField]
	private RectTransform _ScrollViewRectTransform;

	[SerializeField]
	private RectTransform _ViewportRectTransform;

	[SerializeField]
	private GameObject _PlaybackSpeedLinePrefab;

	private string m_SetupForVideoPath;

	private List<CPlaybackSpeedSet> m_lPlaybackSpeedSets = new List<CPlaybackSpeedSet>();

	private bool m_bSetsDirty;

	private void Start()
	{
		AddPlaybackSpeedSet("0.25", 0.25f, bEnabled: false);
		AddPlaybackSpeedSet("0.5", 0.5f, bEnabled: false);
		AddPlaybackSpeedSet("0.75", 0.75f, bEnabled: false);
		AddPlaybackSpeedSet("Normal", 1f, bEnabled: true);
		AddPlaybackSpeedSet("1.25", 1.25f, bEnabled: false);
		AddPlaybackSpeedSet("1.5", 1.5f, bEnabled: false);
		AddPlaybackSpeedSet("1.75", 1.75f, bEnabled: false);
	}

	private void Update()
	{
		if (m_bSetsDirty)
		{
			UpdateSets();
			m_bSetsDirty = false;
		}
	}

	private void AddPlaybackSpeedSet(string title, float fRate, bool bEnabled)
	{
		GameObject gameObject = Object.Instantiate(_PlaybackSpeedLinePrefab, _Content);
		if (gameObject != null)
		{
			Transform transform = gameObject.transform.Find("TitleText");
			Text text = ((transform != null) ? transform.GetComponent<Text>() : null);
			if (text != null)
			{
				text.text = title;
			}
			if (bEnabled)
			{
				Transform transform2 = gameObject.transform.Find("TickIcon");
				Image image = ((transform2 != null) ? transform2.GetComponent<Image>() : null);
				if (image != null)
				{
					image.enabled = true;
				}
			}
			if ((bool)_OptionsMenu)
			{
				Button component = gameObject.GetComponent<Button>();
				if ((bool)component)
				{
					int iIndex = m_lPlaybackSpeedSets.Count;
					component.onClick.AddListener(delegate
					{
						_OptionsMenu.ChangePlaybackSpeed(iIndex);
					});
				}
			}
			CPlaybackSpeedSet cPlaybackSpeedSet = new CPlaybackSpeedSet();
			cPlaybackSpeedSet.m_LineGO = gameObject;
			cPlaybackSpeedSet.m_fRate = fRate;
			cPlaybackSpeedSet.m_DisplayName = title;
			m_lPlaybackSpeedSets.Add(cPlaybackSpeedSet);
		}
		m_bSetsDirty = true;
	}

	public string GetDisplayNameForIndex(int iIndex)
	{
		if (iIndex > -1 && iIndex < m_lPlaybackSpeedSets.Count)
		{
			return m_lPlaybackSpeedSets[iIndex].m_DisplayName;
		}
		return "Normal";
	}

	public void UpdateSets()
	{
		if (m_lPlaybackSpeedSets.Count <= 1)
		{
			return;
		}
		float num = 40f;
		float num2 = num * (float)m_lPlaybackSpeedSets.Count;
		RectTransform rectTransform = ((_Content != null) ? _Content.GetComponent<RectTransform>() : null);
		if (rectTransform != null)
		{
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, num2);
		}
		float num3 = num2 * 0.5f - num * 0.5f;
		foreach (CPlaybackSpeedSet lPlaybackSpeedSet in m_lPlaybackSpeedSets)
		{
			RectTransform component = lPlaybackSpeedSet.m_LineGO.GetComponent<RectTransform>();
			if ((bool)component)
			{
				component.anchoredPosition = new Vector2(0f, num3);
				num3 -= num;
			}
		}
		if ((bool)_ViewportRectTransform && (bool)_ScrollViewRectTransform)
		{
			float max = 330f;
			float num4 = Mathf.Clamp(num2, num, max);
			float num5 = 12f;
			RectTransform component2 = base.transform.GetComponent<RectTransform>();
			component2.sizeDelta = new Vector2(component2.sizeDelta.x, 60f + num4 + num5);
			_ScrollViewRectTransform.sizeDelta = new Vector2(_ScrollViewRectTransform.sizeDelta.x, num4);
			_ViewportRectTransform.sizeDelta = new Vector2(_ViewportRectTransform.sizeDelta.x, num4);
		}
	}

	public void ChangeVideoPlaybackSpeed(int iPlaybackSpeedIndex)
	{
		_MediaPlayer.VideoTracks.GetActiveVideoTrack();
		if (!_MediaPlayer)
		{
			return;
		}
		_MediaPlayer.Control.SetPlaybackRate(m_lPlaybackSpeedSets[iPlaybackSpeedIndex].m_fRate);
		int num = 0;
		foreach (CPlaybackSpeedSet lPlaybackSpeedSet in m_lPlaybackSpeedSets)
		{
			Transform transform = lPlaybackSpeedSet.m_LineGO.transform.Find("TickIcon");
			Image image = ((transform != null) ? transform.GetComponent<Image>() : null);
			if (image != null)
			{
				image.enabled = num == iPlaybackSpeedIndex;
			}
			num++;
		}
	}
}
