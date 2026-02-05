using Godot;
using System;
using System.Collections.Generic;

public partial class Character : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 300;
	
	public String facingDirection = "down";
	public String animation_name = "idle_";

	public Vector2 velocity = Vector2.Zero;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		
		Velocity = velocity;
		MoveAndSlide();
		velocity = Velocity;
		var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Play(animation_name + facingDirection);
	}
}
