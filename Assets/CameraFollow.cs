using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{

	public Transform player;
	// Use this for initialization
	void Start()
	{
		//transform.Translate( (transform.position.x - player.position.x )*Vector3.right);//-Vector3.forward+3*Vector3.up;
	}
	
	// Update is called once per frame
	void Update()
	{
		float xDiff = player.position.x - transform.position.x;
		if (player.position.x - transform.position.x>0) {
			transform.Translate(xDiff*Vector3.right);
		}
	}
}
