using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class AllyPerfectFollowState : CharacterState
{

	public override bool EvaluateStateCondition()
    {
		return (character as Ally).state == Ally.AllyStates.PerfectFollow;
    }

	public override void _Ready()
	{
		base._Ready();
	}

	private Player player;

	public override void Enter()
	{
		base.Enter();
		player = (character as Ally).getPlayer();
		player.addAlly(character as Ally);
		Global.Instance.Connect("AlertGuards",new Callable(this,"PrepareFlee"));
		Global.Instance.allyDict[(character as Ally).id]["isFollowing"] = true;
		Global.Instance.allyDict[(character as Ally).id]["isImprisoned"] = false;
	}

	public override void Exit()
	{
		base.Exit();
		player.removeAlly(character as Ally);
		player = null;
		Global.Instance.Disconnect("AlertGuards",new Callable(this,"PrepareFlee"));
		Global.Instance.allyDict[(character as Ally).id]["isFollowing"] = false;
		Global.Instance.allyDict[(character as Ally).id]["sceneName"] = Global.Instance.currentSceneName;
		Global.Instance.allyDict[(character as Ally).id]["position"] = character.GlobalPosition;

	}
	
	public void PrepareFlee(Vector2 alertPosition, Character spottedPrisoner)
	{
		GD.Print("Ally flees");
		if (spottedPrisoner == (character as Ally) || spottedPrisoner == player)
		{
			(character as Ally).state = Ally.AllyStates.Flee;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
			
		Follow(delta);
		
	}

	public bool idle = false;

	protected virtual void Follow(double delta)
	{

		int allyIndex = player.getAllyIndex(character as Ally);
		
		Vector2 targetPosition =  (allyIndex == 0) ? player.GlobalPosition: player.followingAllies[allyIndex - 1].GlobalPosition ;
		Vector2 localTargetPosition = (targetPosition - (character as Character).GlobalPosition);
		float distance = localTargetPosition.Length() - 50;
		Vector2 direction = localTargetPosition.Normalized();

		if (distance <= 25.0f)
		{
			idle = true;
			(character as Character).velocity = Vector2.Zero;
			if (character != null) (character as Character).animation_name = "idle_";
			return;
		}
		else if (idle && distance <= 50.0f) 
		{
			(character as Character).velocity = Vector2.Zero;
			if (character != null) (character as Character).animation_name = "idle_";
			return;
		}
		else
		{
			idle = false;
		}
		
		
		(character as Character).velocity = distance * (character as Ally).followWeight * direction;
		if (character != null) (character as Character).animation_name = "walk_";


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
