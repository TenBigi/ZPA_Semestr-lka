using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class experience_bottle_component : Node
{
	[Export(PropertyHint.Range, "0,1")] public double dropPercent = 0.5;
	[Export] public PackedScene _bottleScene;
	[Export] public health_component _healthComponent;
	private RandomNumberGenerator rng = new();

	public override void _Ready()
	{
		_healthComponent.Died += OnDied;
		rng.Randomize();
	}

	public void OnDied() {
		if (rng.Randf() < dropPercent)
		{
			if(!(Owner is Node2D) || _bottleScene != null)
			{
				Vector2 spawnPosition = (Owner as Node2D)?.GlobalPosition ?? Vector2.Zero;
				var bottleInstance = _bottleScene.Instantiate() as Node2D;
				Node entitiesLayer = GetTree().GetFirstNodeInGroup("entities_layer");
				entitiesLayer.AddChild(bottleInstance);
				bottleInstance.GlobalPosition = spawnPosition;
			}
		}
	}
}
