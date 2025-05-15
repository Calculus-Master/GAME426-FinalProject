using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class WaypointManager : MonoBehaviour
{
    public List<Vector3> waypoints = new();

    private void Start()
    {
        this.waypoints.AddRange(this.GetComponentsInChildren<Transform>().Select(t => t.position));
    }

    public Vector3 GetRandomWaypoint(Vector3 current)
    {
        Vector3 rwp;
        do
        {
            rwp = this.waypoints[Random.Range(0, this.waypoints.Count)];
        } while (rwp == current);
        
        return rwp;
    }
}
