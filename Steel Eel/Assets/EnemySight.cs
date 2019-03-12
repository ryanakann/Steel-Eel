using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(AIPath))]
public class EnemySight : MonoBehaviour {
	public float fieldOfViewAngle = 110f;
	private float halfFieldOfViewAngle;
	public float maxDistance = 20f;
	public bool playerInSight;
	public bool playerHeadInSight;

	RaycastHit2D[] hits;
	int rayCount;

	private void Start () {
		rayCount = GameManager.playerPieces.Length;
		hits = new RaycastHit2D[rayCount];
		halfFieldOfViewAngle = fieldOfViewAngle / 2f;
	}

	private void Update () {
		playerInSight = false;
		playerHeadInSight = false;
		for (int i = 0; i < rayCount; i++) {
			Vector3 direction = GameManager.playerPieces[i].transform.position - transform.position;
			float angle = Vector3.Angle(direction, transform.up);

			if (angle < halfFieldOfViewAngle) {
				hits[i] = Physics2D.Raycast(transform.position, direction, maxDistance);
				if (hits[i]) {
					if (hits[i].transform.CompareTag("Player")) {
						if (hits[i].transform.GetComponent<EelMovement>()) {
							playerHeadInSight = true;
						}
						playerInSight = true;
					}
				}
			}
		}
	}
}