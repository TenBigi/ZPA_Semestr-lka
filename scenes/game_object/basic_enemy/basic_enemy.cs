using Godot;
using System;

public partial class basic_enemy : CharacterBody2D
{
	private velocity_component velocityComponent;
	private hurtbox_component hurtboxComponent;
	private Node2D visuals;
	private random_stream_player_2d_component randomStreamPlayer2D;

	public override void _Ready()
	{
		velocityComponent = GetNode<velocity_component>("VelocityComponent");
		visuals = GetNode<Node2D>("Visuals");
		hurtboxComponent = GetNode<hurtbox_component>("HurtboxComponent");
		randomStreamPlayer2D = GetNode<random_stream_player_2d_component>("RandomStreamPlayer2DComponent");

		hurtboxComponent.Hit += OnHit;
	}

	public override void _Process(double delta)
	{
		velocityComponent.AccelerateToPlayer();
		velocityComponent.Move(this);

		float moveSign = Math.Sign(Velocity.X);
		if (moveSign != 0){
			visuals.Scale = new Vector2(-moveSign, 1);
		}
	}

	public void OnHit()
	{
		randomStreamPlayer2D.PlayRandom();
	}
}
