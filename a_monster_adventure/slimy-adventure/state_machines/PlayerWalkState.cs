using Godot;
using System;

[GlobalClass]
public partial class PlayerWalkState : CharacterState
{
	[Export]
	public State playerIdleState { get; set; } = null;

	public override void enter()
	{
		base.enter();
		if (character != null) (character as Character).animation_name = "walk_";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 input = Input.GetVector("move_left","move_right","move_up","move_down");

		if (input != Vector2.Zero)
		{
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
		else
		{
			(character as Character).velocity = Vector2.Zero;
			exit(playerIdleState);
		}
	}
}
