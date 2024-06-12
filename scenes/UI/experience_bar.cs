using Godot;
using System;

public partial class experience_bar : CanvasLayer
{
	[Export]
	public experience_manager experienceManager;
	private ProgressBar progressBar;
	public override void _Ready()
	{
		progressBar = GetNode<ProgressBar>("MarginContainer/ProgressBar");
		progressBar.Value = 0;

		experienceManager.ExperienceUpdated += OnExperienceUpdated;
	}

	public void OnExperienceUpdated(float currentExperience, float targetExperience)
	{
		var percent = currentExperience / targetExperience;
		progressBar.Value = percent;
	}
}
