using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class PetInfoDisplay : MonoBehaviour
{
    public static PetInfoDisplay Instance;

    public GameObject panel;
    public TextMeshProUGUI infoText;

    private Coroutine updateCoroutine;
    private PetEntity currentPet;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (panel != null)
            panel.SetActive(false);
    }

    private void OnEnable()
    {
        this.updateCoroutine = StartCoroutine(UpdateInfoDisplay());
    }

    private IEnumerator UpdateInfoDisplay()
    {
        while (this.panel.activeSelf)
        {
            yield return new WaitForSeconds(0.5F);
            this.ShowPetInfo(this.currentPet);
        }
    }

    public void ShowPetInfo(PetEntity pet)
    {
        if (panel != null && infoText != null)
        {
            panel.SetActive(true);
            infoText.text = BuildPetInfo(pet);

            this.currentPet = pet;
        }
    }

    private string BuildPetInfo(PetEntity pet)
    {
        string info = $"<b>{pet.name}</b>\n\n";

        info += $"<b>Needs:</b>\n";
        info += $"- Hunger: {pet.Hunger():0.00}\n";
        info += $"- Thirst: {pet.Thirst():0.00}\n";
        info += $"- Energy: {pet.EnergyLevel():0.00}\n";
        info += $"- Social: {pet.SocialNeed():0.00}\n";

        var toyProfile = pet.GetComponent<ToyAffinityProfile>();
        if (toyProfile != null)
        {
            info += "\n<b>Toy Preferences:</b>\n";
            foreach (var affinity in toyProfile.toyAffinities)
                info += $"- {affinity.itemName}: {affinity.affinityValue:0.00}\n";
        }

        var petAffinityProfile = pet.GetComponent<PetAffinityProfile>();
        if (petAffinityProfile != null)
        {
            info += "\n<b>Pet Affinities:</b>\n";
            foreach (var affinity in petAffinityProfile.affinities)
                info += $"- {affinity.targetPet.name}: {affinity.affinityValue:0.00}\n";
        }

        var fsm = pet.GetComponent<FiniteStateMachine>();
        if (fsm != null)
        {
            string state = fsm.currentState is FSMSuperState s 
                ? $"{s.name} - {s.GetCurrentSubState(pet.GetInstanceID()).name}" 
                : fsm.currentState?.name;
            info += $"\n<b>Current State:</b> {state}\n";
        }

        return info;
    }

    public void HidePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }

}
