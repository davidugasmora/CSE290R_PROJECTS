using Godot;
using System;

public partial class LoadingScreen : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Timer>("LoadingTimer").Timeout += OnTimerTimeout;
	}

	private void OnTimerTimeout()
	{
		var global = GetNode<Global>("/root/Global");

		string finalScene = global.NextScene;

		if(!string.IsNullOrEmpty(finalScene))
		{
			GetTree().ChangeSceneToFile(finalScene);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
