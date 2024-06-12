using Godot;
using System;

public partial class main_menu : CanvasLayer
{
	private PackedScene optionsScene;
	private sound_button playButton;
	private sound_button optionsButton;
	private sound_button quitButton;
	public override void _Ready()
	{
		optionsScene = GD.Load<PackedScene>("res://scenes/UI/options_menu.tscn");
		playButton = GetNode<sound_button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/PlayButton");
		optionsButton = GetNode<sound_button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/OptionsButton");
		quitButton = GetNode<sound_button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/QuitButton");

		playButton.Pressed += OnPlayButtonPressed;
		optionsButton.Pressed += OnOptionsButtonPressed;
		quitButton.Pressed += OnQuitButtonPressed;
	}

	public void OnPlayButtonPressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/main/main.tscn");
	}

	public void OnOptionsButtonPressed()
	{
		var optionsMenuInstance = optionsScene.Instantiate() as options_menu;
		AddChild(optionsMenuInstance);
		optionsMenuInstance.BackPressed += () => OnOptionsClosed(optionsMenuInstance);
	}

	public void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}

	public void OnOptionsClosed(options_menu optionsInstance)
	{
		optionsInstance.QueueFree();
	}
}
