using UnityEngine;
using UnityEngine.AI;

public class MoveToPetTask : Task
{
    private NavMeshAgent agent;

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        if (pet.CurrentSocialTarget == null)
        {
            Debug.LogError($"{pet.name} has no social target set.");
            return TaskStatus.Failure;
        }

        if (agent == null)
            agent = pet.GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError($"{pet.name} is missing NavMeshAgent!");
            return TaskStatus.Failure;
        }

        agent.SetDestination(pet.CurrentSocialTarget.transform.position);

        float distance = Vector3.Distance(pet.transform.position, pet.CurrentSocialTarget.transform.position);
        return distance <= agent.stoppingDistance + 0.5f
            ? TaskStatus.Success
            : TaskStatus.Running;
    }
}
