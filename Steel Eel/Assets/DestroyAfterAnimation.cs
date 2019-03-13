using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DestroyAfterAnimation : MonoBehaviour {
	Animator animator;

	private void Start () {
		animator = GetComponent<Animator>();

		Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
	}
}