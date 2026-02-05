using Godot;
using System;

public partial class PauseMenu : Control
{
	private Field _field;

	public override void _Ready()
	{
		_field = GetParent<Field>();
		
	}
	private void _on_resume_pressed()
	{
		_field.PauseMenu();
	}
	private void _on_go_back_pressed()
	{
		GetTree().ChangeSceneToFile("res://screens/main_menu/main_menu.tscn");
	}
}
