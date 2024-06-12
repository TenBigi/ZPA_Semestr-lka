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

		gameEvents.PlayerDamaged += OnPlayerDamaged;
	}

	public void OnPlayerDamaged()
	{
		animationPlayer.Play("hit");
	}
}
