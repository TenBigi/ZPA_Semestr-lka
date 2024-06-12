using Godot;
using System.Linq;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class sword_ability_controller : Node
{
	private const float MAX_RANGE = 150f;
	//u exportovane promenne lze upravovat hodnotu primo v rozhrani enginu
	[Export] public PackedScene swordAbility;
	private Timer timer;
	private RandomNumberGenerator rng;
	private game_events gameEvents;
	private int damage = 5;
	private double baseWaitTime;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;

		baseWaitTime = timer.WaitTime;

		rng = new RandomNumberGenerator();
    	rng.Randomize();

		gameEvents = GetNode<game_events>("/root/GameEvents");
		gameEvents.AbilityUpgradeAdded += OnAbilityUpgradeAdded;
	}

	public void OnTimerTimeout()
	{
		var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
		if (player != null)
		{
			var enemies = GetTree().GetNodesInGroup("enemy").OfType<Node2D>().ToArray();
			enemies = enemies.Where(x => x.GlobalPosition.DistanceSquaredTo(player.GlobalPosition) < Mathf.Pow(MAX_RANGE, 2)).ToArray(); 
			if (enemies.Length > 0)
			{
				enemies = enemies.OrderBy(x => x.GlobalPosition.DistanceSquaredTo(player.GlobalPosition)).ToArray();
				var swordInstance = swordAbility.Instantiate() as sword_ability;
				Node foreground_layer = GetTree().GetFirstNodeInGroup("foreground_layer");
				foreground_layer.AddChild(swordInstance);
				swordInstance.hitboxComponent.damage = damage;

				swordInstance.GlobalPosition = enemies[0].GlobalPosition;

				//nahodne umisteni kolem nepritele a nasledna rotace smerem k nepriteli
                swordInstance.GlobalPosition += Vector2.Right.Rotated(rng.RandfRange(0, Mathf.Tau)) * 4;
				var enemyDirection = enemies[0].GlobalPosition - swordInstance.GlobalPosition;
				swordInstance.Rotation = enemyDirection.Angle();
			}
		}
	}

	public void OnAbilityUpgradeAdded(ability_upgrade upgrade, upgrade_data currentUpgrades)
	{
		if (upgrade.id == "sword_rate")
		{
			var percentReduction = (int)currentUpgrades.Data["sword_rate"]["quantity"] * 0.1;
			timer.WaitTime = baseWaitTime * (1 - percentReduction);
			timer.Start();
		}
	}
}
