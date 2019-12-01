using Godot;

namespace ProjectTD.scripts {
public class SwipeDetector : Node {
	
	[Signal]
	public delegate void Swiped(Vector2 direction);
	[Signal]
	public delegate void SwipeCanceled(Vector2 startPosition);
	
	[Export(PropertyHint.Range, "1.0,1.5,0.1")]
	public float MaxDiagonalSlope = 1.3f;

	private Timer _timer;
	private Vector2 _startPosition;
	
	public override void _Ready() {
		_timer = GetNode<Timer>("Timer");
	}

	public override void _Input(InputEvent input) {
		if (!(input is InputEventScreenTouch touch)) return;
		
		if (touch.IsPressed()) {
			StartDetection(touch.Position);
		} else if (!_timer.IsStopped()) {
			StopDetection(touch.Position);
		}
	}


	private void StartDetection(Vector2 position) {
		_startPosition = position;
		_timer.Start();
	}
	
	private void StopDetection(Vector2 position) {
		_timer.Stop();
		var direction = (position - _startPosition).Normalized();
		// check if swipe is valid
		if (Mathf.Abs(direction.x) + Mathf.Abs(direction.y) >= MaxDiagonalSlope) {
			return;
		}

		if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y)) { // horizontal swipe
			EmitSignal(nameof(Swiped), new Vector2(-Mathf.Sign(direction.x), 0.0f));
		} else { // vertical swipe
			EmitSignal(nameof(Swiped), new Vector2(0.0f, -Mathf.Sign(direction.y)));
		}
	}

	private void _on_Timer_timeout() {
		EmitSignal(nameof(SwipeCanceled), _startPosition);
	}
}
}