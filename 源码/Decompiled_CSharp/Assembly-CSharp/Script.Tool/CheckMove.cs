using UnityEngine;
using UnityEngine.Events;

namespace Script.Tool;

public class CheckMove : MonoBehaviour
{
	[Header("灵敏度设置")]
	[Tooltip("判定为滑动的最小像素距离")]
	public float moveThreshold = 0.5f;

	[Tooltip("静止多少秒后触发停止回调(主要针对鼠标)")]
	public float stopDelay = 0.1f;

	[Header("事件回调")]
	public UnityEvent OnScrollStart;

	public UnityEvent OnScrolling;

	public UnityEvent OnScrollStop;

	private Vector3 lastPointerPosition;

	private bool isMoving;

	private float stopTimer;

	private void Start()
	{
		lastPointerPosition = Input.mousePosition;
	}

	private void OnEnable()
	{
		isMoving = false;
	}

	private void Update()
	{
		bool flag = false;
		Vector3 mousePosition = Input.mousePosition;
		if (Vector3.Distance(mousePosition, lastPointerPosition) > moveThreshold)
		{
			flag = true;
		}
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
			{
				if (isMoving)
				{
					TriggerStop();
				}
				lastPointerPosition = mousePosition;
				return;
			}
		}
		if (flag)
		{
			stopTimer = 0f;
			if (!isMoving)
			{
				isMoving = true;
				OnScrollStart?.Invoke();
			}
			OnScrolling?.Invoke();
		}
		else if (isMoving)
		{
			stopTimer += Time.deltaTime;
			if (stopTimer >= stopDelay)
			{
				TriggerStop();
			}
		}
		lastPointerPosition = mousePosition;
	}

	private void TriggerStop()
	{
		isMoving = false;
		stopTimer = 0f;
		OnScrollStop?.Invoke();
	}
}
