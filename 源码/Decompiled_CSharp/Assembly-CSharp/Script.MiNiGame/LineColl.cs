using UnityEngine;

namespace Script.MiNiGame;

public class LineColl : MonoBehaviour
{
	private Item _item;

	private void Awake()
	{
		_item = GetComponent<Item>();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (MiNiGameMrg.Instance.hook.currentState == Hook.State.Shoot)
		{
			Item component = collision.GetComponent<Item>();
			if ((bool)component)
			{
				MiNiGameMrg.Instance.hook.StartRetract(component);
			}
		}
	}
}
