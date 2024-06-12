using Godot;
using System;

public partial class main : Node
{
    [Export] private PackedScene endScreenScene;
    [Export]private PackedScene pauseMenuScene;
    private player player;
    public override void _Ready()
    {
        player = GetNode<player>("Entities/Player");
        pauseMenuScene = GD.Load<PackedScene>("res://scenes/UI/pause_menu.tscn");
        player.healthComponent.Died += OnPlayerDeath;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            var pauseMenuInstance = pauseMenuScene.Instantiate();
            AddChild(pauseMenuInstance);
            GetTree().Root.SetInputAsHandled();
        }
    }

    public void OnPlayerDeath()
    {
        var endScreenInstance = endScreenScene.Instantiate() as end_screen;
        AddChild(endScreenInstance);
        endScreenInstance.SetDefeat();
    }
}
