using Godot;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Character : CharacterBody2D
{
	private AnimatedSprite2D animatedSprite {get; set;}= null;

	[Export]
	public bool detectable = true;

	[Export]
	public int Speed { get; set; } = 250;
	
	public String facingDirection = "down";
	public String animation_name = "idle_";

	public Vector2 velocity = Vector2.Zero;

	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	public AnimatedSprite2D getAnimatedSprite() {return animatedSprite;}

	public override void _Process(double delta)
	{
		
		Velocity = velocity;
		MoveAndSlide();
		velocity = Velocity;
		animatedSprite.Play(animation_name + facingDirection);
	}
}
