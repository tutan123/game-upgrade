using UnityEngine;
using UnityEngine.AI;

public class UltimateNavDebugger : MonoBehaviour
{
	public Transform target;

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere(base.transform.position, 0.5f);
		if (target != null)
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawSphere(target.position, 0.5f);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(base.transform.position, target.position);
			if (NavMesh.SamplePosition(target.position, out var hit, 100f, -1))
			{
				Gizmos.color = Color.green;
				Gizmos.DrawSphere(hit.position, 0.7f);
				Gizmos.DrawLine(target.position, hit.position);
			}
			else
			{
				Debug.DrawRay(target.position, Vector3.up * 10f, Color.red);
			}
		}
	}
}
