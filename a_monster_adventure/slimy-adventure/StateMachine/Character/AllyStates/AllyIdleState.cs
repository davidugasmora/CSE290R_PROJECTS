using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class AllyIdleState : CharacterState
{

	[Export]
	public Area2D joinArea { get; set;} = null;

	public override bool EvaluateStateCondition()
	{
		return (character as Ally).state == Ally.AllyStates.Idle;
	}

	public override void _Ready()
	{
		base._Ready();
		joinArea.BodyEntered += BodyEnters;
	}

	

	protected void BodyEnters(Node2D body)
	{
		if (!active)
		{
			return;
		}
		if (!(body == player))
		{
			return;
		}
		else
		{
			(character as Ally).state = Ally.AllyStates.PerfectFollow;
		}
	}

	private Player player;
	
	public override void Enter()
	{
		base.Enter();
		(character as Character).velocity = Vector2.Zero;
		player = (character as Ally).getPlayer();
		Global.Instance.Connect("AlertGuards",new Callable(this,"PrepareFlee"));
		if (character != null) (character as Character).animation_name = "idle_";

	}

    public override void Exit()
    {
        base.Exit();
		Global.Instance.Disconnect("AlertGuards",new Callable(this,"PrepareFlee"));

    }


	public void PrepareFlee(Vector2 alertPosition, Character spottedPrisoner)
	{
		GD.Print("Ally flees");
		if (spottedPrisoner == (character as Ally))
		{
			(character as Ally).state = Ally.AllyStates.Flee;
		}
	}

}
