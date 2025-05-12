using System;
using UnityEngine;

public class PlacementMenu : MonoBehaviour
{
    public Selector Selector;

    private void Awake()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<Placeable>().selector = Selector;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
