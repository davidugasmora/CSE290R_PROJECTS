using Godot;
using System;

[GlobalClass]
public partial class PlayerRunState : CharacterState
{

	public override void Enter()
	{
		GD.Print("Run");
		base.Enter();
		if (character != null) (character as Character).animation_name = "run_";
	}

	public override bool EvaluateStateCondition()
	{
		Vector2 input = Input.GetVector("move_left","move_right","move_up","move_down");
		return input != Vector2.Zero && Input.IsActionPressed("run");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 input = Input.GetVector("move_left","move_right","move_up","move_down");

		(character as Character).velocity = input * (character as Character).Speed * 1.5f;

		if (input.X != 0.0)
		{
			if (input.X > 0)
			{
				(character as Character).facingDirection = "right";
			} 
			else
			{
				(character as Character).facingDirection = "left";
			}
		}
		else if (input.Y != 0.0)
		{
			if (input.Y > 0)
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
