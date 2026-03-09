using Godot;
using System;

[GlobalClass]
public partial class StateHandlerLink : Node
{
	[Export]
	public StateHandler reference = null;
}
