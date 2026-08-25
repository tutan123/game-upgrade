using FrameWork;
using UnityEngine;

namespace Script.MiNiGame;

public class ShuaiMai : MonoBehaviour
{
	public Sprite[] sprites;

	private TimeData _timeData;

	private void OnEnable()
	{
		_timeData = Timer.IntervalCall(1f, SpawnItem);
	}

	private void OnDisable()
	{
		Timer.DestroyTimer(_timeData);
	}

	private void SpawnItem()
	{
		GameObject obj = Object.Instantiate(ABMrg.Load<GameObject>("SetItem"), MiNiGameMrg.Instance.itemParent);
		Vector2 vector = new Vector2(Random.Range(MiNiGameMrg.Instance.minPos.position.x, MiNiGameMrg.Instance.maxPos.position.x), 6f);
		obj.transform.position = vector;
		obj.GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];
	}
}
