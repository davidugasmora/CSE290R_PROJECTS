using Godot;
using Godot.Collections;
using System;

public partial class Player : Character
{
	[Export]
	public Camera2D camera {get;set;} = null;

	[Export]
	public StateCondition abilitiesStateBranch {get; set;} = null;

	public Array<Ally> followingAllies = [];

	public void addAlly(Ally ally)
	{
		if (!followingAllies.Contains(ally))
		{
			followingAllies.Add(ally);
			if (followingAllies[followingAllies.IndexOf(ally)].abilityStateBranch != null)
				abilitiesStateBranch.AddChild(followingAllies[followingAllies.IndexOf(ally)].createNewStateBranchLink());
		}
	}

	public int getAllyIndex(Ally ally)
	{
		return followingAllies.IndexOf(ally);
	}
	public void removeAlly(Ally ally)
	{
		if (followingAllies.Contains(ally))
		{
			if (followingAllies[followingAllies.IndexOf(ally)].abilityStateBranchLink != null)
				abilitiesStateBranch.RemoveChild(followingAllies[followingAllies.IndexOf(ally)].abilityStateBranchLink);
			followingAllies.Remove(ally);
		}
	}

	public Camera2D getCamera()
	{
		return camera;
	}

	public void PrepareFree()
	{
		Global.Instance.player = null;

		foreach (Ally ally in followingAllies)
		{
			removeAlly(ally);
			ally.state = Ally.AllyStates.Flee;
		}
	}
}