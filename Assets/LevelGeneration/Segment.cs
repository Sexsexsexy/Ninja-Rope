using UnityEngine;
using System.Collections;

public abstract class Segment : MonoBehaviour
{
//	private Vector2 startPoint;
//	private Vector2 endPoint;
//
//	public Vector2 StartPoint { 
//		get { 
//			return startPoint; 
//		} 
//		private set { 
//			startPoint = value;
//		} 
//	}
//
//	public Vector2 EndPoint { 
//		get { 
//			return endPoint; 
//		} 
//		private set {
//			endPoint = value; 
//		}
//	}

	public SegmentJoint beginningJoint;
	public SegmentJoint endJoint;
	public int points;
	public int difficulty;
	
	public void JoinSegmentFromRight(SegmentJoint previousEnd){
		Vector2 dir = previousEnd.RightPoint - beginningJoint.LeftPoint;
		transform.Translate(dir,Space.World);
		Debug.Log(Vector2.Distance(previousEnd.RightPoint, beginningJoint.LeftPoint));
	}
	
	public abstract void Randomize();
}