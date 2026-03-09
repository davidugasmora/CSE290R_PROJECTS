using Godot;
using System;

[GlobalClass]
public partial class PlayerIdleState : CharacterState
{

	public override void Enter()
	{
		base.Enter();
		(character as Character).velocity = Vector2.Zero;
		if (character != null) (character as Character).animation_name = "idle_";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		return;
	}
}
