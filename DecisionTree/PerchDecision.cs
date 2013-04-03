/**
 * This class provides functionality for determining whether or not a bird is perched. Calling the birds is perched
 * method. It simply references the birds pos against the perches.
 * 
 * @Author - Mike Sandt
 * @Version 1.0
 * @Data 2/27/2013
 **/

using UnityEngine;
using System.Collections;
/// <summary>
/// Class for determining whether or not a bird needs to perch
/// </summary>
public class PerchDecision : DecisionNode
{
	/// <summary>
	/// Gets the branch corresponding to the appropriate action given the bird is perched
	/// </summary>
	/// <returns>
	/// The node of the tree corresponding to the appropriate decision
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller to apply to
	/// </param>
	public override TreeNode getBranch(BirdAI bird){
		//if bird is tired set the action to perch
		if(bird.isPerched()){
			return getTrueNode().makeDecision(bird);
		}
		else{
			return getFalseNode().makeDecision (bird);
		}
	}
}

