using FrameWork;
using Script.Scene;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IEndDragHandler
{
	[Header("灵敏度")]
	public float sensitivity = 5f;

	public float keySpeed = 5f;

	[Header("上下视角限制（度）")]
	public float maxY = 85f;

	public float minY = -85f;

	public Transform yawParent;

	public Transform pitchChild;

	private float _yawAngle;

	private float _pitchAngle;

	private void Start()
	{
		_yawAngle = yawParent.eulerAngles.y;
		_pitchAngle = NormalizeAngle(pitchChild.eulerAngles.x);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if ((bool)Video360Scene.Instance)
		{
			Video360Scene.Instance.isLockClick = true;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		Vector2 delta = eventData.delta;
		float num = delta.x * sensitivity * 0.1f;
		float num2 = (0f - delta.y) * sensitivity * 0.1f;
		_yawAngle += 0f - num;
		_pitchAngle += 0f - num2;
		_pitchAngle = Mathf.Clamp(_pitchAngle, minY, maxY);
		yawParent.rotation = Quaternion.Euler(0f, _yawAngle, 0f);
		pitchChild.localRotation = Quaternion.Euler(_pitchAngle, 0f, 0f);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if ((bool)Video360Scene.Instance)
		{
			Video360Scene.Instance.isLockClick = false;
		}
	}

	private float NormalizeAngle(float angle)
	{
		while (angle > 180f)
		{
			angle -= 360f;
		}
		while (angle < -180f)
		{
			angle += 360f;
		}
		return angle;
	}

	private void Update()
	{
		if (UiManager.IsCanPlay())
		{
			float axis = Input.GetAxis("Horizontal");
			float axis2 = Input.GetAxis("Vertical");
			if (Mathf.Abs(axis) > 0.01f || Mathf.Abs(axis2) > 0.01f)
			{
				_yawAngle += axis * keySpeed * Time.deltaTime * 10f;
				_pitchAngle += (0f - axis2) * keySpeed * Time.deltaTime * 10f;
				_pitchAngle = Mathf.Clamp(_pitchAngle, minY, maxY);
				yawParent.rotation = Quaternion.Euler(0f, _yawAngle, 0f);
				pitchChild.localRotation = Quaternion.Euler(_pitchAngle, 0f, 0f);
			}
		}
	}
}
