using UnityEngine;

public class BreakObject : MonoBehaviour
{
	public float health;

	public GameObject destroyed_ob;

	private void OnCollisionEnter(Collision collision)
	{
		if (health > 0f)
		{
			base.transform.SendMessageUpwards("GetBulletDamage", 40, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void GetBulletDamage(float damage)
	{
		health -= damage;
		if (health <= 0f)
		{
			Object.Instantiate(destroyed_ob, base.transform.position, base.transform.rotation);
			Object.Destroy(base.gameObject);
		}
	}
}
