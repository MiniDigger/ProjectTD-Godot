using Godot;
using System;
using ProjectTD.scripts;
using ProjectTD.scripts.turrets;

public class Turret : Node2D {
	[Export(PropertyHint.Range, "0,100000")]
	public int range { get; set; } = 200;

	[Export(PropertyHint.Range, "0,100000")]
	public int damage { get; set; } = 1;

	[Export(PropertyHint.Range, "0,100000")]
	public int interval { get; set; } = 1000;

	[Export(PropertyHint.Enum, "NEAREST,FURTHEST,FIRST,LAST,MIN_HEALTH,MAX_HEALTH")]
	public TurretSelectionStrategy Strategy = TurretSelectionStrategy.NEAREST;

	private TurretDebug _turretDebug;

	private float time = 0;
	private Enemy _target;

	public override void _Ready() {
		_turretDebug = GetNode<TurretDebug>("Debug");
		time = interval / 1000f;

		_turretDebug.init(range);
	}

	public override void _Process(float delta) {
		_target = selectTarget();
		_turretDebug.init(range, _target);

		time -= delta;
		if (_target != null && time < 0) {
			time = interval / 1000f;
			shoot();
		}
	}

	private void shoot() {
		GD.Print("shoot");
		_target.damage(damage);
	}

	private Enemy selectTarget() {
		Enemy current = null;
		float currentData = -1;
		Vector2 pos = GlobalPosition + new Vector2(Nav.HALF_TILE_SIZE, Nav.HALF_TILE_SIZE);
		foreach (var node in GetTree().GetNodesInGroup("enemies")) {
			if (node is Enemy enemy) {
				float distance = enemy._rotatingPathFollow.GlobalPosition.DistanceSquaredTo(pos);
				if (distance <= range * range) {
					Result result = Strategy.Calculate(current, enemy, currentData, distance);
					current = result.enemy;
					currentData = result.data;
				}
			}
		}

		return current;
	}
}