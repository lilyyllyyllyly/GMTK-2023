using Godot;
using System;

public partial class DummyInput : Node, IInput
{
	public Vector2 direction { get { return Vector2.Zero; } }
	public float rot { get { return 0; } }
	public bool shoot { get { return false; } }
}
