using Godot;
using Godot.Collections;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

[GlobalClass]
public partial class AllyFleeState : CharacterState
{
	[Export]
	public NavigationAgent2D navAgent { get; set;} = null;
	[Export]
	public RayCast2D rayCast2D {get; set;} = null;
	// private Line2D line = null;

	public override bool EvaluateStateCondition()
    {
		return (character as Ally).state == Ally.AllyStates.Flee;
    }

	private Timer idleTimer;

	public override void _Ready()
	{

		// line = new Line2D();
		// AddChild(line);
		// line.Width = 1.0f;

		
		idleTimer = new Timer();
		idleTimer.WaitTime = 1.0;
		idleTimer.OneShot = true;
		idleTimer.Timeout += stoppedFleeing;
		AddChild(idleTimer);
		
		base._Ready();
	}

	public override void Enter()
	{
		base.Enter();
		idleTimer.Start();
		findFleePath();
		if (character != null) (character as Character).animation_name = "walk_";
	}

    public override void Exit()
    {
        base.Exit();
		idleTimer.Stop();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		Flee(delta);
		
	}

	private float guardDistanceThreshold = 600.0f;
	private float explorationDistance = 1000.0f;

	private void stoppedFleeing()
	{
		(character as Ally).state = Ally.AllyStates.Idle;
	}

	private void findFleePath(int iteration = 0)
	{

		Array<Vector2> guardDirections = [];
		Array<float> weights = [];
		int numberOfItems = 0;

		foreach (Guard guard in GetTree().GetNodesInGroup("Guard"))
		{
			Vector2 guardLocalPosition = guard.GlobalPosition - character.GlobalPosition;
			guardDirections.Add(guardLocalPosition.Normalized());
			weights.Add(Math.Max((guardDistanceThreshold - guardLocalPosition.Length()) / guardDistanceThreshold,0.0f));
			numberOfItems++;
		}

		Vector2 averageFleeVector = Vector2.Zero;

		for (int i = 0; i < numberOfItems; i++)
		{
			averageFleeVector += -guardDirections[i] * weights[i];
		}

		if (averageFleeVector == Vector2.Zero)
		{
			if (idleTimer.IsStopped()) idleTimer.Start();
			return;
		}

		Vector2 fleeDirection = averageFleeVector.Normalized();
		Vector2 rayCastDirection = fleeDirection.Rotated((float)Math.PI / 4.0f);
		
		float remainingExplorationDistance = explorationDistance;
		int loopCount = 0;

		Array<Vector2> linePoints = [];
		linePoints.Add(rayCast2D.GlobalPosition);

		do
		{
			rayCast2D.TargetPosition = rayCastDirection*remainingExplorationDistance;
			rayCast2D.ForceRaycastUpdate();

			if (rayCast2D.IsColliding())
			{
				Vector2 collisionNormal = rayCast2D.GetCollisionNormal().Normalized();
				Vector2 collisionPoint = rayCast2D.GetCollisionPoint() - rayCastDirection * 50.0f;

				remainingExplorationDistance -= (collisionPoint - rayCast2D.GlobalPosition).Length();
				rayCastDirection = rayCastDirection.Reflect(collisionNormal.Rotated((float)Math.PI / 2.0f));

				rayCast2D.GlobalPosition = collisionPoint;
				linePoints.Add(collisionPoint);
			}
			loopCount += 1;
			if (loopCount > 100) break;
		} while (remainingExplorationDistance > 0.0);

		linePoints.Add(rayCast2D.GlobalPosition + rayCast2D.TargetPosition);
		navAgent.TargetPosition = rayCast2D.GlobalPosition + rayCast2D.TargetPosition;

		rayCast2D.Position = Vector2.Zero;

		// line.Points = linePoints.ToArray();

		if (!navAgent.IsTargetReachable() && iteration < 3) findFleePath(iteration+1);
		if (!idleTimer.IsStopped()) idleTimer.Stop();
	}

	protected virtual void Flee(double delta)
	{
		if ((navAgent.TargetPosition - character.GlobalPosition).Length() < 50.0f) 
		{
			findFleePath();
		}

		Vector2 direction = (navAgent.GetNextPathPosition() - character.GlobalPosition).Normalized();
		
		(character as Character).velocity = direction * (character as Character).Speed;
		Global.Instance.allyDict[(character as Ally).id]["position"] = character.GlobalPosition;

		


		if (Math.Abs(direction.X) >= 0.5)
		{
			if (direction.X > 0)
			{
				(character as Character).facingDirection = "right";
			} 
			else
			{
				(character as Character).facingDirection = "left";
			}
		}
		else if (direction.Y != 0.0)
		{
			if (direction.Y > 0)
			{
				(character as Character).facingDirection = "down";
			} 
			else
			{
				(character as Character).facingDirection = "up";
			}
		}
	}
}
