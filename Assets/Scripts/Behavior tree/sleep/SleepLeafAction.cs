using UnityEngine;

public class SleepLeafAction : Task
{
    private float sleepTimer = 0f;
    private const float sleepDuration = 60f;

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        if (pet.CurrentSleepTarget == null)
        {
            Debug.LogError("SleepLeafAction: No rug target set for " + pet.name);
            return TaskStatus.Failure;
        }

        GameObject rug = pet.CurrentSleepTarget;

        pet.transform.rotation = Quaternion.Euler(pet.transform.eulerAngles.x, pet.transform.eulerAngles.y, 90f);

        sleepTimer += Time.deltaTime;

        if (sleepTimer >= sleepDuration)
        {
            sleepTimer = 0f;
            pet.Sleep();

            ItemAvailability availability = rug.GetComponent<ItemAvailability>();
            if (availability != null && availability.IsClaimedBy(pet))
            {
                availability.Release();
            }

            pet.transform.rotation = Quaternion.Euler(pet.transform.eulerAngles.x, pet.transform.eulerAngles.y, 0f);

            pet.CurrentSleepTarget = null;  // Clearly reset target after finishing sleep.

            Debug.Log("SleepLeafAction: " + pet.name + " has finished sleeping on " + rug.name);
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
