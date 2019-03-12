﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	public bool canSeePlayer;
	public bool canSeePlayerHead;
	public bool soundHeard;
	public bool movingObjectSeen;
	public float speed = 2f;
	[SerializeField] private float timeSpentNotMoving;
	private float maxTimeSpentNotMoving = 6f;
	private Vector3 positionLastFrame;

	public EnemyState state;
	private Pathfinding.AIDestinationSetter destinationSetter;
	private EnemySight lineOfSight;

	[SerializeField] private Transform[] waypoints;

	public Transform player;

	//Patrolling
	private float distanceToNextWaypoint;
	private float timeSpentAtWaypoint;
	private int waypointIndex;
	[SerializeField] private Transform currentWaypoint;

	//Investigating
	private float distanceToInvestigatePoint;
	private float timeSpentInvestigating;
	private float maxTimeSpentInvestigating = 5f;
	private float timeSpentLookingInDirection;
	private bool switchDirections;
	private bool switchDirectionsLastFrame;
	private Transform investigatePoint;

	//Chasing
	private float distanceToPlayer;
	private float timeSpentChasing;
	private float timeSincePlayerLastSeen;
	private float maxTimeSincePlayerLastSeen = 3f;

	//Capturing
	private float timeSpentCapturing;
	private float captureDuration = 2f;

	//Stunned
	private float timeSpentStunned;
	private float stunDuration = 5f;

	private void Awake () {
		UpdateWaitpoints();
	}

	private void Start () {
		distanceToNextWaypoint = Mathf.Infinity;
		timeSpentAtWaypoint = 0f;
		waypointIndex = 1;

		if (waypoints.Length >= 2) {
			currentWaypoint = waypoints[waypointIndex];
		} else {
			currentWaypoint = null;
		}

		distanceToInvestigatePoint = Mathf.Infinity;
		timeSpentInvestigating = 0f;
		timeSpentLookingInDirection = 0f;
		switchDirections = false;
		switchDirectionsLastFrame = switchDirections;

		distanceToPlayer = Mathf.Infinity;
		timeSpentChasing = 0f;
		timeSincePlayerLastSeen = 0f;

		timeSpentCapturing = 0f;

		timeSpentStunned = 0f;

		state = EnemyState.patrolling;

		investigatePoint = new GameObject("InvestigatePoint").transform;
		transform.SetParent(transform);

		destinationSetter = GetComponent<Pathfinding.AIDestinationSetter>();
		destinationSetter.target = currentWaypoint;
		GetComponent<Pathfinding.AIPath>().maxSpeed = speed;

		lineOfSight = GetComponent<EnemySight>();

		if (player == null) {
			if ((player = GameObject.FindWithTag("Player").transform) == null) {
				Debug.LogError("AI cannot function as Player reference is null.");
			}
		}

		positionLastFrame = transform.position;
	}

	void OnDrawGizmos () {
		if (destinationSetter && destinationSetter.target) {
			Gizmos.DrawWireSphere(destinationSetter.target.position, 2f);
		}
	}

	void UpdateWaitpoints () {
		Transform parent;
		waypoints = new Transform[0];
		if ((parent = transform.Find("Waypoints")) != null) {
			List<Transform> waypointList = new List<Transform>(parent.GetComponentsInChildren<Transform>());
			waypointList.RemoveAt(0);
			waypoints = waypointList.ToArray();
		}
		parent.SetParent(null);
	}

	private void Update () {
		UpdateConditions();
		StateController();
	}

	private void UpdateConditions () {
		canSeePlayer = lineOfSight.playerInSight;
		canSeePlayerHead = lineOfSight.playerHeadInSight;
	}

	private void StateController () {
		switch (state) {
			case EnemyState.patrolling:
				if (canSeePlayerHead) {
					timeSpentChasing = 0f;
					state = EnemyState.chasing;
				} else if (canSeePlayer) {
					timeSpentInvestigating = 0f;
					investigatePoint.position = player.position;
					state = EnemyState.investigating;
				} else if (soundHeard) {
					timeSpentInvestigating = 0f;
					state = EnemyState.investigating;
				} else if (movingObjectSeen) {
					timeSpentInvestigating = 0f;
					state = EnemyState.investigating;
				}

				if (currentWaypoint == null) break;

				distanceToNextWaypoint = (currentWaypoint.position - transform.position).sqrMagnitude;
				destinationSetter.target = currentWaypoint;

				if (distanceToNextWaypoint < 0.2f * 0.2f) {
					currentWaypoint = waypoints[++waypointIndex % waypoints.Length];
				}
				break;

			case EnemyState.investigating:
				destinationSetter.target = investigatePoint;
				if (canSeePlayerHead) {
					state = EnemyState.chasing;
				} else if (canSeePlayer) {
					timeSpentInvestigating = 0f;
					investigatePoint.position = player.position;
					state = EnemyState.investigating;
				} else if (soundHeard) {
					timeSpentInvestigating = 0f;
					state = EnemyState.investigating;
				} else if (movingObjectSeen) {
					timeSpentInvestigating = 0f;
					state = EnemyState.investigating;
				}

				if ((investigatePoint.position - transform.position).sqrMagnitude < 0.2f * 0.2f) {
					timeSpentInvestigating += Time.deltaTime;
					//destinationSetter.target = null;

					if (timeSpentLookingInDirection > maxTimeSpentInvestigating / 2.5f) {
						switchDirections = !switchDirections;
						timeSpentLookingInDirection = 0f;
					} else {
						timeSpentLookingInDirection += Time.deltaTime;
					}
				}

				if (switchDirections != switchDirectionsLastFrame) {
					Vector3 position = (Vector3)Random.insideUnitCircle.normalized / 4f + transform.position;
					investigatePoint.position = position;
				}

				switchDirectionsLastFrame = switchDirections;

				if (timeSpentInvestigating > maxTimeSpentInvestigating) {
					destinationSetter.target = null;
					state = EnemyState.patrolling;
				}
				break;

			case EnemyState.chasing:
				destinationSetter.target = player;
				if (canSeePlayer) {
					timeSincePlayerLastSeen = 0f;

					if ((transform.position - player.position).sqrMagnitude < 0.2f * 0.2f) {
						timeSpentCapturing = 0f;
						state = EnemyState.capturing;
					}
				} else {
					timeSincePlayerLastSeen += Time.deltaTime;
				}

				if (timeSincePlayerLastSeen > maxTimeSincePlayerLastSeen) {
					timeSpentInvestigating = 0f;
					state = EnemyState.investigating;
				} else {
					investigatePoint.position = player.position;
				}
				timeSpentChasing += Time.deltaTime;
				break;

			case EnemyState.capturing:
                EelController.instance.can_input = false;
                FadeController.instance.FadeOut(0.16f);
                if (timeSpentCapturing > captureDuration) {
                    state = EnemyState.stunned;
				}

				//STUN CONDITION

				timeSpentCapturing += Time.deltaTime;
				break;

			case EnemyState.stunned:
				break;

			default:
				Debug.LogError("Unkown AI State for " + gameObject.name);
				break;
		}

		if (timeSpentNotMoving < maxTimeSpentNotMoving) {
			if ((transform.position - positionLastFrame).magnitude < 0.25f * speed * Time.deltaTime) {
				timeSpentNotMoving += Time.deltaTime;
			} else {
				timeSpentNotMoving = 0f;
			}
		} else {
			timeSpentNotMoving = 0f;
			state = EnemyState.patrolling;
		}

		positionLastFrame = transform.position;
	}
}

public enum EnemyState {
	patrolling,
	investigating,
	chasing,
	capturing,
	stunned,
}
/*
Patrol
Wander between waypoint list, pausing once reaching each waypoint
Player head spotted
Goto 3
Player non-head spotted
Goto 2
Non-player object moving
Goto 2
Sound within proximity
Goto 2
Investigate
Move to place previously spotted
Investigate duration passed (about 5 seconds)
Goto 1
Chase
Double movement speed
Lose sight for duration (around 2 seconds)
Goto 2
Get in grabbing range
Goto 4
Capture
Player is picked up and can no longer move
Can get captured once and use electrify to escape (with cooldown)
Goto 5
Stunned
Can’t move for duration (around 5 seconds)
*/
