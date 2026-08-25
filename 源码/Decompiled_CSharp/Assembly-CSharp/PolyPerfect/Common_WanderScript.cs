using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

namespace PolyPerfect;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class Common_WanderScript : MonoBehaviour
{
	private const float contingencyDistance = 1f;

	[SerializeField]
	public IdleState[] idleStates;

	[SerializeField]
	private MovementState[] movementStates;

	[SerializeField]
	private AIState[] attackingStates;

	[SerializeField]
	private AIState[] deathStates;

	[SerializeField]
	public string species = "NA";

	[SerializeField]
	[Tooltip("This specific animal stats asset, create a new one from the asset menu under (LowPolyAnimals/NewAnimalStats)")]
	public AIStats stats;

	[SerializeField]
	[Tooltip("How far away from it's origin this animal will wander by itself.")]
	private float wanderZone = 10f;

	private int dominance = 1;

	private int originalDominance;

	[SerializeField]
	[Tooltip("How far this animal can sense a predator.")]
	private float awareness = 30f;

	[SerializeField]
	[Tooltip("How far this animal can sense it's prey.")]
	private float scent = 30f;

	private float originalScent;

	private float stamina = 10f;

	private float power = 10f;

	private float toughness = 5f;

	private float agression;

	private float originalAgression;

	private float attackSpeed = 0.5f;

	private bool territorial;

	private bool stealthy;

	[Tooltip("If true, this animal will never leave it's zone, even if it's chasing or running away from another animal.")]
	[SerializeField]
	private bool constainedToWanderZone;

	[SerializeField]
	[Tooltip("This animal will be peaceful towards species in this list.")]
	private string[] nonAgressiveTowards;

	private static List<Common_WanderScript> allAnimals = new List<Common_WanderScript>();

	[SerializeField]
	[Tooltip("If true, this animal will rotate to match the terrain. Ensure you have set the layer of the terrain as 'Terrain'.")]
	private bool matchSurfaceRotation;

	[SerializeField]
	[Tooltip("How fast the animnal rotates to match the surface rotation.")]
	private float surfaceRotationSpeed = 2f;

	[SerializeField]
	[Tooltip("If true, AI changes to this animal will be logged in the console.")]
	private bool logChanges;

	[Tooltip("If true, gizmos will be drawn in the editor.")]
	[SerializeField]
	private bool showGizmos;

	[SerializeField]
	private bool drawWanderRange = true;

	[SerializeField]
	private bool drawScentRange = true;

	[SerializeField]
	private bool drawAwarenessRange = true;

	public UnityEvent deathEvent;

	public UnityEvent attackingEvent;

	public UnityEvent idleEvent;

	public UnityEvent movementEvent;

	private Color distanceColor = new Color(0f, 0f, 205f);

	private Color awarnessColor = new Color(1f, 0f, 1f, 1f);

	private Color scentColor = new Color(1f, 0f, 0f, 1f);

	private Animator animator;

	private CharacterController characterController;

	private NavMeshAgent navMeshAgent;

	private Vector3 origin;

	private int totalIdleStateWeight;

	private int currentState;

	private bool dead;

	private bool moving;

	private bool useNavMesh;

	private Vector3 targetLocation = Vector3.zero;

	private float currentTurnSpeed;

	private bool attacking;

	public float MaxDistance
	{
		get
		{
			return wanderZone;
		}
		set
		{
			wanderZone = value;
		}
	}

	public static List<Common_WanderScript> AllAnimals => allAnimals;

