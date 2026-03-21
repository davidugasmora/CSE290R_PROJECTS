using Godot;
using System;
using System.Threading.Tasks;

public partial class WorldScenePortal : Area2D
{

	[Export]
	public string sceneName = "";
	[Export]
	public int playerInstantiatorId = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BodyEntered += onBodyEntered;
	}

	public async void onBodyEntered(Node2D body)
	{
		if (body == Global.Instance.player)
		{
			await changeScene();
		}
	}

	public async Task changeScene()
	{
		await Global.Instance.TransitionWorldScene(sceneName,playerInstantiatorId);
	}
}
