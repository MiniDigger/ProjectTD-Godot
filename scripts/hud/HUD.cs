using Godot;
using System;

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
		
		updateMoney(0);
		updateHealth(0);
		updatePoints(0);
		
		AddToGroup("ui");

		_root.Visible = true;
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