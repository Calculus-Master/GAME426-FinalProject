using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Super State")]
public class FSMSuperState : FSMState
{
    public FSMState initialSubState;
    private readonly Dictionary<int, FSMState> _currentSubStates = new();

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += this.ResetState;
    }

    private void ResetState(PlayModeStateChange _)
    {
        this._currentSubStates.Clear();
    }

    public FSMState GetCurrentSubState(int instanceID)
    {
        if(!this._currentSubStates.ContainsKey(instanceID))
            this.SetCurrentSubState(instanceID, this.initialSubState);
        
        return this._currentSubStates[instanceID];
    }
    
    public void SetCurrentSubState(int instanceID, FSMState state)
    {
        this._currentSubStates[instanceID] = state;
    }
}