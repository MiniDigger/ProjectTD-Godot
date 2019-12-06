using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;
using ProjectTD.scripts.data;
using ProjectTD.scripts.screens;
using ProjectTD.scripts.wave;
using Array = Godot.Collections.Array;

namespace ProjectTD.scripts {
public class Level : Node2D {
	[Export] public string LevelName;

	[Export(PropertyHint.Range, "0,100000")]
	public int LevelWidth;

	private PauseMenu _pauseMenu;
	private Nav _nav;
	private Node _enemies;
	private Node _timers;

	private bool done = false;
	private List<Wave> waves = new List<Wave>();
	private int currentWave = -1;

	private PackedScene _enemy = GD.Load<PackedScene>("res://scenes/enemies/Enemy.tscn");
	
	public override void _Ready() {
		_pauseMenu = GetNode<PauseMenu>("PauseMenu");
		_nav = GetNode<Nav>("Nav");
		_enemies = GetNode<Node>("Enemies");
		_timers = GetNode<Node>("Timers");
		
		loadData();
		startNextWave();
	}

	public override void _Input(InputEvent input) {
		if (input.IsActionPressed("Menu")) {
			_pauseMenu.Open();
		}
	}

	private void loadData() {
		waves = FileHandler.loadJson<List<Wave>>($"res://data/waves/{LevelName}.json");
		GD.Print($"Loaded {waves.Count} waves");
	}

	private void startNextWave() {
		if (currentWave == waves.Count) {
			GD.Print("END");
			return;
		}
		currentWave++;
		
		for (var i = 0; i < waves[currentWave].groups.Count; i++) {
			WaveGroup waveGroup = waves[currentWave].groups[i];
			
			// create timer, set name and initial delay
			Timer timer = new Timer();
			timer.Name = waveGroup.name;
			bool hasDelay = waveGroup.delay > 0;
			if (hasDelay) {
				timer.SetWaitTime(waveGroup.delay / 1000f);
			}
			else {
				timer.SetWaitTime(waveGroup.interval / 1000f);
			}
			
			// connect signal
			Array param = new Array();
			param.Add(timer);
			param.Add(i);
			param.Add(hasDelay);
			timer.Connect("timeout", this, nameof(timerTimeout), param);
			timer.Start();
			
			_timers.AddChild(timer);
		}
	}

	private void timerTimeout(Timer timer, int waveGroupId, bool isInitialDelay) {
		WaveGroup waveGroup = waves[currentWave].groups[waveGroupId];
		if (isInitialDelay) {
			// inital wait is over, set the real deal now
			timer.Stop();
			timer.WaitTime = waveGroup.delay / 2000f;
			timer.Start();
			
			// reconnect signal
			timer.Disconnect("timeout", this, nameof(timerTimeout));
			Array param = new Array();
			param.Add(timer);
			param.Add(waveGroupId);
			param.Add(false);
			timer.Connect("timeout", this, nameof(timerTimeout), param);
			
			GD.Print($"Initial delay for {waveGroup.name} is over");
		}
		
		spawn(waveGroup);
		
		if (waveGroup.count == 0) {
			GD.Print($"Wavegroup {waveGroup.name} is over");
			timer.QueueFree();
		}
	}

	private void spawn(WaveGroup waveGroup) {
		GD.Print($"Spawn {waveGroup.name}");
		
		Enemy enemy = (Enemy) _enemy.Instance();
		enemy.speed = waveGroup.speed;
		enemy.maxHealth = waveGroup.health;
		enemy.moneyBounty = waveGroup.money;
		enemy.pointBounty = waveGroup.points;
		enemy.penalty = waveGroup.penalty;
		_enemies.AddChild(enemy);
		enemy.setPath(_nav.getPathCurve(waveGroup.spawn, waveGroup.goal));
		enemy.Sprite.Texture = GD.Load<Texture>(waveGroup.sprite);

		waveGroup.count--;
	}
}
}