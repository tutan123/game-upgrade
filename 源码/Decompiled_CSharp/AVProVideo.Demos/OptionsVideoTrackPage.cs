using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public class OptionsVideoTrackPage : MonoBehaviour
{
	private class CVideoTrackSet
	{
		public GameObject m_LineGO;
	}

	[SerializeField]
	[Header("Media Player")]
	private MediaPlayer _MediaPlayer;

	[Header("Options Menu")]
	[SerializeField]
	private OptionsMenu _OptionsMenu;

	[Header("Content")]
	[SerializeField]
	private Transform _Content;

	[SerializeField]
	private RectTransform _ScrollViewRectTransform;

	[SerializeField]
	private RectTransform _ViewportRectTransform;

	[SerializeField]
	private GameObject _VideoTrackLinePrefab;

	private string m_SetupForVideoPath;

	private List<CVideoTrackSet> m_lVideoTrackSets = new List<CVideoTrackSet>();

	private void Start()
	{
		UpdateSets();
	}

	private void Update()
	{
		UpdateSets();
	}

	private void AddVideoTrackSet(string title, bool bEnabled)
	{
		GameObject gameObject = Object.Instantiate(_VideoTrackLinePrefab, _Content);
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
			Button component = gameObject.GetComponent<Button>();
			if ((bool)component)
			{
				int iIndex = m_lVideoTrackSets.Count;
				component.onClick.AddListener(delegate
				{
					_OptionsMenu.ChangeVideoTrack(iIndex);
				});
			}
		}
		CVideoTrackSet cVideoTrackSet = new CVideoTrackSet();
		cVideoTrackSet.m_LineGO = gameObject;
		m_lVideoTrackSets.Add(cVideoTrackSet);
	}

	public void UpdateSets()
	{
		if (!(_MediaPlayer != null) || !_MediaPlayer.Control.HasMetaData() || (m_SetupForVideoPath != null && m_SetupForVideoPath.Equals(_MediaPlayer.MediaPath.Path)))
		{
			return;
		}
		m_SetupForVideoPath = _MediaPlayer.MediaPath.Path;
		foreach (CVideoTrackSet lVideoTrackSet in m_lVideoTrackSets)
		{
			Object.Destroy(lVideoTrackSet.m_LineGO);
			lVideoTrackSet.m_LineGO = null;
		}
		m_lVideoTrackSets.Clear();
		foreach (VideoTrack videoTrack in _MediaPlayer.VideoTracks.GetVideoTracks())
		{
			AddVideoTrackSet(videoTrack.DisplayName, bEnabled: false);
		}
		if (m_lVideoTrackSets.Count > 1)
		{
			float num = 40f;
			float num2 = num * (float)m_lVideoTrackSets.Count;
			RectTransform rectTransform = ((_Content != null) ? _Content.GetComponent<RectTransform>() : null);
			if (rectTransform != null)
			{
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, num2);
			}
			float num3 = num2 * 0.5f - num * 0.5f;
			foreach (CVideoTrackSet lVideoTrackSet2 in m_lVideoTrackSets)
			{
				RectTransform component = lVideoTrackSet2.m_LineGO.GetComponent<RectTransform>();
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
		ChangeVideoTrack(_MediaPlayer.VideoTracks.GetVideoTracks().GetActiveTrackIndex(), bSetTrack: false);
	}

	public void ChangeVideoTrack(int iTrackIndex, bool bSetTrack = true)
	{
		VideoTracks videoTracks = (_MediaPlayer ? _MediaPlayer.VideoTracks.GetVideoTracks() : null);
		if (videoTracks == null)
		{
			return;
		}
		if (bSetTrack)
		{
			_MediaPlayer.VideoTracks.SetActiveVideoTrack((iTrackIndex > -1 && iTrackIndex < videoTracks.Count) ? videoTracks[iTrackIndex] : null);
		}
		int num = 0;
		foreach (CVideoTrackSet lVideoTrackSet in m_lVideoTrackSets)
		{
			Transform transform = lVideoTrackSet.m_LineGO.transform.Find("TickIcon");
			Image image = ((transform != null) ? transform.GetComponent<Image>() : null);
			if (image != null)
			{
				image.enabled = num == iTrackIndex;
			}
			num++;
		}
	}
}
