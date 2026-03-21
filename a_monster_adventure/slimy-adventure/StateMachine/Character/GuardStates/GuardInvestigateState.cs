using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class GuardInvestigateState : CharacterState
{

	[Export]
	public NavigationAgent2D navAgent { get; set;} = null;
	[Export]
	public float investigateSpeed = 150.0f;

	protected float pointDistanceThreshold = 50.0f;

	public override bool EvaluateStateCondition()
	{
		return (character as Guard).state == Guard.GuardStates.Investigate;
	}

	public override void Enter()
	{
		base.Enter();
		navAgent.TargetPosition = (character as Guard).investigatePos;
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
			navAgent.TargetPosition = alertPosition;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		SearchForPlayer();
		
		MoveToPoint(delta);
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

		if ((navAgent.TargetPosition - character.GlobalPosition).Length() < pointDistanceThreshold)
		{
			(character as Guard).state = Guard.GuardStates.Pause;
			return;
		}

		(character as Character).velocity = direction * investigateSpeed;


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
