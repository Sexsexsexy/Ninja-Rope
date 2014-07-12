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
//	private static int segmentNumber;
	private static int currentDifficulty;
	// Use this for initialization
	void Start()
	{
		currentDifficulty = 1;
		foreach (Segment seg in segmentsToUse) {
			allSegments.Add(seg);
		}
		UpdateAvailableSegments();
//		segmentNumber = allSegments.Count;

		activeSegments.Add(startSegment);
		activeSegments.Add(startSegment);
		activeSegments.Add(RandomSegment());

		UpdateAvailableSegments();

		startSegment.transform.position = Vector2.zero;
		player.transform.position = startSegment.StartPosition;

		activeSegments [2].JoinSegmentFromRight(activeSegments [1].endJoint);
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
		Debug.Log("Available Segments : " + availableSegments.Count);
	}

	public static void PassedSegment()
	{
		//availableSegments.Add(activeSegments [0]);
		activeSegments.RemoveAt(0);
		UpdateAvailableSegments();
		activeSegments.Add(RandomSegment());
//		activeSegments [0] = activeSegments [1];
//		activeSegments [1] = activeSegments [2];
//		activeSegments [2] = RandomSegment();
		activeSegments [2].JoinSegmentFromRight(activeSegments [1].endJoint);
		currentDifficulty++;
		UpdateAvailableSegments();
	}

	private static Segment RandomSegment()
	{
		int number = (int)Random.Range(0, availableSegments.Count);
		availableSegments [number].Randomize();
		return availableSegments [number];
	}
}