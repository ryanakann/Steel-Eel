using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EelMovement : MonoBehaviour {

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
	private Vector3 mouseScreenPosition;
	private Vector3 mouseScreenPositionLF;
	[SerializeField] private float mouseSpeedMultiplier;
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
        EelController.instance.DashEvent += Dash;
		mouseSpeedMultiplier = 0f;
	}

	private void FixedUpdate () {
		GetPointerPosition();

		if (targetDirection.magnitude > 0.2f) {
			rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity * (1 + mouseSpeedMultiplier), ref refVelocity, 1 / sensitivity);
		}

		if (currentDashCooldown > 0) {
			currentDashCooldown -= Time.fixedDeltaTime;
		}
        else {
            currentDashCooldown = 0;
        }

		//print("Difference: " + (mouseScreenPosition - mouseScreenPositionLF).magnitude);
		mouseSpeedMultiplier = Mathf.Clamp((mouseScreenPosition - mouseScreenPositionLF).magnitude / 10f, 0f, 2f);
		mouseScreenPositionLF = mouseScreenPosition;
	}

	private void Dash () {
        if (currentDashCooldown <= 0) {
            rb.velocity = (mouseWorldPosition - (Vector2)transform.position).normalized * dashSpeed;
            currentDashCooldown = maxDashCooldown;
        }
	}

	private void GetPointerPosition () {
		mouseScreenPosition = Input.mousePosition;

        Vector3 screen_pos = EelController.instance.mouse_position;
        if (screen_pos.z == -500)
        {
            screen_pos = mainCamera.WorldToScreenPoint(transform.position);
        }
        mouseScreenRay = mainCamera.ScreenPointToRay(screen_pos);

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
}
