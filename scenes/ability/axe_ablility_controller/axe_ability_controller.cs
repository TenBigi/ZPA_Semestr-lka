using Godot;
using System;

public partial class axe_ability_controller : Node
{
    [Export] public PackedScene axeAbility;
    private Timer timer;
    private int damage = 10;

    public override void _Ready()
    {
        timer = GetNode<Timer>("Timer");
        timer.Timeout += OnTimerTimeout;
    }

    public void OnTimerTimeout()
    {
        var axeAbilityInstance = axeAbility.Instantiate() as axe_ability;
        var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
        if (player != null)
        {
            var foreground = GetTree().GetFirstNodeInGroup("foreground_layer") as Node2D;
            if (foreground != null)
            {
                foreground.AddChild(axeAbilityInstance);
                axeAbilityInstance.GlobalPosition = player.GlobalPosition;
                axeAbilityInstance.hitboxComponent.damage = damage;

            }
        }
    }

}
