using UnityEngine;

public class ItemAvailability : MonoBehaviour
{
    private PetEntity currentUser = null;

    public bool IsAvailable()
    {
        return currentUser == null;
    }

        /// Returns the pet currently claiming this item, or null if unclaimed.
    public PetEntity GetCurrentUser()
    {
        return currentUser;
    }

    public bool Claim(PetEntity pet)
    {
        if (IsAvailable())
        {
            currentUser = pet;
            Debug.Log($"{pet.name} has claimed {gameObject.name}");
            return true;
        }
        Debug.Log($"{gameObject.name} is already claimed by {currentUser?.name}");
        return false;
    }

    public void Release()
    {
        if (currentUser != null)
        {
            Debug.Log($"{currentUser.name} has released {gameObject.name}");
            currentUser = null;
        }
    }

    public bool IsClaimedBy(PetEntity pet)
    {
        return currentUser == pet;
    }
}
