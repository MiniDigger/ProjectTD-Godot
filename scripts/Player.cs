using System;
using Godot;

namespace ProjectTD.scripts {
public class Player : KinematicBody2D {
	[Export] public int PanSpeed = 500;

	private Camera2D _camera;
	private Vector2 _motion;

	public override void _Ready() {
		_camera = GetNode<Camera2D>("Camera");
	}

	public override void _PhysicsProcess(float delta) {
		var screenWidth = GetViewport().GetVisibleRect().Size.x;

		var limitLeft = _camera.LimitLeft;
		var limitRight = _camera.LimitRight - (screenWidth * 2);

		var camX = _camera.GlobalTransform.origin.x;

		if (camX >= limitLeft && camX <= limitRight) { // check limits
			_motion.x = Input.GetActionStrength("Move_Right") - Input.GetActionStrength("Move_Left");
			MoveAndCollide(_motion * PanSpeed * delta);
		}
		else if (camX > limitRight) { // bounce back to the left
			_motion.x = -1;
			MoveAndCollide(_motion * (camX - limitRight));
		}
		else if (camX < limitLeft) { // bounce back to the right
			_motion.x = 1;
			MoveAndCollide(_motion * (limitLeft - camX));
		}
		else {
			GD.Print("Stuck...");
		}
	}

	public void SetLevelWidth(int levelWidth) {
		_camera.LimitRight = levelWidth;
	}

	public void _on_SwipeDetector_Swiped(Vector2 direction) {
		GD.Print("Swiped, implement swiping");
	}
}
}