using UnityEngine;
using System.Collections;

public class RopeHandler : MonoBehaviour
{
	public Transform hook;
	public float hookSpeed;
//	private bool noMouse;
	public float angle;
	private Hook hookScript;
	private SpringJoint2D joint;
	private PlayerController controller;
	private Vector2 shootDir;

	// Use this for initialization
	void Start()
	{
		angle = angle * Mathf.Deg2Rad;
		shootDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
		controller = GetComponent<PlayerController>();
		hookScript = hook.GetComponent<Hook>();
		hookScript.shooter = this;
		joint = GetComponent<SpringJoint2D>();
		joint.enabled = false;

	}

	public void SetNoMouse(bool state)
	{
		//	noMouse = state;
	}

	public void CreateJoint(Vector2 point)
	{
		joint.connectedAnchor = point;
		joint.distance = Vector2.Distance(transform.position, point);
		joint.enabled = true;
		controller.onRope = true;
	}

	public void MoveJoint(Vector2 point, float lengthDiff)
	{
		joint.connectedAnchor = point;
		joint.distance += lengthDiff;//Vector2.Distance(transform.position, point);
	}

	public void ShootRope()
	{
		joint.enabled = false;
		hookScript.Shoot(transform.position, shootDir * hookSpeed);
	}

	public void ReleaseRope()
	{
		joint.enabled = false;
		controller.onRope = false;
		//	hook.rigidbody2D.isKinematic = false;
		hookScript.Release();
	}
}
