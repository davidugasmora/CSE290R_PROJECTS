using Godot;
using System;

[GlobalClass]
public partial class PlayerWalkState : CharacterState
{
	public override void Enter()
	{
		base.Enter();
		if (character != null) (character as Character).animation_name = "walk_";
	}

	public override bool EvaluateStateCondition()
	{
		Vector2 input = Input.GetVector("move_left","move_right","move_up","move_down");
		return input != Vector2.Zero;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 input = Input.GetVector("move_left","move_right","move_up","move_down");

		(character as Character).velocity = input * (character as Character).Speed;

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
