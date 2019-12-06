using Godot;
using System;
using ProjectTD.scripts;

public class HUD : CanvasLayer {

	private Control _root;
	private Label _points;
	private Label _money;
	private Label _health;
	
	public override void _Ready() {
		_root = GetNode<Control>("Root");
		_points = GetNode<Label>("Root/Right/Points");
		_money = GetNode<Label>("Root/Right/Money");
		_health = GetNode<Label>("Root/Right/Health");

		AddToGroup("ui");

		_root.Visible = true;

		GetTree().CallGroup("state", nameof(Game.removeHealth), 0);
		GetTree().CallGroup("state", nameof(Game.addMoney), 0);
		GetTree().CallGroup("state", nameof(Game.addPoints), 0);
	}

	public void updateMoney(int newVal) {
		_money.SetText(newVal + " $");
	}
	
	public void updateHealth(int newVal) {
		_health.SetText(newVal + " Lives");
	}
	
	public void updatePoints(int newVal) {
		_points.SetText(newVal + " Points");
	}
}