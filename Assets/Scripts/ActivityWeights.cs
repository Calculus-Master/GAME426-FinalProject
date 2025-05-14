using UnityEngine;
using UnityEngine.Serialization;

// Class that defines how much weight a pet assigns to each activity, used for prioritizing states in the FSM/BTs
[CreateAssetMenu(menuName = "Pet Activity Weights")]
public class ActivityWeights : ScriptableObject
{
    public float playing = 1.0F;
    public float socializing = 1.0F;
    public float decorations = 1.0F;
}