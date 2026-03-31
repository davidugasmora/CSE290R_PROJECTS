using Godot;
using System;
using Godot.Collections;

public partial class Ending : Control
{
	private Label finishTime;
	private Label endMessage;
	private Label congratsMessage;
	private Godot.Button continueButton;
	private Array<Ally> allies = [];
	private Array<AnimatedSprite2D> allyBoatSprites;
	
	public override void _Ready()
	{
		finishTime = GetNode<Label>("VBoxContainer/Time");
		endMessage = GetNode<Label>("VBoxContainer/Message");
		congratsMessage = GetNode<Label>("VBoxContainer/Label");
		continueButton = GetNode<Godot.Button>("Continue");
		allyBoatSprites = [GetNode<AnimatedSprite2D>("Ally1"), GetNode<AnimatedSprite2D>("Ally2")];
		
		Player player = Global.Instance.player;
		allies = player == null ? [] : player.followingAllies;
		//for testing purposes
		//allies = [new Ally(),new Ally()];
		//allies[0].id = 1;
		//allies[1].id = 2;
		
		displayAllies();
		displayResults();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void displayResults() {
		// change the text based on how many allies escaped
		// switch to credits scene
		
		finishTime.Text = "Time: Placeholder";
		finishTime.VisibleRatio = 0.0f;
		
		if (allies.Count == 2) {
			endMessage.Text = "Well done, You are strongest together!";
		}
		else if (allies.Count == 1) {
			endMessage.Text = "I guess one is better than none...";
		}
		else {
			endMessage.Text = "Wow... so much for teamwork...";
		}
		endMessage.VisibleRatio = 0.0f;
		
		var tween = CreateTween();
		var charReadRate = 0.075f;

		tween.TweenProperty(congratsMessage, "visible_ratio", 1.0f, congratsMessage.Text.Length * charReadRate).From(0.0f);
		tween.TweenInterval(0.5f); 
		tween.TweenProperty(endMessage, "visible_ratio", 1.0f, endMessage.Text.Length * charReadRate);
		tween.TweenInterval(0.5f); 
		tween.TweenProperty(finishTime, "visible_ratio", 1.0f, finishTime.Text.Length * charReadRate);
		tween.TweenInterval(0.5f); 
		tween.Finished += () => { continueButton.Disabled = false; };
	}
	
	public void displayAllies() {
		int currentSpot = 0;
		foreach (Ally ally in allies) {
			allyBoatSprites[currentSpot].Visible = true;
			if (ally.id == 0) {
				allyBoatSprites[currentSpot].Play("default");
			}
			else if (ally.id == 1) {
				allyBoatSprites[currentSpot].Play("ghost");
			}
			else if (ally.id == 2) {
				allyBoatSprites[currentSpot].Play("spider");
			}
			currentSpot += 1;
			
			GD.Print(ally);
			
		}
	}
	
	private void _on_continue_button_down() {
		GetTree().ChangeSceneToFile("res://screens/credits/credits.tscn");
	}
	
}
