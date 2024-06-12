using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class velocity_component : Node
{
    [Export] private int maxSpeed = 40;
    [Export] private double acceleration = 5;
    private Vector2 velocity = Vector2.Zero;

    public void AccelerateToPlayer()
    {
        Node2D owner = Owner as Node2D;
        if (owner != null)
        {
            var player = GetTree().GetFirstNodeInGroup("player") as Node2D;
            if (player != null)
            {
                 Vector2 direction = (player.GlobalPosition - owner.GlobalPosition).Normalized();
                 AccelerateInDirection(direction);
            }
        }
    }

    public void AccelerateInDirection(Vector2 direction)
    {
        Vector2 desiredVelocity = direction * maxSpeed;
        velocity = velocity.Lerp(desiredVelocity, (float)(1 - Mathf.Exp(-acceleration * GetProcessDeltaTime())));
    }

    public void Decelerate()
    {
        AccelerateInDirection(Vector2.Zero);
    }
    
    public void Move(CharacterBody2D characterBody)
    {
        characterBody.Velocity = velocity;
        characterBody.MoveAndSlide();
        velocity = characterBody.Velocity;
    }
}
