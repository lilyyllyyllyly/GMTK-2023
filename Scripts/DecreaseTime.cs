using Godot;
using System;

public partial class DecreaseTime : Timer
{
	[Export] private float _div = 10000;
	[Export] private float _min = 0.5f;
	private float _defWait;

	public override void _Ready()
	{
		_defWait = (float)WaitTime;
	}

	public override void _Process(double delta)
	{
		WaitTime = Mathf.Clamp(_defWait - Time.GetTicksMsec()/_div, _min, _defWait);
	}
}
