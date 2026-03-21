using Godot;
using System;
using System.Threading.Tasks;

public partial class MainMenu : Control
{
	private async void _on_new_game_pressed()
    {
        Global.Instance.NextScene = "res://screens/world/prison/prison.tscn";
        await Global.Instance.TransitionScene("LoadingScreen");
    }

	private void _on_load_game_pressed()
	{
		
	}

	private void _on_settings_pressed()
	{
		GetTree().ChangeSceneToFile("res://screens/settings/setting_screen.tscn");
	}

	private void _on_credits_pressed()
	{
		GetTree().ChangeSceneToFile("res://screens/credits/credits.tscn");
	}

	private void _on_quit_pressed()
	{
		GetTree().Quit();
	}
}
