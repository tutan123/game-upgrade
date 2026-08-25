using UnityEngine;

public class Damage : MonoBehaviour
{
	public float maxMoveDelta = 1f;

	public float maxCollisionStrength = 50f;

	public float YforceDamp = 0.1f;

	public float demolutionRange = 0.5f;

	public float impactDirManipulator;

	public MeshFilter[] optionalMeshList;

	private MeshFilter[] meshfilters;

	private float sqrDemRange;

	public void Start()
	{
		if (optionalMeshList.Length != 0)
		{
			meshfilters = optionalMeshList;
		}
		else
		{
			meshfilters = GetComponentsInChildren<MeshFilter>();
		}
		sqrDemRange = demolutionRange * demolutionRange;
	}

	public void OnCollisionEnter(Collision collision)
	{
		Vector3 relativeVelocity = collision.relativeVelocity;
		relativeVelocity.y *= YforceDamp;
		Vector3 vector = base.transform.position - collision.contacts[0].point;
		float num = relativeVelocity.magnitude * Vector3.Dot(collision.contacts[0].normal, vector.normalized);
		OnMeshForce(collision.contacts[0].point, Mathf.Clamp01(num / maxCollisionStrength));
	}

	public void OnMeshForce(Vector4 originPosAndForce)
	{
		OnMeshForce(originPosAndForce, originPosAndForce.w);
	}

	public void OnMeshForce(Vector3 originPos, float force)
	{
		force = Mathf.Clamp01(force);
		for (int i = 0; i < meshfilters.Length; i++)
		{
			Vector3[] vertices = meshfilters[i].mesh.vertices;
			for (int j = 0; j < vertices.Length; j++)
			{
				Vector3 vector = Vector3.Scale(vertices[j], base.transform.localScale);
				Vector3 vector2 = meshfilters[i].transform.position + meshfilters[i].transform.rotation * vector;
				Vector3 a = vector2 - originPos;
				Vector3 b = base.transform.position - vector2;
				b.y = 0f;
				if (a.sqrMagnitude < sqrDemRange)
				{
					float num = Mathf.Clamp01(a.sqrMagnitude / sqrDemRange);
					float num2 = force * (1f - num) * maxMoveDelta;
					Vector3 vector3 = Vector3.Slerp(a, b, impactDirManipulator).normalized * num2;
					vertices[j] += Quaternion.Inverse(base.transform.rotation) * vector3;
				}
			}
			meshfilters[i].mesh.vertices = vertices;
			meshfilters[i].mesh.RecalculateBounds();
		}
	}
}
