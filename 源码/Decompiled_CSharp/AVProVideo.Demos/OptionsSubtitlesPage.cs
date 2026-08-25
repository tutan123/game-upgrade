using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public class OptionsSubtitlesPage : MonoBehaviour
{
	private class CSubtitleSet
	{
		public GameObject m_LineGO;
	}

	[Header("Media Player")]
	[SerializeField]
	private MediaPlayer _MediaPlayer;

	[SerializeField]
	[Header("Options Menu")]
	private OptionsMenu _OptionsMenu;

	[Header("Content")]
	[SerializeField]
	private Transform _Content;

	[SerializeField]
	private RectTransform _ScrollViewRectTransform;

	[SerializeField]
	private RectTransform _ViewportRectTransform;

	[SerializeField]
	private GameObject _SubtitleLinePrefab;

	private string m_SetupForVideoPath;

	private List<CSubtitleSet> m_lSubtitleSets = new List<CSubtitleSet>();

	private void Start()
	{
		AddSubtitleSet("Off", bEnabled: true);
		UpdateSets();
	}

	private void Update()
	{
		UpdateSets();
	}

	private void AddSubtitleSet(string title, bool bEnabled)
	{
		GameObject gameObject = Object.Instantiate(_SubtitleLinePrefab, _Content);
		if (!(gameObject != null))
		{
			return;
		}
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
			TextTracks textTracks = (_MediaPlayer ? _MediaPlayer.TextTracks.GetTextTracks() : null);
			Button component = gameObject.GetComponent<Button>();
			if ((bool)component && textTracks != null)
			{
				int iIndex = m_lSubtitleSets.Count;
				component.onClick.AddListener(delegate
				{
					_OptionsMenu.ChangeSubtitleTrack((iIndex > 0) ? textTracks[iIndex - 1].Uid : (-1));
				});
			}
		}
		CSubtitleSet cSubtitleSet = new CSubtitleSet();
		cSubtitleSet.m_LineGO = gameObject;
		m_lSubtitleSets.Add(cSubtitleSet);
	}

	public void UpdateSets()
	{
		if (!(_MediaPlayer != null) || !_MediaPlayer.Control.HasMetaData() || (m_SetupForVideoPath != null && m_SetupForVideoPath.Equals(_MediaPlayer.MediaPath.Path)))
		{
			return;
		}
		m_SetupForVideoPath = _MediaPlayer.MediaPath.Path;
		bool flag = true;
		foreach (CSubtitleSet lSubtitleSet in m_lSubtitleSets)
		{
			if (!flag)
			{
				Object.Destroy(lSubtitleSet.m_LineGO);
				lSubtitleSet.m_LineGO = null;
			}
			flag = false;
		}
		if (m_lSubtitleSets.Count > 1)
		{
			m_lSubtitleSets.RemoveRange(1, m_lSubtitleSets.Count - 1);
		}
		foreach (TextTrack textTrack in _MediaPlayer.TextTracks.GetTextTracks())
		{
			AddSubtitleSet(textTrack.DisplayName, bEnabled: false);
		}
		if (m_lSubtitleSets.Count > 1)
		{
			float num = 40f;
			float num2 = num * (float)m_lSubtitleSets.Count;
			RectTransform rectTransform = ((_Content != null) ? _Content.GetComponent<RectTransform>() : null);
			if (rectTransform != null)
			{
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, num2);
			}
			float num3 = num2 * 0.5f - num * 0.5f;
			foreach (CSubtitleSet lSubtitleSet2 in m_lSubtitleSets)
			{
				RectTransform component = lSubtitleSet2.m_LineGO.GetComponent<RectTransform>();
				if ((bool)component)
				{
					component.anchoredPosition = new Vector2(0f, num3);
					num3 -= num;
				}
			}
			if ((bool)_ViewportRectTransform && (bool)_ScrollViewRectTransform)
			{
				float max = 222f;
				float num4 = Mathf.Clamp(num2, num, max);
				float num5 = 12f;
				RectTransform component2 = base.transform.GetComponent<RectTransform>();
				component2.sizeDelta = new Vector2(component2.sizeDelta.x, 60f + num4 + num5);
				_ScrollViewRectTransform.sizeDelta = new Vector2(_ScrollViewRectTransform.sizeDelta.x, num4);
				_ViewportRectTransform.sizeDelta = new Vector2(_ViewportRectTransform.sizeDelta.x, num4);
			}
		}
		ChangeSubtitleTrack(_MediaPlayer.TextTracks.GetTextTracks().GetActiveTrackIndex(), bSetTrack: false);
	}

	public void ChangeSubtitleTrack(int iTrackUid, bool bSetTrack = true)
	{
		TextTracks textTracks = (_MediaPlayer ? _MediaPlayer.TextTracks.GetTextTracks() : null);
		if (textTracks == null)
		{
			return;
		}
		int textTrackArrayIndexFromUid = _MediaPlayer.TextTracks.GetTextTrackArrayIndexFromUid(iTrackUid);
		if (bSetTrack)
		{
			_MediaPlayer.TextTracks.SetActiveTextTrack((textTrackArrayIndexFromUid > -1) ? textTracks[textTrackArrayIndexFromUid] : null);
		}
		int num = 0;
		foreach (CSubtitleSet lSubtitleSet in m_lSubtitleSets)
		{
			Transform transform = lSubtitleSet.m_LineGO.transform.Find("TickIcon");
			Image image = ((transform != null) ? transform.GetComponent<Image>() : null);
			if (image != null)
			{
				image.enabled = num == textTrackArrayIndexFromUid + 1;
			}
			num++;
		}
	}
}
