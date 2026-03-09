using Godot;
using System;

[GlobalClass]
public partial class Ally : Character
{

	[Export]
	public Player player {get; set;} = null;
	[Export]
	public StateHandler abilityStateBranch {get; set;} = null;
	public StateHandlerLink abilityStateBranchLink = null;

	public Player getPlayer() {return player;}

	public StateHandlerLink createNewStateBranchLink()
	{
		if (abilityStateBranch == null) 
			return null;
		if (abilityStateBranchLink != null)
			abilityStateBranchLink.QueueFree();
		
		abilityStateBranchLink = new StateHandlerLink();
		abilityStateBranchLink.reference = abilityStateBranch;
		return abilityStateBranchLink;
	}
	
	public enum AllyStates
	{
		Idle = 1 << 1,
		Follow = 1 << 2,
		Flee = 1 << 3,
	}
	
	[Export]
	public AllyStates state {get; set;} = AllyStates.Idle;
}
