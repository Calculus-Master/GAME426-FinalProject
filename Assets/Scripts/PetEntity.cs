using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetEntity : MonoBehaviour
{
    [Header("Pet Needs")] 
    [Tooltip("How often needs will decay, in seconds")] public float needsDecayInterval = 1F;
    public float hungerDecayRate = 0.1F;
    public float thirstDecayRate = 0.1F;
    public float energyDecayRate = 0.1F;
    
    private float _hunger;
    private float _thirst;
    private float _energyLevel;

    private Coroutine _needsCoroutine;

    private void Start()
    {
        this._hunger = 1.0F;
        this._thirst = 1.0F;
        this._energyLevel = 1.0F;

        this._needsCoroutine = StartCoroutine(this.DepleteNeeds());
    }

    private IEnumerator DepleteNeeds()
    {
        while (true)
        {
            yield return new WaitForSeconds(this.needsDecayInterval);

            if(this._hunger > 0) this._hunger -= this.hungerDecayRate;
            if(this._thirst > 0) this._thirst -= this.thirstDecayRate;
            if(this._energyLevel > 0) this._energyLevel -= this.energyDecayRate;
        }
    }
}
