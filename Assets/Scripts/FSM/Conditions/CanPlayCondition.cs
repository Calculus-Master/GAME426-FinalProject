using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Finite State Machine/Conditions/Can Play Condition")]
public class CanPlayCondition : FSMCondition
{
    public float playfulnessThreshold = 3.0F;
    
    public override bool Test(FiniteStateMachine fsm)
    {
        InfluenceMap im = fsm.GetInfluenceMap();
        PetEntity pet = fsm.GetPet();
        Vector3 petPos = pet.transform.position;

        if (!pet.CanStopWandering) return false;
        
        float play = im.GetInfluenceAt(InfluenceLayers.PLAYFULNESS, petPos);
        play *= pet.petType.playing;

        // Debug.Log($"Testing Playfulness for {pet.name}: {play} ?= {this.playfulnessThreshold}");
        return play >= this.playfulnessThreshold;
    }
}
