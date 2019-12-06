using Godot;

namespace ProjectTD.scripts.wave {
public class WaveGroup {
	
	public string name { get; set; }
	public int delay { get; set; } = 0;
	public int interval { get; set; } = 0;
	public int health { get; set; } = 0;
	public int speed { get; set; } = 0;
	public MinionType type { get; set; } = MinionType.LAND;
	public int count { get; set; } = 0;
	public string sprite { get; set; } = "error";
	public Vector2 spawn { get; set; }
	public Vector2 goal { get; set; }
	public int points { get; set; } = 100;
	public int money { get; set; } = 10;
	public int penalty { get; set; } = 1;
}
}