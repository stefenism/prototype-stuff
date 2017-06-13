using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetectAbes : MonoBehaviour {

	public PlayerControllerAbes player;
	public float groundDistance;
	public LayerMask groundLayer;


	// Use this for initialization
	void Start () {

		//player = GetComponent<PlayerController>();

	}

	// Update is called once per frame
	void Update () {

		GroundDetection();
	}

	void GroundDetection()
	{
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, groundDistance, groundLayer);

		//Ray2D landingRay = new Ray2D(transform.position, -Vector2.up);

		Debug.DrawRay(transform.position, -Vector2.up * groundDistance);

		if(hit.collider != null)
		{

			if(hit.collider.gameObject.tag == "Ground" && (player.rb.velocity.y > 5))
			{
				player.grounded = false;
			}

			else
			{
				player.grounded = true;
			}
		}

		else
		{
			player.grounded = false;
		}
	}
}
