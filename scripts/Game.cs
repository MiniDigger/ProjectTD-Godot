using Godot;
using ProjectTD.scripts.levels;

namespace ProjectTD.scripts {
public class Game : Node2D {
	private Level _Level;
	private Player _player;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		_Level = GetParent<Level>();
		_player = GetNode<Player>("Player");
		_player.SetLevelWidth(_Level.LevelWidth);

		GD.Print($"Loaded level {_Level.Name}");
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
}