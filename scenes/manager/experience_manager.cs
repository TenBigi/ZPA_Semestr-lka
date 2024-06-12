using Godot;
using System;

public partial class experience_manager : Node
{
	[Signal]
	public delegate void ExperienceUpdatedEventHandler(float currentExperience, float targetExperience);
	[Signal]
	public delegate void LevelUpEventHandler(int newLevel);
	private const float TARGET_EXPERIENCE_GROWTH = 5;
	private float currentExperience = 0;
	private float currentLevel = 1;
	private float targetExperience = 5;

	public override void _Ready()
	{
		var gameEvents = GetNode<game_events>("/root/GameEvents");
		gameEvents.ExperienceBottleCollected += OnExperienceBottleCollected;
	}

	public void IncrementExperience(float number) {
        currentExperience = Mathf.Min(currentExperience + number, targetExperience);
		EmitSignal(nameof(ExperienceUpdated), currentExperience, targetExperience);
		if (currentExperience == targetExperience)
		{
			currentLevel += 1;
			targetExperience += TARGET_EXPERIENCE_GROWTH;
			currentExperience = 0;
			EmitSignal(nameof(ExperienceUpdated), currentExperience, targetExperience);
			EmitSignal(nameof(LevelUp), currentLevel);

		}
	}

	public void OnExperienceBottleCollected(float number) {
		IncrementExperience(number);
	}
}
