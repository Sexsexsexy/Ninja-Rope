using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float airResistance;
	public float maxSpeed;
	public float jumpSpeed;
	public float acceleration;
	public float maxJumpTime;
	public float ropeSwing;
	public float ropeReleaseBoost;
	public float ropeReleaseJump;
	public float tapLength;
	public bool noMouse;
	[HideInInspector]
	public bool
		onRope;
	private RopeShooter shooter;
	private float jumpTime;
	private bool grounded;
	private float spaceHold;

	// Use this for initialization
	void Start()
	{
		rigidbody2D.drag = airResistance;
		shooter = GetComponent<RopeShooter>();
		shooter.SetNoMouse(noMouse);
		grounded = false;
		onRope = false;
		jumpTime = 0;
	}

	void Update()
	{
//		if (noMouse) {
//			if (Input.GetKeyDown(KeyCode.Space)) {
//				spaceHold = 0;
//			} else if (Input.GetKeyUp(KeyCode.Space)) {
//				if (spaceHold < tapLength) {
//					//tapped space
//				} else {
//					//released space after holding it
//				}
//			} else if (Input.GetKey(KeyCode.Space)) {
//				spaceHold+=Time.deltaTime;
//
//			}
//
//		} else {
//			if (Input.GetMouseButtonDown(0)) {
//				shooter.ShootRope();
//			}
//			//spacetap in air = shoot, space hold = jump(if not on rope), space tap on rope = releaseRope,space hold on rope = swing
//		}




		if (!noMouse && Input.GetMouseButtonDown(0)) {
			shooter.ShootRope();
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (grounded) {
				//rigidbody2D.AddForce(0.016f * jumpSpeed * Vector3.up );
			} else if (onRope) {
				//rigidbody2D.AddForce(ropeSwing * rigidbody2D.velocity.x * Vector2.right * Time.deltaTime);
				rigidbody2D.AddForce(ropeSwing * -Vector2.up * Time.deltaTime);
			} else if (noMouse) {
				shooter.ShootRope();
			}
		} else if (Input.GetKey(KeyCode.Space)) {
			if (onRope) {
//				rigidbody2D.AddForce(ropeSwing * (maxSwing - Mathf.Abs(rigidbody2D.velocity.x)) * rigidbody2D.velocity.x *Vector2.right* Time.deltaTime);
				rigidbody2D.AddForce(ropeSwing * -Vector2.up * Time.deltaTime);
			} else if (jumpTime < maxJumpTime) {
				rigidbody2D.AddForce(jumpSpeed * Vector3.up * Time.deltaTime);
			}
		} else if (Input.GetKeyUp(KeyCode.Space)) {
			if (onRope) {
				shooter.ReleaseRope();
				rigidbody2D.AddForce((ropeReleaseBoost * rigidbody2D.velocity.normalized + ropeReleaseJump * Vector2.up) * Time.deltaTime);
			}
		}
		if (!grounded) {
			jumpTime += Time.deltaTime;
		} else {
			jumpTime = 0;
			rigidbody2D.AddForce((maxSpeed - rigidbody2D.velocity.x) * acceleration * Vector3.right * Time.deltaTime);
		}
	}

	private void Swing()
	{
	}

	private void Jump()
	{
	}

	private void Run()
	{
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.CompareTag("Ground")) {
			grounded = true;
		}
	}
	
	public void OnCollisionExit2D(Collision2D col)
	{
		if (col.transform.CompareTag("Ground")) {
			grounded = false;
		}
	}
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