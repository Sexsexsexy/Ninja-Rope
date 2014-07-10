using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour
{
	public RopeShooter shooter;
	private Vector2 velocity;
	public Transform player;
	public bool followPlayer;
	private bool drawLine;
	private LineRenderer line;

	// Use this for initialization
	void Start()
	{
		line = GetComponentInChildren<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update()
	{
//		if (velocity.sqrMagnitude != 0) {
//			transform.Translate(velocity*Time.deltaTime,Space.World);
//			//Debug.Log("shouldhavemoved");
//		}
		if (drawLine) {
			line.SetPosition(0,player.position);
			line.SetPosition(1,transform.position);
		}
		if (followPlayer) {
			transform.position = player.position;
		} 
	}

//	public void SetSpeed(Vector2 value)
//	{
//		drawLine = true;
//		line.enabled = true;
//		//Debug.Log("speedset");
//		rigidbody2D.isKinematic = false;
//		//velocity = value;
//		transform.up = value.normalized;
//		rigidbody2D.velocity = value;
//	}

	public void Shoot(Vector2 start, Vector2 force){
		followPlayer = false;
		drawLine = true;
		line.SetPosition(0,player.position);
		line.SetPosition(1,transform.position);
		line.enabled = true;
		//Debug.Log("speedset");
		//velocity = value;
		transform.position = start;
		transform.up = force;
		rigidbody2D.isKinematic = false;
		rigidbody2D.AddForce (force);

	}

	public void Release(){
		followPlayer = true;
		drawLine = false;
		line.enabled = false;
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (!col.transform.CompareTag("Player")) {
			rigidbody2D.isKinematic = true;
			velocity = Vector2.zero;
		}
		shooter.CreateJoint();
	}
}
