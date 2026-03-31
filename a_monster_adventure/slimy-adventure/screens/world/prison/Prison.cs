using Godot;
using System;

public partial class Prison : Area2D
{
	// Called when the node enters the scene tree for the first time.

	private bool _isPlayerNearby = false;
	public string NextScenePath = "res://screens/world/field/field.tscn";
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(_isPlayerNearby && Input.IsActionJustPressed("interact"))
		{
			Escape();
		}
	}

	private async void Escape()
	{
		// Point to the key in sceneDict, e.g., "Field" (make sure "Field" is in your sceneDict!)
		await Global.Instance.TransitionScene("Field"); 
		_isPlayerNearby = false;
		GD.Print("Slime is trying to escape");
	}

	public void OnBodyEntered(Node2D body)
	{
		GD.Print(body.Name);
		if(body.Name == "Player")
		{
			_isPlayerNearby = true;
			GD.Print("Press Space Bar to scape");
		}
	}

	public void OnBodyExited(Node2D body)
	{
		if(body.Name == "Player")
		{
			_isPlayerNearby = false;
			GD.Print("Player has left the toilet");
		}
	}
}
