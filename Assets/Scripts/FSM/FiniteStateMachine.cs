using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

// Adapted from our code in HW2 and Activity 5
public class FiniteStateMachine : MonoBehaviour
{
    public FSMState initialState;
    public FSMState currentState;
    
    private PetEntity petEntity;
    private InfluenceMap influenceMap;

    // Runtime Behavior Tree Instance for Eating
    [HideInInspector]
    public EatingBehaviorTree eatingBehaviorTree;

    private void Start()
    {
        this.currentState = this.initialState;

        this.petEntity = this.GetComponent<PetEntity>();
        this.influenceMap = FindObjectOfType<InfluenceMap>();
        
        this.LogState();
    }

    private void Update()
    {
        FSMTransition triggered = this.currentState.GetTransitions().FirstOrDefault(t => t.IsTriggered(this));
        
        List<FSMAction> actions = new List<FSMAction>();

        if (triggered)
        {
            FSMState targetState = triggered.GetTargetState();
            if(this.currentState.GetExitAction()) actions.Add(this.currentState.GetExitAction());
            if(triggered.GetAction()) actions.Add(triggered.GetAction());
            if(targetState.GetEntryAction()) actions.Add(targetState.GetEntryAction());
            this.currentState = targetState;
            
            this.LogState();
        }
        else
        {
            if (this.currentState is FSMSuperState super)
            {
                FSMState sub = super.currentSubState;
                FSMTransition subTriggered = sub.GetTransitions().FirstOrDefault(t => t.IsTriggered(this));

                if (subTriggered)
                {
                    FSMState targetSubState = subTriggered.GetTargetState();
                    if (sub.GetExitAction()) actions.Add(sub.GetExitAction());
                    if (subTriggered.GetAction()) actions.Add(subTriggered.GetAction());
                    if (targetSubState.GetEntryAction()) actions.Add(targetSubState.GetEntryAction());
                    super.currentSubState = targetSubState;
                    
                    this.LogState();
                }
                else actions.AddRange(sub.GetActions());
            }
            else actions.AddRange(this.currentState.GetActions());
        }
     
        // Execute all the actions
        actions.Where(a => a).ToList().ForEach(a => a.Act(this));
    }

private string lastLoggedState = "";
    private void LogState()
    {
        string subStateInfo = this.currentState is FSMSuperState s ? $" (Sub State = {s.currentSubState.name})" : "";
        //Debug.Log($"Agent {this.gameObject.name}: Current State = {this.currentState.name}{subStateInfo}");
            if (subStateInfo != lastLoggedState)
    {
        lastLoggedState = subStateInfo;
        Debug.Log($"Agent {this.gameObject.name}: Current State = {this.currentState.name}{subStateInfo}");
    }
    }



    public PetEntity GetPet() => this.petEntity;
    public InfluenceMap GetInfluenceMap() => this.influenceMap;
}
