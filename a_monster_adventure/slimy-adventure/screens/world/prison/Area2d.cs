using Godot;
using System;

public partial class Area2d : Area2D
{
	// Called when the node enters the scene tree for the first time.

	private bool _isPlayerNerby = false;
	public string NextScenePath = "res://screens/world/field/field.tscn";
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(_isPlayerNerby && Input.IsActionJustPressed("ui_accept"))
		{
			Escape();
		}
	}

	private void Escape()
	{
		var global = GetNode<Global>("/root/Global");
		global.NextScene = "res://screens/world/field/field.tscn";
		GD.Print("Slime is trying to scape");
		GetTree().ChangeSceneToFile("res://screens/loading/loading_screen.tscn");
	}

	private void OnBodyEntered(Node2D body)
	{
		if(body.Name == "Player")
		{
			_isPlayerNerby = true;
			GD.Print("Press Space Bar to scape");
		}
	}

	private void OnBodyExited(Node2D body)
	{
		if(body.Name == "Player")
		{
			_isPlayerNerby = false;
			GD.Print("Player has left the toilet");
		}
	}
}
