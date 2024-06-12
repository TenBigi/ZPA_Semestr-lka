using Godot;
using System;

public partial class wizard_enemy : CharacterBody2D
{
	private velocity_component velocityComponent;
	private Node2D visuals;
	private bool isMoving = true;
	private random_stream_player_2d_component randomStreamPlayer2D;
	private hurtbox_component hurtboxComponent;

	public override void _Ready()
	{
		velocityComponent = GetNode<velocity_component>("VelocityComponent");
		visuals = GetNode<Node2D>("Visuals");
		randomStreamPlayer2D = GetNode<random_stream_player_2d_component>("RandomStreamPlayer2DComponent");
		hurtboxComponent = GetNode<hurtbox_component>("HurtboxComponent");

		hurtboxComponent.Hit += OnHit;
	}

	public override void _Process(double delta)
	{	
		if (isMoving)
		{
			velocityComponent.AccelerateToPlayer();
		}
		else
		{
			velocityComponent.Decelerate();
		}
		
		velocityComponent.Move(this);

		float moveSign = Math.Sign(Velocity.X);
		if (moveSign != 0){
			visuals.Scale = new Vector2(moveSign, 1);
		}
	}

	public void SetIsMoving(bool moving)
	{
		isMoving = moving;
	}

	public void OnHit()
	{
		randomStreamPlayer2D.PlayRandom();
	}
}
