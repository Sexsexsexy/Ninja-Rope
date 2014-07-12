using UnityEngine;
using System.Collections;

public class StartSegment : Segment
{	
	public Vector2 localStartPoint;

	public Vector2 StartPosition{
		get{
			return transform.TransformPoint(localStartPoint);
		}
	}
	
//	public void PlacePlayer(Transform player){
//		player.position = transform.TransformPoint(localStartPoint);
//	}

	public override void Randomize(){}


}