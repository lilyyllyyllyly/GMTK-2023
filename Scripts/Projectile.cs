using Godot;
using System;

public partial class Projectile : Area2D
{
	[Export] private float _lifetime;
	private float _currLife;

	[Export] private float speed;

	public override void _PhysicsProcess(double delta)
	{
		if (_currLife >= _lifetime) QueueFree();
		Position += Utils.RotateVector(Vector2.Right, Rotation) * speed;
		_currLife += (float)delta;
	}

	// Connected to Projectile (Area2D) area_entered(area: Area2D)
	public void Collide(Node area) {
		QueueFree();
	}
}
