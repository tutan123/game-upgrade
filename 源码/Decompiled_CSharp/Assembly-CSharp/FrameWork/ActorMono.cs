using UnityEngine;

namespace FrameWork;

public class ActorMono : MonoBehaviour
{
	private Actor _actor;

	public Actor GetActor()
	{
		return _actor;
	}

	public void SetActor(Actor actor)
	{
		_actor = actor;
	}

	public int GetIndex()
	{
		return _actor.GetIndex();
	}

	private void Awake()
	{
		_actor?.Awake();
	}

	private void Start()
	{
		_actor?.Start();
	}

	private void OnEnable()
	{
		_actor?.OnEnable();
	}

	private void OnDisable()
	{
		_actor?.OnDisable();
	}

	private void Update()
	{
		_actor?.Update(Time.deltaTime);
	}

	private void FixedUpdate()
	{
		_actor?.FixedUpdate(Time.fixedDeltaTime);
	}

	private void LateUpdate()
	{
		_actor?.LateUpdate();
	}

	private void OnDestroy()
	{
		_actor?.OnDestroy();
	}
}
