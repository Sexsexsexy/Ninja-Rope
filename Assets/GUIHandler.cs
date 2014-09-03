using UnityEngine;
using System.Collections;

public class GUIHandler : MonoBehaviour
{

	// Use this for initialization
	void Start()
	{
	
	}
	
	// Update is called once per frame
	void Update()
	{
	
	}

	void OnGUI()
	{

		if (GUI.Button(new Rect(0, 0, Screen.width / 10, Screen.height / 15), "Reset")) {
			Application.LoadLevel(Application.loadedLevel);
		}
	}
}
