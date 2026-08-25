using UnityEngine;

[ExecuteInEditMode]
public class WizardParticles : MonoBehaviour
{
	private Animator animator;

	public Transform leftHand;

	public Transform rightHand;

	public Vector3 direction;

	public Vector3 midPoint;

	public Quaternion rotation;

	public float scale;

	public ParticleSystem particleSystem;

	public Vector3 sizeMultiplier = Vector3.right;

	private void Start()
	{
		animator = GetComponentInParent<Animator>();
		leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
		rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
		particleSystem = GetComponent<ParticleSystem>();
	}

	private void Update()
	{
		direction = leftHand.position - rightHand.position;
		midPoint = (rightHand.position + leftHand.position) / 2f;
		scale = direction.magnitude / 2f;
		midPoint -= base.transform.position;
	}

	private void OnDrawGizmos()
	{
	}
}
