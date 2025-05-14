using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class MaxInfluenceActivityCondition : FSMCondition
{
    protected InfluenceLayers? GetMaxInfluenceLayer(FiniteStateMachine fsm)
    {
        InfluenceMap im = fsm.GetInfluenceMap();
        PetEntity pet = fsm.GetPet();
        Vector3 petPos = pet.transform.position;

        float play = im.GetInfluenceAt(InfluenceLayers.PLAYFULNESS, petPos);
        float social = im.GetInfluenceAt(InfluenceLayers.POSITION, petPos);
        float decor = im.GetInfluenceAt(InfluenceLayers.DECOR, petPos);

        play *= pet.petType.playing;
        social *= pet.petType.socializing;
        decor *= pet.petType.decorations;

        return play > social && play > decor ? InfluenceLayers.PLAYFULNESS :
            social > play && social > decor ? InfluenceLayers.POSITION :
            decor > play && decor > social ? InfluenceLayers.DECOR :
            null;
    }
}
