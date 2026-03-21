
using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
    /* 
        Next steps
        - fix pause menu appearing on main menu
        - wire up buttons in pause menu
        - implement save logic
        - implement load logic
    */

    public override void _Ready()
    {
       var resumeButton = GetNode<Godot.Button>("VBoxContainer/Resume");
       var quiteButton = GetNode<Godot.Button>("VBoxContainer/Quit");

       resumeButton.Pressed += OnResumePressed;
       quiteButton.Pressed += OnQuitPressed;
    }

    public void OnResumePressed()
    {
        GetTree().Paused = false;
        this.Visible = false;
    }

    public async void OnQuitPressed()
    {
        GetTree().Paused = false;
        this.Visible = false;
        Global.Instance.NextScene = "res://screens/main_menu/main_menu.tscn";
        await Global.Instance.TransitionScene("LoadingScreen");
    }

    public override void _Process(double delta)
    {
        bool pause = GetTree().Paused;
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            GD.Print("Pressed the Escape key.");

            if (GetTree().CurrentScene.Name == "main_menu" || GetTree().CurrentScene.Name == "credits")
            {
                return;
            }

            // toggle pause/unpause
            pause = !pause;
            this.Visible = pause;
            // update the pause state
            GetTree().Paused = pause;
        }
    }
}
