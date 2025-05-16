using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Finite State Machine/Conditions/Hungry Condition")]
public class HungryCondition : FSMCondition
{
    public bool testForHungry;
    
    public override bool Test(FiniteStateMachine fsm)
    {
        PetEntity pet = fsm.GetPet();
        return this.testForHungry ? pet.Hunger() <= pet.HungerThresholds.lower : pet.Hunger() > pet.HungerThresholds.lower;
    }
}
