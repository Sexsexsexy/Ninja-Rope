using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

	public Transform player;
	public Vector2 deadzone;
	public Vector2 playerOffset;
	public float xSmoothing;
	public float ySmoothing;// 0 doesnt move and 1 teleports

	// Use this for initialization
	void Start()
	{
		//transform.Translate( (transform.position.x - player.position.x )*Vector3.right);//-Vector3.forward+3*Vector3.up;
	}
	
	// Update is called once per frame
	void Update()
	{
		float xDiff = player.position.x+playerOffset.x - transform.position.x;
		float yDiff = player.position.y+playerOffset.y - transform.position.y;
		if (Mathf.Abs(xDiff) > deadzone.x) {
			transform.Translate(xDiff*xSmoothing*Vector3.right);
		}
		if (Mathf.Abs(yDiff) > deadzone.y) {
			transform.Translate(yDiff*ySmoothing*Vector3.up);
		}
//		if (player.position.x - transform.position.x>0) {
//			transform.Translate(xDiff*Vector3.right);
//		}
		//transform.position = player.position-2*Vector3.forward;
	}
}