	public void OnDrawGizmosSelected()
	{
		if (!showGizmos)
		{
			return;
		}
		if (drawWanderRange)
		{
			Gizmos.color = distanceColor;
			Gizmos.DrawWireSphere((origin == Vector3.zero) ? base.transform.position : origin, wanderZone);
			Gizmos.DrawIcon(new Vector3(base.transform.position.x, base.transform.position.y + wanderZone, base.transform.position.z), "ico-wander", allowScaling: true);
		}
		if (drawAwarenessRange)
		{
			Gizmos.color = awarnessColor;
			Gizmos.DrawWireSphere(base.transform.position, awareness);
			Gizmos.DrawIcon(new Vector3(base.transform.position.x, base.transform.position.y + awareness, base.transform.position.z), "ico-awareness", allowScaling: true);
		}
		if (drawScentRange)
		{
			Gizmos.color = scentColor;
			Gizmos.DrawWireSphere(base.transform.position, scent);
			Gizmos.DrawIcon(new Vector3(base.transform.position.x, base.transform.position.y + scent, base.transform.position.z), "ico-scent", allowScaling: true);
		}
		if (!Application.isPlaying)
		{
			return;
		}
		if (useNavMesh)
		{
			if (navMeshAgent.remainingDistance > 1f)
			{
				Gizmos.DrawSphere(navMeshAgent.destination + new Vector3(0f, 0.1f, 0f), 0.2f);
				Gizmos.DrawLine(base.transform.position, navMeshAgent.destination);
			}
		}
		else if (targetLocation != Vector3.zero)
		{
			Gizmos.DrawSphere(targetLocation + new Vector3(0f, 0.1f, 0f), 0.2f);
			Gizmos.DrawLine(base.transform.position, targetLocation);
		}
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
		RuntimeAnimatorController runtimeAnimatorController = animator.runtimeAnimatorController;
		if (logChanges)
		{
			if (runtimeAnimatorController == null)
			{
				Debug.LogError($"{base.gameObject.name} has no animator controller, make sure you put one in to allow the character to walk. See documentation for more details (1)");
				base.enabled = false;
				return;
			}
			if (animator.avatar == null)
			{
				Debug.LogError($"{base.gameObject.name} has no avatar, make sure you put one in to allow the character to animate. See documentation for more details (2)");
				base.enabled = false;
				return;
			}
			if (animator.hasRootMotion)
			{
				Debug.LogError($"{base.gameObject.name} has root motion applied, consider turning this off as our script will deactivate this on play as we do not use it (3)");
				animator.applyRootMotion = false;
			}
			if (idleStates.Length == 0 || movementStates.Length == 0)
			{
				Debug.LogError($"{base.gameObject.name} has no idle or movement states, make sure you fill these out. See documentation for more details (4)");
				base.enabled = false;
				return;
			}
			if (idleStates.Length != 0)
			{
				for (int i = 0; i < idleStates.Length; i++)
				{
					if (idleStates[i].animationBool == "")
					{
						Debug.LogError(string.Format("{0} has " + idleStates.Length + " Idle states, you need to make sure that each state has an animation boolean. See documentation for more details (4)", base.gameObject.name));
						base.enabled = false;
						return;
					}
				}
			}
			if (movementStates.Length != 0)
			{
				for (int j = 0; j < movementStates.Length; j++)
				{
					if (movementStates[j].animationBool == "")
					{
						Debug.LogError(string.Format("{0} has " + movementStates.Length + " Movement states, you need to make sure that each state has an animation boolean to see the character walk. See documentation for more details (4)", base.gameObject.name));
						base.enabled = false;
						return;
					}
					if (movementStates[j].moveSpeed <= 0f)
					{
						Debug.LogError($"{base.gameObject.name} has a movement state with a speed of 0 or less, you need to set the speed higher than 0 to see the character move. See documentation for more details (4)");
						base.enabled = false;
						return;
					}
					if (movementStates[j].turnSpeed <= 0f)
					{
						Debug.LogError($"{base.gameObject.name} has a turn speed state with a speed of 0 or less, you need to set the speed higher than 0 to see the character turn. See documentation for more details (4)");
						base.enabled = false;
						return;
					}
				}
			}
			if (attackingStates.Length == 0)
			{
				Debug.Log(string.Format("{0} has " + attackingStates.Length + " this character will not be able to attack. See documentation for more details (4)", base.gameObject.name));
			}
			if (attackingStates.Length != 0)
			{
				for (int k = 0; k < attackingStates.Length; k++)
				{
					if (attackingStates[k].animationBool == "")
					{
						Debug.LogError(string.Format("{0} has " + attackingStates.Length + " attacking states, you need to make sure that each state has an animation boolean. See documentation for more details (4)", base.gameObject.name));
						base.enabled = false;
						return;
					}
				}
			}
			if (stats == null)
			{
				Debug.LogError($"{base.gameObject.name} has no AI stats, make sure you assign one to the wander script. See documentation for more details (5)");
				base.enabled = false;
				return;
			}
		}
		IdleState[] array = idleStates;
		foreach (IdleState idleState in array)
		{
			totalIdleStateWeight += idleState.stateWeight;
		}
		origin = base.transform.position;
		animator.applyRootMotion = false;
		characterController = GetComponent<CharacterController>();
		navMeshAgent = GetComponent<NavMeshAgent>();
		originalDominance = stats.dominance;
		dominance = originalDominance;
		toughness = stats.toughness;
		territorial = stats.territorial;
		stamina = stats.stamina;
		originalAgression = stats.agression;
		agression = originalAgression;
		attackSpeed = stats.attackSpeed;
		stealthy = stats.stealthy;
		originalScent = scent;
		scent = originalScent;
		if ((bool)navMeshAgent)
		{
			useNavMesh = true;
			navMeshAgent.stoppingDistance = 1f;
		}
		if (matchSurfaceRotation && base.transform.childCount > 0)
		{
			base.transform.GetChild(0).gameObject.AddComponent<Common_SurfaceRotation>().SetRotationSpeed(surfaceRotationSpeed);
		}
		allAnimals.Add(this);
	}

