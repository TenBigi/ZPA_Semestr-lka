using Godot;
using System;

public partial class sound_button : Button
{
	private random_stream_player_component randomStreamPlayerComponent;

	public override void _Ready()
	{
		randomStreamPlayerComponent = GetNode<random_stream_player_component>("RandomStreamPlayerComponent");

		Pressed += OnPressed;
	}

	public void OnPressed()
	{
		randomStreamPlayerComponent.PlayRandom();
	}
}
