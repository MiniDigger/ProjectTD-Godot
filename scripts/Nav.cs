using Godot;
using Godot.Collections;

namespace ProjectTD.scripts {
public class Nav : Node2D {
	private TileMap _tileMap;

	private AStar _aStar;

	private Vector2 gridSize;
	private int halfTileSize = 128 / 2;
	private int quarterTileSize = 128 / 4;
	private Array<bool> obstacles = new Array<bool>();

	public override void _Ready() {
		_tileMap = GetNode<TileMap>("TileMap");

		calcGridSize();
		makeGrid();
	}

	public Curve2D getPathCurve(Vector2 start, Vector2 end) {
		Curve2D curve2D = new Curve2D();
		foreach (var point in getPath(start, end)) {
			curve2D.AddPoint(point);
		}

		return curve2D;
	}

	public Array<Vector2> getPath(Vector2 start, Vector2 end) {
		Array<Vector2> array = new Array<Vector2>();
		foreach (var point in _aStar.GetPointPath(
			toArrayPos((int) start.x * 2, (int) start.y * 2),
			toArrayPos((int) end.x * 2, (int) end.y * 2))
		) {
			array.Add(new Vector2(point.x, point.z));
		}

		return array;
	}

	public void addTower(int x, int y) {
		updateTower(x, y, true);
	}

	public void removeTower(int x, int y) {
		updateTower(x, y, false);
	}

	public void makeGrid() {
		_aStar = new AStar();

		var tileSet = _tileMap.GetTileset();
		var usedCells = _tileMap.GetUsedCells();

		// prefill array
		for (int i = 0; i < toArrayPos((int) gridSize.x * 2 + 1, (int) gridSize.y * 2 + 1); i++) {
			obstacles.Add(true);
		}

		int type;
		int bitmask;
		Vector2 autoTileCord;
		Vector2 worldPos;
		int arrayPos;
		Vector3 vec;
		foreach (Vector2 cellPos in usedCells) {
			type = _tileMap.GetCellv(cellPos);
			autoTileCord = _tileMap.GetCellAutotileCoord((int) cellPos.x, (int) cellPos.y);
			bitmask = tileSet.AutotileGetBitmask(type, autoTileCord);

			worldPos = _tileMap.MapToWorld(cellPos);

			vec = new Vector3(worldPos.x, 0, worldPos.y);
			arrayPos = toArrayPos((int) cellPos.x * 2, (int) cellPos.y * 2);
			_aStar.AddPoint(arrayPos, vec);
			obstacles[arrayPos] = (bitmask & (int) TileSet.AutotileBindings.Topleft) != 0;

			vec = new Vector3(worldPos.x + halfTileSize, 0, worldPos.y);
			arrayPos = toArrayPos((int) cellPos.x * 2 + 1, (int) cellPos.y * 2);
			_aStar.AddPoint(arrayPos, vec);
			obstacles[arrayPos] = (bitmask & (int) TileSet.AutotileBindings.Topright) != 0;

			vec = new Vector3(worldPos.x, 0, worldPos.y + halfTileSize);
			arrayPos = toArrayPos((int) cellPos.x * 2, (int) cellPos.y * 2 + 1);
			_aStar.AddPoint(arrayPos, vec);
			obstacles[arrayPos] = (bitmask & (int) TileSet.AutotileBindings.Bottomleft) != 0;

			vec = new Vector3(worldPos.x + halfTileSize, 0, worldPos.y + halfTileSize);
			arrayPos = toArrayPos((int) cellPos.x * 2 + 1, (int) cellPos.y * 2 + 1);
			_aStar.AddPoint(arrayPos, vec);
			obstacles[arrayPos] = (bitmask & (int) TileSet.AutotileBindings.Bottomright) != 0;
		}

		for (int y = 0; y < (int) gridSize.y * 2 - 1; y++) {
			for (int x = 0; x < (int) gridSize.x * 2 - 1; x++) {
				if (!obstacles[toArrayPos(x, y)] && !obstacles[toArrayPos(x + 1, y)]) {
					_aStar.ConnectPoints(toArrayPos(x, y), toArrayPos(x + 1, y));
				}

				if (!obstacles[toArrayPos(x, y)] && !obstacles[toArrayPos(x, y + 1)]) {
					_aStar.ConnectPoints(toArrayPos(x, y), toArrayPos(x, y + 1));
				}
			}
		}

		printArray();
	}

	private void updateTower(int x, int y, bool disabled) {
		obstacles[toArrayPos(x, y)] = disabled;
		obstacles[toArrayPos(x + 1, y)] = disabled;
		obstacles[toArrayPos(x, y + 1)] = disabled;
		obstacles[toArrayPos(x + 1, y + 1)] = disabled;
		_aStar.SetPointDisabled(toArrayPos(x, y), disabled);
		_aStar.SetPointDisabled(toArrayPos(x + 1, y), disabled);
		_aStar.SetPointDisabled(toArrayPos(x, y + 1), disabled);
		_aStar.SetPointDisabled(toArrayPos(x + 1, y + 1), disabled);
	}

	private int toArrayPos(int x, int y) {
		return x * (int) gridSize.x + y;
	}

	private void printArray() {
		for (int y = 0; y < (int) gridSize.y * 2; y++) {
			string buffer = "";
			for (int x = 0; x < (int) gridSize.x * 2; x++) {
				buffer += obstacles[toArrayPos(x, y)] ? "X" : " ";
				// if (x % 2 == 1) {
				// 	buffer += " ";
				// }
			}

			GD.Print(buffer);
			// if (y % 2 == 1) {
			// 	GD.Print("");
			// }
		}
	}

	private void calcGridSize() {
		var usedCells = _tileMap.GetUsedCells();
		int maxX = 0;
		int maxY = 0;
		foreach (Vector2 pos in usedCells) {
			if (pos.x > maxX) {
				maxX = (int) pos.x;
			}

			if (pos.y > maxY) {
				maxY = (int) pos.y;
			}
		}

		gridSize = new Vector2(maxX + 1, maxY + 1);
	}
}
}