using Godot;

namespace ProjectTD.scripts.screens {
public class LevelSelectScreen : Control {
	private LevelList _levelList;
	private ItemList _itemList;

	public override void _Ready() {
		_levelList = GetNode<LevelList>("LevelList");
		_itemList = GetNode<ItemList>("CenterContainer/VBoxContainer/ItemList");
		
		GD.Print($"Loaded {_levelList.Levels.Count} levels");
		foreach (var level in _levelList.Levels) {
			_itemList.AddItem(level);
		}
	}

	public void _on_PlayButton_pressed() {
		var selectedItems = _itemList.GetSelectedItems();
		if (selectedItems.Length > 0) {
			var levelId = selectedItems[0];
			var levelName = _levelList.Levels[levelId];
			GetTree().ChangeScene("scenes/levels/" + levelName + ".tscn");
		}
	}
	
	public void _on_BackButton_pressed() {
		GetTree().ChangeScene("scenes/screens/MainScreen.tscn");
	}
}
}