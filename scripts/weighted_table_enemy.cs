using Godot;
using System;
using System.Collections.Generic;

public partial class weighted_table_enemy : Node
{
    private List<Dictionary<string, object>> items = new List<Dictionary<string, object>>();
    private RandomNumberGenerator rng = new RandomNumberGenerator();
    private int weightSum = 0;

    public override void _Ready()
    {
        rng.Randomize();
    }

    public void AddItem(PackedScene item, int weight)
    {
        var dictionary = new Dictionary<string, object>
        {
            { "item", item},
            { "weight", weight}
        };
        items.Add(dictionary);
        weightSum += weight;
    }

    public PackedScene PickItem()
    {
        int chosenWeight = rng.RandiRange(1, weightSum);
        int itterationSum = 0;
        foreach (var item in items)
        {
            itterationSum += (int)item["weight"];
            if (chosenWeight <= itterationSum)
            {
                return (PackedScene)item["item"];
            }
        }

        return null;
    }

    
}
