using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class AllyFollowState : CharacterState
{
	[Export]
	public NavigationAgent2D navAgent { get; set;} = null;

	public override bool EvaluateStateCondition()
    {
		return (character as Ally).state == Ally.AllyStates.Follow;
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

	protected virtual void Follow(double delta)
	{

		int allyIndex = player.getAllyIndex(character as Ally);
		
		navAgent.TargetPosition =  (allyIndex == 0) ? player.GlobalPosition: player.followingAllies[allyIndex - 1].GlobalPosition ;
		Vector2 direction = (navAgent.GetNextPathPosition() - character.GlobalPosition).Normalized();

		if (navAgent.GetPathLength() < 100.0f) 
		{
			(character as Character).velocity = Vector2.Zero;
			if (character != null) (character as Character).animation_name = "idle_";
			return;
		}
		
		(character as Character).velocity = direction * (character as Character).Speed;
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
