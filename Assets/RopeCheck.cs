using UnityEngine;
using System.Collections;

public class RopeCheck : MonoBehaviour
{

	public EdgeCollider2D col;
	private Hook hook;
	// Use this for initialization
	void Start()
	{
		hook = transform.parent.gameObject.GetComponentInChildren<Hook>();
	}
	void FixedUpdate()
	{
//		rigidbody2D.velocity = Vector2.zero;
//		rigidbody2D.position = new Vector2(0, 0);
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log(col.contacts.Length);
		hook.RopeHit(col, (Vector2)transform.position);
		transform.Translate(-transform.position);

	}
}
