/**
 * 
 * This is the controller class for applying bird behaviors to a bird object. It contains references to a decision 
 * tree as well as several other public variables.
 * 
 * @Author - Michael Sandt
 * @Version 1.1
 * @Date 2/28/2013
 **/

using UnityEngine;
using System.Collections;
using Pathfinding;
/// <summary>
/// Class for a bird object. Holds a reference to a decision tree and various helper methods. 
/// </summary>
public class BirdAI : MonoBehaviour
{
	//Perch object stores the available locations on which a bird can perch
	public GameObject PerchObj;
	//the center of the flyable area for the bird
	public Vector3 center;
	//use these values to manipulate the size of the flyable area for the bird
	public int xThresh, zThresh;
	//Seeker object from the pathfinder class, allows for access to the graph although this functionality is mostly deprecated
    private CharacterController controller;
    
    //The AI's speed per second
    public float speed = 10;
	
	//timers for state transitions
	private float timeFlying;
	private float timePerched;
	
	private ArrayList perches;
	private BirdDecisionTree tree;
	private Vector3 flyTarget;
	private Quaternion orig_orientation;
	private Animation animation;
	/// <summary>
	/// Initializes the necessary components for a the Bird AI to function
	/// </summary>
	void Start ()
	{
		//set up controller for movement and animations
        controller = GetComponent<CharacterController>();
		animation = GetComponentInChildren<Animation>();
		
		//get the perch object and set the birds list of perches
		perches = new ArrayList();
		PerchObj = GameObject.Find("Perches");	
		populatePerches (PerchObj.transform);
		
		//Maintaining a running time of time spent flying and perched for transitions between behaviors
		timeFlying = 0.0f;
		timePerched = 0.0f;
		//reference to decision tree
		//DecisionTree script = (DecisionTree)GameObject.Find("DecisionTree").GetComponent("DecisionTree");
		tree = new BirdDecisionTree();
		
		//keep track of originial orientation for reorientation
		orig_orientation = transform.rotation;
	}
	
	/// <summary>
	/// Traverses the decision tree returning the vector to be applied to the bird, stops and starts animations
	/// </summary>
	void Update ()
	{
		Vector3 dir = tree.makeDecision(this);
		if(isPerched()){
			animation.Stop();
		}else{
			animation.Play ();
		}
		controller.Move (dir);
	}
	
	/// <summary>
	/// Populates the arraylist of perches from perchobj's children
	/// </summary>
	/// <param name='parent'>
	/// The perchobj parent object. Used for access to its children
	/// </param>
	private void populatePerches(Transform parent){
		foreach(Transform child in parent){
			if(child != null)
				perches.Add(child);
		}
	}
	
	/** ----------- Decision Tree State Functions -------- **/
	/// <summary>
	/// Determines whether or not the bird is currently perched
	/// </summary>
	/// <returns>
	/// True if the bird has the same position as a perch, false otherwise
	/// </returns>
	public bool isPerched(){
		foreach(Transform perch in perches){
			if(Vector3.Distance(perch.position, transform.position) < 1.1)
				return true;
		}
		return false;
	}
	/// <summary>
	/// Determines if the bird is tires -- needs to perch
	/// </summary>
	/// <returns>
	/// True if the bird has been flying for longer than 20 seconds, false otherwise
	/// </returns>
	public bool isTired(){
		if(timeFlying > 20.0f)
			return true;
		else
			return false;
	}
	/// <summary>
	/// Determines if the bird is ready to fly again
	/// </summary>
	/// <returns>
	/// True if the bird has been perched for 10 seconds, false otherwise
	/// </returns>
	public bool isRested(){
		if(timePerched > 10.0f)
			return true;
		else
			return false;
	}
	/// <summary>
	/// Gets the amount of time the bird has been flying
	/// </summary>
	/// <returns>
	/// The flying time in seconds
	/// </returns>
	public float getFlyingTime(){
		return timeFlying;
	}
	/// <summary>
	/// Increments the flying time.
	/// </summary>
	public void incrementFlyingTime(){
		timeFlying += 1.0f * Time.deltaTime;
	}
	/// <summary>
	/// Increments the perched time.
	/// </summary>
	public void incrementPerchedTime(){
		timePerched += 1.0f * Time.deltaTime;
	}
	/// <summary>
	/// Resets the fly time.
	/// </summary>
	public void resetFlyTime(){
		timeFlying = 0.0f;
	}
	/// <summary>
	/// Resets the perch time.
	/// </summary>
	public void resetPerchTime(){
		timePerched = 0.0f;
	}
	/** ----------- End Decision Tree State Functions -------- s**/
	
	/** --------------- Flying Functions --------------------- **/
	/// <summary>
	/// Gets the perches.
	/// </summary>
	/// <returns>
	/// An array list of the game worlds perches
	/// </returns>
	public ArrayList getPerches(){
		return perches;
	}
	/// <summary>
	/// Gets the speed.
	/// </summary>
	/// <returns>
	/// The speed.
	/// </returns>
	public float getSpeed(){
		return speed;
	}
	/** -------------- End Flying Functions ----------------- **/
	
	
	/// <summary>
	/// Reorients the bird 
	/// </summary>
	public void reorient(){
		//transform.rotation.Set(orig_orientation.x,orig_orientation.y,
		//	orig_orientation.z,orig_orientation.w);
		transform.rotation = orig_orientation;
	}
	/// <summary>
	/// Gets the force to repel the bird from the edge of the map
	/// </summary>
	/// <returns>
	/// The normal force of the edge as a Vector3
	/// </returns>
	public Vector3 getEdgeForce(){
		if(transform.position.x > center.x + xThresh){
			return new Vector3(-1.0f,0,0);
		}
		else if(transform.position.x < center.x - xThresh){
			return new Vector3(1.0f,0,0);
		}
		else if(transform.position.z > center.z + zThresh){
			return new Vector3(0,0,-1.0f);
		}
		else if(transform.position.z < center.z - zThresh){
			return new Vector3(0,0,1.0f);
		}
		else{
			return new Vector3(0,0,0);
		}
	}

}
