using Godot;

namespace ProjectTD.scripts.screens {
public class MainScreen : Control {
	public void _on_PlayButton_pressed() {
		GetTree().ChangeScene("scenes/screens/LevelSelectScreen.tscn");
	}

	public void _on_QuitButton_pressed() {
		GetTree().Quit();
	}
}
}