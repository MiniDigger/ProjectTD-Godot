using Godot;
using System;
using Godot.Collections;
using ProjectTD.scripts.hud;

public class Enemy : Node2D {
	[Export(PropertyHint.Range, "0,100000")]
	private float maxHealth;

	[Export(PropertyHint.Range, "0,100000")]
	private int worth;

	[Export(PropertyHint.Range, "0,100000")]
	private int speed = 350;

	private float health;


	private Healthbar _healthbar;
	private Path2D _path2d;
	private PathFollow2D _rotatingPathFollow;
	private PathFollow2D _stillPathFollow;

	public override void _Ready() {
		_path2d = GetNode<Path2D>("Path2D");
		_rotatingPathFollow = GetNode<PathFollow2D>("Path2D/Rotating");
		_stillPathFollow = GetNode<PathFollow2D>("Path2D/Still");
		_healthbar = GetNode<Healthbar>("Path2D/Still/Healthbar");

		health = maxHealth;

		UpdateHealthBar();
	}

	public override void _Process(float delta) {
		//damage(delta * 10);
		if (_path2d.GetCurve().GetPointCount() > 0) {
			_rotatingPathFollow.Offset += speed * delta;
			_stillPathFollow.Offset += speed * delta;

			if (Math.Abs(_rotatingPathFollow.UnitOffset - 1) < 0.001) {
				GD.Print("DONE");
			}
		}
	}

	public void setPath(Curve2D curve2D) {
		GD.Print("set curve!");
		GD.Print(curve2D.GetPointCount());
		Position = curve2D.GetPointPosition(0);
		_path2d.SetCurve(curve2D);
		_rotatingPathFollow.SetOffset(0);
		_stillPathFollow.SetOffset(0);
	}

	public void damage(float damage) {
		health -= damage;
		if (health < 0) {
			GetTree().CallGroup("state", "addMoney", worth);
			QueueFree();
		}
		else {
			UpdateHealthBar();
		}
	}

	public void heal(float amount) {
		health += amount;
		UpdateHealthBar();
	}

	private void UpdateHealthBar() {
		_healthbar.setValue((health / maxHealth) * 100);
	}
}