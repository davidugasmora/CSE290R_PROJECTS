using Godot;
using System;

[GlobalClass]
public partial class StateCondition : Node
{
	public enum ExclusiveLevels
	{
    	None,
    	Branch,
    	Tree
	}

    [Export]
    public ExclusiveLevels exclusivity {get; set;} = ExclusiveLevels.Branch;

    public virtual bool EvaluateStateCondition()
    {
        return true;
    }

    public StateHandler SearchForStateHandler(Node searchedNode = null)
	{
		Node searchedParent = null;
		if (searchedNode != null)
		searchedParent = searchedNode.GetParent();
		else
		searchedParent = GetParent();

		if (!(searchedParent is StateHandler)) {
			if (searchedParent != null)
			{
				return SearchForStateHandler(searchedParent);
			}
			else
			{
				return null;
			}
		}
		else
		{
			return searchedParent as StateHandler;
		}
	}
}
