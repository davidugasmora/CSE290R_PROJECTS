using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class State : StateCondition
{
	
	public Node enteredParent;
	public bool active = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetProcess(false);
		StateHandler stateHandler = SearchForStateHandler();
		if (stateHandler != null)
		{
			stateHandler.Ready += HandlerReady;
		}
	}

	public virtual void HandlerReady()
	{
		return;
	}
	
	public virtual void Enter()
	{
		GD.Print("Enter: ", Name);
		active = true;
		enteredParent = GetParent();
		SetProcess(true);
	}

	public virtual void Exit()
	{
		GD.Print("Exit: ", Name);
		active = false;
		enteredParent = null;
		SetProcess(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
