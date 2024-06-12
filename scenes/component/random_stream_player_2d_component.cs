using Godot;
using System;

public partial class random_stream_player_2d_component : AudioStreamPlayer2D
{
	[Export] private AudioStream[] audioStreams;
	[Export] private bool randomizePitch = true;
	[Export] private float minPitch = 0.9f;
	[Export] private float maxPitch = 1.1f;
	RandomNumberGenerator rng = new();
	

    public override void _Ready()
    {
        rng.Randomize();
    }

    public void PlayRandom()
	{	
		if (audioStreams != null || audioStreams.Length > 0)
		{
			AudioStream chosenStream = audioStreams[rng.RandiRange(0, audioStreams.Length - 1)];
			Stream = chosenStream;
			Play();

			if (randomizePitch)
				PitchScale = rng.RandfRange(minPitch, maxPitch);
			else
				PitchScale = 1;
		}
	}
}
