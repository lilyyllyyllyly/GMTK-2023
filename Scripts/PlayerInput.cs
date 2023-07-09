using Godot;
using System;

public partial class PlayerInput : Node2D, IInput
{
	public Node2D player;

	[Export] private float _rotOffset;

	public Vector2 direction 
	{
		get
		{
			return Input.GetVector("left", "right", "up", "down");
		}
	}

	public float rot
	{
		get
		{
			if (!IsInstanceValid(player)) return 0;
			return Utils.AngleBetweenPoints(player.GlobalPosition, GetGlobalMousePosition(), _rotOffset);
		}
	}

	public bool shoot
	{
		get
		{
			return Input.IsActionJustReleased("shoot");
		}
	}
}
