using Godot;
using System;

public interface IInput
{
	Vector2 direction { get; }
	float rot { get; }
	bool shoot { get; }
}
