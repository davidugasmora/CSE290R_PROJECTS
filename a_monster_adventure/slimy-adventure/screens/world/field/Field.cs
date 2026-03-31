using Godot;
using System;

public partial class Field : Node2D
{
	private TileMapLayer bridgesBelow;
    private TileMapLayer bridgesAbove;

    public override void _Ready()
    {
        bridgesBelow = GetNode<TileMapLayer>("BridgesBelow");
        bridgesAbove = GetNode<TileMapLayer>("BridgesAbove");

        var layerSwitches = GetTree().GetNodesInGroup("BridgeSensor");

        foreach (Node node in layerSwitches)
        {
            if (node is BridgeSensor sensor)
            {
                sensor.ChangeLayer += OnChangeLayer;
            }
        }
    }

	private void OnChangeLayer()
    {
        bridgesBelow.Enabled = !bridgesBelow.Enabled;
        bridgesAbove.Enabled = !bridgesAbove.Enabled;
    }
}
