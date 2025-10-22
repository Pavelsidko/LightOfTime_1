using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDialogueTrigger : MonoBehaviour
{
    public NPCDialogue dialogueData;
    private bool hasTriggered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasTriggered || dialogueData == null) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            // Создаём временный NPC-объект или вызываем диалог напрямую
            DialogueManager.Instance.StartDialogue(dialogueData);
        }
    }
}
