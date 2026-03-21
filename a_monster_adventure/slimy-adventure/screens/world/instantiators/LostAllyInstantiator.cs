
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class LostAllyInstantiator : Node2D
{
    public void DoInstantiation()
    {

        foreach (int allyId in Global.Instance.allyDict.Keys)
        {
            if (
                Global.Instance.allyDict[allyId]["sceneName"].AsString() == Global.Instance.currentSceneName &&
                !Global.Instance.allyDict[allyId]["isFollowing"].AsBool() && 
                !Global.Instance.allyDict[allyId]["isImprisoned"].AsBool()
                )
            {
                Ally newAlly = (GD.Load(Global.Instance.allyDict[allyId]["allyScene"].AsString()) as PackedScene).Instantiate() as Ally;
                GetParent().CallDeferred("add_child",newAlly);
                newAlly.GlobalPosition = Global.Instance.allyDict[allyId]["position"].AsVector2();
                newAlly.id = allyId;
            }
        }
    }

    public override void _Ready()
    {
        base._Ready();
        DoInstantiation();
    }
}