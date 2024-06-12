using Godot;
using System;

public partial class sword_ability : Node2D
{
	public hitbox_component hitboxComponent;
	public override void _Ready()
	{
		hitboxComponent = GetNode<hitbox_component>("HitboxComponent");
	}

	public override void _Process(double delta)
	{
	}
}
