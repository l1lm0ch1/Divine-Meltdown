using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaveTutorial : MonoBehaviour
{
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private GameObject acceptPanel;
    [SerializeField] private PlayerController player;
    [SerializeField] private string mainGameSceneName = "MainGame";

    private bool inRange;

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            player.BlockMovement();
            Time.timeScale = 0;
            acceptPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        inRange = true;
        interactPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        inRange = false;
        interactPrompt.SetActive(false);
        acceptPanel.SetActive(false);
        player.EnableMovement();
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void StartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(mainGameSceneName);
    }

    public void Stay()
    {
        acceptPanel.SetActive(false);
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        player.EnableMovement();
    }
}