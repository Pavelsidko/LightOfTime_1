using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionController : MonoBehaviour
{
    public static InteractionController Instance;
    public GameObject interactButtonUI;

    [SerializeField] private NPC currentNPC;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowButton(NPC npc)
    {
        currentNPC = npc;
        interactButtonUI.SetActive(true);
    }

    public void HideButton(NPC npc)
    {
        if (currentNPC == npc && currentNPC.CanInteract())
        {
            currentNPC = null;
            interactButtonUI.SetActive(false);
        }
    }

    public void TryInteract()
    {
        if (currentNPC != null && !PauseController.isPaused)
        {
            currentNPC.Interact();
        }
    }

    public void NextLine()
    {
        if (currentNPC != null && !currentNPC.CanInteract())
        {
            currentNPC.NextLine();
        }
    }

    public void EndDialogue()
    {
        if (currentNPC != null && !currentNPC.CanInteract())
        {
            currentNPC.EndDialogue();
        }
    }
}
