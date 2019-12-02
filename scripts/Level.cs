using System;
using Godot;
using ProjectTD.scripts.screens;

namespace ProjectTD.scripts {
public class Level : Node2D {
	[Export] public string LevelName;

	[Export(PropertyHint.Range, "0,100000")]
	public int LevelWidth;

	private PauseMenu _pauseMenu;

	public override void _Ready() {
		_pauseMenu = GetNode<PauseMenu>("PauseMenu");
	}

	public override void _Input(InputEvent input) {
		if (input.IsActionPressed("Menu")) {
			_pauseMenu.Open();
		}
	}
}
}