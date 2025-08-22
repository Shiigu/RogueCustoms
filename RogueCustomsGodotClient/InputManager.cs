using Godot;

using System.Collections.Generic;
using System.Linq;

public partial class InputManager : Node
{
    private Dictionary<string, float> actionTimers = new Dictionary<string, float>();
    private float inputDelay = 0.25f;

    // Called every frame
    public override void _Process(double delta)
    {
        // Update all timers
        foreach (string action in new List<string>(actionTimers.Keys))
        {
            actionTimers[action] -= (float)delta;
            if (actionTimers[action] <= 0)
            {
                actionTimers.Remove(action);
            }
        }

        // Refresh cooldown of keys that have been released
        foreach (var action in actionTimers.Keys.ToList())
        {
            if (!Input.IsActionPressed(action))
            {
                actionTimers.Remove(action);
            }
        }
    }

    public bool IsActionAllowed(string action)
    {
        if (!actionTimers.ContainsKey(action))
        {
            actionTimers[action] = inputDelay;
            return true;
        }
        return false;
    }
}
