
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
        var saveButton = GetNode<Godot.Button>("VBoxContainer/Save");
        var quiteButton = GetNode<Godot.Button>("VBoxContainer/Quit");
        var loadButton = GetNode<Godot.Button>("VBoxContainer/Load");

        resumeButton.Pressed += OnResumePressed;
        quiteButton.Pressed += OnQuitPressed;

        if (saveButton != null)
        {
            saveButton.Pressed += () => {
            Global.Instance.SaveGame();
            GD.Print("Manual Save Successful");
            };
        }

        if (loadButton != null)
        {
            loadButton.Pressed += () => {
                GetTree().Paused = false; 
                this.Visible = false;
                Global.Instance.LoadGame();
            };
        }

    }

    public void OnResumePressed()
    {
        GetTree().Paused = false;
        this.Visible = false;
        GD.Print("Resume button pressed.");
    }

    public async void OnQuitPressed()
    {
        GD.Print("Quit button pressed.");
        
        // Hide the menu and trigger the global transition
        this.Visible = false;
        await Global.Instance.TransitionScene("MainMenu");
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
