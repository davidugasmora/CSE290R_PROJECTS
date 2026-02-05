using Godot;
using System;

[GlobalClass]
public partial class PlayerIdleState : CharacterState
{
	[Export]
	public State playerWalkState { get; set; } = null;

	public override void enter()
	{
		base.enter();
		if (character != null) (character as Character).animation_name = "idle_";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 input = Input.GetVector("move_left","move_right","move_up","move_down");

		if (input == Vector2.Zero)
		{
			// is idle
			return;
		}
		else
		{
			exit(playerWalkState);
		}
	}
}
