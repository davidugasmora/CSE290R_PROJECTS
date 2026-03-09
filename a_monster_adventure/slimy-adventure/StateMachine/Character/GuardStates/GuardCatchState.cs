using Godot;
using System;
using System.Drawing;

[GlobalClass]
public partial class GuardCatchState : CharacterState
{

	public override bool EvaluateStateCondition()
    {
		return (character as Guard).state == Guard.GuardStates.Catch;
    }

	public override void Enter()
	{
		GD.Print("Caught");
		GetTree().Quit();
	}
}
