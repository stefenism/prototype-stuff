using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	[HideInInspector]
	public Rigidbody2D rb;
	private Animator anim;
	private Animator rootAnim;

	private bool facingRight;


	//run stuff
	public float speed;
	private bool sprinting;
	private bool canRun;

	private Vector2 maxWalkVelocity = new Vector2(12,15);
	private Vector2 maxRunVelocity = new Vector2(15,10);

	//jump Stuff
	public bool jumping;
	private bool canJump;
	public bool grounded;
	public bool jumpAllowed;
	public float jumpDuration;
	public float jumpTime;
	public float jumpForce;
	public float drag;
	public float dragTime;

	//Ledge Stuff
	public bool ledgeDetected;
	private bool ledgeDrop;
	private bool ledgeClimb;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		rootAnim = GetComponent<Animator>();
		anim = transform.GetChild(0).gameObject.GetComponent<Animator>();

		grounded = false;
		facingRight = true;
		jumping = false;
		jumpAllowed = true;
		sprinting = false;
		canJump = true;
		canRun = true;

		ledgeDetected = false;
		ledgeDrop = false;
		ledgeClimb = false;
	}

	// Update is called once per frame
	void Update () {
		CheckInput();

		//MoveRoot();
		Debug.Log("x velocity: " + rb.velocity.x);
	}

	void FixedUpdate()
	{
		if(jumpAllowed)
		{
			Jump();
		}

		//CheckInput();

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
			else if(moveDir <0 && facingRight)
			{
				Flip();
			}
		}

		if(grounded)
		{
			jumping = false;
			jumpDuration = 0;
			canJump = true;
			canRun = true;
		}
		else
		{
			canJump = false;
		}

		JumpButton();

		if(ledgeDetected && !ledgeDrop)
		{
			GrabLedge();
		}

		if(!grounded && jumping)
		{
			jumpDuration += Time.deltaTime;
			//Debug.Log("jumpduration: " + jumpDuration);
			//Debug.Log("rb velocity y: " + rb.velocity.y);
		}

		//AirDrag(jumpDuration * drag);

		DetermineJumpButton();
		DetermineRunButton();
		anim.SetBool("grounded", grounded);
		anim.SetFloat("vspeed", rb.velocity.y);
		anim.SetBool("ledgeClimb", ledgeClimb);
		rootAnim.SetBool("LedgeClimb", ledgeClimb);
		anim.applyRootMotion = ledgeClimb;
		rootAnim.applyRootMotion = ledgeClimb;
	}

	void Run(float moveDir)
	{



		if(sprinting)
		{
			speed = 7f;
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
			rb.velocity = new Vector2(rb.velocity.x, jumpForce * drag);
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
		//Debug.Log("sprinting: " + sprinting);
	}

	void Jump()
	{

		//rb.AddForce(new Vector2(0f,1f * jumpForce), ForceMode2D.Force);

		if(jumping)
		{

			if(jumpDuration < jumpTime)
			{
				rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			}

			/*
			if(jumpDuration == jumpTime)
			{
				StartCoroutine(DragJump(dragTime));
			}


			if(jumpDuration == 0)
			{
				rb.AddForce(new Vector2(0f,1f * jumpForce), ForceMode2D.Impulse);
			}

			if(jumpDuration < jumpTime && jumpDuration > 0)
			{
				rb.AddForce(new Vector2(0f,1f * jumpForce), ForceMode2D.Force);
				//rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			}
		}


		if(rb.velocity.y > maxWalkVelocity.y)
		{
			rb.velocity = new Vector2(rb.velocity.x, maxWalkVelocity.y);
		}
		*/
	}
	}

	private IEnumerator DragJump(float seconds)
	{
		rb.velocity = new Vector2(rb.velocity.x, jumpForce * .01f);
		yield return new WaitForSeconds(seconds);
		rb.velocity = new Vector2(rb.velocity.x, 0f);

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
			rb.AddForce(Vector2.down * ((duration * .9f) / jumpTime), ForceMode2D.Force);//w Vector2(0f, -1f * (duration / jumpTime)), ForceMode2D.Impulse);
			//Debug.Log("dragging");
		}

		if(!jumping && rb.velocity.y > .1f)
		{
			rb.AddForce(Vector2.down * ((jumpTime * .9f) / jumpTime), ForceMode2D.Force);
		}


	}

	void GrabLedge()
	{
		//Debug.Log("Grab the ledge, remove gravity, stop movement yadda yadd");
		rb.velocity = Vector2.zero;
		rb.gravityScale = 0;
		canRun = false;
		canJump = false;

		if(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
		{
			rb.gravityScale = 1;
			ledgeDrop = true;
			StartCoroutine(LedgeDrop(1));
		}

		if(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
		{
			Debug.Log("pull up");


			//rb.AddForce(new Vector2(jumpDirection.x * jumpForce, 1f * jumpForce/3f), ForceMode2D.Impulse);
			canJump = true;
			jumpAllowed = true;
			ledgeClimb = true;
			StartCoroutine(LedgeClimb(1f));
		}
	}

	void Flip()
	{
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

	}

	void MoveRoot()
	{
		Vector3 theRootScale = transform.parent.localScale;
		if(transform.localScale.x < 0)
		{
			theRootScale.x *= -1;
			transform.parent.localScale = theRootScale;
		}
		else if(transform.localScale.x > 0)
		{
			theRootScale.x *= -1;
			transform.parent.localScale = theRootScale;
		}
		transform.parent.position = transform.position - transform.localPosition;
	}

	private IEnumerator LedgeDrop(float seconds)
	{
		yield return new WaitForSeconds(seconds);

		ledgeDrop = false;
	}

	private IEnumerator LedgeClimb(float seconds)
	{
		yield return new WaitForSeconds(seconds);

		//MoveRoot();
		ledgeClimb = false;
		rb.gravityScale = 1;
	}
}
