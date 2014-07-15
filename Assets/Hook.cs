using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hook : MonoBehaviour
{
	public float maxDistance;
	public RopeShooter shooter;
	public Transform player;
	public Vector2 anchorPoint;
	private bool followPlayer;
	private bool drawLine;
	private LineRenderer line;
	private List<Vector2> linePoints;
	private bool hooked;
	private List<Vector2> unwindLimit; // describes at which x-position (x) and direction sign (y)you cant unwind
	private LayerMask groundMask;

	// Use this for initialization
	void Start()
	{
		followPlayer = true;
		line = GetComponentInChildren<LineRenderer>();
		linePoints = new List<Vector2>();
		groundMask = LayerMask.GetMask("Ground");
	}
	
	// Update is called once per frame
	void Update()
	{
		if (drawLine) {
			UpdateLine();
			if (!hooked && Vector2.Distance(player.position, transform.position) > maxDistance) {
				Release();//this may need rethinking since we have a springjoint now...
			}
			DrawLine();		
		}
		if (followPlayer) {
			transform.position = player.position;
		} 
	}

	private void UpdateLine()
	{
		if (!hooked) {
			linePoints [0] = transform.TransformPoint(anchorPoint);
		}
		linePoints.RemoveAt(linePoints.Count - 1);
		Vector2 playerPoint = player.position;
		Vector2 currentJoint = linePoints [linePoints.Count - 1];

		//Checking if there is anything between the player and the current joint. If there is we will move the joint to that position and divide our line.

		RaycastHit2D hit = Physics2D.Raycast(playerPoint, currentJoint - playerPoint, Vector2.Distance(playerPoint, currentJoint) - 0.05f, groundMask);

		if (hit.collider != null) {
			/*if there is something between the player and the joint  we move the joint to the rayhitposition and shorten the line.
			We also  add the point to the list so that a rope between the previous joint and the new one will be drawn*/
			float dist = -Vector2.Distance(currentJoint, hit.point);
			linePoints.Add(hit.point);
			shooter.MoveJoint(hit.point, dist);

		} else if (linePoints.Count > 1) {
			/*if there is nothing between and the joint is not the hook we must check if we should unwind the rope. 
			To do that we cast a ray between the player and the old joint*/

			Vector2 oldJoint = linePoints [linePoints.Count - 2];
			hit = Physics2D.Raycast(playerPoint, oldJoint - playerPoint, Vector2.Distance(playerPoint, oldJoint) - 0.05f, groundMask);

			if (hit.collider == null) {
				/* if there is nothing in the way we should probably unwind the rope. First we should jus check if the rope will move through something. 
				We check this approximately by casting several rays in an arc (between the current joint and the player) to a point between the current and the old joint.*/

				if (ArcCast(currentJoint, playerPoint, oldJoint + (playerPoint - oldJoint) * 0.01f, 0.2f, groundMask).collider == null) {
					float dist = Vector2.Distance(oldJoint, currentJoint);
					linePoints.RemoveAt(linePoints.Count - 1);
					shooter.MoveJoint(oldJoint, dist);
				}
			}
		}

		linePoints.Add(playerPoint);
	}

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

	private void DrawLine()
	{
		line.SetVertexCount(linePoints.Count);
		for (int i = 0; i<linePoints.Count; i++) {
			line.SetPosition(i, linePoints [i]);
		}

	}
	
	public void Shoot(Vector2 start, Vector2 force)
	{
		followPlayer = false;
		drawLine = true;
		linePoints.Clear();
		linePoints.Add(transform.position);
		linePoints.Add(player.position);

		DrawLine();
		line.enabled = true;

		transform.position = start;
		transform.up = force;
		rigidbody2D.isKinematic = false;
		rigidbody2D.AddForce(force);

	}

	public void Release()
	{
		hooked = false;
		followPlayer = true;
		drawLine = false;
		line.enabled = false;
		linePoints.Clear();
		line.SetVertexCount(0);
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		if (!col.transform.CompareTag("Player")) {
			UpdateLine();
			hooked = true;
			rigidbody2D.isKinematic = true;
			//velocity = Vector2.zero;
			shooter.CreateJoint(transform.TransformPoint(anchorPoint));

		}
	}
}
