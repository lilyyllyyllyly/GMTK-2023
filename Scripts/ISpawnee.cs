using Godot;
using System;

public interface ISpawnee
{
	int id { get; set; }
	event Action<int> die;
}
