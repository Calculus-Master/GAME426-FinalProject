using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Super State")]
public class FSMSuperState : FSMState
{
    public FSMState initialSubState;
    public FSMState currentSubState;

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += this.ResetState;
    }

    private void ResetState(PlayModeStateChange _)
    {
        this.currentSubState = this.initialSubState;
    }
}