using Godot;

namespace ProjectTD.scripts {
public class TurretPlacement : Node2D {

    private Sprite _sprite;

    private bool _enabled;
    
    public override void _Ready() {
        _sprite = GetNode<Sprite>("Sprite");
        
        AddToGroup("turret-placement");
    }

    public void toggle(bool newState) {
        _enabled = newState;
        Visible = newState;
    } 
    
    public override void _Input(InputEvent input) {
        if (!_enabled) return;
        if (input is InputEventMouseMotion) {
            _sprite.GlobalPosition = new Vector2(
                Mathf.Round((_sprite.GetGlobalMousePosition().x - Nav.HALF_TILE_SIZE) / Nav.TILE_SIZE) * Nav.TILE_SIZE,
                Mathf.Round((_sprite.GetGlobalMousePosition().y - Nav.HALF_TILE_SIZE) / Nav.TILE_SIZE) * Nav.TILE_SIZE
            );
        } else if (input is InputEventMouseButton) {
            GetTree().CallGroup("level", nameof(Level.placeTurret), _sprite.GlobalPosition, "SomeTurret");
        }
    }
}
}
