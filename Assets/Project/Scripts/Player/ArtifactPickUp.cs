using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;

public class ArtifactPickUp : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private CinemachineCamera externalCamera;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private string creditsSceneName = "Credits";

    private bool isInRange = false;

    private void Start()
    {
        uiPanel.SetActive(false);
        externalCamera.Priority.Value = 0;
    }

    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
            Interact();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isInRange = true;
        uiPanel.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        isInRange = false;
        uiPanel.SetActive(false);
    }

    private void Interact()
    {
        playerController.BlockMovement();
        externalCamera.Priority.Value = 11;
        Destroy(gameObject);
        SceneManager.LoadScene(creditsSceneName);
    }
}