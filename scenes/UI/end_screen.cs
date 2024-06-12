using Godot;
using System;

public partial class end_screen : CanvasLayer
{
	private Button restartButton;
	private Button quitButton;
	private Label titleLabel;
	private Label descriptionLabel;
	private PanelContainer panelContainer;
	private AudioStreamPlayer victoryStreamPlayer;
	private AudioStreamPlayer defeatStreamPlayer;

	public override void _Ready()
	{
		restartButton = GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/RestartButton");
		quitButton = GetNode<Button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/QuitButton");
		titleLabel = GetNode<Label>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/TitleLabel");
		descriptionLabel = GetNode<Label>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/DescriptionLabel");
		panelContainer = GetNode<PanelContainer>("MarginContainer/PanelContainer");
		victoryStreamPlayer = GetNode<AudioStreamPlayer>("VictoryStreamPlayer");
		victoryStreamPlayer = GetNode<AudioStreamPlayer>("DefeatStreamPlayer");

		panelContainer.PivotOffset = panelContainer.Size/2;
		panelContainer.Scale = Vector2.Zero;

		Tween tween = CreateTween();
		tween.TweenProperty(panelContainer, "scale", Vector2.Zero, 0);
		tween.TweenProperty(panelContainer, "scale", Vector2.One, 0.6)
		.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);

		GetTree().Paused = true;

		restartButton.Pressed += OnRestartButtonPressed;
		quitButton.Pressed += OnQuitButtonPressed;
	}

	public void SetDefeat()
	{
		titleLabel.Text = "Defeat";
		descriptionLabel.Text = "You lost";
		PlayJingle(true);
	}

	public void PlayJingle(bool defeat = false)
	{
		if (defeat)
			defeatStreamPlayer.Play();
		else
			victoryStreamPlayer.Play();
	}

	public void OnRestartButtonPressed()
	{
		GetTree().Paused = false;
		GetTree().ChangeSceneToFile("res://scenes/main/main.tscn");
	}

	public void OnQuitButtonPressed()
	{	
		GetTree().Quit();
	}
}
