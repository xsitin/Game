using Godot;

namespace GameGodot;

public partial class Main : Node2D
{
    public override void _Ready()
    {
        GD.Print("GameGodot bootstrap scene loaded");
    }
}
