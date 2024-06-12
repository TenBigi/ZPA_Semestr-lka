using Godot;
using System;

public partial class arena_time_manager : Node
{
	private const float DIFFICULTY_INTERVAL = 5;
	[Signal] public delegate void ArenaDifficultyIncreasedEventHandler(int arenaDifficulty);
	[Export] PackedScene endScreenScene;
	private Timer timer;
	private int arenaDifficulty = 0;
	private float previousTime = 0;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		timer.Timeout += OnTimerTimeout;
	}

    public override void _Process(double delta)
    {
        double nextTimeTarget = timer.WaitTime - ((arenaDifficulty + 1) * DIFFICULTY_INTERVAL);
		if (timer.TimeLeft <= nextTimeTarget)
		{
			arenaDifficulty ++;
			EmitSignal(nameof(ArenaDifficultyIncreased), arenaDifficulty);
		}
    }

    public double GetTimeElapsed() {
		return timer.WaitTime - timer.TimeLeft;
	}

	public void OnTimerTimeout()
	{
		var endScreenInstance = endScreenScene.Instantiate() as end_screen;
		AddChild(endScreenInstance);
		endScreenInstance.PlayJingle();
	}
}
