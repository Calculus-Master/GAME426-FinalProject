using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Finite State Machine/Conditions/Done Playing Condition")]
public class DonePlayingCondition : FSMCondition
{
    public override bool Test(FiniteStateMachine fsm)
    {
        PetEntity pet = fsm.GetPet();

        return pet.IsDonePlaying;
    }
}
