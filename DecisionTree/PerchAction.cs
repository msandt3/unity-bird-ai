/**
 * 
 * This is an instance of ActionNode determining the next action to be taken when a bird needs to perch
 * 
 * @Author - Mike Sandt
 * @Version 1.0
 * @Data 2/27/2013
 **/

using UnityEngine;
using System.Collections;
/// <summary>
/// Action for determining the appropriate vector to apply when a bird needs to perch
/// </summary>
public class PerchAction : ActionNode
{
	private float _wanderSide,_wanderUp,_maxLatitudeSide,_maxLatitudeUp,_smoothRate;
	public PerchAction():base(){
		//vars for determining the smoothness of the steering motion
		//higher values will produce more erratic movement
		_wanderSide = 0.0f;
		_wanderUp = 0.0f;
		_maxLatitudeSide = 0.75f;
		_maxLatitudeUp = 1f;
		_smoothRate = 0.0025f;
	}
	/// <summary>
	/// Gets the action vector based on decision criteria
	/// </summary>
	/// <returns>
	/// Either a vector moving a bird towards a perch, or doing nothing
	/// </returns>
	/// <param name='bird'>
	/// A reference to a BirdAI controller
	/// </param>
	public override Vector3 getAction(BirdAI bird){
		//if the bird is already perched do nothing
		if(bird.isPerched()){
			bird.incrementPerchedTime();
			bird.reorient();
			return new Vector3(0,0,0);
		}
		
		Transform closest_perch = getClosestPerch(bird);
		float dist = Vector3.Distance(bird.transform.position,closest_perch.position);
		Vector3 dir = calculateDownwardSteerForce(bird) + bird.getEdgeForce()
			+ ((dist/Mathf.Pow(dist,2.7f))*GetSeekVector(closest_perch.position,bird.transform.position));
		/**
		if(Mathf.Abs(bird.transform.position.y - closest_perch.position.y) > 5){
			dir = calculateDownwardSteerForce(bird) + (0.0001f * GetSeekVector(closest_perch.position,bird.transform.position));
		}
		else{
			//make sure the bird is looking at where its going
			dir = GetSeekVector(closest_perch.position,bird.transform.position);
			
		}
		**/
		dir = dir.normalized;
		dir *= bird.getSpeed() * Time.fixedDeltaTime;
		Debug.Log("Applying vector -- "+dir);
		Debug.Log ("Distance to perch -- "+Vector3.Distance(bird.transform.position,closest_perch.position));
		Debug.Log ("KEpsilon -- "+Vector3.kEpsilon);
		bird.transform.LookAt(bird.transform.position+dir);
		return dir;
		
		//return the closest one
		
	}
	/// <summary>
	/// Gets the closest perch.
	/// </summary>
	/// <returns>
	/// The closest perch.
	/// </returns>
	/// <param name='bird'>
	/// The Bird AI controller to apply this method to
	/// </param>
	private Transform getClosestPerch(BirdAI bird){
		//otherwise calculate the closest perch
		ArrayList perches = bird.getPerches();
		Transform closest_perch = (Transform)perches[0];
		float min_dist = float.MaxValue;
		//iterate through list of perches
		foreach(Transform perch in perches){
			if(Vector3.Distance(bird.transform.position,perch.position) < min_dist){
				min_dist = Vector3.Distance(bird.transform.position,perch.position);
				closest_perch = perch;
			}
		}
		
		return closest_perch;
	}
	/// <summary>
	/// Calculates a steering force in the negative Y direction. Used for more 
	/// believable movement
	/// </summary>
	/// <returns>
	/// The downward steer force.
	/// </returns>
	/// <param name='bird'>
	/// Bird.
	/// </param>
	private Vector3 calculateDownwardSteerForce(BirdAI bird){
		float speed = bird.getSpeed();
		//pick a random scalar to apply to steering vectors
		var randomSide = scalarRandomWalk(_wanderSide, speed, -_maxLatitudeSide, _maxLatitudeSide);
        var randomDown = scalarRandomWalk(_wanderUp, speed, -_maxLatitudeUp, _maxLatitudeUp);
		//blend the random scalar into a force accumulator so as to achieve smooth movement
		_wanderSide = blendIntoAccumulator(_smoothRate, randomSide, _wanderSide);
        _wanderUp = blendIntoAccumulator(_smoothRate*0.25f, randomDown, _wanderUp);
		
		Vector3 result = (bird.transform.right * _wanderSide) + (bird.transform.up * -0.1f * Mathf.Abs(_wanderUp))
			+ bird.transform.forward;
		return result;
	}
	/// <summary>
	/// Performs a random walk on an invisible circle in front of the object
	/// </summary>
	/// <returns>
	/// A scalar to be applied to a force vector
	/// </returns>
	/// <param name='initial'>
	/// Initial 'wander' value determining how erratic the movement will be
	/// </param>
	/// <param name='walkspeed'>
	/// The bird objects speed
	/// </param>
	/// <param name='min'>
	/// Maximum deviation in the negative direction allowed
	/// </param>
	/// <param name='max'>
	/// Maximum deviation in the positive direction allowed
	/// </param>
	private float scalarRandomWalk (float initial, float walkspeed, float min, float max)
	{
		float next = initial + ((UnityEngine.Random.value * 2 - 1) * walkspeed);
		next = Mathf.Clamp(next, min, max);
		return next;
	}
	/// <summary>
	/// Blends a scalar into an accumulator
	/// </summary>
	/// <returns>
	/// The into accumulator.
	/// </returns>
	/// <param name='smoothRate'>
	/// The rate at which scalar will be smoothed
	/// </param>
	/// <param name='newValue'>
	/// The recently calculated scalar
	/// </param>
	/// <param name='smoothedAccumulator'>
	/// The old smoothed accumulator, so as not to change too quickly
	/// </param>
	public static float blendIntoAccumulator(float smoothRate, float newValue, float smoothedAccumulator)
    {
        return Mathf.Lerp(smoothedAccumulator, newValue, Mathf.Clamp(smoothRate, 0, 1));
    }
	
	/// <summary>
	/// Gets a vector seeking a particular target
	/// </summary>
	/// <returns>
	/// The vector between the object position and the target
	/// </returns>
	/// <param name='target'>
	/// Target positon
	/// </param>
	/// <param name='Position'>
	/// Object position
	/// </param>
	public Vector3 GetSeekVector(Vector3 target, Vector3 Position)
	{
		/*
		 * First off, we calculate how far we are from the target, If this
		 * distance is smaller than the configured vehicle radius, we tell
		 * the vehicle to stop.
		 */
		Vector3 force = Vector3.zero;

		var difference = target - Position;
        float d = difference.sqrMagnitude;
        if (d > 1.1)
		{
			/*
			 * But suppose we still have some distance to go. The first step
			 * then would be calculating the steering force necessary to orient
			 * ourselves to and walk to that point.
			 * 
			 * It doesn't apply the steering itself, simply returns the value so
			 * we can continue operating on it.
			 */
			force = difference;
		}
		return force;
	}
	
}

