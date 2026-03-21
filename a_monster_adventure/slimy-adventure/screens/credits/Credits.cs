using Godot;
using System;

public partial class Credits : Control
{ 
	public override void _Ready() {
		var visibleNotifier = GetNode<VisibleOnScreenNotifier2D>("Node2D/VisibleOnScreenNotifier2D");
		
		// Connect the signals to methods in this script
		visibleNotifier.ScreenExited += OnScreenExited;
	}
  
	public override void _Process(double delta) {
		var direction = new Vector2(0,-1);
		var node2D = GetNode<Node2D>("Node2D");
		
		node2D.Position += direction;

		if (Input.IsActionJustPressed("ui_cancel"))
		{
			GetTree().ChangeSceneToFile("res://screens/main_menu/main_menu.tscn");
		}	
	}
  
	private void OnScreenExited()
	{
		// Play transition animation then go back to menu
		GetTree().ChangeSceneToFile("res://screens/main_menu/main_menu.tscn");
	}
}
