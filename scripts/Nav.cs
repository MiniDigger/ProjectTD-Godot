﻿using Godot;
using Godot.Collections;

namespace ProjectTD.scripts {
public class Nav : Node2D {
	public const int TILE_SIZE = 128;
	public const int HALF_TILE_SIZE = 128 / 2;
	public const int QUARTER_TILE_SIZE = 128 / 4;

	private TileMap _tileMap;
	private DrawNav _drawNav;

	private AStar _aStar;

	private Vector2 gridSize;
	private Array<bool> obstacles = new Array<bool>();

	public override void _Ready() {
		_tileMap = GetNode<TileMap>("TileMap");
		_drawNav = GetNode<DrawNav>("DrawNav");

		calcGridSize();
		makeGrid();

		_drawNav.init(_aStar, obstacles, gridSize);

		AddToGroup("Nav");
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
			array.Add(new Vector2(point.x + QUARTER_TILE_SIZE, point.z + QUARTER_TILE_SIZE));
		}

		_drawNav.Path = array;
		_drawNav.Update();

		return array;
	}

	public void addTower(int x, int y) {
		GD.Print("AddTower " + x + " " + y);
		updateTower(x, y, true);
		_drawNav.Update();
	}

	public void removeTower(int x, int y) {
		updateTower(x, y, false);
		_drawNav.Update();
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

			vec = new Vector3(worldPos.x + HALF_TILE_SIZE, 0, worldPos.y);
			arrayPos = toArrayPos((int) cellPos.x * 2 + 1, (int) cellPos.y * 2);
			_aStar.AddPoint(arrayPos, vec);
			obstacles[arrayPos] = (bitmask & (int) TileSet.AutotileBindings.Topright) != 0;

			vec = new Vector3(worldPos.x, 0, worldPos.y + HALF_TILE_SIZE);
			arrayPos = toArrayPos((int) cellPos.x * 2, (int) cellPos.y * 2 + 1);
			_aStar.AddPoint(arrayPos, vec);
			obstacles[arrayPos] = (bitmask & (int) TileSet.AutotileBindings.Bottomleft) != 0;

			vec = new Vector3(worldPos.x + HALF_TILE_SIZE, 0, worldPos.y + HALF_TILE_SIZE);
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

			int lastX = (int) gridSize.x * 2 - 1;
			if (!obstacles[toArrayPos(lastX, y)] && !obstacles[toArrayPos(lastX, y + 1)]) {
				_aStar.ConnectPoints(toArrayPos(lastX, y), toArrayPos(lastX, y + 1));
			}
		}

		//printArray();
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