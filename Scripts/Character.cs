using Godot;
using System;

public partial class Character : Movable
{
	[Export] private NodePath _weaponPath;
	private Weapon _weapon;
	public bool stun;

	public override void _Ready()
	{
		base._Ready();

		if (_weaponPath != "") _weapon = GetNode<Weapon>(_weaponPath);
		ChangeInput(_input);
	}

	public void ChangeInput(IInput input)
	{
		if (!IsInstanceValid((Node)input)) return;
		_input = input;
		if (IsInstanceValid((Node)_weapon)) _weapon.ChangeInput(input);
	}

	public void ChangeInput(Node input)
	{
		if (!IsInstanceValid(input)) return;
		ChangeInput(input.GetNode<IInput>("."));
	}

	// Connected to StunCheck (Area2D) area_entered(area: Area2D)
	public void StunCollide(Node2D area)
	{
		ChangeInput((IInput)(new DummyInput()));
		stun = true;
		area.GetNode<Tentacle>(".").canCatch = false;
	}
}
