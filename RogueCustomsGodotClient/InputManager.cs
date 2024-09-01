using Godot;

using System.Collections.Generic;

public partial class InputManager : Node
{
    private Dictionary<string, float> actionTimers = new Dictionary<string, float>();
    private float inputDelay = 0.25f;

    // Called every frame
    public override void _Process(double delta)
    {
        // Update all timers
        List<string> keys = new List<string>(actionTimers.Keys);
        foreach (string action in keys)
        {
            actionTimers[action] -= (float)delta;
            if (actionTimers[action] <= 0)
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
