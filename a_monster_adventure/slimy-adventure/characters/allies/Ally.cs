using Godot;
using System;

[GlobalClass]
public partial class Ally : Character
{
	[Export]
	public int id {get; set;} = 0;
	[Export]
	public float followWeight {get; set;} = 5.0f;
	[Export]
	public StateHandler abilityStateBranch {get; set;} = null;
	public StateHandlerLink abilityStateBranchLink = null;

	public Player getPlayer() {return Global.Instance.player;}

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
		PerfectFollow = 1 << 4,
	}
	
	[Export]
	public AllyStates state {get; set;} = AllyStates.Idle;

	[Signal]
	public delegate void CapturedEventHandler();

}
