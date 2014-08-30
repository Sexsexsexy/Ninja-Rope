using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float airResistance;
	public float normalSpeed;
	public float maxSpeed;
	public float jumpSpeed;
	public float acceleration;
	public float ropeSwing;
	public float ropeReleaseBoost;
	public float tapLength;
	public bool noMouse;
	public float groundDistance;
	[HideInInspector]
	public bool
		onRope;
	private RopeShooter shooter;
	//private float jumpTime;
	private bool onGround;
	private SpringJoint2D joint;

	private float spaceHold;

	Animator anim;


	// Use this for initialization
	void Start()
	{
		rigidbody2D.drag = airResistance;
		shooter = GetComponent<RopeShooter>();
		shooter.SetNoMouse(noMouse);
		anim = GetComponent<Animator> ();
		onGround = false;
		onRope = false;
		joint = GetComponent<SpringJoint2D>();

		//rigidbody2D.velocity = Vector2.right * 10;
	}

	void Update()
	{
		CheckGround();
		if (noMouse) {
//			if (onRope){
//				joint.distance+=0.1f* Mathf.Sign(transform.position.x - shooter.hook.position.x);
//			}
			if (Input.GetKeyDown(KeyCode.Space)) {
				spaceHold = 0;
				if (!onRope && !onGround) {
					ShootRope();
				}
				if (onGround) { 
					Jump();
				}
			} else if (Input.GetKeyUp(KeyCode.Space)) {
				if (spaceHold < tapLength) {
					//tapped space
					if (onRope) {
						ReleaseRope();
						DirectionBoost();
					}
				} else {
					//released space after holding it
				}
			} else if (Input.GetKey(KeyCode.Space)) {
				//will always happen when you hold down space
				spaceHold += Time.deltaTime;
				if (onGround) {
					FastRun();
				} else if (onRope) {
					Swing();
				}
			} else {

				if (onGround) {
					rigidbody2D.AddForce((normalSpeed - rigidbody2D.velocity.x) * acceleration * Vector3.right * Time.deltaTime);

				}
			}
		}

		anim.SetBool ("Swinging", onRope);
		anim.SetBool ("Jumping", !onGround);

	}

	private void Swing()
	{
		rigidbody2D.AddForce(ropeSwing * -Vector2.up * Time.deltaTime);
	}

	private void Jump()
	{
		rigidbody2D.AddForce(jumpSpeed * Vector3.up, ForceMode2D.Impulse);
	}

	private void FastRun()
	{
		rigidbody2D.AddForce((maxSpeed - rigidbody2D.velocity.x) * acceleration * Vector3.right * Time.deltaTime);
	}

	private void DirectionBoost()
	{
		rigidbody2D.AddForce(ropeReleaseBoost * rigidbody2D.velocity.normalized, ForceMode2D.Impulse);

	}

	private void ReleaseRope()
	{
		shooter.ReleaseRope();
	}

	private void ShootRope()
	{
		shooter.ShootRope();
	}

	private void CheckGround()
	{
		//onGround = Physics2D.Raycast(transform.position, Vector3.down, groundDistance);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up,groundDistance,LayerMask.GetMask("Ground"));
		if (hit.collider != null) {
			onGround = true;
		} else {
			onGround=false;
		}
//		Debug.Log(onGround);


	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.CompareTag("Ground")) {
			if (onRope) {
				ReleaseRope();
			}
		}
	}
	
//	public void OnCollisionExit2D(Collision2D col)
//	{
//		if (col.transform.CompareTag("Ground")) {
//			onGround = false;
//		}
//	}
}

// Update is called once per frame
//	void FixedUpdate()
//	{	
//		if (Input.GetKeyDown(KeyCode.Space)) {
//			if (grounded) {
//				rigidbody2D.AddForce(2 * jumpSpeed * Vector3.up * Time.fixedDeltaTime);
//			} else if (onRope) {
//				rigidbody2D.AddForce(ropeSwing * (maxSwing - rigidbody2D.velocity.magnitude) * rigidbody2D.velocity.normalized * Time.fixedDeltaTime);
//			} else if (noMouse) {
//				shooter.ShootRope();
//			}
////		} else if (Input.GetKey(KeyCode.Space)) {
////			if (onRope) {
////				rigidbody2D.AddForce(ropeSwing * (maxSwing - rigidbody2D.velocity.magnitude) * rigidbody2D.velocity.normalized * Time.fixedDeltaTime);
////			} else if (jumpTime < maxJumpTime) {
////				rigidbody2D.AddForce(jumpSpeed * Vector3.up * Time.fixedDeltaTime);
////			}
//		} else if (Input.GetKeyUp(KeyCode.Space)) {
//			shooter.ReleaseRope();
//			rigidbody2D.AddForce(2 * jumpSpeed * Vector3.up * Time.fixedDeltaTime);
//		}
//		if (!grounded) {
//			jumpTime += Time.fixedDeltaTime;
//		} else {
//			jumpTime = 0;
//			rigidbody2D.AddForce((maxSpeed - rigidbody2D.velocity.x) * acceleration * Vector3.right * Time.fixedDeltaTime);
//		}
//}

//	public void OnCollisionEnter2D(Collision2D col)
//	{
//		if (col.transform.CompareTag("Ground")) {
//			grounded = true;
//		}
//	}
//
//	public void OnCollisionExit2D(Collision2D col)
//	{
//		if (col.transform.CompareTag("Ground")) {
//			grounded = false;
//		}
//	}
//}