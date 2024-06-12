using Godot;
using System;
using System.Numerics;
using System.Reflection.Metadata;
using Vector2 = Godot.Vector2;

public partial class death_component : Node2D
{
	[Export] private health_component healthComponent;
	[Export] private Sprite2D sprite;
	private AnimationPlayer animationPlayer;
	private GpuParticles2D particles;
	private random_stream_player_2d_component randomStreamPlayer2D;
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		particles = GetNode<GpuParticles2D>("GPUParticles2D");
		randomStreamPlayer2D = GetNode<random_stream_player_2d_component>("RandomStreamPlayer2DComponent");

		healthComponent.Died += OnDied;
		particles.Texture = sprite.Texture;
	}

	public void OnDied()
	{
		if (Owner != null || Owner is Node2D)
		{
			Vector2 spawnPosition = ((Node2D)Owner).GlobalPosition;
			Node entities = GetTree().GetFirstNodeInGroup("entities_layer");
			GetParent().RemoveChild(this);
			entities.AddChild(this);
			GlobalPosition = spawnPosition;
			animationPlayer.Play("default");
			randomStreamPlayer2D.PlayRandom();
		}
	}
}
