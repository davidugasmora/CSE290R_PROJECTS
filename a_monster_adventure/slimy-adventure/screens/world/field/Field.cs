using Godot;
using System;

public partial class Field : Node2D
{
	private TileMapLayer bridgesBellow;
    private TileMapLayer bridgesAbove;

    public override void _Ready()
    {
        bridgesBellow = GetNode<TileMapLayer>("BridgesBellow");
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
        bridgesBellow.Enabled = !bridgesBellow.Enabled;
        bridgesAbove.Enabled = !bridgesAbove.Enabled;
    }
}
