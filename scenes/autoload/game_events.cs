using Godot;
using System;
using System.Collections.Generic;

public partial class game_events : Node
{
	[Signal] public delegate void ExperienceBottleCollectedEventHandler(float number);
	[Signal] public delegate void AbilityUpgradeAddedEventHandler(ability_upgrade upgrade, upgrade_data currentUpgrades);
	[Signal] public delegate void PlayerDamagedEventHandler();

	public void EmitExperienceBottleCollected(float number)
	{
		EmitSignal(nameof(ExperienceBottleCollected), number);
	}

	public void EmitAbilityUpgradeAdded(ability_upgrade upgrade, upgrade_data currentUpgrades)
	{
		EmitSignal(nameof(AbilityUpgradeAdded), upgrade, currentUpgrades);
	}

	public void EmitPlayerDamaged()
	{
		EmitSignal(nameof(PlayerDamaged));
	}
}
