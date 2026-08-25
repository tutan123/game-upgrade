using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Script.Tool;

public class ClickTouchEvent : MonoBehaviour
{
	public LayerMask excludeLayer;

	public UnityEvent<Vector3> onClick;

	private void Update()
	{
		if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !EventSystem.current.IsPointerOverGameObject())
		{
			ShootRay(Input.mousePosition);
		}
		if (Input.touchCount > 0)
		{
			Touch touch = Input.GetTouch(0);
			if (touch.phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
			{
				ShootRay(touch.position);
			}
		}
	}

	private void ShootRay(Vector2 screenPosition)
	{
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		if (Physics.Raycast(ray, out var hitInfo, 2000f, excludeLayer))
		{
			Vector3 point = hitInfo.point;
			_ = hitInfo.collider.gameObject;
			Debug.DrawLine(ray.origin, point, Color.red, 2f);
			onClick?.Invoke(point);
		}
	}
}
