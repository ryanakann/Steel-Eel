using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointGizmo : MonoBehaviour {
	public bool gizmosEnabled = false;
	public Color color;
	public float pointRadius = 0.25f;


	private void OnDrawGizmos () {
		if (!gizmosEnabled) return;

		Gizmos.color = color;
		foreach (Transform child in transform) {
			Gizmos.DrawSphere(child.position, pointRadius);
		}
	}
}
