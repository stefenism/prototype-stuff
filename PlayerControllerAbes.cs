using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerAbes : MonoBehaviour {

	[HideInInspector]
	public Rigidbody2D rb;
	private Animator anim;

	private bool facingRight;
	public bool jumping;
	public bool jumpingForward;
	private bool canJump;
	private bool canRun;
	private bool sprinting;

	public bool grounded;
	public bool jumpAllowed;
	public bool ledgeDetected;

	public float jumpForce;
	public float speed;

	public Vector2 maxVelocity = new Vector2(3, 200);
	public GameObject jumpTowardsObject;
	private Vector3 jumpDirection;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		grounded = false;
		ledgeDetected = false;
		facingRight = true;

		jumping = false;
		jumpingForward = false;
		jumpAllowed = true;
		canJump = true;

		sprinting = false;
		canRun = true;
	}

	// Update is called once per frame
	void Update () {
		CheckInput();
	}

	void FixedUpdate()
	{
		if(jumping || jumpingForward)
		{
			if(jumpAllowed)
			{
				Jump();
			}

		}

	}

	void CheckInput()
	{
		float moveDir = Input.GetAxis("Horizontal");
		if(canRun)
		{
			Run(moveDir);

			if(moveDir > 0 && !facingRight)
			{
				Flip();
			}
			else if(moveDir < 0 && facingRight)
			{
				Flip();
			}
		}


		if(grounded)
		{
			jumping = false;
			jumpingForward = false;
			canJump = true;
			canRun = true;
		}
		else
		{
			canJump = false;
		}

		JumpButton();



		if(ledgeDetected)
		{
			GrabLedge();
		}

		DetermineRunButton();
		anim.SetBool("grounded", grounded);
		anim.SetFloat("vspeed", rb.velocity.y);
	}

	void Run(float moveDir)
	{
		if(sprinting)
		{
			speed = 5f;
		}
		else
		{
			speed = 3f;
		}

		maxVelocity = new Vector2(speed, maxVelocity.y);

		rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
		anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
	}

	void JumpButton()
	{
		if(Input.GetKeyDown(KeyCode.Space) && rb.velocity.x == 0 && jumpAllowed)
		{
			jumping = true;
			canJump = false;
			canRun = false;

		}

		if(Input.GetKeyDown(KeyCode.Space) && rb.velocity.x != 0 && jumpAllowed)
		{
			jumpDirection = (jumpTowardsObject.transform.position - transform.position).normalized;
			jumpingForward = true;
			canRun = false;
			canJump = false;
		}
	}

	void Jump()
	{
		if(jumping)
		{
			jumping = false;
			rb.AddForce(new Vector2(0f, 1f * jumpForce), ForceMode2D.Impulse);
		}

		if(jumpingForward)
		{
			jumpingForward = false;
			rb.AddForce(new Vector2(jumpDirection.x * jumpForce / 3, 1f * jumpForce/2f), ForceMode2D.Impulse);
		}

		if(rb.velocity.y > maxVelocity.y)
		{
			rb.velocity = maxVelocity;
		}

	}

	void GrabLedge()
	{
		Debug.Log("Grab the ledge, remove gravity, stop movement yadda yadd");
	}

	void DetermineRunButton()
	{
		if(Input.GetKey(KeyCode.LeftShift))
		{
			sprinting = true;
		}
		else
		{
			sprinting = false;
		}
	}

	void Flip()
	{
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


}
