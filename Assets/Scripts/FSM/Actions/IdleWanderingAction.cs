using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Idle Wander")]
public class IdleWanderingAction : FSMAction
{
    public override void Act(FiniteStateMachine fsm)
    {
        PetEntity pet = fsm.GetPet();
        NavMeshAgent agent = pet.GetAgent();
        
        if (pet.CurrentWaypoint == Vector3.zero || this.IsAgentAtDestination(agent))
        {
            pet.CurrentWaypoint = pet.Waypoints.GetRandomWaypoint(pet.CurrentWaypoint);
            
            agent.SetDestination(pet.CurrentWaypoint);
        }
    }

    // Adapted from Activity 5/HW 2
    private bool IsAgentAtDestination(NavMeshAgent agent)
    {
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f) return true;
        }
        
        return false;
    }
}
