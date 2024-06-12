using Godot;
using System;

public partial class health_component : Node
{
	[Signal]
	public delegate void DiedEventHandler();
	[Signal]
	public delegate void HealthChangedEventHandler();

	[Export]
	public float maxHealth = 10;
	public float currentHealth;

	public override void _Ready()
	{
		currentHealth = maxHealth;
	}

	public void Damage(int damageAmount)
	{
		currentHealth = Mathf.Max(currentHealth - damageAmount, 0);
		EmitSignal(nameof(HealthChanged));
		//zavolani funkce na dalsim idle framu
		CallDeferred(nameof(CheckDeath));
	}

	public float GetHealthPercent()
	{
		if (maxHealth <= 0)
		{
			return 0;
		}
		else
		{
			return Math.Min(currentHealth / maxHealth, 1);
		}
	}

	public void CheckDeath()
	{
		if (currentHealth == 0)
		{
			EmitSignal(nameof(Died));
			Owner.QueueFree();
		}
	}
}
