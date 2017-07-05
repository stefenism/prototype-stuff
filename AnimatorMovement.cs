using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorMovement : MonoBehaviour {

	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {

	}

	void OnAnimatorMove()
	{
		Vector3 newPosition = transform.parent.position;
		newPosition = anim.deltaPosition;
		transform.parent.position += newPosition;
	}
}
