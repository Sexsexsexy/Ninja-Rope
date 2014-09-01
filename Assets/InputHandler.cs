using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour
{

	public NewPlayerController controller;

	// Use this for initialization
	void Start()
	{
		controller = GetComponentInChildren<NewPlayerController>();
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
		Debug.Log("Unity Editor");
		#endif
		
		#if UNITY_IPHONE
		Debug.Log("Iphone");
		#endif
	}
}
