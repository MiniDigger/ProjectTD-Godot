using Godot;

namespace ProjectTD.scripts.hud {
public class Healthbar : Control {
	private TextureProgress _textureProgress;

	public override void _Ready() {
		_textureProgress = GetNode<TextureProgress>("TextureProgress");
		
		setValue(0);
	}

	public void setValue(float value) {
		_textureProgress.SetValue(value);
	}
}
}