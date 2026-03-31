using Godot;
using System;
using System.Collections.Generic;

public partial class Dialogue : CanvasLayer
{
	private double CHAR_READ_RATE = 0.02;
	private MarginContainer textboxContainer;
	private Label startSymbol;
	private Label endSymbol;
	private Label label;
	
	private enum State { READY, READING, FINISHED };
	private State currentState = State.READY;
	
	private Queue<string> dialogueQueue = new Queue<string>();
	
	private Tween tween;
	
	public override void _Ready() {
		textboxContainer = GetNode<MarginContainer>("TextboxContainer");
		startSymbol = GetNode<Label>("TextboxContainer/Panel/MarginContainer/HBoxContainer/Start");
		endSymbol = GetNode<Label>("TextboxContainer/Panel/MarginContainer/HBoxContainer/End");
		label = GetNode<Label>("TextboxContainer/Panel/MarginContainer/HBoxContainer/Label");
		
		GD.Print("Starting State: State.READY");
		
		HideTextbox();
		QueueText("This is the first line of text! Adding more lines so that it takes more time to finish.");
		QueueText("This is the second line of text! Adding more lines so that it takes more time to finish.");
		QueueText("This is the third line of text! Adding more lines so that it takes more time to finish.");
		QueueText("This is the fourth line of text! Adding more lines so that it takes more time to finish.");

	}

	public override void _Process(double delta) {
		switch (currentState) {
			case State.READY: 
				if (dialogueQueue.Count != 0) {
					DisplayText();
				}
				break;
			case State.READING:
				if (Input.IsActionJustPressed("ui_accept")) {
					label.VisibleRatio = 1.0f;
					StopTween();
					endSymbol.Text = "v";
					ChangeState(State.FINISHED);
				}
				break;
			case State.FINISHED:
				if (Input.IsActionJustPressed("ui_accept")) 
				{
					ChangeState(State.READY);
					HideTextbox();
				}
				break;
					
		}
	}
	
	public void QueueText(string nextText) {
		dialogueQueue.Enqueue(nextText);
	}
	
	public void HideTextbox() {
		startSymbol.Text = "";
		endSymbol.Text = "";
		label.Text = "";
		textboxContainer.Hide();
	}
	
	public void ShowTextbox() {
		startSymbol.Text = "*";
		textboxContainer.Show();
	}
	
	public void DisplayText() {
		string nextText = dialogueQueue.Dequeue();
		label.Text = nextText;
		label.VisibleRatio = 0.0f;
		ChangeState(State.READING);
		ShowTextbox();
		
		StartTween(nextText);
	}
	
	private void OnTweenFinished(){
		endSymbol.Text = "v";
		ChangeState(State.FINISHED);
	}
	
	private void ChangeState(State nextState) {
		currentState = nextState;
		switch (currentState) {
			case State.READY: 
				GD.Print("Changing to: State.READY");
				break;
			case State.READING:
				GD.Print("Changing to: State.READING");
				break;
			case State.FINISHED:
				GD.Print("Changing to: State.FINISHED");
				break;
		}
	}
	
	private void StartTween(string nextText){
		// Kill existing tween if it exists
		if (tween != null && tween.IsRunning()) {
			tween.Kill();
		}

		tween = CreateTween();

		tween.TweenProperty(
			label,
			"visible_ratio",
			1.0f,
			nextText.Length * CHAR_READ_RATE
		).From(0.0f);

		tween.Finished += OnTweenFinished;
	}
	
	private void StopTween() {
		if (tween != null && tween.IsRunning()){
			tween.Kill();
		}
	}
}