	private void Start()
	{
		if (Common_WanderManager.Instance != null && Common_WanderManager.Instance.PeaceTime)
		{
			SetPeaceTime(peace: true);
		}
		StartCoroutine(InitYield());
	}

	private void OnDestroy()
	{
		allAnimals.Remove(this);
	}

	private IEnumerator InitYield()
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(0, 200) / 100);
		DecideNextState(wasIdle: false, firstState: true);
	}

	private void DecideNextState(bool wasIdle, bool firstState = false)
	{
		attacking = false;
		if (awareness > 0f)
		{
			for (int i = 0; i < allAnimals.Count; i++)
			{
				if (!allAnimals[i].dead && !(allAnimals[i] == this) && !(allAnimals[i].species == species) && allAnimals[i].dominance > dominance && !allAnimals[i].stealthy && allAnimals[i].gameObject.activeSelf && !(Vector3.Distance(base.transform.position, allAnimals[i].transform.position) > awareness))
				{
					if (useNavMesh)
					{
						RunAwayFromAnimal(allAnimals[i]);
					}
					else
					{
						NonNavMeshRunAwayFromAnimal(allAnimals[i]);
					}
					if (logChanges)
					{
						Debug.Log($"{base.gameObject.name}: Found predator ({allAnimals[i].gameObject.name}), running away.");
					}
					return;
				}
			}
		}
		if (dominance > 0)
		{
			for (int j = 0; j < allAnimals.Count; j++)
			{
				if (!allAnimals[j].dead && !(allAnimals[j] == this) && (!(allAnimals[j].species == species) || territorial) && allAnimals[j].dominance <= dominance && !allAnimals[j].stealthy && Array.IndexOf(nonAgressiveTowards, allAnimals[j].species) <= -1 && !(Vector3.Distance(base.transform.position, allAnimals[j].transform.position) > scent) && !((float)UnityEngine.Random.Range(0, 99) > agression))
				{
					if (logChanges)
					{
						Debug.Log($"{base.gameObject.name}: Found prey ({allAnimals[j].gameObject.name}), chasing.");
					}
					if (!(allAnimals[j] == null))
					{
						ChaseAnimal(allAnimals[j]);
						return;
					}
				}
			}
		}
		if (wasIdle && movementStates.Length != 0)
		{
			if (logChanges)
			{
				Debug.Log($"{base.gameObject.name}: Wandering.");
			}
			BeginWanderState();
		}
		else if (idleStates.Length != 0)
		{
			if (logChanges)
			{
				Debug.Log($"{base.gameObject.name}: Idling.");
			}
			BeginIdleState(firstState);
		}
		else if (idleStates.Length == 0)
		{
			BeginWanderState();
		}
		else if (movementStates.Length == 0)
		{
			BeginIdleState();
		}
	}

	private void BeginIdleState(bool firstState = false)
	{
		if (!firstState)
		{
			int num = UnityEngine.Random.Range(0, totalIdleStateWeight);
			for (int i = 0; i < idleStates.Length; i++)
			{
				if (num < idleStates[i].stateWeight)
				{
					currentState = i;
					break;
				}
				num -= idleStates[i].stateWeight;
			}
		}
		if (idleStates.Length == 0)
		{
			BeginWanderState();
			return;
		}
		if (!string.IsNullOrEmpty(idleStates[currentState].animationBool))
		{
			animator.SetBool(idleStates[currentState].animationBool, value: true);
		}
		float stateTime = (firstState ? (UnityEngine.Random.Range(50f, idleStates[currentState].minStateTime * 100f) / 100f) : (UnityEngine.Random.Range(idleStates[currentState].minStateTime * 100f, idleStates[currentState].maxStateTime * 100f) / 100f));
		StartCoroutine(IdleState(stateTime));
	}

	private IEnumerator IdleState(float stateTime)
	{
		moving = false;
		yield return new WaitForSeconds(stateTime);
		if ((idleStates[currentState] != null || idleStates.Length < currentState) && !string.IsNullOrEmpty(idleStates[currentState].animationBool))
		{
			animator.SetBool(idleStates[currentState].animationBool, value: false);
		}
		idleEvent.Invoke();
		DecideNextState(wasIdle: true);
	}

	private void BeginWanderState()
	{
		Vector3 target = RandonPointInRange();
		int num = 0;
		for (int i = 0; i < movementStates.Length; i++)
		{
			if (movementStates[i].moveSpeed < movementStates[num].moveSpeed)
			{
				num = i;
			}
		}
		currentState = num;
		if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
		{
			animator.SetBool(movementStates[currentState].animationBool, value: true);
		}
		movementEvent.Invoke();
		if (useNavMesh)
		{
			StartCoroutine(MovementState(target));
		}
		else
		{
			StartCoroutine(NonNavMeshMovementState(target));
		}
	}

	private IEnumerator MovementState(Vector3 target)
	{
		moving = true;
		navMeshAgent.speed = movementStates[currentState].moveSpeed;
		navMeshAgent.angularSpeed = movementStates[currentState].turnSpeed;
		navMeshAgent.SetDestination(target);
		float timeMoving = 0f;
		while ((navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance || timeMoving < 0.1f) && timeMoving < movementStates[currentState].maxStateTime)
		{
			timeMoving += Time.deltaTime;
			yield return null;
		}
		navMeshAgent.SetDestination(base.transform.position);
		if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
		{
			animator.SetBool(movementStates[currentState].animationBool, value: false);
		}
		DecideNextState(wasIdle: false);
	}

	private IEnumerator NonNavMeshMovementState(Vector3 target)
	{
		moving = true;
		targetLocation = target;
		currentTurnSpeed = movementStates[currentState].turnSpeed;
		float walkTime = 0f;
		float timeUntilAbortWalk = Vector3.Distance(base.transform.position, target) / movementStates[currentState].moveSpeed;
		while (Vector3.Distance(base.transform.position, target) > 1f && walkTime < timeUntilAbortWalk)
		{
			characterController.SimpleMove(base.transform.TransformDirection(Vector3.forward) * movementStates[currentState].moveSpeed);
			Quaternion b = Quaternion.LookRotation(target - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * (currentTurnSpeed / 10f));
			currentTurnSpeed += Time.deltaTime;
			walkTime += Time.deltaTime;
			yield return null;
		}
		targetLocation = Vector3.zero;
		if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
		{
			animator.SetBool(movementStates[currentState].animationBool, value: false);
		}
		DecideNextState(wasIdle: false);
	}

	private void RunAwayFromAnimal(Common_WanderScript predator)
	{
		moving = true;
		Quaternion rotation = base.transform.rotation;
		base.transform.rotation = Quaternion.LookRotation(base.transform.position - predator.transform.position);
		NavMesh.SamplePosition(base.transform.position + base.transform.forward * 5f, out var hit, 5f, 1 << NavMesh.GetAreaFromName("Walkable"));
		Vector3 vector = hit.position;
		base.transform.rotation = rotation;
		if (constainedToWanderZone && Vector3.Distance(vector, origin) > wanderZone)
		{
			vector = RandonPointInRange();
		}
		int num = 0;
		for (int i = 0; i < movementStates.Length; i++)
		{
			if (movementStates[i].moveSpeed > movementStates[num].moveSpeed)
			{
				num = i;
			}
		}
		currentState = num;
		if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
		{
			animator.SetBool(movementStates[currentState].animationBool, value: true);
		}
		StartCoroutine(RunAwayState(vector, predator));
	}

	private IEnumerator RunAwayState(Vector3 target, Common_WanderScript predator)
	{
		navMeshAgent.speed = movementStates[currentState].moveSpeed;
		navMeshAgent.angularSpeed = movementStates[currentState].turnSpeed;
		navMeshAgent.SetDestination(target);
		float timeMoving = 0f;
		while ((navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance || timeMoving < 0.1f) && timeMoving < stamina)
		{
			timeMoving += Time.deltaTime;
			yield return null;
		}
		navMeshAgent.SetDestination(base.transform.position);
		if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
		{
			animator.SetBool(movementStates[currentState].animationBool, value: false);
		}
		if (timeMoving > stamina || predator.dead || Vector3.Distance(base.transform.position, predator.transform.position) > awareness)
		{
			BeginIdleState();
		}
		else
		{
			RunAwayFromAnimal(predator);
		}
	}

	private void NonNavMeshRunAwayFromAnimal(Common_WanderScript predator)
	{
		moving = true;
		Quaternion rotation = base.transform.rotation;
		base.transform.rotation = Quaternion.LookRotation(base.transform.position - predator.transform.position);
		targetLocation = base.transform.position + base.transform.forward * 5f;
		base.transform.rotation = rotation;
		if (constainedToWanderZone && Vector3.Distance(targetLocation, origin) > wanderZone)
		{
			targetLocation = RandonPointInRange();
		}
		int num = 0;
		for (int i = 0; i < movementStates.Length; i++)
		{
			if (movementStates[i].moveSpeed > movementStates[num].moveSpeed)
			{
				num = i;
			}
		}
		currentState = num;
		if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
		{
			animator.SetBool(movementStates[currentState].animationBool, value: true);
		}
		StartCoroutine(NonNavMeshRunAwayState(targetLocation, predator));
	}

	private IEnumerator NonNavMeshRunAwayState(Vector3 target, Common_WanderScript predator)
	{
		currentTurnSpeed = movementStates[currentState].turnSpeed;
		float walkTime = 0f;
		float timeUntilAbortWalk = Vector3.Distance(base.transform.position, target) / movementStates[currentState].moveSpeed;
		while (Vector3.Distance(base.transform.position, target) > 1f && walkTime < timeUntilAbortWalk && stamina > 0f)
		{
			characterController.SimpleMove(base.transform.TransformDirection(Vector3.forward) * movementStates[currentState].moveSpeed);
			Quaternion b = Quaternion.LookRotation(target - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * (currentTurnSpeed / 10f));
			currentTurnSpeed += Time.deltaTime;
			walkTime += Time.deltaTime;
			stamina -= Time.deltaTime;
			yield return null;
		}
		targetLocation = Vector3.zero;
		if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
		{
			animator.SetBool(movementStates[currentState].animationBool, value: false);
		}
		if (stamina <= 0f || predator.dead || Vector3.Distance(base.transform.position, predator.transform.position) > awareness)
		{
			BeginIdleState();
		}
		else
		{
			NonNavMeshRunAwayFromAnimal(predator);
		}
	}

	private void ChaseAnimal(Common_WanderScript prey)
	{
		_ = prey.transform.position;
		prey.BeginChase(this);
		if (movementStates.Length == 0)
		{
			Debug.Log("Movement states length is 0");
			base.enabled = false;
			return;
		}
		int num = 0;
		for (int i = 0; i < movementStates.Length; i++)
		{
			if (movementStates[i].moveSpeed > movementStates[num].moveSpeed)
			{
				num = i;
			}
		}
		currentState = num;
		if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
		{
			animator.SetBool(movementStates[currentState].animationBool, value: true);
		}
		if (useNavMesh)
		{
			StartCoroutine(ChaseState(prey));
		}
		else
		{
			StartCoroutine(NonNavMeshChaseState(prey));
		}
	}

	private IEnumerator ChaseState(Common_WanderScript prey)
	{
		moving = true;
		navMeshAgent.speed = movementStates[currentState].moveSpeed;
		navMeshAgent.angularSpeed = movementStates[currentState].turnSpeed;
		navMeshAgent.SetDestination(prey.transform.position);
		float timeMoving = 0f;
		bool gotAway = false;
		while ((navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance || timeMoving < 0.1f) && timeMoving < stamina)
		{
			navMeshAgent.SetDestination(prey.transform.position);
			timeMoving += Time.deltaTime;
			if (Vector3.Distance(base.transform.position, prey.transform.position) < 2f)
			{
				if (logChanges)
				{
					Debug.Log($"{base.gameObject.name}: Caught prey ({prey.gameObject.name})!");
				}
				if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
				{
					animator.SetBool(movementStates[currentState].animationBool, value: false);
				}
				AttackAnimal(prey);
				yield break;
			}
			if (constainedToWanderZone && Vector3.Distance(base.transform.position, origin) > wanderZone)
			{
				gotAway = true;
				navMeshAgent.SetDestination(base.transform.position);
				break;
			}
			yield return null;
		}
		navMeshAgent.SetDestination(base.transform.position);
		if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
		{
			animator.SetBool(movementStates[currentState].animationBool, value: false);
		}
		if (timeMoving > stamina || prey.dead || Vector3.Distance(base.transform.position, prey.transform.position) > scent || gotAway)
		{
			BeginIdleState();
		}
		else
		{
			ChaseAnimal(prey);
		}
	}

	private IEnumerator NonNavMeshChaseState(Common_WanderScript prey)
	{
		moving = true;
		targetLocation = prey.transform.position;
		currentTurnSpeed = movementStates[currentState].turnSpeed;
		float walkTime = 0f;
		bool gotAway = false;
		float timeUntilAbortWalk = Vector3.Distance(base.transform.position, targetLocation) / movementStates[currentState].moveSpeed;
		while (Vector3.Distance(base.transform.position, targetLocation) > 1f && walkTime < timeUntilAbortWalk && stamina > 0f)
		{
			characterController.SimpleMove(base.transform.TransformDirection(Vector3.forward) * movementStates[currentState].moveSpeed);
			targetLocation = prey.transform.position;
			Quaternion b = Quaternion.LookRotation(targetLocation - base.transform.position);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, b, Time.deltaTime * (currentTurnSpeed / 10f));
			currentTurnSpeed += Time.deltaTime;
			walkTime += Time.deltaTime;
			stamina -= Time.deltaTime;
			if (Vector3.Distance(base.transform.position, prey.transform.position) < 2f)
			{
				if (logChanges)
				{
					Debug.Log($"{base.gameObject.name}: Caught prey ({prey.gameObject.name})!");
				}
				if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
				{
					animator.SetBool(movementStates[currentState].animationBool, value: false);
				}
				AttackAnimal(prey);
				yield break;
			}
			if (constainedToWanderZone && Vector3.Distance(base.transform.position, origin) > wanderZone)
			{
				gotAway = true;
				targetLocation = base.transform.position;
				break;
			}
			yield return null;
		}
		targetLocation = Vector3.zero;
		if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
		{
			animator.SetBool(movementStates[currentState].animationBool, value: false);
		}
		if (stamina <= 0f || prey.dead || Vector3.Distance(base.transform.position, prey.transform.position) > scent || gotAway)
		{
			BeginIdleState();
		}
		else
		{
			ChaseAnimal(prey);
		}
	}

	private void AttackAnimal(Common_WanderScript target)
	{
		attacking = true;
		if (logChanges)
		{
			Debug.Log($"{base.gameObject.name}: Attacking {target.gameObject.name}!");
		}
		if (useNavMesh)
		{
			navMeshAgent.SetDestination(base.transform.position);
		}
		else
		{
			targetLocation = base.transform.position;
		}
		currentState = UnityEngine.Random.Range(0, attackingStates.Length);
		if (attackingStates.Length != 0 && !string.IsNullOrEmpty(attackingStates[currentState].animationBool))
		{
			animator.SetBool(attackingStates[currentState].animationBool, value: true);
		}
		StartCoroutine(MakeAttack(target));
	}

	private IEnumerator MakeAttack(Common_WanderScript target)
	{
		target.GetAttacked(this);
		float timer = 0f;
		while (!target.dead)
		{
			timer += Time.deltaTime;
			if (timer > attackSpeed)
			{
				target.TakeDamage(power);
				timer = 0f;
			}
			yield return null;
		}
		if (attackingStates.Length != 0 && !string.IsNullOrEmpty(attackingStates[currentState].animationBool))
		{
			animator.SetBool(attackingStates[currentState].animationBool, value: false);
		}
		attackingEvent.Invoke();
		StopAllCoroutines();
		DecideNextState(wasIdle: false);
	}

	private void GetAttacked(Common_WanderScript attacker)
	{
		if (attacking)
		{
			return;
		}
		if (logChanges)
		{
			Debug.Log($"{base.gameObject.name}: Getting attacked by {attacker.gameObject.name}!");
		}
		StopAllCoroutines();
		StartCoroutine(TurnToLookAtTarget(attacker.transform));
		if (agression > 0f)
		{
			if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
			{
				animator.SetBool(movementStates[currentState].animationBool, value: false);
			}
			AttackAnimal(attacker);
		}
		else if (moving)
		{
			if (useNavMesh)
			{
				navMeshAgent.SetDestination(base.transform.position);
			}
			else
			{
				targetLocation = base.transform.position;
			}
			if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
			{
				animator.SetBool(movementStates[currentState].animationBool, value: false);
			}
			moving = false;
		}
		else if (idleStates.Length != 0 && !string.IsNullOrEmpty(idleStates[currentState].animationBool))
		{
			animator.SetBool(idleStates[currentState].animationBool, value: false);
		}
	}

	private void TakeDamage(float damage)
	{
		toughness -= damage;
		if (toughness <= 0f)
		{
			Die();
		}
	}

	public void Die()
	{
		if (logChanges)
		{
			Debug.Log($"{base.gameObject.name}: Died!");
		}
		StopAllCoroutines();
		dead = true;
		if (useNavMesh)
		{
			navMeshAgent.SetDestination(base.transform.position);
		}
		else
		{
			targetLocation = base.transform.position;
		}
		IdleState[] array = idleStates;
		foreach (AIState aIState in array)
		{
			if (!string.IsNullOrEmpty(aIState.animationBool))
			{
				animator.SetBool(aIState.animationBool, value: false);
			}
		}
		MovementState[] array2 = movementStates;
		foreach (AIState aIState2 in array2)
		{
			if (!string.IsNullOrEmpty(aIState2.animationBool))
			{
				animator.SetBool(aIState2.animationBool, value: false);
			}
		}
		AIState[] array3 = attackingStates;
		foreach (AIState aIState3 in array3)
		{
			if (!string.IsNullOrEmpty(aIState3.animationBool))
			{
				animator.SetBool(aIState3.animationBool, value: false);
			}
		}
		if (deathStates.Length != 0)
		{
			currentState = UnityEngine.Random.Range(0, deathStates.Length);
			if (!string.IsNullOrEmpty(deathStates[currentState].animationBool))
			{
				animator.SetBool(deathStates[currentState].animationBool, value: true);
			}
		}
		else
		{
			SkinnedMeshRenderer[] componentsInChildren = GetComponentsInChildren<SkinnedMeshRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
		}
		deathEvent.Invoke();
		base.enabled = false;
	}

	public void SetPeaceTime(bool peace)
	{
		if (peace)
		{
			dominance = 0;
			scent = 0f;
			agression = 0f;
		}
		else
		{
			dominance = originalDominance;
			scent = originalScent;
			agression = originalAgression;
		}
	}

	private Vector3 RandonPointInRange()
	{
		Vector3 vector = origin + UnityEngine.Random.insideUnitSphere * wanderZone;
		return new Vector3(vector.x, base.transform.position.y, vector.z);
	}

	private IEnumerator TurnToLookAtTarget(Transform target)
	{
		while (true)
		{
			Vector3 vector = target.position - base.transform.position;
			if (!(Vector3.Angle(vector, base.transform.forward) < 1f))
			{
				float maxRadiansDelta = 2f * Time.deltaTime;
				Vector3 forward = Vector3.RotateTowards(base.transform.forward, vector, maxRadiansDelta, 0f);
				base.transform.rotation = Quaternion.LookRotation(forward);
				yield return null;
				continue;
			}
			break;
		}
	}

	private void BeginChase(Common_WanderScript chasingAnimal)
	{
		if (!attacking)
		{
			StartCoroutine(ChaseCheck(chasingAnimal));
		}
	}

	private IEnumerator ChaseCheck(Common_WanderScript chasingAnimal)
	{
		while (Vector3.Distance(base.transform.position, chasingAnimal.transform.position) > awareness)
		{
			yield return new WaitForSeconds(0.5f);
		}
		StopAllCoroutines();
		if (moving)
		{
			if (useNavMesh)
			{
				navMeshAgent.SetDestination(base.transform.position);
			}
			else
			{
				targetLocation = base.transform.position;
			}
			if (!string.IsNullOrEmpty(movementStates[currentState].animationBool))
			{
				animator.SetBool(movementStates[currentState].animationBool, value: false);
			}
			moving = false;
		}
		else if (idleStates.Length - 1 >= currentState && idleStates.Length != 0 && !string.IsNullOrEmpty(idleStates[currentState].animationBool))
		{
			animator.SetBool(idleStates[currentState].animationBool, value: false);
		}
		DecideNextState(wasIdle: false);
	}
}
