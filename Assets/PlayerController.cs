using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public float airResistance;
	public float normalSpeed;
	public float maxSpeed;
	public float jumpSpeed;
	public float acceleration;
	public float tapLength;
	public bool noMouse;
	public float groundDistance;
	public float groundCheckRadius;
	[HideInInspector]
	public bool
		onRope;
	private RopeHandler shooter;
	//private float jumpTime;
	private bool onGround;
	private SpringJoint2D joint;

	private float spaceHold;

	Animator anim;


	// Use this for initialization

	void Start()
	{
		rigidbody2D.drag = airResistance;
		shooter = GetComponent<RopeHandler>();
		shooter.SetNoMouse(noMouse);
		anim = GetComponent<Animator>();
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
					}
				} else {
					//released space after holding it
				}
			} else if (Input.GetKey(KeyCode.Space)) {
				//will always happen when you hold down space
				spaceHold += Time.deltaTime;
			} else {
				if (onGround) {
					rigidbody2D.AddForce((normalSpeed - rigidbody2D.velocity.x) * acceleration * Vector3.right * Time.deltaTime);
				}
			}
		}
		anim.SetBool("Swinging", onRope);
		anim.SetBool("Jumping", !onGround);
	}

	private void Jump()
	{
		rigidbody2D.AddForce(jumpSpeed * Vector3.up, ForceMode2D.Impulse);
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
		//Physics2D.OverlapCircle(transform.position + groundDistance);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, groundDistance, LayerMask.GetMask("Ground"));
		if (hit.collider != null) {
			onGround = true;
		} else {
			onGround = false;
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
