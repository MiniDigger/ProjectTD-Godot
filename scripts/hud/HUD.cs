using Godot;
using System;

public class HUD : CanvasLayer {

	private Label _money;
	private Label _health;
	
	public override void _Ready() {
		_money = GetNode<Label>("Root/Right/Money");
		_health = GetNode<Label>("Root/Right/Health");
		
		updateMoney(0);
		updateHealth(0);
		
		AddToGroup("ui");
	}

	public void updateMoney(int newVal) {
		_money.SetText(newVal + " $");
	}
	
	public void updateHealth(int newVal) {
		_health.SetText(newVal + " <3");
	}
}