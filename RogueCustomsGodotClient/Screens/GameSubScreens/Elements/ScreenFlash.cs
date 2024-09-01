using Godot;
using System;

public partial class ScreenFlash : ColorRect
{
    private float flashDuration = 0.1f;

    public async void Flash(Color color)
    {
        Modulate = color;
        Show();
        await ToSignal(GetTree().CreateTimer(flashDuration), "timeout");
        Hide();
    }
}
