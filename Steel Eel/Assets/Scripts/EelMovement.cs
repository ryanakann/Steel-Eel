using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EelMovement : MonoBehaviour {
	public bool showGizmos = true;

	[Header("Mouse Properties")]
	public bool lookAtMouse = true;
	public float sensitivity = 1f;
	public float maxMouseDistance = 3f;

	[Header("Speed Properties")]
	public Vector2 speedRange;
	public float dashSpeed = 5f;
	public float maxDashCooldown = 2f;
	[SerializeField] private float currentDashCooldown;

	private Ray mouseScreenRay;
	private float mouseRayDistance;
	private Vector2 mouseWorldPosition;
	private Camera mainCamera;
	private Plane playArea;

	private Rigidbody2D rb;
	private Vector2 targetDirection;
	private float targetSpeed;
	private Vector2 targetVelocity;
	private Vector2 refVelocity;

	private void Start () {
		mainCamera = Camera.main;
		playArea = new Plane(Vector3.forward, 0f); //Assumes player always at z = 0
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate () {
		GetMousePosition();

		if (targetDirection.magnitude > 0.2f) {
			rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref refVelocity, 1 / sensitivity);
		}

		if (currentDashCooldown <= 0) {
			if (Input.GetMouseButtonDown(0)) {
				Dash();
			}
			currentDashCooldown = 0;
		} else {
			currentDashCooldown -= Time.fixedDeltaTime;
		}
	}

	private void Dash () {
		rb.velocity = (mouseWorldPosition - (Vector2)transform.position).normalized * dashSpeed;
		currentDashCooldown = maxDashCooldown;
	}

	private void GetMousePosition () {
		mouseScreenRay = mainCamera.ScreenPointToRay(Input.mousePosition);

		if (playArea.Raycast(mouseScreenRay, out mouseRayDistance)) {
			mouseWorldPosition = mouseScreenRay.GetPoint(mouseRayDistance);
			targetDirection = (mouseWorldPosition - (Vector2)transform.position) / maxMouseDistance;

			if (targetDirection.sqrMagnitude > 1f) {
				targetDirection.Normalize();
			}

			targetSpeed = Mathf.Lerp(speedRange.x, speedRange.y, targetDirection.magnitude);
			targetVelocity = targetDirection.normalized * targetSpeed;
		}
	}

	private void OnDrawGizmos () {
		if (!showGizmos) return;

		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position, transform.position + (Vector3)targetVelocity);
	}
}
