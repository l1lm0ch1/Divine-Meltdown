using UnityEngine;
using UnityEngine.SceneManagement;

public class BossDoorInteraction : MonoBehaviour
{
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private GameObject interactPrompt;
    [SerializeField] private string bossSceneName = "BossArena";

    private bool inRange = false;

    private void Start()
    {
        uiPanel.SetActive(false);
    }

    private void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            uiPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        inRange = true;

        if (interactPrompt != null)
            interactPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        inRange = false;

        if (interactPrompt != null)
            interactPrompt.SetActive(false);
    }

    public void Yes()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(bossSceneName);
    }

    public void No()
    {
        uiPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
    }
}