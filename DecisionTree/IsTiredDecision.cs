/**
 * This class is an extension of a decision Node determining whether the bird object has flown sufficiently 
 * long to be tired.
 * 
 * @Author - Mike Sandt
 * @Version 1.0
 * @Data 2/27/2013
 **/ 

using UnityEngine;
using System.Collections;
/// <summary>
/// DecisionNode for determing whether or not a bird is tired
/// </summary>
public class IsTiredDecision : DecisionNode
{
	public IsTiredDecision():base(){
	}
	/// <summary>
	/// Gets the branch corresponding to the appropriate action to take given a bird is tired or not
	/// </summary>
	/// <returns>
	/// The tree node corresponding to the appropriate decision
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller to apply to
	/// </param>
	public override TreeNode getBranch(BirdAI bird){
		//if bird is tired set the action to perch
		if(bird.isTired ()){
			return getTrueNode().makeDecision(bird);
		}
		else{
			return getFalseNode().makeDecision(bird);
		}
	}
}

