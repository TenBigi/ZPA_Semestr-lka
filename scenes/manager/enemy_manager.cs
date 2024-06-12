using Godot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class enemy_manager : Node
{
	private const float SPAWN_RADIUS = 370f;
	[Export] public arena_time_manager arenaTimeManager;
	[Export] public PackedScene basicEnemyScene;
	[Export] public PackedScene wizardEnemyScene;
	private weighted_table_enemy enemyTable = new();
	private Timer timer;
	private RandomNumberGenerator rng;
	private double baseSpawnTime = 0;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;

		enemyTable.AddItem(basicEnemyScene, 10);

		baseSpawnTime = timer.WaitTime;

		arenaTimeManager.ArenaDifficultyIncreased += OnArenaDifficultyIncreased;

		rng = new RandomNumberGenerator();
    	rng.Randomize();
	}

	//metoda zajistuje, ze se nepratele nespawnuji mimo plochu areny, tz. nevylezaji ze zdi
	public Vector2 GetSpawnPosition()
	{
		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if (player != null)
		{
			Vector2 spawnPosition = Vector2.Zero;
			Vector2 randomDirection = Vector2.Right.Rotated(rng.RandfRange(0, Mathf.Tau));
			for(int i = 0; i < 4; i++)
			{
				spawnPosition = player.GlobalPosition + (randomDirection * SPAWN_RADIUS);
				PhysicsRayQueryParameters2D queryParameters = new PhysicsRayQueryParameters2D
				{
					From = player.GlobalPosition,
					To = spawnPosition,
					CollisionMask = 1 << 0
				};

				var result = GetTree().Root.World2D.DirectSpaceState.IntersectRay(queryParameters);
				if (result.Count == 0)
				{
					break;
				}
				else
				{
					randomDirection = randomDirection.Rotated(Mathf.DegToRad(90));
				}
			}
			return spawnPosition;
		}
		else
		{
			return Vector2.Zero;
		}
	}

	public void OnTimerTimeout()
	{
		timer.Start();

		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if (player != null)
		{
			
			PackedScene enemyScene = enemyTable.PickItem();
			Node2D enemy = enemyScene.Instantiate() as Node2D;
			Node entitiesLayer = GetTree().GetFirstNodeInGroup("entities_layer");
			entitiesLayer.AddChild(enemy);
			enemy.GlobalPosition = GetSpawnPosition();
		}
	}

	public void OnArenaDifficultyIncreased(int arenaDifficulty)
	{
		double timeOff = 0.1 / 12 * arenaDifficulty;
		timeOff = Math.Min(timeOff, 0.7);
		timer.WaitTime = baseSpawnTime - timeOff;

		if (arenaDifficulty == 6)
		{
			enemyTable.AddItem(wizardEnemyScene, 20);
		}
	}

	
}
