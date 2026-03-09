using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class AllyIdleState : CharacterState
{

	[Export]
	public Area2D joinArea { get; set;} = null;

	public override bool EvaluateStateCondition()
	{
		return (character as Ally).state == Ally.AllyStates.Idle;
	}

	public override void _Ready()
	{
		base._Ready();
		joinArea.BodyEntered += BodyEnters;
	}

	protected void BodyEnters(Node2D body)
	{
		if (!active)
		{
			return;
		}
		if (!(body == player))
		{
			return;
		}
		else
		{
			(character as Ally).state = Ally.AllyStates.Follow;
		}
	}

	private Player player;
	
	public override void Enter()
	{
		base.Enter();
		player = (character as Ally).player;

	}

}
