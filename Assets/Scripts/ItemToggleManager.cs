using System.Collections.Generic;
using UnityEngine;

public class ItemToggleManager : MonoBehaviour
{
    public List<GameObject> items;

    private Dictionary<string, GameObject> itemDict = new Dictionary<string, GameObject>();

    void Start()
    {
        foreach (var item in items)
        {
            itemDict[item.name] = item;
        }
    }

    public void ToggleItem(string itemName)
    {
        if (itemDict.ContainsKey(itemName))
        {
            GameObject obj = itemDict[itemName];
            obj.SetActive(!obj.activeSelf);
        }
    }

    public bool IsItemActive(string itemName)
    {
        return itemDict.ContainsKey(itemName) && itemDict[itemName].activeSelf;
    }
}
