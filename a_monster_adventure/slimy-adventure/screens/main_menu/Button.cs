using Godot;
using System;

public partial class Button : TextureButton
{
    public override void _Ready()
    {
		PivotOffset = Size / 2;
        MouseEntered += _on_mouse_entered;
        MouseExited += _on_mouse_exited;
    }

    private void _on_mouse_entered()
    {
        Scale = new Vector2(1.1f, 1.1f); // 10% bigger
    }

    private void _on_mouse_exited()
    {
        Scale = new Vector2(1.0f, 1.0f); // normal size
    }
}
