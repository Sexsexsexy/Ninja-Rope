using UnityEngine;
using System.Collections;

public class RopeShooter : MonoBehaviour
{
	public Transform hook;
	public float hookSpeed;
	private bool noMouse;
	public float angle;
	private Hook hookScript;
	private DistanceJoint2D joint;
	private PlayerController controller;

	// Use this for initialization
	void Start()
	{
		angle = angle * Mathf.Deg2Rad;
		controller = GetComponent<PlayerController>();
		hookScript = hook.GetComponent<Hook>();
		hookScript.shooter = this;
		joint = GetComponent<DistanceJoint2D>();
		joint.connectedBody = hook.rigidbody2D;
		joint.enabled = false;
	}

	public void SetNoMouse(bool state){
		noMouse = state;
	}

	public void CreateJoint()
	{
		joint.distance = Vector2.Distance(transform.position, hook.position);
		joint.enabled = true;
		controller.onRope = true;
	}
	
	public void ShootRope()
	{
		joint.enabled = false;
		Vector3 direction;
		if (noMouse) {
			direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
		} else {
			Vector3 worldpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, Mathf.Abs(Camera.main.transform.position.z)));
			direction = (worldpoint - transform.position).normalized;
		}
		//Vector2 dir = new Vector2(direction.x,direction.y);
		//hook.transform.position = transform.position;
		//hook.position = transform.position+direction;
		hookScript.Shoot(transform.position, direction*hookSpeed);
//		hook.position = transform.position;
//		hookScript.SetSpeed(direction * hookSpeed);
	}

	public void ReleaseRope()
	{
		joint.enabled = false;
		controller.onRope = false;
		//	hook.rigidbody2D.isKinematic = false;
		hookScript.Release();
	}
}
