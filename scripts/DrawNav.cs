using Godot;
using Godot.Collections;
using ProjectTD.scripts;

public class DrawNav : Node2D {
	[Export()] public bool enabled { get; set; } = true;
	private bool shouldDraw = false;

	private AStar _aStar;
	private Array<bool> _obstacles;
	private Vector2 _gridSize;

	public Array<Vector2> Path { get; set; } = new Array<Vector2>();

	public override void _Ready() { }


	public override void _Draw() {
		if (enabled && shouldDraw) {
			Vector3 vector3;
			Vector2 pointPos = new Vector2();
			Vector2 otherPointPos = new Vector2();
			for (var point = 0; point < _obstacles.Count; point++) {
				if (!_aStar.HasPoint(point)) continue;
				vector3 = _aStar.GetPointPosition(point);
				pointPos.Set(vector3.x + Nav.QUARTER_TILE_SIZE, vector3.z + Nav.QUARTER_TILE_SIZE);
				// draw obstacles
				if (_obstacles[point]) {
					DrawCircle(pointPos, 10, Colors.Red);
				}
				else {
					DrawCircle(pointPos, 10, Colors.Green);
				}

				// draw connections
				foreach (var otherPoint in _aStar.GetPointConnections(point)) {
					vector3 = _aStar.GetPointPosition(otherPoint);
					otherPointPos.Set(vector3.x + Nav.QUARTER_TILE_SIZE, vector3.z + Nav.QUARTER_TILE_SIZE);
					DrawLine(pointPos, otherPointPos, Colors.Black);
				}
			}

			if (Path.Count > 0) {
				for (var i = 0; i < Path.Count - 1; i++) {
					DrawLine(Path[i], Path[i + 1], Colors.Yellow, 10);
				}
			}
		}
	}

	public void init(AStar aStar, Array<bool> obstacles, Vector2 gridSize) {
		_aStar = aStar;
		_obstacles = obstacles;
		_gridSize = gridSize;
		shouldDraw = true;
		Update();
	}

	private int toArrayPos(int x, int y) {
		return x * (int) _gridSize.x + y;
	}
}