using DG.Tweening;
using FrameWork;
using Script.Mrg;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Script.Scene;

public class MapScene : MonoBehaviour
{
	public static MapScene Instance;

	public RectTransform map;

	public PlayerMove playerMove;

	public float speed = 5f;

	private Tweener _tweener;

	private void Awake()
	{
		Instance = this;
		SingletonAsMono<GlobalMrg>.Instance.videoState = VideoState.Map;
	}

	private void Start()
	{
		if (SingletonAsMono<GameDataMrg>.Instance.IsMorning)
		{
			EventManager.DispatchEvent(MessageType.Video, VideoMessageType.CheckMsgRi);
		}
	}

	public void MoveToPoint(PointerEventData eventData)
	{
		if (!(playerMove == null))
		{
			_tweener?.Kill();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(map, eventData.position, eventData.pressEventCamera, out var localPoint);
			float duration = Vector2.Distance(localPoint, playerMove.transform.localPosition) / speed;
			_tweener = playerMove.transform.DOLocalMove(localPoint, duration);
		}
	}
}
