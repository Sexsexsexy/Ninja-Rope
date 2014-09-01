using UnityEngine;
using System.Collections;

public class NewPlayerController : MonoBehaviour
{

	public float runSpeed;
	public float runAcceleration;
	public float jumpForce;
	public float dashForce;
	public float slideForce;
	public bool onRope;
	public Transform groundCheck;
	public float groundRadius;
	public LayerMask whatIsGround;
	public bool holdRope;

	private RopeHandler ropeHandler;
	private Animator animator;
	private bool grounded;
	private bool sliding;
	private bool jump;
	private bool slide;
	private Vector2 dash;


	void Start()
	{
		ropeHandler = GetComponent<RopeHandler>();
		animator = GetComponent<Animator>();
	}
	
	void Update()
	{

	}

	void FixedUpdate()
	{
		CheckGround();
		if (grounded) {
			if (jump) {
				Jump();
				//jump = false;
			}
			if (slide) {
				Slide();
				//slide = false;
			}
			if (sliding) {
				if (rigidbody2D.velocity.x < 1)
					sliding = false;
			} else
				Run();
		} else {
			if (dash != Vector2.zero) {
				Dash();
			}
		}
		animator.SetBool("Swinging", onRope);
		animator.SetBool("Jumping", !grounded);
		animator.SetBool ("Sliding", sliding);
	}

	private void CheckGround()
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		//animator.SetBool("Jumping", !grounded);
	}

	private void Run()
	{
		rigidbody2D.AddForce((runSpeed - rigidbody2D.velocity.x) * runAcceleration * Vector2.right);
	}

	private void Jump()
	{
		rigidbody2D.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
		jump = false;
	}

	private void Dash()
	{
		rigidbody2D.AddForce(dashForce * dash, ForceMode2D.Impulse);
		dash = Vector2.zero;
		animator.SetTrigger ("Dash");
	}

	private void Slide()
	{
		rigidbody2D.AddForce(slideForce * Vector2.right, ForceMode2D.Impulse);
		slide = false;
		sliding = true;
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.CompareTag("Ground")) {
			if (onRope) {
				ropeHandler.ReleaseRope();
			}
		} else if (col.transform.CompareTag("Deadly")) {

		}
	}

	// This section handles the input
	public void Pressed()
	{
		if (grounded) {
			jump = true;
		} else {
			if (!onRope)
				ropeHandler.ShootRope();
			else if (!holdRope)
				ropeHandler.ReleaseRope();
		}
	}

	public void Released()
	{
		if (onRope && holdRope) {
			ropeHandler.ReleaseRope();
		}
	}

	public void Swipe(Vector2 dir)
	{
		if (grounded) {
			slide = true;
		} else {
			if (!onRope)
				dash = dir;
		}
	}

}
