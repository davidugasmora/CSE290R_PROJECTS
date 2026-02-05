using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class GuardPatrolState : CharacterState
{
	[Export]
	public Line2D line2D { get; set; } = null;
	[Export]
	public NavigationAgent2D navAgent { get; set;} = null;
	[Export]
	public float patrolSpeed = 50.0f;

	protected int pointIndex = 0;
	protected Vector2 currentPoint = Vector2.Zero;

	protected float pointDistanceThreshold = 10.0f;

	public override void enter()
	{
		base.enter();
		if (line2D != null)
		{
			GetNextPoint();
		}
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (line2D != null)
		{
			MoveToPoint();
			GD.Print(currentPoint);

			AttemptGetNextPoint();
		}
		
	}

	protected virtual void MoveToPoint()
	{

		Vector2 direction = (navAgent.GetNextPathPosition() - character.GlobalPosition).Normalized();

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
}
