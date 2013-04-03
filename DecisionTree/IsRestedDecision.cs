/**
 * 
 * This class is an instance of DecisionNode determining whether or not a bird is rested enough to fly
 * 
 * @Author - Mike Sandt
 * @Version 1.0
 * @Data 2/27/2013
 **/

using UnityEngine;
using System.Collections;
/// <summary>
/// Decision Node for determining whether or not a bird is rested
/// </summary>
public class IsRestedDecision : DecisionNode
{
	/// <summary>
	/// Initializes a new instance of the <see cref="IsRestedDecision"/> class.
	/// </summary>
	public IsRestedDecision():base(){
	}
	/// <summary>
	/// This method returns the appropriate branch according to whether or not the bird is rested
	/// </summary>
	/// <returns>
	/// The TreeNode corresponding to a true or false decision
	/// </returns>
	/// <param name='bird'>
	///  BirdAI controller to base the decision on
	/// </param>
	public override TreeNode getBranch(BirdAI bird){
		if(bird.isRested()){
			bird.resetFlyTime();
			bird.resetPerchTime();
			return getTrueNode().makeDecision(bird);
		}
		else{
			return getFalseNode().makeDecision (bird);
		}
	}
}

