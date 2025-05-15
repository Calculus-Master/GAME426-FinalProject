using UnityEngine;
using UnityEngine.AI;

public class MoveToFoodDish : Task
{
    private GameObject foodDish;
    private NavMeshAgent agent;
    private bool destinationSet = false;

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        if (foodDish == null)
            foodDish = GameObject.Find("food");  // Ensure lowercase "food" matches the scene object name exactly.

        if (foodDish == null)
        {
            Debug.LogError("MoveToFoodDish: Food object not found!");
            return TaskStatus.Failure;
        }

        if (agent == null)
        {
            agent = pet.GetComponent<NavMeshAgent>();
            if (agent == null)
            {
                Debug.LogError("MoveToFoodDish: " + pet.name + " is missing NavMeshAgent!");
                return TaskStatus.Failure;
            }
        }

        // Set destination if not already set
        if (!destinationSet)
        {
            Debug.Log("MoveToFoodDish: Setting destination to " + foodDish.transform.position + " for " + pet.name);
            agent.SetDestination(foodDish.transform.position);
            destinationSet = true;
        }

        // Log remaining distance for debugging
        Debug.Log("MoveToFoodDish: Remaining distance for " + pet.name + ": " + agent.remainingDistance);

        // Check if agent has arrived
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            Debug.Log("MoveToFoodDish: " + pet.name + " reached the food dish.");
            destinationSet = false;  // Reset for future calls
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
