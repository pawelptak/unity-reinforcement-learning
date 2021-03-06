using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class RollerAgent : Agent
{
	[SerializeField] private TrackCheckpoints trackCheckpoints;
	private Vector3 startPosition;
	public float movingForce = 10;

	private Rigidbody rBody;
	private Checkpoint nextCheckpoint;
	private float initialTime;

	public override void Initialize()
    {
		initialTime = Time.time;
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
		startPosition = new Vector3(Random.Range(-4f, 4f), -7.03f, Random.Range(2f, 4f)); // simple lvl
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
		Vector3 checkpointPosition = Vector3.zero;


		if (nextCheckpoint != null){ 
		toNextCheckpoint = nextCheckpoint.transform.position - transform.position;
		checkpointForward = nextCheckpoint.transform.forward;
		}

		sensor.AddObservation(toNextCheckpoint);

		//float directionDot = Vector3.Dot(transform.forward, checkpointForward);
		//sensor.AddObservation(directionDot);

		//agents position
		//sensor.AddObservation(transform.position.normalized);

        //agents velocity
        //sensor.AddObservation(rBody.velocity.normalized);

        //distance to nearest checkpoint
        //sensor.AddObservation(toNextCheckpoint.magnitude);


        //// Target and Agent positions
        //sensor.AddObservation(nextCheckpoint.transform.position);
        //sensor.AddObservation(this.transform.localPosition);


    }

	public override void OnActionReceived(ActionBuffers actionBuffers)
	{
		// Actions, size = 2
		Vector3 controlSignal = Vector3.zero;
		controlSignal.x = actionBuffers.ContinuousActions[0];
		controlSignal.z = actionBuffers.ContinuousActions[1];
		rBody.AddForce(controlSignal * movingForce);


		if (nextCheckpoint == null)
        {
			Debug.Log($"Finished. Time: {Time.time - initialTime:0.00}s. Score: {GetCumulativeReward()}");
			initialTime = Time.time;
			EndEpisode();
        }

	}
	
	public override void Heuristic(in ActionBuffers actionsOut)
	{
		var continuousActionsOut = actionsOut.ContinuousActions;
		continuousActionsOut[0] = Input.GetAxis("Horizontal");
		continuousActionsOut[1] = Input.GetAxis("Vertical");
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
