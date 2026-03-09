using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class CharacterStateHandler : StateHandler
{
	public Character character = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node parent = GetParent();
		if (parent is Character) 
			character = parent as Character;
		else
			GD.PushWarning("CharacterStateHandler must be parented to Character to work, silly.");
		
		base._Ready();
	}


	
}
