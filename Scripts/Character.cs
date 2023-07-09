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
		Tentacle tentacle = area.GetNode<Tentacle>("..");
		if (!tentacle.canCatch) return;

		ChangeInput((IInput)(new DummyInput()));
		stun = true;
		tentacle.canCatch = false;
	}

	// Connected to StunCheck (Area2D) area_exited(area: Area2D)
	public void StunRelease(Node2D area)
	{
		stun = false;
	}

	// Connected to AttackCheck (Area2D) area_entered(area: Area2D)
	public void AttackCollide(Node2D area)
	{
		area.GetNode<ISpawnee>("..").die += OnTentacleDie;
		area.GetNode<Tentacle>("..").killTimer.Timeout += QueueFree;
		stun = false;
		Visible = false;
		SetProcess(false);
	}

	public void OnTentacleDie(int id)
	{
		GD.Print("IT WORKED?????");
		Visible = true;
		SetProcess(true);
	}
}
