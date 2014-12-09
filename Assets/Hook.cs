using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hook : MonoBehaviour
{
	public float maxDistance;
	public RopeHandler shooter;
	public Transform player;
	public Vector2 anchorPoint;
	public float offset = 0.05f;

	private bool drawLine;
	private LineRenderer line;
	private List<Vector2> linePoints;
	private bool hooked;
	private List<Vector2> unwindLimits; // describes at which x-position (x) and in which direction sign (y) you can unwind
	private LayerMask groundMask;
	private EdgeCollider2D edge;
	private Vector2 newJoint;
	private Vector3 oldPos;

	// Use this for initialization
	void Start()
	{
		edge = transform.parent.gameObject.GetComponentInChildren<EdgeCollider2D>();
		edge.enabled = false;
		Debug.Log(edge);
		//edge.points=new Vector2[10];
		line = GetComponent<LineRenderer>();
		linePoints = new List<Vector2>();
		unwindLimits = new List<Vector2>();
		groundMask = LayerMask.GetMask("Ground");
	}
	
	// Update is called once per frame
	void Update()
	{
		if (drawLine) {
			UpdateLine();
			//UpdateEdge();
			if (!hooked && Vector2.Distance(player.position, transform.position) > maxDistance) {
				Release();//this may need rethinking since we have a springjoint now...
			}
			DrawLine();		
		}
		oldPos = player.position;
	}

	public void RopeHit(Collision2D col, Vector2 colliderOffset)
	{
		if (col.transform.CompareTag("Ground")) {
			ContactPoint2D contact = col.contacts [0];
			foreach (ContactPoint2D point in col.contacts) {
				if (point.point.y < contact.point.y) {
					contact = point;
				}
			}
			newJoint = contact.point - contact.normal * offset + colliderOffset;
		} else if (col.transform.CompareTag("Deadly")) {
			Release();
		}
	}

//	private void UpdateEdge(){
//		for(int i=)
//	}

	private void UpdateLine()
	{
		// if we are not yet hooked we want to move the line and the collider with the hook
		if (!hooked) {
			linePoints [0] = transform.TransformPoint(anchorPoint);
		}
		linePoints.RemoveAt(linePoints.Count - 1);
		Vector2 playerPoint = player.position;
		Vector2 currentJoint = linePoints [linePoints.Count - 1];


		// new solution using line collider!

		//if we have a new joint lets fix it!
		if (newJoint != Vector2.zero) {
			unwindLimits.Add(new Vector2(playerPoint.x, -player.rigidbody2D.velocity.x));
			float dist = -Vector2.Distance(currentJoint, newJoint);
			linePoints.Add(newJoint);
			shooter.MoveJoint(newJoint, dist);
			newJoint = Vector2.zero;
		}

		// then we check if we should unwind
		if (linePoints.Count > 1) {
			//new solution
			Vector2 from = playerPoint - currentJoint;
			Vector2 to = linePoints [linePoints.Count - 2] - currentJoint;
			float angle = CheckAngle(from, to);
			Debug.Log(angle);

			if (angle > 180) {
				Debug.Log("Time to unwind!");
				Vector2 oldJoint = linePoints [linePoints.Count - 2];
				float dist = Vector2.Distance(oldJoint, currentJoint);
				linePoints.RemoveAt(linePoints.Count - 1);
				shooter.MoveJoint(oldJoint, dist);
			}
			/*
			 * old solution
			if (unwindLimits.Count > 0) {
				Vector2 unwind = unwindLimits [unwindLimits.Count - 1];

				if ((unwind.y < 0 && playerPoint.x < unwind.x && player.rigidbody2D.velocity.x < 0)
					|| (unwind.y > 0 && playerPoint.x > unwind.x && player.rigidbody2D.velocity.x > 0)) {
					Debug.Log("Time to unwind!");
					Vector2 oldJoint = linePoints [linePoints.Count - 2];
					float dist = Vector2.Distance(oldJoint, currentJoint);
					linePoints.RemoveAt(linePoints.Count - 1);
					shooter.MoveJoint(oldJoint, dist);
				}
			}
*/
		}


		//now we add player position as the end point for the line and collider

		linePoints.Add(playerPoint);
		edge.points = linePoints.ToArray();
	}
/*
	private RaycastHit2D ArcCast(Vector2 from, Vector2 to, Vector2 center, float interpolation, LayerMask mask)
	{
		Vector2 current = Vector2.Lerp(from, to, interpolation);
		float accuracy = Vector2.Distance(from, current);
		RaycastHit2D hit = new RaycastHit2D();
		while (Vector2.Distance(current,to)>accuracy) {
		
			hit = Physics2D.Linecast(current, center, mask);
			if (hit.collider != null) {
				return hit;
			}
			current = Vector2.Lerp(current, to, interpolation);
		}
		return hit;
	}
*/

	//Returns the counterclockwise angle between two vectors (from first to second) 
	private float CheckAngle(Vector2 from, Vector2 toVec)
	{
		float ang = Vector2.Angle(from, toVec);
		Vector3 cross = Vector3.Cross(from, toVec);
		
		if (cross.z < 0)
			ang = 360 - ang;	
		return ang;
	}

	// This may need optimization
	private void DrawLine()
	{
		line.SetVertexCount(linePoints.Count);
		for (int i = 0; i<linePoints.Count; i++) {
			line.SetPosition(i, linePoints [i]);
		}
	}


	public void Shoot(Vector2 start, Vector2 force)
	{
		transform.position = start;
		transform.up = force;
		drawLine = true;
		linePoints.Clear();
		linePoints.Add(transform.position);
		linePoints.Add(player.position);
		DrawLine();
		line.enabled = true;
		edge.enabled = true;
		edge.points = linePoints.ToArray();
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.isKinematic = false;
		rigidbody2D.AddForce(force);
	}

	public void Release()
	{
		edge.enabled = false;
		hooked = false;
		drawLine = false;
		line.enabled = false;
		linePoints.Clear();
		line.SetVertexCount(0);
		transform.position = new Vector3(-100, 0, 0);
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (!col.transform.CompareTag("Player")) {
			UpdateLine();
			hooked = true;
			rigidbody2D.isKinematic = true;
			shooter.CreateJoint(transform.TransformPoint(anchorPoint));
		}
	}
}
