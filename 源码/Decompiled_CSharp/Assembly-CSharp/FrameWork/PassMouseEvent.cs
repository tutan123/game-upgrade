using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrameWork;

[RequireComponent(typeof(Graphic))]
public class PassMouseEvent : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler, IPointerExitHandler
{
	private GameObject _lastHoveredTarget;

	private static readonly List<RaycastResult> _results = new List<RaycastResult>();

	private static bool _isProcessing;

	private Graphic _graphic;

	private Graphic Graphic => _graphic ?? (_graphic = GetComponent<Graphic>());

	public void OnPointerClick(PointerEventData eventData)
	{
		PassEvent(eventData, ExecuteEvents.pointerClickHandler);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		PassEvent(eventData, ExecuteEvents.pointerDownHandler);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		PassEvent(eventData, ExecuteEvents.pointerUpHandler);
	}

	public void OnPointerMove(PointerEventData eventData)
	{
		if (_isProcessing)
		{
			return;
		}
		GameObject gameObject = FindUnderlyingObject(eventData);
		if (gameObject != _lastHoveredTarget)
		{
			_isProcessing = true;
			if (_lastHoveredTarget != null)
			{
				ExecuteEvents.Execute(_lastHoveredTarget, eventData, ExecuteEvents.pointerExitHandler);
			}
			if (gameObject != null)
			{
				ExecuteEvents.Execute(gameObject, eventData, ExecuteEvents.pointerEnterHandler);
			}
			_lastHoveredTarget = gameObject;
			_isProcessing = false;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (_lastHoveredTarget != null)
		{
			_isProcessing = true;
			ExecuteEvents.Execute(_lastHoveredTarget, eventData, ExecuteEvents.pointerExitHandler);
			_lastHoveredTarget = null;
			_isProcessing = false;
		}
	}

	private GameObject FindUnderlyingObject(PointerEventData eventData)
	{
		RaycastThrough(eventData);
		for (int i = 0; i < _results.Count; i++)
		{
			GameObject eventHandler = ExecuteEvents.GetEventHandler<IPointerEnterHandler>(_results[i].gameObject);
			if (eventHandler != null && eventHandler != base.gameObject)
			{
				return eventHandler;
			}
		}
		return null;
	}

	private void PassEvent<T>(PointerEventData data, ExecuteEvents.EventFunction<T> function) where T : IEventSystemHandler
	{
		if (_isProcessing)
		{
			return;
		}
		RaycastThrough(data);
		for (int i = 0; i < _results.Count; i++)
		{
			GameObject gameObject = _results[i].gameObject;
			if (gameObject != base.gameObject)
			{
				GameObject eventHandler = ExecuteEvents.GetEventHandler<T>(gameObject);
				if (eventHandler != null)
				{
					_isProcessing = true;
					ExecuteEvents.Execute(eventHandler, data, function);
					_isProcessing = false;
					break;
				}
			}
		}
	}

	private void RaycastThrough(PointerEventData data)
	{
		_results.Clear();
		bool raycastTarget = Graphic.raycastTarget;
		Graphic.raycastTarget = false;
		EventSystem.current.RaycastAll(data, _results);
		Graphic.raycastTarget = raycastTarget;
	}
}
