using Godot;
using System;

[GlobalClass]
public partial class State : Node
{
	[Export]
	public bool enterOnReady { get; set; } = false;
	
	public Node enteredParent;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (enterOnReady)
		{
			enter();
		} 
		else 
		{
			SetProcess(false);
		}
	}
	
	public virtual void enter()
	{
		enteredParent = GetParent();
		// GD.Print(enteredParent);
		SetProcess(true);
	}

	public virtual void branch(State parallel_state)
	{
		parallel_state.enter();
	}

	public virtual void exit(State succeeding_state)
	{
		enteredParent = null;
		SetProcess(false);

		if (succeeding_state != null)
		{
			succeeding_state.enter();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
