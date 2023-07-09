using Godot;
using System;

public partial class Weapon : Node2D
{
	[Export] private PackedScene _projectile;
	[Export] private NodePath _tipPath;
	private Node2D _tip;

	public IInput _input;

	[Export] private float _shootDelay;
	private float _lastShoot;

	public override void _Ready()
	{
		_tip = GetNode<Node2D>(_tipPath);
		_lastShoot = _shootDelay;
	}

	public override void _Process(double delta)
	{
		if (!IsInstanceValid((Node)_input)) return;
		Rotation = _input.rot;

		// Shooting
		_lastShoot += (float)delta;
		if (_input.shoot && _lastShoot >= _shootDelay) {
			Node2D newProj = (Node2D)_projectile.Instantiate();
			newProj.Position = _tip.GlobalPosition;
			newProj.Rotation = Rotation;
			GetTree().CurrentScene.AddChild(newProj);

			_lastShoot = 0;
		}
	}

	public void ChangeInput(IInput input)
	{
		_input = input;
	}
}
