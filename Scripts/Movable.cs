using Godot;
using System;

public partial class Movable : CharacterBody2D
{
    protected IInput _input;
    [Export] private NodePath _inputPath;

    [Export] protected float _maxSpeed;
    [Export] protected float _maxAccel;
    public bool isMoving;

    public override void _Ready() 
    {
        if (_inputPath != "") _input = GetNode<IInput>(_inputPath);
    }

    public override void _PhysicsProcess(double delta)
    {
        Move(delta);
    }

    protected virtual void Move(double delta)
    {
        if (!IsInstanceValid((Node)_input)) 
        {
            return;
        }

        isMoving = _input.direction.Length() > 0;

        Vector2 goalSpeed = _input.direction * _maxSpeed;
        Vector2 neededAccel = goalSpeed - Velocity;

        if (neededAccel.Length() > _maxAccel) {
            neededAccel = neededAccel.Normalized() * _maxAccel;
        }

        Velocity += neededAccel;
        MoveAndSlide();
    }
}
