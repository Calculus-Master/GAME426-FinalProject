using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PetInfoDisplay : MonoBehaviour
{
    public static PetInfoDisplay Instance;

    public GameObject panel;
    public TextMeshProUGUI infoText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (panel != null)
            panel.SetActive(false);
    }

    public void ShowPetInfo(PetEntity pet)
    {
        if (panel != null && infoText != null)
        {
            panel.SetActive(true);
            infoText.text = BuildPetInfo(pet);
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
            info += $"\n<b>Current State:</b> {fsm.currentState?.name}\n";
        }

        return info;
    }

    public void HidePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }

}
