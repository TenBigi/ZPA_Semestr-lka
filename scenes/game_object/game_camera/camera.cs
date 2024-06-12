using Godot;
using System;

public partial class camera : Camera2D
{
	private Vector2 targetPosition = Vector2.Zero;

	public override void _Ready()
	{
		MakeCurrent();
	}

	public override void _Process(double delta)
	{
		AcquireTarget();
		//matika, ktera docili toho, ze kamera ma lehke zpozdeni za hracem a pri zastaveni k nemu hladce dojede.. pouze pro efekt
		GlobalPosition = GlobalPosition.Lerp(targetPosition, (float)(1.0f - Mathf.Exp(-delta * 20)));
	}

	private void AcquireTarget() 
	{
		var playerNodes = GetTree().GetNodesInGroup("player");
		if (playerNodes.Count > 0)
		{
			var player = playerNodes[0] as Node2D;
			if (player != null)
			{
				targetPosition = player.GlobalPosition;
			}
		}
	}

}
