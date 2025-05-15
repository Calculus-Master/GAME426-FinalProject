using UnityEngine;

public class ClickableItem : MonoBehaviour
{
    public string itemName;
    public ItemToggleManager toggleManager;

    void OnMouseDown()
    {
        if (toggleManager != null)
        {
            toggleManager.ToggleItem(itemName);
        }
    }
}
