using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class PrisonerCaptureVisual : Node2D
{
	
	[Signal]
	public delegate void CageLandedEventHandler();
	[Signal]
	public delegate void CageFadingEventHandler();
	[Signal]
	public delegate void CageGoneEventHandler();

	[Export]
	AnimatedSprite2D animatedSprite {get; set;}
	[Export]
	Node2D cagePositionNode {get; set;}
	[Export]
	float cageAcceleration = 1000.0f;
	[Export]
	float cagePauseTime = 5.0f;
	[Export]
	float cageFadeTime = 1.0f;

	Vector2 cageTargetPos = Vector2.Zero;
	float velocity = 0.0f;

	Timer pauseTimer = new Timer();
	Timer fadeTimer = new Timer();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cagePositionNode.Position = new Vector2(0.0f,-100.0f);

		
		pauseTimer.WaitTime = cagePauseTime;
		pauseTimer.OneShot = true;
		AddChild(pauseTimer);
		pauseTimer.Timeout += PauseTimeout;

		fadeTimer.WaitTime = cageFadeTime;
		fadeTimer.OneShot = true;
		AddChild(fadeTimer);
		fadeTimer.Timeout += FadeTimeout;
	}
	
	public void PauseTimeout()
	{
		EmitSignal("CageFading");
		fading = true;
		fadeTimer.Start();
	}

	public void FadeTimeout()
	{
		EmitSignal("CageGone");
		QueueFree();
	}
 
	public void setPrisonerSpriteFrames(SpriteFrames spriteFrames)
	{
		animatedSprite.SpriteFrames = spriteFrames.Duplicate() as SpriteFrames;
		animatedSprite.Play("idle_down");
	}

	private bool landed = false;
	private bool fading = false;

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!landed)
		{
			Vector2 vectorToTarget = cageTargetPos - cagePositionNode.Position;

			velocity += cageAcceleration * (float)delta;

			cagePositionNode.Position += Math.Min(velocity * (float)delta,vectorToTarget.Length()) * vectorToTarget.Normalized();

			if (vectorToTarget == Vector2.Zero)
			{
				landed = true;
				pauseTimer.Start();
				EmitSignal("CageLanded");
			}
		}
		else if (fading)
		{
			Modulate = new Color(1.0f,1.0f,1.0f,(float)fadeTimer.TimeLeft / cageFadeTime);
		}
	}
}
