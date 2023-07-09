using Godot;
using System.Collections.Generic;

public partial class Spawner : Node
{
	private Vector2[] spawnPoints;
	private bool[] occupied;
	private int spawnCount;

	[Export] private PackedScene spawnee;

	public override void _Ready()
	{
		// Filling spawnPoints array with the positions of the children
		spawnCount = GetChildCount();
		Godot.Collections.Array<Node> children = GetChildren();

		spawnPoints = new Vector2[spawnCount];
		for (int i = 0; i < spawnCount; ++i) {
			spawnPoints[i] = ((Node2D)children[i]).Position;
		}

		occupied = new bool[spawnCount];
	}

	// Connected to (Timer) timeout()
	public void Spawn()
	{
		int? r;
		if ((r = ChooseLocation()) == null) return;

		Node2D newSpawn = (Node2D)spawnee.Instantiate();
		newSpawn.Position = spawnPoints[(int)r];
		GetTree().CurrentScene.AddChild(newSpawn);
		
		ISpawnee ispawnee = newSpawn.GetNode<ISpawnee>(".");
		ispawnee.id = (int)r;
		ispawnee.die += (id) => occupied[id] = false;

		occupied[(int)r] = true;
	}

	private int? ChooseLocation()
	{
		List<int> availableLocations = new List<int>();
		for (int i = 0; i < spawnCount; ++i) {
			if (!occupied[i]) {
				availableLocations.Add(i);
			}
		}

		if (availableLocations.Count < 1) return null;

		GD.Randomize();
		int r = (int) (GD.Randi() % availableLocations.Count);
		return availableLocations[r];
	}
}
