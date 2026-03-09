using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class CharacterState : State
{
	public CharacterBody2D character { get; set; } = null;

    public override void _Ready()
    {
        base._Ready();
		StateHandler stateHandler = SearchForStateHandler();
		if (!(stateHandler is CharacterStateHandler))
		{
			GD.PushWarning("CharacterState must be parented to CharacterStateHandler to work silly.");
		}
    }

    public override void HandlerReady()
    {
        base.HandlerReady();
		StateHandler stateHandler = SearchForStateHandler();
		if (stateHandler is CharacterStateHandler)
		{
			character = (stateHandler as CharacterStateHandler).character;
		}
    }

	public override bool EvaluateStateCondition()
	{
		return true;
	}

    public override void Enter()
    {
        base.Enter();
		StateHandler stateHandler = SearchForStateHandler();
		if (stateHandler is CharacterStateHandler)
		{
			character = (stateHandler as CharacterStateHandler).character;
		}
    }

}
