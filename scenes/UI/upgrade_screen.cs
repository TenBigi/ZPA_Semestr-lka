using Godot;
using System;

public partial class upgrade_screen : CanvasLayer
{
	[Signal] public delegate void UpgradeSelectedEventHandler(ability_upgrade upgrade);
	[Export] public PackedScene upgradeCardScene;
	private HBoxContainer cardContainer;
	private AnimationPlayer animationPlayer;
	public override void _Ready()
	{
		cardContainer = GetNode<HBoxContainer>("MarginContainer/CardContainer");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		GetTree().Paused = true;
	}

	public void SetAbilityUpgrades(ability_upgrade[] upgrades)
	{
		double delay = 0;
		foreach (var upgrade in upgrades)
		{
			var cardInstance = upgradeCardScene.Instantiate() as ability_upgrade_card;
			cardContainer.AddChild(cardInstance);
			cardInstance.SetAbilityUpgrade(upgrade);
			cardInstance.PlayIn(delay);
			cardInstance.Selected += () => OnUpgradeSelected(upgrade);
			delay += 0.2;
		}
	}

	public async void OnUpgradeSelected(ability_upgrade upgrade)
	{
		EmitSignal(nameof(UpgradeSelected), upgrade);
		animationPlayer.Play("out");
		await ToSignal(animationPlayer, "animation_finished");
		GetTree().Paused = false;
		QueueFree();
	}
	
}
