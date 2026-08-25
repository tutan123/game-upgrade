using UnityEngine;
using UnityEngine.AI;

namespace Script.Tool;

public class CheckPlayAnim : MonoBehaviour
{
	public NavMeshAgent navMeshAgent;

	public Animator animator;

	public MapPlayer mapPlayer;

	private void Update()
	{
		UpdateAnim();
	}

	private void UpdateAnim()
	{
		if (mapPlayer.mtcItem != null)
		{
			animator.SetBool("People", value: false);
			if (mapPlayer.mtcItem.ZjMode == 1)
			{
				animator.SetBool("Bike1", value: true);
			}
			else
			{
				animator.SetBool("Bike2", value: true);
			}
		}
		else
		{
			animator.SetBool("People", value: true);
			if (mapPlayer.isMove)
			{
				animator.SetBool("Walk", value: true);
			}
			else
			{
				animator.SetBool("Walk", value: false);
			}
		}
	}
}
