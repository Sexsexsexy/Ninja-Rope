using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	public float maxSpeed;
	public float jumpSpeed;
	public float acceleration;
	public float maxJumpTime;
	public float ropeSwing;
	public float maxSwing;
	[HideInInspector]
	public bool onRope;
	private RopeShooter shooter;
	private float jumpTime;
	private bool grounded;

	// Use this for initialization
	void Start()
	{
		shooter = GetComponent<RopeShooter>();
	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0)) {
			shooter.ShootRope();
		}

	}
	// Update is called once per frame
	void FixedUpdate()
	{	
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (grounded) {
				rigidbody2D.AddForce(2 * jumpSpeed * Vector3.up * Time.fixedDeltaTime);
			} else if (onRope) {
				rigidbody2D.AddForce(ropeSwing * (maxSwing - rigidbody2D.velocity.magnitude) * rigidbody2D.velocity.normalized * Time.fixedDeltaTime);
			}
		} else if (Input.GetKey(KeyCode.Space)) {
			if (onRope) {
				rigidbody2D.AddForce(ropeSwing * (maxSwing - rigidbody2D.velocity.magnitude) * rigidbody2D.velocity.normalized * Time.fixedDeltaTime);
			} else if (jumpTime < maxJumpTime) {
				rigidbody2D.AddForce(jumpSpeed * Vector3.up * Time.fixedDeltaTime);
			}
		}
		else if (Input.GetKeyUp(KeyCode.Space)) {
				Debug.Log("Keyup");
				shooter.ReleaseRope();
			}
			if (!grounded) {
				jumpTime += Time.fixedDeltaTime;
			} else {
				jumpTime = 0;
				rigidbody2D.AddForce((maxSpeed - rigidbody2D.velocity.x) * acceleration * Vector3.right * Time.fixedDeltaTime);
			}
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
