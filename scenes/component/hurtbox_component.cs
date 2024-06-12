using Godot;
using System;

public partial class hurtbox_component : Area2D
{
	[Export] private health_component healthComponent;
	[Signal] public delegate void HitEventHandler();
	private PackedScene floatingTextScene = GD.Load<PackedScene>("res://scenes/UI/floating_text.tscn");

	public override void _Ready()
	{
		AreaEntered += OnAreaEntered;
	}

	public void OnAreaEntered(Area2D otherArea)
	{
		if (otherArea is hitbox_component && healthComponent != null)
		{
			hitbox_component hitboxComponent = otherArea as hitbox_component;
			healthComponent.Damage(hitboxComponent.damage);

			floating_text floatingTextInstance = floatingTextScene.Instantiate() as floating_text;
			GetTree().GetFirstNodeInGroup("foreground_layer").AddChild(floatingTextInstance);

			floatingTextInstance.GlobalPosition = GlobalPosition + (Vector2.Up * 16);
			floatingTextInstance.StartTween(hitboxComponent.damage.ToString());

			EmitSignal(nameof(Hit));
		}
	}
}
