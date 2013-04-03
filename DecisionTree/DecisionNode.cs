/**
 * This is a generic decision node class for use in decision trees. Developed initially for use
 * with bird behavior ai it can be extended. It contains references to its true and false nodes
 * depending on the decision made at that particular node
 */

using UnityEngine;
using System.Collections;
/// <summary>
/// Generic Decision node for use in Bird AI Decision tree
/// </summary>
public class DecisionNode : TreeNode
{
	private TreeNode trueNeighbor;
	private TreeNode falseNeighbor;
	/// <summary>
	/// Initializes a new instance of the <see cref="DecisionNode"/> class.
	/// </summary>
	public DecisionNode(){
		trueNeighbor=null;
		falseNeighbor=null;
	}
	/// <summary>
	/// The make decision method for decision nodes simply returns the appropriate node
	/// calculated by get branch. Allowing for generic traversal of the tree
	/// </summary>
	/// <returns>
	/// Tree node corresponding to the appropriate decision
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller to make decision based on
	/// </param>
	public override TreeNode makeDecision(BirdAI bird){
		return getBranch (bird);
	}
	/// <summary>
	/// The get branch method returns the appropriate child node to return based on 
	/// game state data
	/// </summary>
	/// <returns>
	/// The child node corresponding to the appropriate decision
	/// </returns>
	/// <param name='bird'>
	/// Bird AI controller to make decision based on
	/// </param>
	public virtual TreeNode getBranch(BirdAI bird){
		return null;
	}
	/// <summary>
	/// Sets the true node of this decision node
	/// </summary>
	/// <param name='t'>
	/// The TreeNode object to set as the true node
	/// </param>
	public void setTrueNode(TreeNode t){
		this.trueNeighbor = t;
	}
	/// <summary>
	/// Sets the false node of this decision node.
	/// </summary>
	/// <param name='f'>
	/// The TreeNode object to set as the false node
	/// </param>
	public void setFalseNode(TreeNode f){
		this.falseNeighbor = f;
	}
	/// <summary>
	/// Gets the true node.
	/// </summary>
	/// <returns>
	/// The TreeNode corresponding to a true decision
	/// </returns>
	public TreeNode getTrueNode(){
		return this.trueNeighbor;
	}
	/// <summary>
	/// Gets the false node.
	/// </summary>
	/// <returns>
	/// The TreeNode corresponding to a false decision
	/// </returns>
	public TreeNode getFalseNode(){
		return this.falseNeighbor;
	}
	/// <summary>
	/// Gets the action for a Bird Controller.
	/// </summary>
	/// <returns>
	/// This returns no action and is just used for extensibility
	/// </returns>
	/// <param name='bird'>
	/// Bird AI object to apply the action to
	/// </param>
	public override Vector3 getAction(BirdAI bird){
		return new Vector3(0,0,0);
	}
}

