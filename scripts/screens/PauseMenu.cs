using System;
using Godot;

namespace ProjectTD.scripts.screens {
public class PauseMenu : CanvasLayer {

	private Popup _popup;

	public override void _Ready() {
		_popup = GetNode<Popup>("Popup");
	}

	public void Open() {
		_popup.PopupCentered();
		_popup.Show();
		GD.Print("open");
		// TODO figure out why it doesnt actually open
		// TODO properly pause the game
	}

	public void _on_MenuButton_pressed() {
		GetTree().ChangeScene("scenes/screens/MainScreen.tscn");
	}

	public void _on_OptionsButton_pressed() {
		throw new NotImplementedException();
	}

	public void _on_ContinueButton_pressed() {
		_popup.Hide();
	}
}
}