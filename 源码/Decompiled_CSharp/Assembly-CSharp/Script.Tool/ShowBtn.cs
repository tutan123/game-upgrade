using DG.Tweening;
using UnityEngine;

namespace Script.Tool;

public class ShowBtn : MonoBehaviour
{
	public CanvasGroup canvasGroup;

	private Tweener _tweener;

	public void MoveStart()
	{
		_tweener.Kill();
		_tweener = canvasGroup.DOFade(1f, 0.5f);
	}

	public void MoveStop()
	{
		_tweener.Kill();
		_tweener = canvasGroup.DOFade(0f, 0.5f);
	}
}
