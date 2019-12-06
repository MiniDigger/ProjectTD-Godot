using Godot;
using System;
using ProjectTD.scripts;

public class TurretDebug : Node2D {
	private int _range = 200;
	private bool _shouldDraw = false;
	private Enemy _target = null;

	public void init(int range, Enemy target = null) {
		_range = range;
		_shouldDraw = true;
		_target = target;
		Update();
	}

	public override void _Draw() {
		if (!_shouldDraw) return;

		Vector2 pos = Position + new Vector2(Nav.HALF_TILE_SIZE, Nav.HALF_TILE_SIZE);
		DrawCircleLine(
			pos,
			_range,
			Colors.Black);

		if (_target != null) {
			Vector2 targetPos = _target.getPosition() - GlobalPosition;
			DrawCircle(targetPos, 10, Colors.Red);
			DrawLine(targetPos, pos, Colors.Red, 5);
		}
	}

	private void DrawCircleLine(Vector2 center, int radius, Color color, int nbPoints = 64) {
		var pointsArc = new Vector2[nbPoints];

		for (int i = 0; i < nbPoints; ++i) {
			float anglePoint = Mathf.Deg2Rad(i * 360f / nbPoints - 90f);
			pointsArc[i] = center + new Vector2(Mathf.Cos(anglePoint), Mathf.Sin(anglePoint)) * radius;
		}

		for (int i = 0; i < nbPoints - 1; ++i) {
			DrawLine(pointsArc[i], pointsArc[i + 1], color);
		}

		DrawLine(pointsArc[nbPoints - 1], pointsArc[0], color);
	}
}