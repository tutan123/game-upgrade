using System.Collections.Generic;
using System.Text;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

public class OptionsQualityPage : MonoBehaviour
{
	private class CVariantSet
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
	private GameObject _QualityLinePrefab;

	private string m_SetupForVideoPath;

	private int m_iCachedNumVariants = -1;

	private List<CVariantSet> m_lVariantSets = new List<CVariantSet>();

	private void Start()
	{
		AddVariantSet("Auto", bEnabled: true);
	}

	private void Update()
	{
		UpdateSets();
	}

	private void AddVariantSet(string title, bool bEnabled)
	{
		GameObject gameObject = Object.Instantiate(_QualityLinePrefab, _Content);
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
				int iIndex = m_lVariantSets.Count;
				component.onClick.AddListener(delegate
				{
					_OptionsMenu.ChangeVideoVariant(iIndex);
				});
			}
		}
		CVariantSet cVariantSet = new CVariantSet();
		cVariantSet.m_LineGO = gameObject;
		m_lVariantSets.Add(cVariantSet);
	}

	public void UpdateSets()
	{
		if (!(_MediaPlayer != null) || !_MediaPlayer.Control.HasMetaData() || (m_SetupForVideoPath != null && m_SetupForVideoPath.Equals(_MediaPlayer.MediaPath.Path) && m_iCachedNumVariants == _MediaPlayer.Variants.Count))
		{
			return;
		}
		m_SetupForVideoPath = _MediaPlayer.MediaPath.Path;
		m_iCachedNumVariants = _MediaPlayer.Variants.Count;
		foreach (CVariantSet lVariantSet in m_lVariantSets)
		{
			Object.Destroy(lVariantSet.m_LineGO);
			lVariantSet.m_LineGO = null;
		}
		m_lVariantSets.Clear();
		int count = _MediaPlayer.Variants.Count;
		for (int i = 0; i < count; i++)
		{
			Variant variant = _MediaPlayer.Variants[i];
			StringBuilder stringBuilder = new StringBuilder();
			if (variant.Width > 0 && variant.Height > 0)
			{
				stringBuilder.AppendFormat("{0}x{1}", variant.Width, variant.Height);
			}
			else
			{
				stringBuilder.AppendFormat("{0}bps", variant.PeakDataRate);
			}
			if (variant.FrameRate > 0f)
			{
				stringBuilder.AppendFormat("@{0:G}fps", variant.FrameRate);
			}
			if (variant.VideoCodecType != 0)
			{
				stringBuilder.AppendFormat(" {0}", variant.VideoCodecName);
			}
			if (variant.AudioCodecType != 0)
			{
				stringBuilder.AppendFormat(" {0}", variant.AudioCodecName);
			}
			if (variant.IsUnsupported)
			{
				stringBuilder.AppendFormat(" (U)");
			}
			AddVariantSet(stringBuilder.ToString(), bEnabled: false);
		}
		if (count > 0)
		{
			AddVariantSet("Auto", bEnabled: true);
		}
		if (m_lVariantSets.Count <= 1)
		{
			return;
		}
		float num = 40f;
		float num2 = num * (float)m_lVariantSets.Count;
		RectTransform rectTransform = ((_Content != null) ? _Content.GetComponent<RectTransform>() : null);
		if (rectTransform != null)
		{
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, num2);
		}
		float num3 = num2 * 0.5f - num * 0.5f;
		foreach (CVariantSet lVariantSet2 in m_lVariantSets)
		{
			RectTransform component = lVariantSet2.m_LineGO.GetComponent<RectTransform>();
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

	public void ChangeVideoVariant(int iVariantIndex)
	{
		if (_MediaPlayer == null || _MediaPlayer.Variants == null)
		{
			return;
		}
		Variant variant = ((iVariantIndex < 0 || iVariantIndex >= _MediaPlayer.Variants.Count) ? Variant.Auto : _MediaPlayer.Variants[iVariantIndex]);
		_MediaPlayer.Variants.SelectVariant(variant);
		int num = 0;
		foreach (CVariantSet lVariantSet in m_lVariantSets)
		{
			Transform transform = lVariantSet.m_LineGO.transform.Find("TickIcon");
			Image image = ((transform != null) ? transform.GetComponent<Image>() : null);
			if (image != null)
			{
				image.enabled = num == iVariantIndex;
			}
			num++;
		}
	}
}
