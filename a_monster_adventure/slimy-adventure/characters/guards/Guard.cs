using Godot;
using System;

public partial class Guard : Character
{
	[Export]
	public Line2D line2D { get; set; } = null;
	[Export]
	public RayCast2D raycast2D { get; set; } = null;
	
	[Export]
	public Node2D lightPivot { get; set; } = null;
	[Export]
	public float visionRadius { get; set; } = 600.0f;
	[Export]
	public float visionArc { get; set; } = 0.982f;

	Vector2 lookDirection = Vector2.Zero;
	[Export]
	float lookSpeed { get; set; } = 5.0f;

	public void guardLooks(Vector2 targetLookDirection, double delta)
	{
		float newAngle = Mathf.RotateToward(lookDirection.Angle(), targetLookDirection.Angle(), lookSpeed * (float)delta);
		lookDirection = Vector2.FromAngle(newAngle);

		lightPivot.Rotation = newAngle - Mathf.Pi / 2.0f;
	}


	public bool GuardSeesAnyPrisoner()
	{

		foreach (Character prisoner in GetTree().GetNodesInGroup("Prisoner"))
		{
			if (GuardSeesPrisoner(prisoner))
			{
				return true;
			}
		}
		
		return false;
	}

	public Character _targetPrisoner;
	public bool GuardSeesNewTargetPrisoner()
	{

		Character closestSeenPrisoner = null;
		float closestPrisonerDistance = 999.9f;

		foreach (Character prisoner in GetTree().GetNodesInGroup("Prisoner"))
		{
			if (GuardSeesPrisoner(prisoner))
			{
				float prisonerDistance = (GlobalPosition - prisoner.GlobalPosition).Length();

				if (prisonerDistance < closestPrisonerDistance)
				{
					closestSeenPrisoner = prisoner;
					closestPrisonerDistance = prisonerDistance;
				}
			}
		}
		
		if (closestSeenPrisoner != null)
		{
			_targetPrisoner = closestSeenPrisoner;
			return true;
		}
		
		return false;
	}



	public bool GuardSeesPrisoner(Character prisoner)
	{
		if (prisoner == null)
		{
			return false;
		}

		Vector2 directionToPlayer = ToLocal(prisoner.GlobalPosition).Normalized();

		if (Math.Abs(lookDirection.AngleTo(directionToPlayer)) > visionArc)
		{
			return false;
		}

		raycast2D.TargetPosition = directionToPlayer * visionRadius;
		raycast2D.ForceRaycastUpdate();

		var collider = raycast2D.GetCollider();

		if (collider != prisoner)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
	
	public enum GuardStates
	{
		Patrol = 1 << 1,
		Chase = 1 << 2,
		Pause = 1 << 3,
		Catch = 1 << 4,
	}
	
	[Export]
	public GuardStates state {get; set;} = GuardStates.Patrol;
}
