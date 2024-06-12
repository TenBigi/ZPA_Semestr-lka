using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class experience_bottle : Node2D
{
	private Area2D area2D;
	private game_events gameEvents;
	private CollisionShape2D collisionShape2D;
	private Sprite2D sprite;
	private random_stream_player_2d_component randomStreamPlayer2D;
	public override void _Ready()
	{
		randomStreamPlayer2D = GetNode<random_stream_player_2d_component>("RandomStreamPlayer2DComponent");
		collisionShape2D = GetNode<CollisionShape2D>("Area2D/CollisionShape2D");
		sprite = GetNode<Sprite2D>("Sprite2D");
		area2D = GetNode<Area2D>("Area2D");
		area2D.AreaEntered += OnAreaEntered;

		gameEvents = GetNode<game_events>("/root/GameEvents");
	}

	public void TweenCollect(float percent, Vector2 startPosition)
	{
		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if (player != null)
		{
			GlobalPosition = startPosition.Lerp(player.GlobalPosition, percent);
			Vector2 directionFromStart = player.GlobalPosition - startPosition;
			float targetRotation = directionFromStart.Angle() + Mathf.DegToRad(90);
			Rotation = (float)Mathf.LerpAngle(Rotation, targetRotation, 1 - Mathf.Exp(-2 * GetProcessDeltaTime()));
        }
	}
	public void Collect()
	{
		gameEvents.EmitExperienceBottleCollected(1);
		QueueFree();
	}

	public void TweenCollectWrapper(float percent)
    {
        TweenCollect(percent, GlobalPosition);
    }

	public void OnAreaEntered(Area2D otherArea) {
		Tween tween = CreateTween();
		tween.SetParallel();
		tween.TweenMethod(new Callable(this, nameof(TweenCollectWrapper)), 0.0, 1.0, 0.5);
		tween.TweenProperty(sprite, "scale", Vector2.Zero, 0.15).SetDelay(0.1);
		tween.Chain();
		tween.TweenCallback(Callable.From(Collect));

		randomStreamPlayer2D.PlayRandom();
	}
}
