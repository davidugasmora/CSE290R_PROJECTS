using Godot;
using System;
using System.Drawing;
using System.Threading.Tasks;

[GlobalClass]
public partial class GuardCatchState : CharacterState
{
	
	[Export]
	public Area2D captureArea { get; set;} = null;

	private string captureVisualScene = "res://StateMachine/Character/GuardStates/prisoner_capture_visual.tscn";

	public override bool EvaluateStateCondition()
	{
		return (character as Guard).state == Guard.GuardStates.Catch;
	}

	private bool caughtPrisonerIsPlayer = false;

	public override void Enter()
	{
		base.Enter();
		
		if (character != null) (character as Character).animation_name = "idle_";

		if ((character as Guard)._targetPrisoner is Character)
		{
			Character caughtPrisoner = (character as Guard)._targetPrisoner as Character;
			
			PrisonerCaptureVisual captureVisual = (GD.Load(captureVisualScene) as PackedScene).Instantiate() as PrisonerCaptureVisual;
			character.GetParent().AddChild(captureVisual);
			captureVisual.GlobalPosition = caughtPrisoner.GlobalPosition;
			captureVisual.setPrisonerSpriteFrames(caughtPrisoner.getAnimatedSprite().SpriteFrames);
			captureVisual.Connect("CageFading",new Callable(this,"endCatch"));

			if (caughtPrisoner == Global.Instance.player)
			{
				Player player = caughtPrisoner as Player;
				player.PrepareFree();
				player.RemoveChild(player.camera);
				captureVisual.AddChild(player.camera);
				caughtPrisonerIsPlayer = true;
			}
			else if (caughtPrisoner is Ally)
			{
				Ally ally = caughtPrisoner as Ally;
				int allyId = ally.id;

				Global.Instance.allyDict[allyId]["isImprisoned"] = true;
				ally.EmitSignal("Captured");
				ally.RemoveFromGroup("Prisoner");
			}

			caughtPrisoner.QueueFree();
			
		}

		(character as Guard)._targetPrisoner = null;
	}

	public async Task endCatch()
	{
		if (caughtPrisonerIsPlayer)
		{
			await Global.Instance.TransitionWorldScene("Prison",0);
		}
		(character as Guard).state = Guard.GuardStates.Pause;
	}
}
