using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelHandler : MonoBehaviour
{
	public static List<Segment> allSegments = new List<Segment>();
	public static List<Segment> activeSegments = new List<Segment>();
	public static List<Segment> availableSegments = new List<Segment>();
	public StartSegment startSegment;
	public List<Segment> segmentsToUse;
	public Transform player;
	private static int currentDifficulty;

	// Use this for initialization
	void Awake()
	{
		currentDifficulty = 1;
		foreach (Segment seg in segmentsToUse) {
			allSegments.Add(seg);
		}
		UpdateAvailableSegments();

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

	public static void UpdateAvailableSegments()
	{
		availableSegments.Clear();
		foreach (Segment segment in allSegments) {
			if (activeSegments.Contains(segment)) {
			} else if (segment.difficulty > currentDifficulty) {
			} else {
				availableSegments.Add(segment);
			}
		}
	}

	public static void PassedSegment(Segment passedSegment)
	{
		Debug.Log(passedSegment);
		//availableSegments.Add(activeSegments [0]);
		if (passedSegment == activeSegments [1]) {
			activeSegments.RemoveAt(0);
			UpdateAvailableSegments();
			activeSegments.Add(RandomSegment());
			activeSegments [2].JoinSegmentFromRight(activeSegments [1].endJoint);
			currentDifficulty++;
			UpdateAvailableSegments();
			//make use of passedSegment.points or something
		}
	}

	private static Segment RandomSegment()
	{
		int number = (int)Random.Range(0, availableSegments.Count);
		availableSegments [number].Randomize();
		return availableSegments [number];
	}
}