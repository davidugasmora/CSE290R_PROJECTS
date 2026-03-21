using Godot;
using System;

[GlobalClass]
public partial class PlayerWebAbilityState : PlayerGenericAbilityState
{
	[Export]
	public RayCast2D rayCast2D;
	[Export]
	public ShotWebVisual webVisual;
	[Export]
	public float pullSpeed = 800.0f;
	[Export]
	public float webLength = 2000.0f;
	[Export]
	public float distanceThreshold = 50.0f;

	public Vector2 webDestination = Vector2.Zero;
	private bool webMadeContact = false;
	private bool webIsPulling = false;
	private bool webIsReturning = false;

	public bool persists = false;

	public override void _Ready()
	{
		base._Ready();
		webVisual.Connect("ReachedTarget",new Callable(this,"webReachedTarget"));
	}

	public void webReachedTarget()
	{
		if (webMadeContact)
		{
			webIsPulling = true;
		}
		else
		{
			if (webIsReturning)
			{
				persists = false;
			}
			else
			{
				webVisual.webReturn();
				webIsReturning = true;
			}
		}
	}



	public override void Enter()
	{
		base.Enter();

		persists = true;

		rayCast2D.ClearExceptions();
		rayCast2D.AddException(player);

		foreach (Ally ally in player.followingAllies)
		{
			ally.followWeight = 10.0f;
			ally.detectable = false;
		}

		rayCast2D.GlobalPosition = player.GlobalPosition;
		rayCast2D.TargetPosition = player.GetLocalMousePosition().Normalized() * webLength;
		rayCast2D.ForceRaycastUpdate();

		if (rayCast2D.IsColliding()) {
			webDestination = rayCast2D.GetCollisionPoint();
			webMadeContact = true;
			webVisual.shoot(player.GlobalPosition,webDestination);
		}
		else
		{
			webMadeContact = false;
			webVisual.shoot(player.GlobalPosition,player.GlobalPosition + rayCast2D.TargetPosition);
		}

		GD.Print(persists);

		if (player != null) player.animation_name = "walk_";
	}

    public override void Exit()
    {
        base.Exit();

		
		foreach (Ally ally in player.followingAllies)
		{
			ally.followWeight = 5.0f;
			ally.detectable = true;
		}

		webMadeContact = false;
		webIsPulling = false;
		webIsReturning = false;
		
		webVisual.webEnd();

		player.velocity = Vector2.Zero;
    }


	public override bool EvaluateStateCondition()
	{
		return base.EvaluateStateCondition() || persists;
	}

	public override void _Process(double delta)
	{
		webVisual.setOrigin(player.GlobalPosition);

		if (!webIsPulling) return;

		base._Process(delta);
		Vector2 localWebDestination = webDestination - player.GlobalPosition;

		player.velocity = localWebDestination.Normalized() * pullSpeed;
		if (localWebDestination.Length() < distanceThreshold) 
		{
			persists = false;
		}
	}


}
