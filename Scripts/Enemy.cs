using Godot;
using System;

public partial class Enemy : Node2D, ISpawnee
{
	public int id { get; set; }
	public event Action<int> die;

	[Export] protected int _maxHealth;
	protected int _health;

	public override void _Ready()
	{
		_health = _maxHealth;
	}

	public override void _Process(double delta)
	{
		if (_health <= 0) {
			die?.Invoke(id);
			QueueFree();
		}
	}
}
