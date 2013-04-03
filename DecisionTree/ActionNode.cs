/**
 * This is a generic action node used in a decision tree. Initially developed for bird behavior
 * it can be extended for future use in other examples.
 * 
 * @Author - Mike Sandt
 * @Version 1.0
 * @Data 2/27/2013
 **/

using UnityEngine;
using System.Collections;
/// <summary>
/// Generic action node class for a bird decision tree
/// </summary>
public class ActionNode : TreeNode
{

	public ActionNode():base(){
	}
	/// <summary>
	/// Generic make decision method
	/// </summary>
	/// <returns>
	/// All action nodes return themselves
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller to apply the action to
	/// </param>
	public override TreeNode makeDecision(BirdAI bird){
		return this;
	}
	/// <summary>
	/// Gets the appropriate action for the bird object.
	/// </summary>
	/// <returns>
	/// The vector corresponding to the movement action -- can be extended to support other types
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller to apply the action to
	/// </param>
	public override Vector3 getAction(BirdAI bird){
		return new Vector3(0,0,0);
	}
	
	
	
	
}

