using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class upgrade_manager : Node
{
	[Export] public experience_manager experienceManager;
	[Export] public PackedScene upgradeScreenScene;
	[Export] public ability_upgrade[] upgradePool;
	private game_events gameEvents;
	private Dictionary<string, Dictionary<string, object>> currentUpgrades;
	private Random rnd;

	public override void _Ready()
	{
		experienceManager = GetNode<experience_manager>("/root/Main/ExperienceManager");
		gameEvents = GetNode<game_events>("/root/GameEvents");

		experienceManager.LevelUp += OnLevelUp;

		rnd = new Random();

		currentUpgrades = new Dictionary<string, Dictionary<string, object>>();
	}

	public void ApplyUpgrade(ability_upgrade upgrade)
	{
		if (!currentUpgrades.ContainsKey(upgrade.id))
		{
			currentUpgrades[upgrade.id] = new Dictionary<string, object>
			{
				{"resources", upgrade},
				{"quantity", 1}
			};
		}
		else
		{
        	currentUpgrades[upgrade.id]["quantity"] = (int)currentUpgrades[upgrade.id]["quantity"] + 1;
    	}

		if(upgrade.maxQuantity > 0)
		{
			int currentQuantity = (int)currentUpgrades[upgrade.id]["quantity"];
			if (currentQuantity == upgrade.maxQuantity)
			{
				upgradePool = upgradePool.Where(x => x.id != upgrade.id).ToArray();
            }
		}
    
    	var upgradeDataInstance = new upgrade_data(currentUpgrades);
    	gameEvents.EmitAbilityUpgradeAdded(upgrade, upgradeDataInstance);
	}

	public ability_upgrade[] PickUpgrades()
	{
		List<ability_upgrade> chosenUpgrades = new List<ability_upgrade>();
		ability_upgrade[] filteredUpgrades = (ability_upgrade[])upgradePool.Clone();
	
        for (int i = 0; i < upgradePool.Length; i++)
        {	
			if (filteredUpgrades.Length == 0)
			{
				break;
			} 
            ability_upgrade chosenUpgrade = filteredUpgrades[rnd.Next(filteredUpgrades.Length)];
            chosenUpgrades.Add(chosenUpgrade);
            filteredUpgrades = filteredUpgrades.Where(x => x.id != chosenUpgrade.id).ToArray();
        }

		return chosenUpgrades.ToArray();
	}

	public void OnLevelUp(int currentLevel)
	{
			var upgradeScreenInstance = upgradeScreenScene.Instantiate() as upgrade_screen;
			AddChild(upgradeScreenInstance);
			ability_upgrade[] chosenUpgrades = PickUpgrades();
			upgradeScreenInstance.SetAbilityUpgrades(chosenUpgrades);
			upgradeScreenInstance.UpgradeSelected += OnUpgradeSelected;
	}

	public void OnUpgradeSelected(ability_upgrade upgrade)
	{
		ApplyUpgrade(upgrade);
	}
}
