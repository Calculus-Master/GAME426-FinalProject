using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class PetEntity : MonoBehaviour
{

    [Header("Pet Type")] public ActivityWeights petType;

    [Header("Pet Needs Decay")] [Tooltip("How often needs will decay, in seconds")]
    public float needsDecayInterval = 1F;

    public float hungerDecayRate = 0.15F;
    public float thirstDecayRate = 0.05F;
    public float energyDecayRate = 0.1F;
    public float socialDecayRate = 0.1F;


    [Header("Pet Needs Thresholds")] [Tooltip("Values where the pet feels hungry and full")]
    public NeedsThreshold HungerThresholds = new(0.2F, 0.8F);

    [Tooltip("Values where the pet feels thirsty and not")]
    public NeedsThreshold ThirstThresholds = new(0.1F, 0.8F);

    [Tooltip("Values where the pet feels tired and energetic")]
    public NeedsThreshold EnergyThresholds = new(0.2F, 0.8F);

    [Tooltip("Values where the pet feels lonely and happy")]
    public NeedsThreshold SocialThresholds = new(0.5F, 0.8F);

    [Header("Misc")] [Tooltip("How long the pet spends wandering (minimum)")]
    public float minWanderingTime = 5F;

    private float _hunger;
    private float _thirst;
    private float _energyLevel;
    public float _socialNeed;
    public GameObject CurrentSleepTarget { get; set; }
    public GameObject CurrentPlayTarget { get; set; }
    public GameObject CurrentSocialTarget { get; set; }

    public bool IsNeedDecayPaused { get; set; } = false;
    public bool IsDonePlaying { get; set; } = true;

    private Coroutine _needsCoroutine;
    private NavMeshAgent _navMeshAgent;
    private InfluenceMap _influenceMap;

    // Idle wandering
    public WaypointManager Waypoints { get; set; }
    public Vector3 CurrentWaypoint { get; set; } = Vector3.zero;
    public bool CanStopWandering { get; set; } = true;

    private void Start()
    {
        this._hunger = 1.0F;
        this._thirst = 1.0F;
        this._energyLevel = 1.0F;
        this._socialNeed = 1.0F;

        this._needsCoroutine = StartCoroutine(this.DepleteNeeds());
        this._navMeshAgent = GetComponent<NavMeshAgent>();

        this.Waypoints = FindObjectOfType<WaypointManager>();
        this._influenceMap = FindObjectOfType<InfluenceMap>();
    }

    private IEnumerator DepleteNeeds()
    {
        while (true)
        {
            yield return new WaitForSeconds(this.needsDecayInterval);

            if (!this.IsNeedDecayPaused)
            {
                if (this._hunger > 0) this._hunger -= this.hungerDecayRate;
                if (this._thirst > 0) this._thirst -= this.thirstDecayRate;
                if (this._energyLevel > 0) this._energyLevel -= this.energyDecayRate;
                if (this._socialNeed > 0) this._socialNeed -= this.socialDecayRate;
            }
        }
    }

    public void ObserveDecor()
    {
        // Energy Level gain from surrounding decor
        if (this._energyLevel < 1.0F)
        {
            float decor = this._influenceMap.GetInfluenceAt(InfluenceLayers.DECOR, this.transform.position);
            decor *= this.petType.decorations;

            this._energyLevel += decor / 4F;
        }
    }

public float SocialNeed() => _socialNeed;
    public void Socialize() => _socialNeed = 1.0F;
    public float Hunger() => this._hunger;
    public float Thirst() => this._thirst;
    public float EnergyLevel() => this._energyLevel;

    public void Eat() => this._hunger = 1.0F;
    public void Drink() => this._thirst = 1.0F;
    public void Sleep() => this._energyLevel = 1.0F;
    
    public NavMeshAgent GetAgent() => this._navMeshAgent;

    public void StartIdleWandering()
    {
        this.CanStopWandering = false;
        StartCoroutine(this.StartWanderingTimer());
    }

    private IEnumerator StartWanderingTimer()
    {
        yield return new WaitForSeconds(this.minWanderingTime);
        this.CanStopWandering = true;
        yield return null;
    }
}

[Serializable]
public struct NeedsThreshold
{
    public float lower;
    public float upper;

    public NeedsThreshold(float lower, float upper)
    {
        this.lower = lower;
        this.upper = upper;
    }
}
