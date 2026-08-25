using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UiTool;

public class ScoreItem : MonoBehaviour
{
	public TMP_Text title;

	public TMP_Text score;

	public Image scoreImg;

	public Sprite[] scoreImgs;

	public PropertyTypeValue propertyTypeValue;

	public void Init()
	{
		title.text = propertyTypeValue.GetName();
		score.text = SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyTypeValue.ToString(), "Property", 0L).ToString() ?? "";
		scoreImg.sprite = scoreImgs[0];
	}
}
