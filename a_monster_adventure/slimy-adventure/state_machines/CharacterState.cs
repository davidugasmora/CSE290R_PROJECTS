using Godot;
using System;

[GlobalClass]
public partial class CharacterState : State
{

	public CharacterBody2D character { get; set; } = null;
	
	public override void enter()
	{
		base.enter();
		if (enteredParent is CharacterBody2D)
		{
			character = enteredParent as CharacterBody2D;
		}
	}
}
