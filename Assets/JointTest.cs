using UnityEngine;
using System.Collections;

public class JointTest : MonoBehaviour {

	public Segment one;
	public Segment two;

	// Use this for initialization
	void Start () {
		one.JoinSegmentFromRight(two.endJoint);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
