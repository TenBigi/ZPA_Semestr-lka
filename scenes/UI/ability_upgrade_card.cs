using Godot;
using System;

public partial class ability_upgrade_card : PanelContainer
{
    [Signal] public delegate void SelectedEventHandler();
	private Label nameLabel;
	private Label descriptionLabel;
	private AnimationPlayer animationPlayer;
	private AnimationPlayer hoverAnimationPlayer;

	bool disabled = false;

	public override void _Ready()
	{
		nameLabel = GetNode<Label>("MarginContainer/VBoxContainer/PanelContainer/NameLabel");
		descriptionLabel = GetNode<Label>("MarginContainer/VBoxContainer/DescriptionLabel");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		hoverAnimationPlayer = GetNode<AnimationPlayer>("HoverAnimationPlayer");


		GuiInput += OnGuiInput;
		MouseEntered += OnMouseEntered;
	}

	public async void PlayIn(double delay = 0)
	{	
		Modulate = new Color(1, 1, 1, 0);
		await ToSignal(GetTree().CreateTimer(delay), "timeout");
		Modulate = new Color(1, 1, 1);
		animationPlayer.Play("in");
	}

	public void SetAbilityUpgrade(ability_upgrade upgrade)
	{
		nameLabel.Text = upgrade.name;
		descriptionLabel.Text = upgrade.description;
	}

	public void OnGuiInput(InputEvent inputEvent)
	{
		if(inputEvent.IsActionPressed("left_click"))
		{
			SelectCard();
		}
	}

	public async void SelectCard()
	{
		disabled = true;
		animationPlayer.Play("selected");
		
		foreach (ability_upgrade_card otherCard in GetTree().GetNodesInGroup("upgrade_card"))
		{
			if (otherCard != this)
			{
				otherCard.PlayDiscard();
			}
		}

		await ToSignal(animationPlayer, "animation_finished");
		EmitSignal(nameof(Selected));
	}

	public void PlayDiscard()
	{
		animationPlayer.Play("discard");
	}

	public void OnMouseEntered()
	{
		if (!disabled)
		{
			hoverAnimationPlayer.Play("hover");
		}
	}
}
