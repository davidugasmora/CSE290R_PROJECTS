using Godot;
using Godot.Collections;
using System;

public partial class ShotWebVisual : Node2D
{
	[Export]
	public Sprite2D webHead;
	[Export]
	public Sprite2D webTailTemplate;
	[Export]
	public float webSpeed = 4000f;

	private Vector2 webOrigin = Vector2.Zero;
	private Vector2 webPosition = Vector2.Zero;
	private Vector2 webTargetPosition = Vector2.Zero;
	private Array<Sprite2D> webTails = [];
	private bool shot = false;

	[Signal]
	public delegate void ReachedTargetEventHandler();


    public override void _Ready()
    {
        base._Ready();
    }


	public void shoot(Vector2 setOrigin, Vector2 setTargetPosition)
	{
		Visible = true;
		shot = true;
		webPosition = setOrigin;
		webOrigin = setOrigin;
		webTargetPosition = setTargetPosition;

		updateWeb();
	}

	public void webReturn()
	{
		webTargetPosition = webOrigin;
	}

	public void webEnd()
	{
		Visible = false;
		for (int webSegment = webTails.Count - 1; webSegment > 0; webSegment--)
		{
			webTails[webSegment].QueueFree();
			webTails.RemoveAt(webSegment);
		}
		webHead.Position = Vector2.Zero;

		shot = false;
	}

	public void setOrigin(Vector2 setOrigin)
	{
		webOrigin = setOrigin;
	}

	[Export]
	public float webSegmentLength = 64.0f;

	public void updateWeb()
	{

		Vector2 webDirection = (webPosition - webOrigin).Normalized();
		float webLength = (webPosition - webOrigin).Length();

		int webSegments = (int)Math.Ceiling(webLength / webSegmentLength);

		for (int webSegment = 0; webSegment < webSegments ; webSegment++)
		{

			if (webSegment == webSegments - 1) // Head Segment{
			{
				webHead.GlobalPosition = webPosition - webDirection * webSegmentLength / 2.0f;
				webHead.GlobalRotation = webDirection.Angle();
			}
			else
			{
				if (webSegment >= webTails.Count)
				{
					webTails.Add(webTailTemplate.Duplicate() as Sprite2D);
					AddChild(webTails[webSegment]);
					MoveChild(webTails[webSegment],0);
					webTails[webSegment].Visible = true;
				}

				webTails[webSegment].GlobalPosition = webOrigin + ((float)webSegment * webSegmentLength) * webDirection + webDirection * webSegmentLength / 2.0f;;
				webTails[webSegment].GlobalRotation = webDirection.Angle();
			}
		}

		for (int webSegment = webTails.Count - 1; webSegment > webSegments - 1; webSegment--)
		{
			webTails[webSegment].QueueFree();
			webTails.RemoveAt(webSegment);
		}

		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!shot) return;

		if (webPosition != webTargetPosition)
		{
			float distanceToWebTarget = (webTargetPosition - webPosition).Length();
			Vector2 directionToWebTarget = (webTargetPosition - webPosition).Normalized();
			webPosition += directionToWebTarget * Math.Min(webSpeed * (float)delta, distanceToWebTarget);

			if (webPosition == webTargetPosition)
			{
				EmitSignal("ReachedTarget");
			}
		}
		
		updateWeb();
	}
}
