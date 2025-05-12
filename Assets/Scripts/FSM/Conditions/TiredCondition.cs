using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Finite State Machine/Conditions/Tired Condition")]
public class TiredCondition : FSMCondition
{
    public bool testForTired;
    
    public override bool Test(FiniteStateMachine fsm)
    {
        PetEntity pet = fsm.GetPet();
        return this.testForTired ? pet.EnergyLevel() <= pet.EnergyThresholds.lower : pet.EnergyLevel() >= pet.EnergyThresholds.upper;
    }
}
