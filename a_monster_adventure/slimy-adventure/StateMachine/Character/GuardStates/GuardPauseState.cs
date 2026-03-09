using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class GuardPauseState : CharacterState
{

	public override bool EvaluateStateCondition()
    {
		return (character as Guard).state == Guard.GuardStates.Pause;
    }

	[Export]
	public float pauseTimer { get; set; } = 2.0f;

	protected Timer timer = null;

	public override void _Ready()
	{
		base._Ready();
		timer = new Timer();
		timer.WaitTime = pauseTimer;
		timer.Timeout += unpause;
		timer.OneShot = true;
		AddChild(timer);
	}

	protected void unpause()
	{
		(character as Guard).state = Guard.GuardStates.Patrol;
	}

	protected virtual void SearchForPlayer()
	{
		bool seesPrisoner = (character as Guard).GuardSeesPrisoner((character as Guard)._targetPrisoner);
		if (!seesPrisoner) seesPrisoner = (character as Guard).GuardSeesNewTargetPrisoner();

		if (seesPrisoner) 
		{
			Global.instance.EmitSignal(Global.SignalName.AlertGuards, (character as Guard)._targetPrisoner);
			timer.Stop();
			(character as Guard).state = Guard.GuardStates.Chase;
		}
	}

	public override void Enter()
	{
		base.Enter();
		timer.Start();
		(character as Character).velocity = Vector2.Zero;
	}
	
	public override void _Process(double delta)
	{
		SearchForPlayer();
	}
}
