using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingInfluencerTest : Influencer
{
    public InfluenceMap influenceMap;
    public float destThresh = 0.2f;
    public float speed = 1.5f;
    private Vector2Int prevGridLocation;
    private Vector3 destination;
    
    private void MoveToRandom()
    {
        destination = GridMap.Instance.GridToWorld(GridMap.Instance.GetRandomGridLocation());
        destination.y = transform.position.y;
        // destination.z = transform.position.z;
    }
    
    private void Start()
    {
        prevGridLocation = GridMap.Instance.WorldToGrid(transform.position);
        MoveToRandom();        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(Vector3.Distance(destination, transform.position) < destThresh)
        {
            MoveToRandom();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * speed);    
        }
        
        Vector2Int newLoc = GridMap.Instance.WorldToGrid(transform.position);
        if (prevGridLocation != newLoc)
        {
            influenceMap.UpdateUnitInfluence(this);
            influenceMap.UpdateVisualization();
            prevGridLocation = newLoc;
        }
        
    }
    
    
}
