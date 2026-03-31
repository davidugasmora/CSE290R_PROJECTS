using Godot;
using System;

public partial class PlayerCamera : Camera2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		(Vector2 cameraTopLeftLimit, Vector2 cameraBottomRightLimit) = Global.Instance.getCameraLimits();
		LimitLeft = (int)cameraTopLeftLimit.X;
		LimitTop = (int)cameraTopLeftLimit.Y;
		LimitRight = (int)cameraBottomRightLimit.X;
		LimitBottom = (int)cameraBottomRightLimit.Y;
	}
}
