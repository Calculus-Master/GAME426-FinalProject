using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Finite State Machine/Conditions/Thirsty Condition")]
public class ThirstyCondition : FSMCondition
{
    public bool testForThirsty;
    
    public override bool Test(FiniteStateMachine fsm)
    {
        PetEntity pet = fsm.GetPet();
        return this.testForThirsty ? pet.Thirst() <= pet.ThirstThresholds.lower : pet.Thirst() >= pet.ThirstThresholds.upper;
    }
}
