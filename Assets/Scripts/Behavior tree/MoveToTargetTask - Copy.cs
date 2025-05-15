using UnityEngine;
using UnityEngine.AI;

public class MoveToTargetTask : Task
{
    private NavMeshAgent agent;
    private bool destinationSet = false;

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        if (pet.CurrentSleepTarget == null)
        {
            Debug.LogError("MoveToTargetTask: Pet " + pet.name + " has no CurrentSleepTarget set.");
            return TaskStatus.Failure;
        }

        if (agent == null)
        {
            agent = pet.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("MoveToTargetTask: Pet " + pet.name + " is missing NavMeshAgent!");
                return TaskStatus.Failure;
            }
        }

        if (!destinationSet)
        {
            agent.SetDestination(pet.CurrentSleepTarget.transform.position);
            destinationSet = true;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            ItemAvailability availability = pet.CurrentSleepTarget.GetComponent<ItemAvailability>();
            if (availability != null && availability.IsAvailable())
            {
                availability.Claim(pet);
            }

            destinationSet = false;
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
