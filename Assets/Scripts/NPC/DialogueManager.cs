using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject hideDialoguePanel;
    public GameObject showDialoguePanel;

    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    
    private int dialogueIndex;
    private bool isTyping;
    private NPCDialogue currentDialogue;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void StartDialogue(NPCDialogue dialogueData)
    {
        ClearDialogue();

        currentDialogue = dialogueData;
        dialogueIndex = 0;

        nameText.SetText(dialogueData.npcName);
        portraitImage.sprite = dialogueData.npcPortrait;

        showDialoguePanel.SetActive(true);
        hideDialoguePanel.SetActive(false);

        //PauseController.Instance.SetPaused(true, false);

        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach (char letter in currentDialogue.dialogueLines[dialogueIndex])
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(currentDialogue.typingSpeed);
        }

        isTyping = false;

        if (currentDialogue.autoProgressLines.Length > dialogueIndex &&
            currentDialogue.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSecondsRealtime(currentDialogue.autoProgressDelay);
            NextLine();
        }
    }

    public void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(currentDialogue.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if (++dialogueIndex < currentDialogue.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {

        StopAllCoroutines();
        dialogueText.SetText("");
        showDialoguePanel.SetActive(false);

        PauseController.Instance.SetPaused(false, false);

        currentDialogue = null;
    }

    private void ClearDialogue()
    {
        StopAllCoroutines();
        dialogueText.SetText("");
        nameText.SetText("");
        portraitImage.sprite = null;
        isTyping = false;
        currentDialogue = null;
    }
}
