using UnityEngine;
using UnityEngine.AI;

public class MoveToFoodDish : Task
{
    /// Cached reference to the food dish GameObject.
    private GameObject foodDish;

    /// Cached reference to the NavMeshAgent.
    private NavMeshAgent agent;

    /// Flag to track if destination has been set.
    private bool destinationSet = false;

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        // Find the food dish in the scene if not cached
        if (foodDish == null)
        {
            foodDish = GameObject.Find("food");
            if (foodDish == null)
            {
                Debug.LogError("MoveToFoodDish: Food object not found!");
                return TaskStatus.Failure;
            }
        }

        // Get the NavMeshAgent component if not cached
        if (agent == null)
        {
            agent = pet.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("MoveToFoodDish: " + pet.name + " is missing NavMeshAgent!");
                return TaskStatus.Failure;
            }
        }

        // Access ItemAvailability to check claims
        ItemAvailability availability = foodDish.GetComponent<ItemAvailability>();

        if (availability != null && !availability.IsAvailable() && !availability.IsClaimedBy(pet))
        {
            // Face the food dish without moving if it's claimed by someone else
            Vector3 directionToFood = (foodDish.transform.position - pet.transform.position).normalized;
            directionToFood.y = 0f; // Keep rotation on ground plane
            if (directionToFood != Vector3.zero)
            {
                pet.transform.rotation = Quaternion.LookRotation(directionToFood);
            }

            return TaskStatus.Running;
        }

        // Set destination if not already set
        if (!destinationSet)
        {
            agent.SetDestination(foodDish.transform.position);
            destinationSet = true;
        }

        // Check if agent has arrived at the destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (availability != null && availability.IsAvailable())
            {
                availability.Claim(pet);
                Debug.Log("MoveToFoodDish: " + pet.name + " reached and claimed the food dish.");
            }

            destinationSet = false; // Reset for future runs
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
