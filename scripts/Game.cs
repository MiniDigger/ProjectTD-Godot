using Godot;

namespace ProjectTD.scripts {
public class Game : Node2D {
	private Level _Level;
	private Player _player;

	private int _money;
	[Export()]
	private int _startingMoney = 0;
	private int _health;
	[Export()]
	private int _startingHealth = 10;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		_Level = GetParent<Level>();
		_player = GetNode<Player>("Player");
		_player.SetLevelWidth(_Level.LevelWidth);

		AddToGroup("state");

		_money = _startingMoney;
		addMoney(0);
		_health = _startingHealth;
		removeHealth(0);
		
		GD.Print($"Loaded level {_Level.Name}");
	}

	public void addMoney(int amount) {
		_money += amount;
		
		GetTree().CallGroup("ui", "updateMoney", _money);
	}	
	
	public void removeHealth(int amount) {
		_health -= amount;
		
		GetTree().CallGroup("ui", "updateHealth", _money);
	}
}
}