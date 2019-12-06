using Godot;
using System;
using Godot.Collections;
using ProjectTD.scripts;
using ProjectTD.scripts.data;
using ProjectTD.scripts.hud;

public class Enemy : Node2D {
	[Export(PropertyHint.Range, "0,100000")]
	public float maxHealth { get; set; }

	[Export(PropertyHint.Range, "0,100000")]
	public int moneyBounty { get; set; }

	[Export(PropertyHint.Range, "0,100000")]
	public int pointBounty { get; set; }

	[Export(PropertyHint.Range, "0,100000")]
	public int penalty { get; set; } = 1;

	[Export(PropertyHint.Range, "0,100000")]
	public int speed { get; set; } = 350;

	public float health { get; private set; }
	public Sprite Sprite { get; private set; }

	private Healthbar _healthbar;
	private Path2D _path2d;
	internal PathFollow2D _rotatingPathFollow;
	private PathFollow2D _stillPathFollow;

	public override void _Ready() {
		_path2d = GetNode<Path2D>("Path2D");
		_rotatingPathFollow = GetNode<PathFollow2D>("Path2D/Rotating");
		Sprite = GetNode<Sprite>("Path2D/Rotating/Sprite");
		_stillPathFollow = GetNode<PathFollow2D>("Path2D/Still");
		_healthbar = GetNode<Healthbar>("Path2D/Still/Healthbar");

		health = maxHealth;

		AddToGroup("enemies");

		UpdateHealthBar();
	}

	public override void _Process(float delta) {
		//damage(delta * 10);
		if (_path2d.GetCurve() != null && _path2d.GetCurve().GetPointCount() > 0) {
			_rotatingPathFollow.Offset += speed * delta;
			_stillPathFollow.Offset += speed * delta;

			if (Math.Abs(_rotatingPathFollow.UnitOffset - 1) < 0.001) {
				GetTree().CallGroup("state", nameof(Game.removeHealth), penalty);
				QueueFree();
			}
		}
	}

	public void setPath(Curve2D curve2D) {
		GlobalPosition = curve2D.GetPointPosition(0);
		Position = new Vector2(0, 0);
		_path2d.SetCurve(curve2D);
		_rotatingPathFollow.SetOffset(0);
		_stillPathFollow.SetOffset(0);
	}

	public void damage(float damage) {
		health -= damage;
		if (health < 0) {
			GetTree().CallGroup("state", nameof(Game.addMoney), moneyBounty);
			GetTree().CallGroup("state", nameof(Game.addPoints), pointBounty);
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

	public Vector2 getPosition() {
		return Sprite.GlobalPosition;
	}

	private void UpdateHealthBar() {
		_healthbar.setValue((health / maxHealth) * 100);

		_healthbar.Visible = Math.Abs(maxHealth - health) > 0.001;
	}
}