using System;
using UnityEngine;
using UnityEngine.UI;

public class Placeable : MonoBehaviour
{
    public Selector selector;

    public GameObject prefab;

    public Sprite icon;

    public void Start()
    {
        GetComponent<Image>().sprite = icon;
    }

    public void Select()
    {
        selector.Select(this);
    }
}
