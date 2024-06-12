using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class axe_ability : Node2D
{
	private const int MAX_RADIUS = 100;
	public hitbox_component hitboxComponent;
	private Vector2 baseRotation = Vector2.Right;
	private RandomNumberGenerator rng;

	public override void _Ready()
	{
		hitboxComponent = GetNode<hitbox_component>("HitboxComponent");
		rng = new RandomNumberGenerator();
    	rng.Randomize();

		baseRotation = Vector2.Right.Rotated(rng.RandfRange(0, Mathf.Tau));

		Tween tween = CreateTween();
		tween.TweenMethod(new Callable(this, nameof(TweenMethod)), 0.0, 2.0, 3);
		tween.TweenCallback(new Callable(this, nameof(OnTweenCompleted)));
	}

	public void TweenMethod(float rotations)
	{
		float currentRadius = rotations / 2 * MAX_RADIUS;
		Vector2 currentDirection = baseRotation.Rotated(rotations * Mathf.Tau);

		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if (player != null)
		{
			GlobalPosition = player.GlobalPosition + (currentDirection * currentRadius);
		}
	}

	private void OnTweenCompleted()
    {
        QueueFree();
    }
}
