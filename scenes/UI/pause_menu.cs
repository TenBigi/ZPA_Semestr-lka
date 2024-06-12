using Godot;
using System;

public partial class pause_menu : CanvasLayer
{
    private AnimationPlayer animationPlayer;
    private PanelContainer panelContainer;
    private sound_button resumeButton;
    private sound_button optionsButton;
    private PackedScene optionsMenuScene;
    private sound_button exitButton;
    private bool isClosing;

    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        panelContainer = GetNode<PanelContainer>("MarginContainer/PanelContainer");
        Button resumeButton = GetNode<sound_button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/ResumeButton");
        Button optionsButton = GetNode<sound_button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/OptionsButton");
        Button exitButton = GetNode<sound_button>("MarginContainer/PanelContainer/MarginContainer/VBoxContainer/VBoxContainer/QuitButton");
        optionsMenuScene = GD.Load<PackedScene>("res://scenes/UI/options_menu.tscn");

        GetTree().Paused = true;        
        resumeButton.Pressed += OnResumePressed;
        optionsButton.Pressed += OnOptionsPressed;
        exitButton.Pressed += OnExitPressed;

        animationPlayer.Play("default");

        Tween tween = CreateTween();
        tween.TweenProperty(panelContainer, "scale", Vector2.Zero, 0);
        tween.TweenProperty(panelContainer, "scale", Vector2.One, 0.3)
        .SetEase(Tween.EaseType.Out).SetTrans(Tween.TransitionType.Back);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("pause"))
        {
            Close();
            GetTree().Root.SetInputAsHandled();
        }
    }

    public async void Close()
    {
        if (!isClosing)
        {
            isClosing = true;
            animationPlayer.PlayBackwards("default");

            Tween tween = CreateTween();
            tween.TweenProperty(panelContainer, "scale", Vector2.One, 0);
            tween.TweenProperty(panelContainer, "scale", Vector2.Zero, 0.3)
            .SetEase(Tween.EaseType.In).SetTrans(Tween.TransitionType.Back);

            await ToSignal(tween, "finished");
            GetTree().Paused = false;
            QueueFree();
        }
    }

    public void OnResumePressed()
    {
        Close();
    }

    public void OnOptionsPressed()
    {
        var optionsMenuInstance = optionsMenuScene.Instantiate() as options_menu;
        AddChild(optionsMenuInstance);
        optionsMenuInstance.BackPressed += () => OnOptionsBackPressed(optionsMenuInstance);
    }

    public void OnOptionsBackPressed(Node optionsMenu)
    {
        optionsMenu.QueueFree();
    }

    public void OnExitPressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://scenes/UI/main_menu.tscn");
    }

}
