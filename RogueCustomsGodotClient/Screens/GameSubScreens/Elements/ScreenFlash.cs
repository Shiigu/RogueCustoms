using Godot;
using System;
using System.Threading.Tasks;

public partial class ScreenFlash : ColorRect
{
    private float flashDuration = 0.1f;

    public async Task Flash(Color color)
    {
        Modulate = color;
        Show();
        await ToSignal(GetTree(), "process_frame");
        await ToSignal(GetTree().CreateTimer(flashDuration), "timeout");
        Hide();
    }
}
