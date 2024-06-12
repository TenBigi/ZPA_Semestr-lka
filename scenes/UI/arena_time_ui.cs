using Godot;
using System;

public partial class arena_time_ui : CanvasLayer
{
	[Export]
	public Node arenaTimeManagerNode;
	private arena_time_manager arenaTimeManager;
	private Label _label;

	public override void _Ready()
	{
		_label = GetNode<Label>("MarginContainer/Label");
	}

	public override void _Process(double delta)
	{
		arenaTimeManager = arenaTimeManagerNode as arena_time_manager;
		if (arenaTimeManager != null)
		{
			double timeElapsed = arenaTimeManager.GetTimeElapsed();
			_label.Text = FormatSeconds(timeElapsed);
		}
	}

	public string FormatSeconds(double seconds)
	{
		double minutes = Mathf.Floor(seconds / 60);
		double remainingSeconds = Mathf.Floor(seconds - (minutes * 60));
		return minutes.ToString() + ":" + remainingSeconds.ToString("00");
	}
}
