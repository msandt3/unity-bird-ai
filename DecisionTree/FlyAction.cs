/**
 * 
 * This is an instance of ActionNode representing the aciton taken when a bird needs to fly as determined
 * by upstream decision node
 * 
 * @Author - Mike Sandt
 * @Version 1.0
 * @Data 2/27/2013
 **/

using UnityEngine;
using System.Collections;
using Pathfinding;
/// <summary>
/// Class used for determining the action to take given that the bird is flying
/// </summary>
public class FlyAction : ActionNode {
	private float _wanderSide,_wanderUp,_maxLatitudeSide,_maxLatitudeUp,_smoothRate;
	/// <summary>
	/// Initializes a new instance of the <see cref="FlyAction"/> class.
	/// </summary>
	public FlyAction():base(){
		_wanderSide = 0.0f;
		_wanderUp = 0.0f;
		_maxLatitudeSide = 0.75f;
		_maxLatitudeUp = 1f;
		_smoothRate = 0.0025f;
	}
	/// <summary>
	/// Gets the appropriate action for a flying bird
	/// </summary>
	/// <returns>
	/// The action vector to apply to the character controller
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller
	/// </param>
	public override Vector3 getAction(BirdAI bird){
		//if the bird isnt at graph height yet we need to get it there
		if( (20.0f - bird.transform.position.y ) > Vector3.kEpsilon ){
			//Debug.Log ("Moving bird to graph");
			Vector3 up = calculateUpwardSteerForce(bird);
			bird.transform.LookAt(bird.transform.position+up);
			Vector3 dir = up.normalized;
			dir *= bird.getSpeed() * Time.fixedDeltaTime;
			return dir;
		}
		else if((20.0f - bird.transform.position.y) < Vector3.kEpsilon && bird.getFlyingTime() == 0.0f){
			//Debug.Log ("Reached graph, set orientation of bird");
			//Debug.Log (bird.transform);
			//bird.setRandomFlyTarget();
			bird.incrementFlyingTime();
			bird.reorient();
			return new Vector3(0,0,0);
			
		}
		else{
			bird.incrementFlyingTime();
			//Debug.Log("Edge force"+edge);
			Vector3 dir = calculateSteerForce(bird) + bird.getEdgeForce();
			//Vector3 dir = calculateSteerForce(bird) + bird.getEdgeForce();
			dir = dir.normalized;
			//Debug.Log ("Steering vector is"+dir.ToString());
			dir *= bird.getSpeed () * Time.fixedDeltaTime;
			bird.transform.LookAt(bird.transform.position+dir);
			Debug.Log ("Applying vector "+dir.ToString());
        	//dir *= bird.getSpeed() * Time.fixedDeltaTime;
			return dir;
		}
	}
	/// <summary>
	/// Calculates the upward steer force to be applied to a bird in flight
	/// </summary>
	/// <returns>
	/// The vector corresponding to the upward steering force to be applied
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller to apply this to
	/// </param>
	private Vector3 calculateUpwardSteerForce(BirdAI bird){
		float speed = bird.getSpeed();
		var randomSide = scalarRandomWalk(_wanderSide, speed, -_maxLatitudeSide, _maxLatitudeSide);
        var randomUp = scalarRandomWalk(_wanderUp, speed, -_maxLatitudeUp, _maxLatitudeUp);
		_wanderSide = blendIntoAccumulator(_smoothRate*0.0001f, randomSide, _wanderSide);
        _wanderUp = blendIntoAccumulator(_smoothRate*0.0001f, randomUp, _wanderUp);
		
		Vector3 result = (bird.transform.right * _wanderSide) + (bird.transform.up * 0.15f * Mathf.Abs(_wanderUp))
			+ bird.transform.forward * 1.25f;
		
		return result;
	}
	/// <summary>
	/// Calculates the horizontal steering force for a bird
	/// </summary>
	/// <returns>
	/// Vector corresponding to a horizontal wandering force
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller to apply this to
	/// </param>
	private Vector3 calculateSteerForce(BirdAI bird){
		float speed = bird.getSpeed();
		var randomSide = scalarRandomWalk(_wanderSide, speed, -_maxLatitudeSide, _maxLatitudeSide);
        var randomUp = scalarRandomWalk(_wanderUp, speed, -_maxLatitudeUp, _maxLatitudeUp);
		_wanderSide = blendIntoAccumulator(_smoothRate, randomSide, _wanderSide);
        _wanderUp = blendIntoAccumulator(_smoothRate, randomUp, _wanderUp);
		
		Vector3	 result = (bird.transform.right * _wanderSide) 
			+ bird.transform.forward*1.25f;
		
		return result;
	}
	/// <summary>
	/// Perform a random walk on an invisible circle
	/// </summary>
	/// <returns>
	/// The float to scale the force vector by
	/// </returns>
	/// <param name='initial'>
	/// The initial scalar
	/// </param>
	/// <param name='walkspeed'>
	/// The speed at which to perform the walk
	/// </param>
	/// <param name='min'>
	/// Maximum allowable deviation in the negative direction
	/// </param>
	/// <param name='max'>
	/// Maximum allowable deviation in the positive direction
	/// </param>
	private float scalarRandomWalk (float initial, float walkspeed, float min, float max)
	{
		float next = initial + ((UnityEngine.Random.value * 2 - 1) * walkspeed);
		next = Mathf.Clamp(next, min, max);
		return next;
	}
	/// <summary>
	/// Blends a scalar into accumulator.
	/// </summary>
	/// <returns>
	/// The scalar once blended
	/// </returns>
	/// <param name='smoothRate'>
	/// The smoothing rate to apply to the interpolation method
	/// </param>
	/// <param name='newValue'>
	/// The scalar to be blended into the accumulator
	/// </param>
	/// <param name='smoothedAccumulator'>
	/// The previous smoothed scalar value
	/// </param>
	public static float blendIntoAccumulator(float smoothRate, float newValue, float smoothedAccumulator)
    {
        return Mathf.Lerp(smoothedAccumulator, newValue, Mathf.Clamp(smoothRate, 0, 1));
    }
}
