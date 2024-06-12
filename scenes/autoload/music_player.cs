using Godot;
using System;

public partial class music_player : AudioStreamPlayer
{
	private Timer timer;
	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		Finished += OnFinished;
		timer.Timeout += OnTimerTimeout;
	}

	public void OnFinished()
	{
		timer.Start();
	}

	public void OnTimerTimeout()
	{
		Play();
	}
}
