using UnityEngine;

namespace Script.MiNiGame;

public class XiaoTouRandom : MonoBehaviour
{
	public Vector3[] points;

	public float speed = 5f;

	private int _index;

	private void OnEnable()
	{
		_index = Random.Range(0, points.Length);
	}

	private void Update()
	{
		if (Vector3.Distance(base.transform.position, points[_index]) < 0.1f)
		{
			_index = Random.Range(0, points.Length);
			return;
		}
		base.transform.Translate((points[_index] - base.transform.position).normalized * speed * Time.deltaTime);
		if (points[_index].x > base.transform.position.x)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		else
		{
			base.transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}
}
