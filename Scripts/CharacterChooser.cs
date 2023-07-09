using Godot;
using System;

public partial class CharacterChooser : Node2D
{
	[Export] private NodePath _inputPath;
	private PlayerInput _input;

	[Export] private NodePath _selectPath;
	private Node2D _selectArea;

	private bool _firstPick = true;
	[Export] private NodePath _textPath;
	private Label _text;
	[Export] private NodePath _timerPath;
	private Timer _spawnerTimer;

	public override void _Ready()
	{
		_input = GetNode<PlayerInput>(_inputPath);
		_selectArea = GetNode<Node2D>(_selectPath);
		_selectArea.SetProcess(false);

		_text = GetNode<Label>(_textPath);
		_spawnerTimer = GetNode<Timer>(_timerPath);
	}

	public override void _Process(double delta)
	{
		_selectArea.SetProcess(Input.IsActionJustReleased("select"));
		if (Input.IsActionJustReleased("select")) {
			_selectArea.Position = GetGlobalMousePosition();
		}
	}

	public void Select(Node2D body)
	{
		Character character = body.GetNode<Character>(".");

		if (character.stun == true) return;

		if (IsInstanceValid(_input.player)) {
			_input.player.GetNode<Character>(".").ChangeInput((IInput)(new DummyInput()));
		}

		character.ChangeInput((IInput)_input);
		_input.player = body;

		if (_firstPick) {
			_text.QueueFree();
			_spawnerTimer.Start();
			_firstPick = false;
		}
	}
}
