using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class AllyFollowState : CharacterState
{
	[Export]
	public NavigationAgent2D navAgent { get; set;} = null;

	public override bool EvaluateStateCondition()
    {
		return (character as Ally).state == Ally.AllyStates.Follow;
    }

	public override void _Ready()
	{
		base._Ready();
	}

	private Player player;

	public override void Enter()
	{
		base.Enter();
		player = (character as Ally).player;
		player.addAlly(character as Ally);

	}

	public override void Exit()
	{
		base.Exit();
		player.removeAlly(character as Ally);
		player = null;

	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
			
		Follow(delta);
		
	}

	protected virtual void Follow(double delta)
	{

		int allyIndex = player.getAllyIndex(character as Ally);
		
		navAgent.TargetPosition =  (allyIndex == 0) ? player.GlobalPosition: player.followingAllies[allyIndex - 1].GlobalPosition ;
		Vector2 direction = (navAgent.GetNextPathPosition() - character.GlobalPosition).Normalized();

		if (navAgent.GetPathLength() < 100.0f) 
		{
			(character as Character).velocity = Vector2.Zero;
			return;
		}
		
		(character as Character).velocity = direction * (character as Character).Speed;


		if (Math.Abs(direction.X) >= 0.5)
		{
			if (direction.X > 0)
			{
				(character as Character).facingDirection = "right";
			} 
			else
			{
				(character as Character).facingDirection = "left";
			}
		}
		else if (direction.Y != 0.0)
		{
			if (direction.Y > 0)
			{
				(character as Character).facingDirection = "down";
			} 
			else
			{
				(character as Character).facingDirection = "up";
			}
		}
	}
}
