using Godot;
using System;

public partial class MainMenu : Control
{
    private void _on_new_game_pressed()
    {
        GetTree().ChangeSceneToFile("res://screens/world/field/field.tscn");
    }

    private void _on_load_game_pressed()
    {
        GetTree().ChangeSceneToFile("res://screens/saves/saves.tscn");
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
