using Godot;
using System;

public partial class floating_text : Node2D
{
	private Label label;
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
	}

	public void StartTween(string text)
	{
		label.Text = text;

		Tween tween = CreateTween();
		tween.SetParallel();

		tween.TweenProperty(this, "global_position", GlobalPosition + (Vector2.Up * 16), 0.3)
		.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		tween.Chain();

		tween.TweenProperty(this, "global_position", GlobalPosition + (Vector2.Up * 48), 0.5)
		.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
		tween.TweenProperty(this, "scale", Vector2.Zero, 0.5)
		.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
		tween.Chain();

		tween.TweenCallback(Callable.From(QueueFree));

		Tween scaleTween = CreateTween();
		scaleTween.TweenProperty(this, "scale", Vector2.One * 1.5f, 0.3)
		.SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Cubic);
		scaleTween.TweenProperty(this, "scale", Vector2.One, 0.15)
		.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
	}
}
