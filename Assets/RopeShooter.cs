using UnityEngine;
using System.Collections;

public class RopeShooter : MonoBehaviour
{
	public Transform hook;
	public float hookSpeed;
	private Hook hookScript;
	private DistanceJoint2D joint;
	private PlayerController controller;

	// Use this for initialization
	void Start()
	{
		controller = GetComponent<PlayerController>();
		hookScript = hook.GetComponent<Hook>();
		hookScript.shooter = this;
		joint = GetComponent<DistanceJoint2D>();
		joint.connectedBody = hook.rigidbody2D;
		joint.enabled = false;
	}


	public void ShootRope()
	{
		joint.enabled = false;
		hookScript.followPlayer = false;
		Vector3 worldpoint=Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Mathf.Abs(Camera.main.transform.position.z)));
		Vector3 direction = (worldpoint - transform.position).normalized;
		//Vector2 dir = new Vector2(direction.x,direction.y);
		//hook.transform.position = transform.position;
		//hook.position = transform.position+direction;
		hook.position = transform.position;
		hookScript.SetSpeed(direction * hookSpeed);
	}

	public void CreateJoint(){
		joint.distance = Vector2.Distance(transform.position , hook.position);
		joint.enabled = true;
		controller.onRope = true;
	}

	public void ReleaseRope(){
		joint.enabled = false;
		controller.onRope = false;
		//	hook.rigidbody2D.isKinematic = false;
		hookScript.followPlayer = true;
	}
}
