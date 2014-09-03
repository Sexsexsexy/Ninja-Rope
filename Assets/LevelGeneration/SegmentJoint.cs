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
			return point;
		}
	}

	public Vector2 RightPoint {
		get {
			Vector3 point = transform.position + box.bounds.extents.x * Vector3.right;
			return point;
		}
	}

	void Awake()
	{
		box = GetComponent<BoxCollider2D>();
		parent = gameObject.GetComponentInParent<Segment>();
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (PlayerTrigger && col.tag == "Player") {
			LevelHandler.PassedSegment(parent);
		}
	}
}

