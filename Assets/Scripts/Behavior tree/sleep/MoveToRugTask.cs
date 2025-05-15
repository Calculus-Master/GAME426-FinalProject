using UnityEngine;
using UnityEngine.AI;

public class MoveToRugTask : Task
{
    private NavMeshAgent agent;
    private bool destinationSet = false;

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        // Make sure the pet has a target rug assigned by SleepTargetingTask
        if (pet.CurrentSleepTarget == null)
        {
            Debug.LogError("MoveToRugTask: No rug target set for " + pet.name);
            return TaskStatus.Failure;
        }

        GameObject rug = pet.CurrentSleepTarget;

        // Get the NavMeshAgent
        if (agent == null)
        {
            agent = pet.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("MoveToRugTask: " + pet.name + " is missing NavMeshAgent!");
                return TaskStatus.Failure;
            }
        }

        // Set destination if not already set
        if (!destinationSet)
        {
            agent.SetDestination(rug.transform.position);
            destinationSet = true;
        }

        // Check if agent has arrived
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Claim the rug if it's available
            ItemAvailability availability = rug.GetComponent<ItemAvailability>();
            if (availability != null && availability.IsAvailable())
            {
                availability.Claim(pet);
                Debug.Log("MoveToRugTask: " + pet.name + " reached and claimed " + rug.name);
            }

            destinationSet = false;  // Reset for future calls
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
