
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class PlayerInstantiator : Node2D
{
    [Export]
    public int id {get; set;} = 0;
    [Export]
    public Node2D allySpawnNode2d;

    public string playerScene = "res://characters/player/player.tscn";

    public void DoInstantiation()
    {
        Global.Instance.player = (GD.Load(playerScene) as PackedScene).Instantiate() as Player;
        GetParent().CallDeferred("add_child",Global.Instance.player);
        Global.Instance.player.GlobalPosition = GlobalPosition;

        foreach (int allyId in Global.Instance.allyDict.Keys)
        {
            if (Global.Instance.allyDict[allyId]["isFollowing"].AsBool())
            {
                Ally newAlly = (GD.Load(Global.Instance.allyDict[allyId]["allyScene"].AsString()) as PackedScene).Instantiate() as Ally;
                GetParent().CallDeferred("add_child",newAlly);
                newAlly.GlobalPosition = allySpawnNode2d.GlobalPosition;
                Global.Instance.player.addAlly(newAlly);
                newAlly.state = Ally.AllyStates.PerfectFollow;
                newAlly.id = allyId;
            }
        }
    }
}