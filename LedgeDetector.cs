using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeDetector : MonoBehaviour {

	public GameObject[]  detectionRays;
	public float ledgeDistance;

	private PlayerController player;

	public LayerMask groundLayer;


	// Use this for initialization
	void Start () {
		player = GetComponent<PlayerController>();

		//detectionRays = new GameObject[2];
	}

	// Update is called once per frame
	void FixedUpdate () {

		DetectLedge();
	}

	void DetectLedge()
	{
		//first ray is the head, Second ray is in front of the body
		RaycastHit2D eyeLevelRay = Physics2D.Raycast(detectionRays[0].transform.position,
		 										(Vector2.right * transform.localScale.x), ledgeDistance, groundLayer);


		RaycastHit2D chestLevelRay = Physics2D.Raycast(detectionRays[1].transform.position,
		 										(Vector2.right  * transform.localScale.x), ledgeDistance, groundLayer);


		Debug.DrawRay(detectionRays[0].transform.position, (Vector2.right   * transform.localScale.x) * ledgeDistance);
		Debug.DrawRay(detectionRays[1].transform.position, (transform.right  * transform.localScale.x) * ledgeDistance);

		if(chestLevelRay.collider != null)
		{
			if(chestLevelRay.collider.gameObject.tag == "Ground")
			{
				if (eyeLevelRay.collider == null)
				{
					//Debug.Log("!!!!!eyeLevelRay collider: " + eyeLevelRay.collider.gameObject.tag);


					player.ledgeDetected = true;
				}
				if(eyeLevelRay.collider != null)
				{
					if(eyeLevelRay.collider.gameObject.tag != "Ground")
					{
						player.ledgeDetected = true;
					}
				}
			}
		}

		else
		{
			player.ledgeDetected = false;
		}
	}
}
