using DG.Tweening;
using UnityEngine;

namespace Script.UiTool;

public class SlideTest : MonoBehaviour
{
	public RectTransform arr;

	public float maxX = 700f;

	private void Start()
	{
		arr.DOLocalMoveX(maxX, 0.4f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
	}
}
