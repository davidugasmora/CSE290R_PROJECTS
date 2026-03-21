using System.Linq;
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class PlayerInvisibilityAbilityState : PlayerGenericAbilityState
{
	[Export]
	public float abilityDuration {get; set;} = 5.0f;
	public Timer durationTimer;
	public bool persists = false;

	private Array<Ally> invisibleAllies = new Array<Ally>();

	public override bool EvaluateStateCondition()
    {
		return base.EvaluateStateCondition() || persists;
    }

    public override void _Ready()
    {
        base._Ready();

		durationTimer = new Timer();
		durationTimer.WaitTime = abilityDuration;
		durationTimer.OneShot = true;
		durationTimer.Timeout += OnAbilityFinished;
		AddChild(durationTimer);
    }

	public void OnAbilityFinished()
	{

		persists = false;

	}


	public override void Enter()
	{
		base.Enter();
		persists = true;
		durationTimer.Start();

		if (player != null)
		{
			player.Modulate = new Color(1,1,1,0.3f);
			player.detectable = false;

			foreach (Ally ally in player.followingAllies)
			{
				invisibleAllies.Add(ally);
				ally.Modulate = new Color(1,1,1,0.3f);
				ally.detectable = false;
			}
		}
	}

	public override void Exit()
    {
		if (player != null)
		{
			player.Modulate = new Color(1,1,1,1f);
			player.detectable = true;

			foreach (Ally ally in invisibleAllies.Reverse<Ally>())
			{
				GD.Print("Removed Invis Ally.");
				invisibleAllies.Remove(ally);
				ally.Modulate = new Color(1,1,1,1f);
				ally.detectable = true;
			}
		}
        base.Exit();
    }

	

}
