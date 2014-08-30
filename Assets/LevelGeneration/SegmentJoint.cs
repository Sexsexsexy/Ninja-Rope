using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BoxCollider2D))]

public class SegmentJoint : MonoBehaviour
{
	private BoxCollider2D box;
	private Segment parent;

	public BoxCollider2D Box {
		get {
			return box;
		}
	}

	public bool PlayerTrigger;

	public Vector2 LeftPoint {
		get {
			Vector3 point = transform.position - box.bounds.extents.x * Vector3.right;
			return point;//transform.TransformPoint(point);
		}
	}

	public Vector2 RightPoint {
		get {
			Vector3 point = transform.position + box.bounds.extents.x * Vector3.right;
			return point;// transform.TransformPoint(point);
		}
	}

	void Awake()
	{
		//box = FindObjectOfType<BoxCollider2D>();
		box = GetComponent<BoxCollider2D> ();
		parent = gameObject.GetComponentInParent<Segment>();
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log("trigged by " + col.tag);
//		if (col.CompareTag("Player") && PlayerTrigger) {
		if (PlayerTrigger && col.tag=="Player") {
			LevelHandler.PassedSegment(parent);
			//box.enabled=false;
		}
	}
}

