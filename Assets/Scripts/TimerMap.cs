using System.Collections.Generic;

public class TimerMap
{
    private readonly Dictionary<int, float> _timers = new();

    public void Update(int instanceID, float dT)
    {
        this._timers.TryAdd(instanceID, 0F);
        this._timers[instanceID] += dT;
    }
    
    public float Get(int instanceID)
    {
        return this._timers[instanceID];
    }
    
    public void Reset(int instanceID)
    {
        this._timers[instanceID] = 0F;
    }
}