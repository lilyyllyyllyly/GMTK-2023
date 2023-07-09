using Godot;
using System;

public partial class CharacterChooser : Node2D
{
	[Export] private NodePath _inputPath;
	private PlayerInput _input;

	[Export] private NodePath _selectPath;
	private Node2D _selectArea;

	public override void _Ready()
	{
		_input = GetNode<PlayerInput>(_inputPath);
		_selectArea = GetNode<Node2D>(_selectPath);
		_selectArea.SetProcess(false);
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

		if (_input.player != null) {
			_input.player.GetNode<Character>(".").ChangeInput((IInput)(new DummyInput()));
		}

		character.ChangeInput((IInput)_input);
		_input.player = body;
	}
}
