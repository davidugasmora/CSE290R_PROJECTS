using Godot;
using System;

[GlobalClass]
public partial class PlayerWebAbilityState : PlayerGenericAbilityState
{
	[Export]
	public RayCast2D rayCast2D;
	[Export]
	public float pullSpeed = 800.0f;
	[Export]
	public float webLength = 2000.0f;
	[Export]
	public float distanceThreshold = 50.0f;

	public Vector2 webDestination = Vector2.Zero;
	public bool persists = false;

	public override void _Ready()
	{
		base._Ready();
	}

	public override void Enter()
	{
		base.Enter();
		rayCast2D.ClearExceptions();
		rayCast2D.AddException(player);

		rayCast2D.GlobalPosition = player.GlobalPosition;
		rayCast2D.TargetPosition = player.GetLocalMousePosition().Normalized() * webLength;
		rayCast2D.ForceRaycastUpdate();

		if (rayCast2D.IsColliding()) {
			webDestination = rayCast2D.GetCollisionPoint();
			persists = true;
		}

		if (player != null) player.animation_name = "walk_";
	}

	public override bool EvaluateStateCondition()
	{
		return base.EvaluateStateCondition() || persists;
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		Vector2 localWebDestination = webDestination - player.GlobalPosition;

		player.velocity = localWebDestination.Normalized() * pullSpeed;
		if (localWebDestination.Length() < distanceThreshold) persists = false;
	}


}
