using Godot;

using System.Threading.Tasks;

public partial class AimingSquare : ColorRect
{
    public bool Disabled { get; set; }
    private float flashInterval = 0.15f; // Flash interval in seconds

    public Vector2 Coordinates
    {
        get => new((int) Position.X / 16 - 1, (int)Position.Y / 16 - 1);
        set
        {
            Position = new(16 * value.X + 16, 16 * value.Y + 16);
        }
    }

    public AimingSquare()
    {
        CustomMinimumSize = Size = new(16, 16);
        Disabled = false;
        Visible = false;
        Color = new Color { R8 = 255, G8 = 255, B8 = 255, A = 1 };
    }

    private async void Flash()
    {
        while (!Disabled)
        {
            await ToSignal(GetTree().CreateTimer(flashInterval), "timeout");
            Visible = !Disabled && !Visible;
        }
    }

    public void StartTargeting()
    {
        Disabled = false;
        Visible = true;
        Flash();
    }

    public void StopTargeting()
    {
        Disabled = true;
        Visible = false;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
