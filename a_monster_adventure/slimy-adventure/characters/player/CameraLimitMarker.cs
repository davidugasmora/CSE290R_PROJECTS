using Godot;

[GlobalClass]
public partial class CameraLimitMarker : Node2D
{
	public enum CameraLimitType
	{
		TopLeft = 0,
		BottomRight = 1
	}

	[Export]
	public CameraLimitType limitType {get; set;} = CameraLimitType.TopLeft;

	public override void _Ready()
	{
		base._Ready();
		Global.Instance.setCameraLimits(GlobalPosition,(int)limitType);
	}
}
