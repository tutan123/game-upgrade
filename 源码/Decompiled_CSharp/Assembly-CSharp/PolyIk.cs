using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PolyIk : MonoBehaviour
{
	public List<HumanBoneOffset> rotationOffsets = new List<HumanBoneOffset>();

	public Transform leftFootTarget;

	public Transform rightFootTarget;

	public Transform LeftHandTarget;

	public Transform rightHandTarget;

	public Transform lookAtTarget;

	public Transform leftFootPole;

	public Transform rightFootPole;

	public Transform LeftHandPole;

	public Transform rightHandPole;

	[Range(0f, 1f)]
	public float rotationWeightLeftFoot;

	[Range(0f, 1f)]
	public float rotationWeightRightFoot;

	[Range(0f, 1f)]
	public float rotationWeightLeftHand;

	[Range(0f, 1f)]
	public float rotationWeightRightHand;

	[Range(0f, 1f)]
	public float lookAtWeight;

	[Range(0f, 1f)]
	public float lookAtHeadWeight;

	[Range(0f, 1f)]
	public float LookAtEyesWeight;

	[Range(0f, 1f)]
	public float rightHandWeight;

	[Range(0f, 1f)]
	public float leftHandWeight;

	[Range(0f, 1f)]
	public float rightFootWeight;

	[Range(0f, 1f)]
	public float leftFootWeight;

	public bool leftFootIk;

	public bool lookAtIk;

	public bool rightHandIk;

	public bool leftHandIk;

	public bool rightFootIk;

	public Animator animator;

	private void Start()
	{
		animator = GetComponent<Animator>();
		if (!animator.isHuman)
		{
			base.enabled = false;
			Debug.Log("The rig needs to be humanoid for this script to work");
		}
	}

	private void Update()
	{
		if (Application.isEditor)
		{
			animator.Update(0f);
		}
	}

	private void OnAnimatorIK(int layerIndex)
	{
		SetIkTargetsAndWeights();
		foreach (HumanBoneOffset rotationOffset in rotationOffsets)
		{
			if (rotationOffset.active)
			{
				OffsetSpine(rotationOffset.bone, rotationOffset.rotationOffset);
			}
		}
	}

	private void SetIkTargetsAndWeights()
	{
		if (leftFootTarget != null && leftFootIk)
		{
			animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, leftFootWeight);
			animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget.position);
			animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTarget.rotation);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, rotationWeightLeftFoot);
			animator.SetIKHintPosition(AvatarIKHint.LeftKnee, leftFootPole.position);
			animator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, leftFootWeight);
		}
		if (rightFootTarget != null && rightFootIk)
		{
			animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, rightFootWeight);
			animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTarget.position);
			animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTarget.rotation);
			animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, rotationWeightRightFoot);
			animator.SetIKHintPosition(AvatarIKHint.RightKnee, rightFootPole.position);
			animator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, rightFootWeight);
		}
		if (LeftHandTarget != null && leftHandIk)
		{
			animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
			animator.SetIKPosition(AvatarIKGoal.LeftHand, LeftHandTarget.position);
			animator.SetIKRotation(AvatarIKGoal.LeftHand, LeftHandTarget.rotation);
			animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, rotationWeightLeftHand);
			animator.SetIKHintPosition(AvatarIKHint.LeftElbow, LeftHandPole.position);
			animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, leftHandWeight);
		}
		if (rightHandTarget != null && rightHandIk)
		{
			animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
			animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandTarget.position);
			animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandTarget.rotation);
			animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rotationWeightRightHand);
			animator.SetIKHintPosition(AvatarIKHint.RightElbow, rightHandPole.position);
			animator.SetIKHintPositionWeight(AvatarIKHint.RightElbow, rightHandWeight);
		}
		if (lookAtTarget != null && lookAtIk)
		{
			animator.SetLookAtWeight(lookAtWeight, 0f, lookAtHeadWeight, LookAtEyesWeight, 0f);
			animator.SetLookAtPosition(lookAtTarget.position);
		}
	}

	public void OffsetSpine(HumanBodyBones bone, Vector3 target)
	{
		Quaternion localRotation = animator.GetBoneTransform(bone).localRotation;
		Quaternion quaternion = Quaternion.Euler(target);
		animator.SetBoneLocalRotation(bone, Quaternion.Inverse(localRotation) * quaternion);
	}
}
