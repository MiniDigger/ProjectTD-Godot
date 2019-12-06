using System.Collections.Generic;
using Godot.Collections;

namespace ProjectTD.scripts.wave {
public class Wave {
	public string name { get; set; }
	public List<WaveGroup> groups { get; set; } = new List<WaveGroup>();
	public WaveType type { get; set; } = WaveType.NORMAL;
	public int points { get; set; } = 100;
	public int money { get; set; } = 10;
}
}