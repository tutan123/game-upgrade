using DG.Tweening;
using UnityEngine;

namespace Script.Tool;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupTool : MonoBehaviour
{
	private CanvasGroup _canvasGroup;

	public float time = 0.3f;

	private Tweener _tweener;

	private void Awake()
	{
		_canvasGroup = GetComponent<CanvasGroup>();
	}

	public void Enter()
	{
		_tweener?.Kill();
		_tweener = _canvasGroup.DOFade(1f, time);
	}

	public void Exit()
	{
		_tweener?.Kill();
		_tweener = _canvasGroup.DOFade(0f, time);
	}

	private void OnEnable()
	{
		_canvasGroup.alpha = 0f;
	}
}
