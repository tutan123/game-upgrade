using System.Collections.Generic;
using RenderHeads.Media.AVProVideo;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UiTool;

public class PlayIconSet : MonoBehaviour
{
	public List<Image> img;

	public Sprite stopIcon;

	public Sprite playIcon;

	public MediaPlayer mediaPlayer;

	private void Update()
	{
		if (!mediaPlayer)
		{
			return;
		}
		for (int i = 0; i < img.Count; i++)
		{
			if (!mediaPlayer.Control.IsPaused())
			{
				img[i].sprite = playIcon;
			}
			else
			{
				img[i].sprite = stopIcon;
			}
		}
	}
}
