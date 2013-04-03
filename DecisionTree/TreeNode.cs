/**
 * Generic tree node class for decision trees. Methods currently only accept BirdAI objects
 * as the tree was originally built for Bird behavior. Should be easy enough to extend to more generic types
 * 
 * @Author - Mike Sandt
 * @Version 1.0
 * @Data 2/27/2013
 * 
 **/


using UnityEngine;
using System.Collections;
/// <summary>
/// Generic TreeNode class used for extension in decision trees
/// </summary>
public abstract class TreeNode {
	/// <summary>
	/// Initializes a new instance of the <see cref="TreeNode"/> class.
	/// </summary>
	public TreeNode(){
	}
	/// <summary>
	/// Makes a decision based on information obtained from parameters
	/// </summary>
	/// <returns>
	/// The TreeNode corresponding to the appropriate decision
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller to base decision on
	/// </param>
	public abstract TreeNode makeDecision(BirdAI bird);
	/// <summary>
	/// Gets the action for a particular object
	/// </summary>
	/// <returns>
	/// The action vector to apply to the object
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller to apply vector to
	/// </param>
	public abstract Vector3 getAction(BirdAI bird);
}
