
using Godot;
using Godot.Collections;

[GlobalClass]
public partial class ImprisonedAllyInstantiator : Node2D
{
    [Export]
    public int allyId {get; set;} = 0;

    public void DoInstantiation()
    {
        if (Global.Instance.allyDict[allyId]["isImprisoned"].AsBool())
        {
            Ally newAlly = (GD.Load(Global.Instance.allyDict[allyId]["allyScene"].AsString()) as PackedScene).Instantiate() as Ally;
            GetParent().CallDeferred("add_child",newAlly);
            newAlly.GlobalPosition = GlobalPosition;
            newAlly.id = allyId;
            newAlly.Connect("Captured",new Callable(this,"DoInstantiation"));
        }
    }

    public override void _Ready()
    {
        base._Ready();
        DoInstantiation();
    }
}