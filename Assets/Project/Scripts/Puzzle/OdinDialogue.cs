using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OdinDialogue : MonoBehaviour
{
    [TextArea(3, 10)]
    [SerializeField] private string[] dialogueLines;
    [SerializeField] private AudioClip interactionSound;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Collider dialogueCollider;
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueTextBox;
    [SerializeField] private TMP_Text nameTextBox;
    [SerializeField] private string speakerName = "Odin";

    private AudioSource audioSource;
    private bool isPlayerInRange = false;
    private Queue<string> dialogueQueue;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        nameTextBox.text = speakerName;
        dialogueQueue = new Queue<string>(dialogueLines);
        dialogueBox.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
            DisplayNextLine();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInRange = true;
        playerController.BlockMovement();
        OpenDialogue();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isPlayerInRange = false;
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
            return;
        }

        dialogueTextBox.text = dialogueQueue.Dequeue();

        if (interactionSound != null)
            audioSource.PlayOneShot(interactionSound);
    }

    private void CloseDialogue()
    {
        dialogueBox.SetActive(false);
        playerController.EnableMovement();

        if (dialogueCollider != null)
            dialogueCollider.enabled = false;
    }
}