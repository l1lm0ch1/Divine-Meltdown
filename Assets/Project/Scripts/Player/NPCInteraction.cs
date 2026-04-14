using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [SerializeField] private string npcName;
    [SerializeField] private AudioClip interactionSound;

    [TextArea(3, 10)]
    [SerializeField] private string[] dialogueLines;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueTextBox;
    [SerializeField] private TMP_Text npcNameTextBox;
    [SerializeField] private GameObject interactionPrompt;

    private AudioSource audioSource;
    private bool isPlayerInRange = false;
    private Queue<string> dialogueQueue;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        npcNameTextBox.text = npcName;
        dialogueQueue = new Queue<string>(dialogueLines);
        dialogueBox.SetActive(false);
    }

    private void Update()
    {
        if (!isPlayerInRange || !Input.GetKeyDown(KeyCode.E))
            return;

        interactionPrompt.SetActive(false);

        if (dialogueBox.activeInHierarchy)
            DisplayNextLine();
        else
            OpenDialogue();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInRange = true;
        interactionPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInRange = false;
        interactionPrompt.SetActive(false);

        if (dialogueBox.activeSelf)
            CloseDialogue();
    }

    private void OpenDialogue()
    {
        dialogueQueue = new Queue<string>(dialogueLines);
        dialogueBox.SetActive(true);
        DisplayNextLine();
    }

    private void DisplayNextLine()
    {
        if (dialogueQueue.Count == 0)
        {
            CloseDialogue();
            interactionPrompt.SetActive(true);
            return;
        }

        dialogueTextBox.text = dialogueQueue.Dequeue();

        if (interactionSound != null)
            audioSource.PlayOneShot(interactionSound);
    }

    private void CloseDialogue()
    {
        dialogueBox.SetActive(false);
    }
}