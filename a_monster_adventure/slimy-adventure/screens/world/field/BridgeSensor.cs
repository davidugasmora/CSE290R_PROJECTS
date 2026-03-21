using Godot;
using System;

public partial class BridgeSensor : Area2D
{
	[Signal]
	public delegate void ChangeLayerEventHandler();

	private CollisionShape2D collisionShape2D;

	Vector2 entryPoint = Vector2.Zero;

	public override void _Ready()
	{
		AddToGroup("BridgeSensor");
		collisionShape2D = GetNode<CollisionShape2D>("CollisionShape2D");
	}

	private void _change_layer(Character body)
	{
		var shape = collisionShape2D.Shape as RectangleShape2D;

		Vector2 changePoint = shape.Size / 2;
    	Vector2 movedDistance = entryPoint - body.GlobalPosition;

		if (Mathf.Abs(movedDistance.Y) > changePoint.Y)
		{
			EmitSignal(SignalName.ChangeLayer);
		}
	}

	private void _on_body_entered(Character body)
	{
		if (body.IsInGroup("Player"))
		{
			entryPoint = body.GlobalPosition;
		}
	}

	private void _on_body_exited(Character body)
	{
		if (body.IsInGroup("Player"))
		{
			_change_layer(body);
		}
	}
}
