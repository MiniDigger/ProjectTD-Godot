using System.Collections.Generic;
using Godot;

namespace ProjectTD.scripts {
public class LevelList : Node2D {
	public List<string> Levels { get; } = new List<string>();

	public override void _Ready() {
		var levelDir = new Directory();
		if (levelDir.Open("res://scenes/levels") == Error.Ok) {
			levelDir.ListDirBegin();
			var levelFileName = levelDir.GetNext();
			while (levelFileName != "") {
				if (!levelDir.CurrentIsDir()) {
					if (levelFileName.EndsWith(".tscn")) {
						Levels.Add(levelFileName.Replace(".tscn", ""));
					}
				}

				levelFileName = levelDir.GetNext();
			}
		}
		else {
			GD.Print("An error occurred when trying to access the path.");
		}

		if (Levels.Count > 0) {
			GD.Print($"Found {Levels.Count} levels: {Levels}");
		}
		else {
			GD.Print("Found no levels!");
		}
	}
}
}