using System;
using Godot;
using ProjectTD.scripts.screens;

namespace ProjectTD.scripts {
public class Level : Node2D {
	[Export] public string LevelName;

	[Export(PropertyHint.Range, "0,100000")]
	public int LevelWidth;

	private PauseMenu _pauseMenu;
	private Nav _nav;
	private Enemy _enemy;

	private bool done = false;

	public override void _Ready() {
		_pauseMenu = GetNode<PauseMenu>("PauseMenu");
		_nav = GetNode<Nav>("Nav");
		_enemy = GetNode<Enemy>("Enemies/Enemy");
	}

	public override void _Process(float delta) {
		if (!done) {
			done = true;
			_enemy.setPath(_nav.getPathCurve(new Vector2(0, 2), new Vector2(18, 5)));
		}
	}

	public override void _Input(InputEvent input) {
		if (input.IsActionPressed("Menu")) {
			_pauseMenu.Open();
		}
	}
}
}