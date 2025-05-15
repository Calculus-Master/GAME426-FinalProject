using UnityEngine;

public class ClickablePet : MonoBehaviour
{
    void OnMouseDown()
    {
        PetEntity petEntity = GetComponentInParent<PetEntity>();

        if (petEntity != null)
        {
            PetInfoDisplay.Instance.ShowPetInfo(petEntity);
        }
        else
        {
            Debug.LogWarning("ClickablePet: No PetEntity found in parent hierarchy.");
        }
    }
}
