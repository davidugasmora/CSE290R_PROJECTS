using Godot;
using System;
using Godot.Collections;
using System.Threading.Tasks;

public partial class Global : Node
{
	
	public static Global Instance { get; private set; }
	public Player player {get; set;}

	[Signal]
	public delegate void AlertGuardsEventHandler(Vector2 alertPosition, Character spottedPrisoner);

	public Dictionary<int,Dictionary<string,Variant>> allyDict = new Dictionary<int,Dictionary<string,Variant>>
        {
            { 0, new Dictionary<string, Variant>
				{
					{"isFollowing", false},
					{"isImprisoned", true},
					{"sceneName", ""},
					{"position", new Vector2(0,0)},
					{"allyScene", "res://characters/allies/ghost_ally.tscn"}
				} 
			},
            { 1, new Dictionary<string, Variant>
				{
					{"isFollowing", false},
					{"isImprisoned", true},
					{"sceneName", ""},
					{"position", new Vector2(0,0)},
					{"allyScene", "res://characters/allies/spider_ally.tscn"}
				} 
			},
            { 2, new Dictionary<string, Variant>
				{
					{"isFollowing", false},
					{"isImprisoned", true},
					{"sceneName", ""},
					{"position", new Vector2(0,0)},
					{"allyScene", "res://characters/allies/ally.tscn"}
				} 
			}
        };
	
	public Dictionary<string,string> sceneDict = new Dictionary<string, string>
	{
    	["NA"] = "",
    	["LoadingScreen"] = "res://screens/loading/loading_screen.tscn",
    	["GuardTest"] = "res://screens/world/testing_levels/guard_test_scene.tscn",
    	["AllyTest"] = "res://screens/world/testing_levels/ally_test_scene.tscn.tscn",
		["Prison"] = "res://screens/world/testing_levels/ally_test_scene.tscn.tscn",
	};

	public string NextScene = "";
	public string currentSceneName = "GuardTest";
	public async Task TransitionScene(string sceneName)
	{
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		
		currentSceneName = sceneName;
		GetTree().ChangeSceneToFile(sceneDict[sceneName]);
	}

	public async Task TransitionWorldScene(string sceneName,int playerInstantiatorId){
		
		await TransitionScene(sceneName);
		await ToSignal(GetTree(), SceneTree.SignalName.SceneChanged);
		PrintAllNodes();
		DoPlayerInstantiation(playerInstantiatorId);
	}

	public void DoPlayerInstantiation(int playerInstantiatorId)
	{
		Array<PlayerInstantiator> playerInstantiators = GetPlayerInstantiators(GetTree().Root);

		foreach (PlayerInstantiator playerInstantiator in playerInstantiators)
		{
			if (playerInstantiator.id == playerInstantiatorId)
			{
				GD.Print(playerInstantiatorId);
				playerInstantiator.DoInstantiation();
				break;
			}
		}
	}

	public Array<PlayerInstantiator> GetPlayerInstantiators(Node currentNode)
	{
		Array<PlayerInstantiator> playerInstantiators = [];

		foreach (Node child in currentNode.GetChildren())
		{
			if (child is PlayerInstantiator) playerInstantiators.Add(child as PlayerInstantiator);
			else playerInstantiators += GetPlayerInstantiators(child);
		}

		return playerInstantiators;
	}
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Instance = this;
		DoPlayerInstantiation(0);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void PrintAllNodes(Node currentNode = null)
	{
		if (currentNode == null)
		{
			PrintAllNodes(GetTree().Root);
			return;
		}

		foreach (Node child in currentNode.GetChildren())
		{
			GD.Print(child," : ",child.Name);
			PrintAllNodes(child);
		}
	}
}