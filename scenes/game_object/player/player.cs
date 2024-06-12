using Godot;
using System;

public partial class player : CharacterBody2D
{
	private const int MAX_SPEED = 150;
	private const int ACCELERATION_SMOOTHING = 5;
	private Area2D collisionArea2D;
	public health_component healthComponent;
	private Timer damageIntervalTimer;
	private ProgressBar healthBar;
	private Node abilities;
	private game_events gameEvents;
	private AnimationPlayer animationPlayer;
	private Node2D visuals;
	private int collidingBodiesCount;
	private random_stream_player_2d_component randomStreamPlayer2D;

	public override void _Ready()
	{
		healthComponent = GetNode<health_component>("HealthComponent");
		collisionArea2D = GetNode<Area2D>("CollisionArea2D");
		damageIntervalTimer = GetNode<Timer>("DamageIntervalTimer");
		healthBar = GetNode<ProgressBar>("HealthBar");
		abilities = GetNode<Node>("Abilities");
		gameEvents = GetNode<game_events>("/root/GameEvents");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		visuals = GetNode<Node2D>("Visuals");
		randomStreamPlayer2D = GetNode<random_stream_player_2d_component>("RandomStreamPlayer2DComponent");



		collisionArea2D.BodyEntered += OnBodyEntered;
		collisionArea2D.BodyExited += OnBodyExited;
		damageIntervalTimer.Timeout += OnDamageIntervalTimerTimeout;
		healthComponent.HealthChanged += OnHealthChanged;
		gameEvents.AbilityUpgradeAdded += OnAbilityUpgradeAdded;

		UpdateHealthDisplay();
	}

	public override void _Process(double delta)
	{
		Vector2 movementVector = GetMovementVector();
		Vector2 direction = movementVector.Normalized();
		Vector2 targetVelocity = direction * MAX_SPEED;

		Velocity = Velocity.Lerp(targetVelocity, (float)(1 - Mathf.Exp(-delta * ACCELERATION_SMOOTHING)));

		MoveAndSlide();

		if(movementVector.X != 0 || movementVector.Y != 0)
		{
			animationPlayer.Play("walk");
		}
		else
		{
			animationPlayer.Play("RESET");
		}

		float moveSign = Math.Sign(movementVector.X);
		if (moveSign != 0){
			visuals.Scale = new Vector2(moveSign, 1);
		}
	}

	public Vector2 GetMovementVector() {
		float xMovement = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left");
		float yMovement = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up");

		return new Vector2(xMovement, yMovement);
	}

	public void CheckForDamage()
	{
		if (collidingBodiesCount > 0 || !damageIntervalTimer.IsStopped())
		{
			healthComponent.Damage(1);
			damageIntervalTimer.Start();
		}
		else
		{
			damageIntervalTimer.Stop();
		}
	}

	public void UpdateHealthDisplay()
	{
		healthBar.Value = healthComponent.GetHealthPercent();
	}

	public void OnBodyEntered(Node2D otherBody)
	{
		collidingBodiesCount += 1;
		CheckForDamage();
	}

	public void OnBodyExited(Node2D otherBody)
	{
		collidingBodiesCount -= 1;
	}

	public void OnDamageIntervalTimerTimeout()
	{
		CheckForDamage();
	}

	public void OnHealthChanged()
	{	
		gameEvents.EmitPlayerDamaged();
		UpdateHealthDisplay();
		randomStreamPlayer2D.PlayRandom();
	}

	public void OnAbilityUpgradeAdded(ability_upgrade upgrade, upgrade_data currentUpgrades)
	{
		if(upgrade is ability)
		{
			ability upgradedAbility = (ability)upgrade;
			abilities.AddChild(upgradedAbility.abilityControllerScene.Instantiate());
		}
	}
}
