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
	[HideInInspector]
	public bool
		onRope;
	private RopeShooter shooter;
	//private float jumpTime;
	private bool onGround;
	private float spaceHold;

	// Use this for initialization
	void Start()
	{
		rigidbody2D.drag = airResistance;
		shooter = GetComponent<RopeShooter>();
		shooter.SetNoMouse(noMouse);
		onGround = false;
		onRope = false;
		//rigidbody2D.velocity = Vector2.right * 10;
	//	jumpTime = 0;
	}

	void Update()
	{
		if (noMouse) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				spaceHold = 0;
				if (!onRope && !onGround) {
					ShootRope();
				}
			} else if (Input.GetKeyUp(KeyCode.Space)) {
				if (spaceHold < tapLength) {
					//tapped space
					if (onGround) { 
						Jump();
					} else if (onRope) {
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
			} else{
				if(onGround){
					rigidbody2D.AddForce((normalSpeed - rigidbody2D.velocity.x) * acceleration * Vector3.right * Time.deltaTime);

				}
			}
		} 
//		else {
//			if (Input.GetKeyDown(KeyCode.Space)) {
//				spaceHold = 0;
//			} else if (Input.GetKeyUp(KeyCode.Space)) {
//				if (spaceHold < tapLength) {
//					//tapped space
//					if (onGround) {
//						if (onRope) {
//							ReleaseRope();
//						} else {
//							Jump();
//						}
//					} else if (onRope) {
//						ReleaseRope();
//					} else {
//						ShootRope();
//					}
//				} else {
//					//released space after holding it
//				}
//			} else if (Input.GetKey(KeyCode.Space)) {
//				//will always happen when you hold down space
//				spaceHold += Time.deltaTime;
//				if(onGround){
//					Run();
//				}else if(onRope){
//					Swing();
//				}
//			}
//			if (Input.GetMouseButtonDown(0)) {
//				shooter.ShootRope();
//			}
//			//spacetap in air = shoot, space hold = jump(if not on rope), space tap on rope = releaseRope,space hold on rope = swing
//		}


//
//
//		if (!noMouse && Input.GetMouseButtonDown(0)) {
//			shooter.ShootRope();
//		}
//		if (Input.GetKeyDown(KeyCode.Space)) {
//			if (grounded) {
//				//rigidbody2D.AddForce(0.016f * jumpSpeed * Vector3.up );
//			} else if (onRope) {
//				//rigidbody2D.AddForce(ropeSwing * rigidbody2D.velocity.x * Vector2.right * Time.deltaTime);
//				rigidbody2D.AddForce(ropeSwing * -Vector2.up * Time.deltaTime);
//			} else if (noMouse) {
//				shooter.ShootRope();
//			}
//		} else if (Input.GetKey(KeyCode.Space)) {
//			if (onRope) {
////				rigidbody2D.AddForce(ropeSwing * (maxSwing - Mathf.Abs(rigidbody2D.velocity.x)) * rigidbody2D.velocity.x *Vector2.right* Time.deltaTime);
//				rigidbody2D.AddForce(ropeSwing * -Vector2.up * Time.deltaTime);
//			} else if (jumpTime < maxJumpTime) {
//				rigidbody2D.AddForce(jumpSpeed * Vector3.up * Time.deltaTime);
//			}
//		} else if (Input.GetKeyUp(KeyCode.Space)) {
//			if (onRope) {
//				shooter.ReleaseRope();
//				rigidbody2D.AddForce((ropeReleaseBoost * rigidbody2D.velocity.normalized + ropeReleaseJump * Vector2.up) * Time.deltaTime);
//			}
//		}
//		if (!grounded) {
//			jumpTime += Time.deltaTime;
//		} else {
//			jumpTime = 0;
//			rigidbody2D.AddForce((maxSpeed - rigidbody2D.velocity.x) * acceleration * Vector3.right * Time.deltaTime);
//		}
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

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.CompareTag("Ground")) {
			if(onRope){
				ReleaseRope();
			}
			onGround = true;
		}
	}
	
	public void OnCollisionExit2D(Collision2D col)
	{
		if (col.transform.CompareTag("Ground")) {
			onGround = false;
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