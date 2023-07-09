using Godot;
using System;

public partial class Character : Movable
{
	[Export] private NodePath _weaponPath;
	private Weapon _weapon;
	public bool stun;
	private Tentacle _tentacle;

	[Export] private NodePath _stunPath;
	private Timer _stunTimer;

	public override void _Ready()
	{
		base._Ready();

		if (_weaponPath != "") _weapon = GetNode<Weapon>(_weaponPath);
		ChangeInput(_input);

		_stunTimer = GetNode<Timer>(_stunPath);
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

		ChangeInput((IInput)(new DummyInput()));
		stun = true;
		_stunTimer.Start();
		area.GetNode<ISpawnee>("..").die += (id) => StunRelease();
	}

	// Connected to StunTimer (Timer) timeout()
	public void StunRelease()
	{
		stun = false;
	}

	// Connected to AttackCheck (Area2D) area_entered(area: Area2D)
	public void AttackCollide(Node2D area)
	{
		if (!stun) return;

		_tentacle = area.GetNode<Tentacle>("..");
		_tentacle.caught = true;
		_tentacle.killTimer.Timeout += QueueFree;
		area.GetNode<ISpawnee>("..").die += OnTentacleDie;

		stun = false;
		CallDeferred("Disable");
	}

	public void OnTentacleDie(int id)
	{
		CallDeferred("Enable");
	}

	private void Disable()
	{
		Hide();
		ProcessMode = (ProcessModeEnum)4; // PROCESS_MODE_DISABLED
	}

	private void Enable()
	{
		Show();
		ProcessMode = (ProcessModeEnum)0; // PROCESS_MODE_INHERIT
	}

	public override void _ExitTree()
	{
		if (!IsInstanceValid(_tentacle)) return;
		_tentacle.GetNode<ISpawnee>(".").die -= OnTentacleDie;
		_tentacle.GetNode<ISpawnee>(".").die -= (id) => StunRelease();
		_tentacle.killTimer.Timeout -= QueueFree;
	}
}
