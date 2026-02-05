using Godot;
using System;

public partial class Credits : Control
{
    private void _on_back_pressed()
    {
        GetTree().ChangeSceneToFile("res://screens/main_menu/main_menu.tscn");
    }
}
