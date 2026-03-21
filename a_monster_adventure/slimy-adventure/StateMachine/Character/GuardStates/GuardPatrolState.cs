using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class GuardPatrolState : CharacterState
{

	[Export]
	public NavigationAgent2D navAgent { get; set;} = null;
	[Export]
	public float patrolSpeed = 50.0f;

	protected Line2D line2D = null;
	protected int pointIndex = 0;
	protected Vector2 currentPoint = Vector2.Zero;

	protected float pointDistanceThreshold = 10.0f;

	public override bool EvaluateStateCondition()
	{
		return (character as Guard).state == Guard.GuardStates.Patrol;
	}

	public override void Enter()
	{
		base.Enter();
		if (character is Guard)
		{
			line2D = (character as Guard).line2D;
			
			if (line2D != null)
				GetRandomPoint();//GetClosestPoint();
		}
		Global.Instance.Connect("AlertGuards",new Callable(this,"PrepareInvestigate"));
		if (character != null) (character as Character).animation_name = "walk_";
	}

    public override void Exit()
    {
        base.Exit();
		Global.Instance.Disconnect("AlertGuards",new Callable(this,"PrepareInvestigate"));
    }


	public void PrepareInvestigate(Vector2 alertPosition, Character spottedPrisoner = null)
	{
		float distanceFromAlert = (character.GlobalPosition - alertPosition).Length();
		if (distanceFromAlert <= (character as Guard).alertRadius)
		{
			(character as Guard).investigatePos = alertPosition;
			(character as Guard).state = Guard.GuardStates.Investigate;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (line2D != null)
		{
			SearchForPlayer();
			
			MoveToPoint(delta);

			AttemptGetNextPoint();
			if (character != null) (character as Character).animation_name = "walk_";
		}
		
	}

	protected virtual void SearchForPlayer()
	{
		bool seesPrisoner = (character as Guard).GuardSeesPrisoner((character as Guard)._targetPrisoner);
		if (!seesPrisoner) seesPrisoner = (character as Guard).GuardSeesNewTargetPrisoner();

		if (seesPrisoner) 
		{
			Global.Instance.EmitSignal("AlertGuards", (character as Guard)._targetPrisoner.GlobalPosition, (character as Guard)._targetPrisoner);
			(character as Guard).state = Guard.GuardStates.Chase;
		}
	}

	// protected virtual void ListenForAlert

	protected virtual void MoveToPoint(double delta)
	{

		Vector2 direction = (navAgent.GetNextPathPosition() - character.GlobalPosition).Normalized();
		(character as Guard).guardLooks(direction,delta);

		(character as Character).velocity = direction * patrolSpeed;


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

	protected virtual void AttemptGetNextPoint()
	{
		if ((currentPoint - character.GlobalPosition).Length() > pointDistanceThreshold)
		{
			return;
		}
		else
		{
			GetNextPoint();
		}
	}

	protected virtual void GetNextPoint()
	{
		Vector2[] points = line2D.Points;
		int numPoints = points.Length;
		pointIndex = (pointIndex + 1) % numPoints;

		currentPoint = line2D.GlobalPosition + points[pointIndex];
		navAgent.TargetPosition = currentPoint;
	}
	
	protected virtual void GetClosestPoint()
	{
		Vector2[] points = line2D.Points;
		int closestPointIndex = -1;
		float closestPointDistance = 9999.9f;
		
		for (int i = 0; i < points.Length; i++) 
		{
			float pointDistance = (character.GlobalPosition - (line2D.GlobalPosition + points[i])).Length();

			if (pointDistance < closestPointDistance)
			{
				closestPointIndex = i;
				closestPointDistance = pointDistance;
			}
		}
		
		pointIndex = closestPointIndex;

		currentPoint = line2D.GlobalPosition + points[pointIndex];
		navAgent.TargetPosition = currentPoint;
	}

	protected virtual void GetRandomPoint()
	{
		Vector2[] points = line2D.Points;
		pointIndex = GD.RandRange(0,points.Length - 1);

		currentPoint = line2D.GlobalPosition + points[pointIndex];
		navAgent.TargetPosition = currentPoint;
	}
}
