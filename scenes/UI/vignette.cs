using Godot;
using System;

public partial class vignette : CanvasLayer
{
	private game_events gameEvents;
	private AnimationPlayer animationPlayer;

	public override void _Ready()
	{
		gameEvents = GetNode<game_events>("/root/GameEvents");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");

		if (!IsInstanceValid(animationPlayer))
			GD.PrintErr("AnimationPlayer is not valid");

		gameEvents.PlayerDamaged += OnPlayerDamaged;
	}

	public void OnPlayerDamaged()
	{
		if (IsInstanceValid(animationPlayer))
			animationPlayer.Play("hit");
	}
}
