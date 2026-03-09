using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node
{
	
	public static Global instance { get; private set; }

	public string NextScene;
	
	[Signal]
	public delegate void AlertGuardsEventHandler(Vector2 guardPosition, Character spottedPrisoner);
	
	public Dictionary<string,string> sceneDict = new Dictionary<string, string>
	{
    	["NA"] = "",
    	["LoadingScreen"] = "res://screens/loading/loading_screen.tscn",
    	["LoadingScreen"] = "res://screens/loading/loading_screen.tscn"
	};

	public void TransitionScene(string sceneName){
		
		GetTree().ChangeSceneToFile(sceneDict[sceneName]);
	}
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
