using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour
{
	public RopeShooter shooter;
	private Vector2 velocity;
	public Transform player;
	public bool followPlayer;

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
//		if (velocity.sqrMagnitude != 0) {
//			transform.Translate(velocity*Time.deltaTime,Space.World);
//			//Debug.Log("shouldhavemoved");
//		}
		if (followPlayer) {
			transform.position = player.position;
		}
	}

	public void SetSpeed(Vector2 value)
	{
		//Debug.Log("speedset");
		rigidbody2D.isKinematic = false;
		//velocity = value;
		transform.up = value.normalized;
		rigidbody2D.velocity = value;
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (!col.CompareTag("Player")) {
			rigidbody2D.isKinematic = true;
			velocity = Vector2.zero;
		}
		shooter.CreateJoint();
	}
}
