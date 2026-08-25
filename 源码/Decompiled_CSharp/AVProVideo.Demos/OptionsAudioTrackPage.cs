using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public class OptionsAudioTrackPage : MonoBehaviour
{
	private class CAudioTrackSet
	{
		public GameObject m_LineGO;
	}

	[Header("Media Player")]
	[SerializeField]
	private MediaPlayer _MediaPlayer;

	[SerializeField]
	[Header("Options Menu")]
	private OptionsMenu _OptionsMenu;

	[SerializeField]
	[Header("Content")]
	private Transform _Content;

	[SerializeField]
	private RectTransform _ScrollViewRectTransform;

	[SerializeField]
	private RectTransform _ViewportRectTransform;

	[SerializeField]
	private GameObject _AudioTrackLinePrefab;

	private string m_SetupForAudioPath;

	private List<CAudioTrackSet> m_lAudioTrackSets = new List<CAudioTrackSet>();

	private void Start()
	{
		UpdateSets();
	}

	private void Update()
	{
		UpdateSets();
	}

	private void AddAudioTrackSet(string title, bool bEnabled)
	{
		GameObject gameObject = Object.Instantiate(_AudioTrackLinePrefab, _Content);
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
				int iIndex = m_lAudioTrackSets.Count;
				component.onClick.AddListener(delegate
				{
					_OptionsMenu.ChangeAudioTrack(iIndex);
				});
			}
		}
		CAudioTrackSet cAudioTrackSet = new CAudioTrackSet();
		cAudioTrackSet.m_LineGO = gameObject;
		m_lAudioTrackSets.Add(cAudioTrackSet);
	}

	public void UpdateSets()
	{
		if (!(_MediaPlayer != null) || !_MediaPlayer.Control.HasMetaData() || (m_SetupForAudioPath != null && m_SetupForAudioPath.Equals(_MediaPlayer.MediaPath.Path)))
		{
			return;
		}
		m_SetupForAudioPath = _MediaPlayer.MediaPath.Path;
		foreach (CAudioTrackSet lAudioTrackSet in m_lAudioTrackSets)
		{
			Object.Destroy(lAudioTrackSet.m_LineGO);
			lAudioTrackSet.m_LineGO = null;
		}
		m_lAudioTrackSets.Clear();
		foreach (AudioTrack audioTrack in _MediaPlayer.AudioTracks.GetAudioTracks())
		{
			AddAudioTrackSet(audioTrack.DisplayName, bEnabled: false);
		}
		if (m_lAudioTrackSets.Count > 1)
		{
			float num = 40f;
			float num2 = num * (float)m_lAudioTrackSets.Count;
			RectTransform rectTransform = ((_Content != null) ? _Content.GetComponent<RectTransform>() : null);
			if (rectTransform != null)
			{
				rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, num2);
			}
			float num3 = num2 * 0.5f - num * 0.5f;
			foreach (CAudioTrackSet lAudioTrackSet2 in m_lAudioTrackSets)
			{
				RectTransform component = lAudioTrackSet2.m_LineGO.GetComponent<RectTransform>();
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
		ChangeAudioTrack(_MediaPlayer.AudioTracks.GetAudioTracks().GetActiveTrackIndex(), bSetTrack: false);
	}

	public void ChangeAudioTrack(int iTrackIndex, bool bSetTrack = true)
	{
		AudioTracks audioTracks = (_MediaPlayer ? _MediaPlayer.AudioTracks.GetAudioTracks() : null);
		if (audioTracks == null)
		{
			return;
		}
		if (bSetTrack)
		{
			_MediaPlayer.AudioTracks.SetActiveAudioTrack((iTrackIndex > -1 && iTrackIndex < audioTracks.Count) ? audioTracks[iTrackIndex] : null);
		}
		int num = 0;
		foreach (CAudioTrackSet lAudioTrackSet in m_lAudioTrackSets)
		{
			Transform transform = lAudioTrackSet.m_LineGO.transform.Find("TickIcon");
			Image image = ((transform != null) ? transform.GetComponent<Image>() : null);
			if (image != null)
			{
				image.enabled = num == iTrackIndex;
			}
			num++;
		}
	}
}
