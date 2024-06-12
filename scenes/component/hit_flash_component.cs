using Godot;
using System;

public partial class hit_flash_component : Node
{
	[Export] private health_component healthComponent;
	[Export] private Sprite2D sprite;
	[Export] private ShaderMaterial hitFlashComponentMaterial;
	private Tween hitFlashTween;

	public override void _Ready()
	{
		healthComponent.HealthChanged += OnHealthChanged;
		sprite.Material = hitFlashComponentMaterial;
	}

	public void OnHealthChanged()
	{
		if (hitFlashTween != null && hitFlashTween.IsValid())
		{
			hitFlashTween.Kill();
		}

		 if (sprite.Material is ShaderMaterial shaderMaterial)
        {
            shaderMaterial.SetShaderParameter("lerp_percent", 1.0);
        }

		hitFlashTween = CreateTween();
		hitFlashTween.TweenProperty(sprite.Material, "shader_parameter/lerp_percent", 0.0, 0.25)
		.SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Cubic);
	}

	
}
