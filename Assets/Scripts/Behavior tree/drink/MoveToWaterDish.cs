using UnityEngine;
using UnityEngine.AI;

public class MoveToWaterDish : Task
{
    /// Cached reference to the water dish GameObject.
    private GameObject waterDish;

    /// Cached reference to the NavMeshAgent.
    private NavMeshAgent agent;

    /// Flag to track if destination has been set.
    private bool destinationSet = false;

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        // Find the water dish in the scene if not cached
        if (waterDish == null)
        {
            waterDish = GameObject.Find("water");
            if (waterDish == null)
            {
                Debug.LogError("MoveTowaterDish: water object not found!");
                return TaskStatus.Failure;
            }
        }

        // Get the NavMeshAgent component if not cached
        if (agent == null)
        {
            agent = pet.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("MoveTowaterDish: " + pet.name + " is missing NavMeshAgent!");
                return TaskStatus.Failure;
            }
        }

        // Access ItemAvailability to check claims
        ItemAvailability availability = waterDish.GetComponent<ItemAvailability>();

        if (availability != null && !availability.IsAvailable() && !availability.IsClaimedBy(pet))
        {
            // Face the water dish without moving if it's claimed by someone else
            Vector3 directionTowater = (waterDish.transform.position - pet.transform.position).normalized;
            directionTowater.y = 0f; // Keep rotation on ground plane
            if (directionTowater != Vector3.zero)
            {
                pet.transform.rotation = Quaternion.LookRotation(directionTowater);
            }

            return TaskStatus.Running;
        }

        // Set destination if not already set
        if (!destinationSet)
        {
            agent.SetDestination(waterDish.transform.position);
            destinationSet = true;
        }

        // Check if agent has arrived at the destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (availability != null && availability.IsAvailable())
            {
                availability.Claim(pet);
                Debug.Log("MoveTowaterDish: " + pet.name + " reached and claimed the water dish.");
            }

            destinationSet = false; // Reset for future runs
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
