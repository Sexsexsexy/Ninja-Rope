using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{

	public PlayerController controller;
	private bool pressed;
	private bool swiped;
	private Vector2 swipeBegin;
	// Use this for initialization
	void Start()
	{
		controller = GetComponentInChildren<PlayerController>();

	}
	
	// Update is called once per frame
	void Update()
	{

		#if UNITY_STANDALONE
		if (Input.GetKeyDown(KeyCode.Space)) 
			controller.Pressed();
		else if (Input.GetKeyUp(KeyCode.Space)) 
			controller.Released();

		if (Input.GetKeyDown(KeyCode.W))
			controller.Swipe(Vector2.right);
		

		#endif
	
		#if UNITY_ANDROID
		foreach (Touch touch in Input.touches) {
			if (touch.position.x < Screen.width / 2) {
				switch (touch.phase) {
					case TouchPhase.Began:
						controller.Pressed();
						break;
					case TouchPhase.Ended:
						controller.Released();
						break;
				}
			} else {
				switch (touch.phase) {
					case TouchPhase.Began:
						swipeBegin = touch.position;
						break;
					case TouchPhase.Ended:
						Vector2 delta = touch.position - swipeBegin;
						if (delta.sqrMagnitude > Screen.width * Screen.height / 100)
							controller.Swipe(delta.normalized);
						break;
				}
			}
		}
		#endif
		
		#if UNITY_IPHONE
		foreach (Touch touch in Input.touches) {
			if (touch.position.x < Screen.width / 2 && !pressed) {
				pressed = true;
				switch (touch.phase) {
					case TouchPhase.Began:
						controller.Pressed();
						break;
					case TouchPhase.Ended:
						controller.Released();
						break;
				}
			} else if (!swiped) {
				switch (touch.phase) {
					case TouchPhase.Began:
						swipeBegin = touch.position;
						break;
					case TouchPhase.Ended:
						Vector2 delta = touch.position - swipeBegin;
						if (delta.sqrMagnitude > Screen.width * Screen.height / 20)
							controller.Swipe(delta.normalized);
						break;
				}
				swiped = true;
			}
			if (swiped && pressed) {
				swiped = false;
				pressed = false;
				break;
			}
		}		
		#endif
	}
}
