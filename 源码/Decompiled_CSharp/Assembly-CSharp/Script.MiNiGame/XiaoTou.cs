using UnityEngine;

namespace Script.MiNiGame;

public class XiaoTou : MonoBehaviour
{
	public float moveTime = 2f;

	public float moveSpeed = 1f;

	private bool _isLeft = true;

	private float _curMoveTime;

	private void Update()
	{
		_curMoveTime += Time.deltaTime;
		if (_curMoveTime > moveTime)
		{
			_curMoveTime = 0f;
			_isLeft = !_isLeft;
		}
		if (_isLeft)
		{
			base.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			base.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}
}
