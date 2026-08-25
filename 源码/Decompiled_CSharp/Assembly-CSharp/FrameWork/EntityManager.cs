using System;
using System.Collections.Generic;

namespace FrameWork;

public class EntityManager : SingletonAsClass<EntityManager>
{
	private Dictionary<string, ObjectPool<Actor>> _objectPools = new Dictionary<string, ObjectPool<Actor>>();

	public Actor CreateEntity<T>() where T : Actor, new()
	{
		Type typeFromHandle = typeof(T);
		string text = typeFromHandle.Namespace + "." + typeFromHandle.Name;
		if (_objectPools.TryGetValue(text, out var value) && value.GetSize() > 0)
		{
			Actor actor = value.DeQueue();
			actor.GetGameObject().SetActive(value: true);
			return actor;
		}
		return (Actor)typeFromHandle.Assembly.CreateInstance(text);
	}

	public Actor CreateEntity(Type type)
	{
		string text = type.Namespace + "." + type.Name;
		if (_objectPools.TryGetValue(text, out var value) && value.GetSize() > 0)
		{
			Actor actor = value.DeQueue();
			actor.GetGameObject().SetActive(value: true);
			return actor;
		}
		return (Actor)type.Assembly.CreateInstance(text);
	}

	public void EnQueue(Actor actor)
	{
		string name = actor.GetType().Name;
		if (_objectPools.TryGetValue(name, out var value))
		{
			value.EnQueue(actor);
			actor.GetGameObject().SetActive(value: false);
		}
		else
		{
			ObjectPool<Actor> objectPool = new ObjectPool<Actor>();
			objectPool.EnQueue(actor);
			_objectPools.Add(name, objectPool);
		}
	}
}
