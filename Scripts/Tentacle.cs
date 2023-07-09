using Godot;
using System;

public partial class Tentacle : Enemy
{
	[Export] private NodePath _atkLeftPath;
	private Area2D _atkLeft;
	[Export] private NodePath _atkRightPath;
	private Area2D _atkRight;

	[Export] private NodePath _attackTimerPath;
	private Timer _attackTimer;
	[Export] private NodePath _warnTimerPath;
	private Timer _warnTimer;
	[Export] private NodePath killTimerPath;
	public Timer killTimer;

	[Export] private NodePath _spritePath;
	private Sprite2D _sprite;
	[Export] private NodePath _animPath;
	private AnimationPlayer _anim;
	
	private bool _left;
	public bool caught;

	public override void _Ready()
	{
		base._Ready();

		_atkLeft  = GetNode<Area2D>(_atkLeftPath);
		_atkRight = GetNode<Area2D>(_atkRightPath);
		_atkLeft.Visible = false;
		_atkRight.Visible = false;
		_atkLeft.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;
		_atkRight.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = true;

		_left = GlobalPosition.X < GetViewport().GetVisibleRect().Size.X/2;

		_sprite = GetNode<Sprite2D>(_spritePath);
		_sprite.ZIndex = Mathf.FloorToInt(GlobalPosition.Y);
		_sprite.FlipH = !_left;

		_attackTimer = GetNode<Timer>(_attackTimerPath);
		_warnTimer = GetNode<Timer>(_warnTimerPath);
		killTimer = GetNode<Timer>(killTimerPath);

		_anim = GetNode<AnimationPlayer>(_animPath);
	}

	private void WarnSetEnabled(bool enabled) {
		if (_left) _atkRight.Visible = enabled;
		else	    _atkLeft.Visible = enabled;
	}

	private void AttackSetEnabled(bool enabled) {
		if (_left) _atkRight.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = !enabled;
		else	    _atkLeft.GetNode<CollisionShape2D>("CollisionShape2D").Disabled = !enabled;
	}

	// Connected to AttackTimer (Timer) timeout()
	public void Attack()
	{
		WarnSetEnabled(false);
		AttackSetEnabled(true);
		_anim.Play(_left ? "AttackLeft" : "AttackRight");
	}
	
	// Connected to WarnTimer (Timer) timeout()
	public void Warn()
	{
		WarnSetEnabled(true);
	}

	// Connected to AnimationPlayer (AnimationPlayer) animation_finished(anim_name: StringName)
	public void OnAnimationFinished(StringName anim_name)
	{
		string[] animations = {"AttackLeft", "AttackRight", "Killing"};
		if (!Array.Exists(animations, e => e == anim_name)) return;

		if (caught) {
			_anim.Play("Killing");
			killTimer.Start();
			return;
		}

		AttackSetEnabled(false);
		_attackTimer.Start();
		_warnTimer.Start();
		_anim.Play("Default");
	}

	// Connected to KillTimer (Timer) timeout()
	public void OnKillFinished()
	{
		OnAnimationFinished("Killing");
		_anim.Play("Default");
		caught = false;
	}

	// Connected to Tentacle (Area2D) area_entered(area: Area2D)
	public void Collide(Area2D area)
	{
		--_health;
	}
}
