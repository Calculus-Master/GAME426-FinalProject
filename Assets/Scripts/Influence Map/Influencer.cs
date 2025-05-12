using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Influencer : MonoBehaviour
{
    public InfluenceLayers myLayer;
    // public Vector2Int gridPosition;
    public float baseStrength;

    public List<InfluenceNode> influenced = new List<InfluenceNode>();

    public void ResetInfluences()
    {
        foreach (var influenceNode in influenced)
        {
            // influenceNode.totalStrength -= influenceNode.influences[this];
            influenceNode.UpdateStrength(myLayer, -influenceNode.influences[this]);
            // influenceNode.influences.Remove(this);
            influenceNode.influences[this] = 0;
        }
        influenced.Clear();
    }
    // public float playfulness; //could put in sub class pet
    // public float happiness;
    // private float rechargingPlayfulness;

    //function to go to nearest influence point
    //time to recharge based on happiness levels
    //lose playfulness over playtime
    //seek other units/toys to play with
    // public Vector3Int GetLocation()
    // {
    //     return new Vector3Int(gridPosition.x, gridPosition.y, 0);
    // }

    public Vector3 GetLocation()
    {
        var gl = GridMap.Instance.WorldToGrid(transform.position);
        Vector3 location = new Vector3(gl.x, gl.y, 0f);
        return location;
    }

    public virtual float GetStrength()
    {
        return baseStrength;
    }
}