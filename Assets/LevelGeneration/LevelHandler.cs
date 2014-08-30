using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour
{
	public static Dictionary<Difficulty, List<Segment>> AllSegments = new Dictionary<Difficulty, List<Segment>>();

	public static List<Segment> allSegments = new List<Segment>();
	public static List<Segment> activeSegments = new List<Segment>();
	public static List<Segment> availableSegments = new List<Segment>();
	public StartSegment startSegment;
	public List<Segment> segmentsToUse;
	public Transform player;
	private static int passedSegments;
	private static Difficulty currentDifficulty;

	// Use this for initialization
	void Awake()
	{
		passedSegments = 1;

		currentDifficulty = Difficulty.Beginner;

		int DiffLevels = System.Enum.GetValues (typeof(Difficulty)).Length;

		for (int i = 0; i < DiffLevels; i++) {
			AllSegments.Add((Difficulty)i, new List<Segment>());
		}

		for (int j = 0; j < segmentsToUse.Count; j++) {
			Segment seg = segmentsToUse[j];
			AllSegments[seg.difficulty].Add(seg);
		}

		
		FillAvailableSegments();

		activeSegments.Add(startSegment);
		activeSegments.Add(startSegment);
		activeSegments.Add(startSegment);

	}

	void Start()
	{
		startSegment.transform.position = Vector2.zero;
		player.transform.position = startSegment.StartPosition;
		PassedSegment(activeSegments[1]);
	}
	
	// Update is called once per frame
	void Update()
	{
		
	}

	private static void FillAvailableSegments(){
		availableSegments.Clear ();

		foreach (var item in AllSegments[currentDifficulty]) {
			availableSegments.Add(item);
		}

		foreach (var item in AllSegments[Difficulty.None]) {
			availableSegments.Add(item);
		}
	}
		
	public static void PassedSegment(Segment passedSegment)
	{
		Debug.Log(passedSegment);
		//availableSegments.Add(activeSegments [0]);
		if (passedSegment == activeSegments [1]) { //needed so that you cant pass the same segment twice (and only the middle segment)
			Segment seg = activeSegments[0];
			
			if(seg.difficulty == currentDifficulty )
				availableSegments.Add(seg); //  ADD CHECK FOR DIFFICULTY WHEN SEGMENTS HAVE DIFFICULTY

			activeSegments.Add(RandomSegment());

			activeSegments.Remove(seg);
			activeSegments [2].JoinSegmentFromRight(activeSegments[1].endJoint);
			passedSegments++;
			//make use of passedSegment.points or something
		}
	}

	private static Segment RandomSegment()
	{
        int segments = availableSegments.Count;
        int number = Random.Range(0, segments);

		Debug.Log ("Random Number:" + number);
		Debug.Log ("Available Seg:" + segments);

        Segment seg = availableSegments[number];
        seg.Randomize();
        availableSegments.Remove(seg);

		return seg;
	}


}

public enum Difficulty { None, Beginner, Easy, Medium, Hard, Extreme}
