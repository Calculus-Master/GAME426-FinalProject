using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Finite State Machine/Conditions/Can Socialize Condition")]
public class CanSocializeCondition : FSMCondition
{
    public override bool Test(FiniteStateMachine fsm)
    {
        PetEntity pet = fsm.GetPet();

        int numPets = FindObjectsOfType<PetEntity>().Length;
        return numPets >= 2 
               && pet.CanStopWandering 
               && pet.SocialNeed() <= pet.SocialThresholds.lower;
    }
}
