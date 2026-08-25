using FrameWork;
using UnityEngine;

namespace Script.Mrg;

public class MouseAutoHideMrg : SingletonAsMono<MouseAutoHideMrg>
{
	[Header("鼠标闲置多久后隐藏（秒）")]
	public float hideDelay = 1f;

	private float _lastMouseMoveTime;

	private Vector3 _lastMousePosition;

	private void Start()
	{
		_lastMouseMoveTime = Time.realtimeSinceStartup;
		_lastMousePosition = Input.mousePosition;
		Cursor.visible = true;
	}

	private void Update()
	{
		if (Input.mousePosition != _lastMousePosition)
		{
			_lastMouseMoveTime = Time.realtimeSinceStartup;
			_lastMousePosition = Input.mousePosition;
			Cursor.visible = true;
		}
		else if (Time.realtimeSinceStartup - _lastMouseMoveTime > hideDelay)
		{
			Cursor.visible = false;
		}
	}
}
