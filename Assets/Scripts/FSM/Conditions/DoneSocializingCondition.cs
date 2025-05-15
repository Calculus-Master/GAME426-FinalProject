using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Finite State Machine/Conditions/Done Socializing Condition")]
public class DoneSocializingCondition : FSMCondition
{
    public override bool Test(FiniteStateMachine fsm)
    {
        PetEntity pet = fsm.GetPet();

        return pet.SocialNeed() >= pet.SocialThresholds.upper;
    }
}
