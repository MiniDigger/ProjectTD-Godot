using System;
using Godot;

namespace ProjectTD.scripts {
public class TurretPlacement : Node2D {
	private Sprite _sprite;

	private bool _enabled;

	public override void _Ready() {
		_sprite = GetNode<Sprite>("Sprite");

		AddToGroup("turret-placement");
	}

	public void toggle(bool newState) {
		_enabled = newState;
		Visible = newState;
	}

	public override void _Input(InputEvent input) {
		if (!_enabled) return;
		if (input is InputEventMouseMotion) {
			_sprite.GlobalPosition = new Vector2(
				toGrid(_sprite.GetGlobalMousePosition().x) * Nav.TILE_SIZE,
				toGrid(_sprite.GetGlobalMousePosition().y) * Nav.TILE_SIZE
			);
		}
		else if (input is InputEventMouseButton) {
			GetTree().CallGroup("level", nameof(Level.placeTurret), _sprite.GlobalPosition, "SomeTurret");
			GetTree().CallGroup("Nav", nameof(Nav.addTower),
				toGrid(_sprite.GetGlobalMousePosition().x) * 2,
				toGrid(_sprite.GetGlobalMousePosition().y) * 2
			);
		}
	}

	private float toGrid(float f) {
		return Mathf.Round((f - Nav.HALF_TILE_SIZE) / Nav.TILE_SIZE);
	}
}
}