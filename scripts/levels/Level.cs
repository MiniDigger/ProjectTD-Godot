using System;
using Godot;

namespace ProjectTD.scripts.levels {
public class Level : Node2D {
	[Export] public string LevelName;

	[Export(PropertyHint.Range, "0,100000")]
	public int LevelWidth;
}
}