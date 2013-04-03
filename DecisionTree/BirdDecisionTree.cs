/**
 * This class represents the decision tree for a bird object - only three levels deep
 * 
 * @Author - Mike Sandt
 * @Version 1.0
 * @Data 2/27/2013
 **/

using UnityEngine;
using System.Collections;
/// <summary>
/// Bird decision tree, alternating decision and action nodes
/// </summary>
public class BirdDecisionTree
{
	private DecisionNode root;
	// Use this for initialization
	/// <summary>
	/// Initializes a new instance of the <see cref="BirdDecisionTree"/> class.
	/// </summary>
	public BirdDecisionTree(){
		
		this.root = new PerchDecision();
		DecisionNode tired = new IsTiredDecision();
		DecisionNode rested = new IsRestedDecision();
		root.setTrueNode (rested);
		root.setFalseNode (tired);
		
		ActionNode fly = new FlyAction();
		ActionNode perch = new PerchAction();
		tired.setTrueNode(perch);
		tired.setFalseNode(fly);
		
		rested.setTrueNode(fly);
		rested.setFalseNode(perch);
	}
	/// <summary>
	/// This method traverses the decision tree returning the action node leaf the bird is to follow
	/// </summary>
	/// <returns>
	/// The vector to be applied to the bird
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller to apply the vector to
	/// </param>
	public Vector3 makeDecision(BirdAI bird){
		TreeNode act_node = root.makeDecision(bird);
		return act_node.getAction(bird);
	}
	
}

