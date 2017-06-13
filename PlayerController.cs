using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[HideInInspector]
	public Rigidbody2D rb;
	private Animator anim;

	private bool facingRight;
	public bool jumping;
	private bool canJump;
	private bool sprinting;

	private Vector2 maxWalkVelocity = new Vector2(12,10);
	private Vector2 maxRunVelocity = new Vector2(15,10);

	public bool grounded;
	public bool jumpAllowed;

	public float jumpDuration;
	public float jumpTime;
	public float speed;
	public float jumpForce;
	public float drag;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();

		grounded = false;
		facingRight = true;
		jumping = false;
		jumpAllowed = true;
		sprinting = false;
	}

	// Update is called once per frame
	void Update () {
		CheckInput();
	}

	void FixedUpdate()
	{
		if(jumping && jumpAllowed)
		{
			Jump();
		}

		//CheckInput();

	}

	void CheckInput()
	{
		float moveDir = Input.GetAxis("Horizontal");
		Run(moveDir);
		if(grounded)
		{
			jumping = false;
			jumpDuration = 0;
			canJump = true;
		}
		else
		{
			canJump = false;
		}

		JumpButton();

		if(moveDir > 0 && !facingRight)
		{
			Flip();
		}
		else if(moveDir <0 && facingRight)
		{
			Flip();
		}
		if(!grounded)
		{
			jumpDuration += Time.deltaTime;
			//Debug.Log("jumpduration: " + jumpDuration);
			//Debug.Log("rb velocity y: " + rb.velocity.y);
		}

		if(jumping)
		{
			AirDrag(jumpDuration * drag);
		}

		DetermineJumpButton();
		DetermineRunButton();
		anim.SetBool("grounded", grounded);
		anim.SetFloat("vspeed", rb.velocity.y);
	}

	void Run(float moveDir)
	{



		if(sprinting)
		{
			speed = 8f;
		}
		else
		{
			speed = 5f;
		}

		rb.velocity = new Vector2(moveDir * speed, rb.velocity.y);
		anim.SetFloat("speed", Mathf.Abs(rb.velocity.x));
		/*
		if((Mathf.Abs(rb.velocity.x) > maxWalkVelocity.x) && !sprinting)
		{
			rb.velocity = new Vector2((maxWalkVelocity.x), rb.velocity.y);
			speed = 5;
		}

		if(sprinting && Mathf.Abs(rb.velocity.x) > maxRunVelocity.x)
		{
			rb.velocity = new Vector2((maxRunVelocity.x), rb.velocity.y);
			speed = 7;
		}
		*/
	}

	void JumpButton()
	{
		if(Input.GetKey(KeyCode.Space) && jumpAllowed)
		{
			jumping = true;
			canJump = false;

			if(grounded)
			{
				//Debug.Log("jumpdust :)");
			}
		}

		if(jumpDuration >= jumpTime)
		{
			jumping = false;
			jumpAllowed = false;
		}

		else if(Input.GetKeyUp(KeyCode.Space))
		{
			jumping = false;
		}
	}

	void DetermineJumpButton()
	{
		if(jumpDuration == 0)//Input.GetKeyUp(KeyCode.Space))
		{
			jumpAllowed = true;
		}
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
		Debug.Log("sprinting: " + sprinting);
	}

	void Jump()
	{

		//rb.AddForce(new Vector2(0f,1f * jumpForce), ForceMode2D.Force);
		if(jumpDuration <= jumpTime)
		{
			rb.AddForce(new Vector2(0f,1f * jumpForce), ForceMode2D.Impulse);
			//rb.velocity = new Vector2(rb.velocity.x, jumpForce);
		}

		if(rb.velocity.y > maxWalkVelocity.y)
		{
			rb.velocity = new Vector2(rb.velocity.x, maxWalkVelocity.y);
		}

	}

	/*
	AirDrag is currently not working the way it was intended to work
	I need to figure out how to run checks in the tenth of a second range
	in update I suppose
	*/


	void AirDrag(float duration)
	{
		if(rb.velocity.y > .1f && jumpDuration < jumpTime)
		{
			rb.AddForce(Vector2.down * (duration / jumpTime), ForceMode2D.Force);//w Vector2(0f, -1f * (duration / jumpTime)), ForceMode2D.Impulse);
			//Debug.Log("dragging");
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
