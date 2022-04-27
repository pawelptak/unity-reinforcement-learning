using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TrackCheckpoints : MonoBehaviour
{
	private List<Checkpoint> checkpointList;
	private int nextCheckpointIndex;

	public event EventHandler OnCorrectCheckpoint;
	public event EventHandler OnWrongCheckpoint;

	private void Awake(){
		Transform checkpointsTransform = transform.Find("Checkpoints");
		checkpointList = new List<Checkpoint>();
		
		foreach (Transform checkpointTransform in checkpointsTransform){
			Checkpoint cp = checkpointTransform.GetComponent<Checkpoint>();
			cp.SetTrackCheckpoints(this);
			checkpointList.Add(cp);
		}
		
		nextCheckpointIndex = 0;
	}
	
	public void PlayerThroughCheckpoint(Checkpoint checkpoint){
		if (checkpointList.IndexOf(checkpoint) == nextCheckpointIndex){
			// correct checkpoint
			nextCheckpointIndex++;
			//Debug.Log("Correct checkpoint");
			OnCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
		} else {
			// wrong checkpoint
			//Debug.Log("Wrong checkpoint");
			OnWrongCheckpoint?.Invoke(this, EventArgs.Empty);
		}
	}

	public void ResetCheckpoints()
    {
		nextCheckpointIndex = 0;

	}

	public Checkpoint GetNextCheckpoint()
	{
		if (nextCheckpointIndex < checkpointList.Count)
		{
			return checkpointList[nextCheckpointIndex];
		}
		else return null;
	}
		
}
