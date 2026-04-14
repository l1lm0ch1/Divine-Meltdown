using UnityEngine;
using UnityEngine.Playables;

public class Shield : MonoBehaviour
{
    [SerializeField] private int requiredPinCount = 3;
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject interactionPrompt;
    [SerializeField] private string puzzleId = "ShieldPuzzle";

    private int insertedPinCount = 0;
    private bool gateOpen = false;
    private bool playerInRange = false;
    private PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.Pause();
        director.enabled = false;

        if (GameManager.Instance.IsPuzzleComplete(puzzleId))
        {
            gateOpen = true;
            director.enabled = true;
            director.Play();
        }
    }

    private void Update()
    {
        if (!playerInRange || !Input.GetKeyDown(KeyCode.E))
            return;

        if (insertedPinCount >= requiredPinCount && !gateOpen)
            OpenGate();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;
        interactionPrompt.SetActive(!uiPanel.activeInHierarchy);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;
        uiPanel.SetActive(false);
        interactionPrompt.SetActive(false);
    }

    public void AddPin(PinInteractable pin)
    {
        if (insertedPinCount >= requiredPinCount)
            return;

        insertedPinCount++;
        UIManager.Instance.UpdateInsertedPinCount(insertedPinCount);
    }

    public void InsertPin()
    {
        OpenGate();
    }

    private void OpenGate()
    {
        gateOpen = true;
        director.enabled = true;
        director.Play();
        GameManager.Instance.CompletePuzzle(puzzleId);
    }
}