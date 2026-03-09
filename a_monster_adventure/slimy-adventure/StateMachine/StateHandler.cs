using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

[GlobalClass]
public partial class StateHandler : Node
{
	[Export]
	public bool Active {get; set;} = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (!Active) {
			SetProcess(false);
		}
	}

	private Array<State> activeStates = [];
	private Array<State> statesToDeactivate = [];
	private bool ExploreStateTree(Node currentNode)
	{
		foreach (Node child in currentNode.GetChildren())
		{
		
			StateCondition childStateCondition;

			if (child is StateCondition)
			{
				childStateCondition = child as StateCondition;
			}
			else if (child is StateHandlerLink){
				bool continueExploration = ExploreStateTree((child as StateHandlerLink).reference);

				if (!continueExploration) // Excluded rest of tree
				{
					return false;
				}

				continue;
			}
			else
			{
				continue;
			}
			if (childStateCondition.EvaluateStateCondition())
			{
				
				if (!(childStateCondition is State))
				{
					bool continueExploration = ExploreStateTree(childStateCondition);
					if (!continueExploration) // Excluded rest of tree
					{
						return false;
					}
				}
				else
				{
					State childState = childStateCondition as State;
					
					if (!childState.active)
						childState.Enter();
					
					activeStates.Add(childState);
					if (statesToDeactivate.Contains(childState)) 
						statesToDeactivate.Remove(childState);
				}
				
				if (childStateCondition.exclusivity == StateCondition.ExclusiveLevels.Branch)
				{
					break; // Excluded rest of branch
				}
				else if (childStateCondition.exclusivity == StateCondition.ExclusiveLevels.Tree)
				{
					return false; // Excluded rest of tree
				}
			}
		}

		return true;
	}

	public void DoStateMachine()
	{
		statesToDeactivate = activeStates.Duplicate();
		activeStates.Clear();

		ExploreStateTree(this);

		foreach (State stateToDeactivate in statesToDeactivate)
			if (stateToDeactivate.active) 
				stateToDeactivate.Exit();
	}

	public override void _Process(double delta)
	{
		if (Active) DoStateMachine();
	}

	public StateHandlerLink CreateLink()
	{
		StateHandlerLink newStateHandlerLink = new StateHandlerLink();
		newStateHandlerLink.reference = this;
		return newStateHandlerLink;
	}
}
