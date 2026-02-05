using Godot;
using System;

public partial class Field : TileMapLayer
{
	private Control _pauseMenu;
	private bool _paused = false;

	public override void _Ready()
	{
		_pauseMenu = GetNode<Control>("PauseMenu");
	}


	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("pause"))
        {
            PauseMenu();
        }
	}

	public void PauseMenu()
    {
        if (_paused)
        {
            _pauseMenu.Hide();
            Engine.TimeScale = 1;
        }
        else
        {
            _pauseMenu.Show();
            Engine.TimeScale = 0;
        }

        _paused = !_paused;
    }
}
