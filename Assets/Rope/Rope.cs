using UnityEngine;
using System.Collections;

public class Rope : MonoBehaviour {

	public Sprite hook;
	public Sprite rope;
	private float hooklength;
	private float ropelength;
	public float overlap;
	// Use this for initialization
	void Start () {
		hooklength = hook.bounds.size.x/100;
		ropelength = rope.bounds.size.x/100;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void CreateRope(Vector2 start, Vector2 end){
		float dist = Vector2.Distance (start, end);
		int numberOfPieces = (int)((dist - hooklength+overlap) / (ropelength-overlap));
		for (int i=0; i<numberOfPieces; i++) {
					
		}
	}
}
