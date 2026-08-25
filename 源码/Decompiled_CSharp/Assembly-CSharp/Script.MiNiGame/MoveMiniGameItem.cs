using UnityEngine;

namespace Script.MiNiGame;

public class MoveMiniGameItem : MonoBehaviour
{
	private float moveTime = 2f;

	public float moveSpeed = 1f;

	private bool _isLeft = true;

	private float _curMoveTime;

	private void OnEnable()
	{
		moveTime = Random.Range(0.5f, 2f);
	}

	private void Update()
	{
		_curMoveTime += Time.deltaTime;
		if (_curMoveTime > moveTime || (MiNiGameMrg.Instance != null && MiNiGameMrg.Instance.CheckPos(base.transform.position)))
		{
			_curMoveTime = 0f;
			_isLeft = !_isLeft;
			moveTime = Random.Range(0.5f, 2f);
		}
		if (_isLeft)
		{
			base.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
		}
		else
		{
			base.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
		}
	}
}
