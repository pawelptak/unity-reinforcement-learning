using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class RollerAgent : Agent
{
	[SerializeField] private TrackCheckpoints trackCheckpoints;
	//public Transform Target;
	private Vector3 startPosition;
	public float movingForce = 10;

	private Rigidbody rBody;
	private Checkpoint nextCheckpoint;

    public override void Initialize()
    {
		rBody = GetComponent<Rigidbody>();
		trackCheckpoints.OnCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
		trackCheckpoints.OnWrongCheckpoint += TrackCheckpoints_OnCarWrongCheckpoint;
	}

	private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, System.EventArgs e)
    {
		AddReward(1f);
		Debug.Log("Correct checkpoint. Reward++");
	}

	private void TrackCheckpoints_OnCarWrongCheckpoint(object sender, System.EventArgs e)
	{
		AddReward(-1f);
		Debug.Log("Wrong checkpoint. Reward--");
	}


    public override void OnEpisodeBegin()
    {
		startPosition = new Vector3(Random.Range(-4f, 4f), -7f, Random.Range(2f, 4f)); // simple lvl
		//startPosition = new Vector3(Random.Range(-9f, -11f), 0, Random.Range(-5f, 4f)); // complicated lvl
		
		
		rBody.angularVelocity = Vector3.zero;
		rBody.velocity = Vector3.zero;
		transform.localPosition = startPosition;
		trackCheckpoints.ResetCheckpoints();

    }
	
	public override void CollectObservations(VectorSensor sensor)
	{
		Vector3 toNextCheckpoint = Vector3.zero;
		nextCheckpoint = trackCheckpoints.GetNextCheckpoint();

		Vector3 checkpointForward = Vector3.zero;

		//vector to the next checkpoint
		if (nextCheckpoint != null)
        {
			toNextCheckpoint = nextCheckpoint.transform.position - transform.position;
			checkpointForward = nextCheckpoint.transform.forward;
		}

		sensor.AddObservation(toNextCheckpoint.normalized);

		float directionDot = Vector3.Dot(transform.forward, checkpointForward);
		sensor.AddObservation(directionDot);
		//Debug.Log(directionDot);


		//distance to nearest checkpoint
		//sensor.AddObservation(toNextCheckpoint.magnitude);


		//// Target and Agent positions
		//sensor.AddObservation(nextCheckpoint.transform.position);
		//sensor.AddObservation(this.transform.localPosition);

		//// Agent velocity
		//sensor.AddObservation(rBody.velocity.x);
		//sensor.AddObservation(rBody.velocity.z);
	}


	public void MoveAgent(ActionBuffers actionBuffers)
	{
		var dirToGo = Vector3.zero;
		var rotateDir = Vector3.zero;


		int movement = actionBuffers.DiscreteActions[0];
		// Get the action index for jumping
		int rotation = actionBuffers.DiscreteActions[1];
		//var action = act[0];

		switch (movement)
		{
			case 1:
				dirToGo = transform.forward * 1f;
				break;
			case 2:
				dirToGo = transform.forward * -1f;
				break;
			case 3:
				dirToGo = transform.right * -0.75f;
				break;
			case 4:
				dirToGo = transform.right * 0.75f;
				break;
		}

		switch (rotation)
		{
			case 1:
				rotateDir = transform.up * 1f;
				break;
			case 2:
				rotateDir = transform.up * -1f;
				break;
		}

		transform.Rotate(rotateDir, Time.fixedDeltaTime * 200f);
		rBody.AddForce(dirToGo * movingForce,
			ForceMode.VelocityChange);
	}

	public override void OnActionReceived(ActionBuffers actionBuffers)
	{
		// Actions, size = 2
		//Vector3 controlSignal = Vector3.zero;
		//controlSignal.x = actionBuffers.ContinuousActions[0];
		//controlSignal.z = actionBuffers.ContinuousActions[1];
		//rBody.AddForce(controlSignal * movingForce);
		MoveAgent(actionBuffers);

		if (nextCheckpoint == null)
        {
			Debug.Log("Track finished.");
			EndEpisode();
        }

	}
	
	public override void Heuristic(in ActionBuffers actionsOut)
	{
		//var continuousActionsOut = actionsOut.ContinuousActions;
		//continuousActionsOut[0] = Input.GetAxis("Horizontal");
		//continuousActionsOut[1] = Input.GetAxis("Vertical");
		var discreteActionsOut = actionsOut.DiscreteActions;
		if (Input.GetKey(KeyCode.D))
		{
			discreteActionsOut[1] = 1;
		}
		else if (Input.GetKey(KeyCode.A))
		{
			discreteActionsOut[1] = 2;
		}

		if (Input.GetKey(KeyCode.W))
		{
			discreteActionsOut[0] = 1;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			discreteActionsOut[0] = 2;
		}


	}
	
	//private void OnTriggerEnter(Collider other){
	//	if (other.TryGetComponent<Checkpoint>(out Checkpoint checkpoint)){
	//		AddReward(1f);
	//		//EndEpisode();
	//	}
	//}
	
	private void OnCollisionEnter(Collision collision){
		if (collision.gameObject.TryGetComponent(out Wall wal)){
			AddReward(-.5f);
			Debug.Log("Collision enter. Reward--");
			//EndEpisode();
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (collision.gameObject.TryGetComponent(out Wall wal))
		{
			AddReward(-.1f);
			Debug.Log("Collistion stay. Reward--");
		}
	}


}
