using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsMenuPanel;
    [SerializeField] private string creditsSceneName = "Credits";

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    public void NewGame()
    {
        GameManager.Instance.StartNewGame();
    }

    public void Continue()
    {
        GameManager.Instance.ContinueGame();
    }

    public void Credits()
    {
        SceneManager.LoadScene(creditsSceneName);
    }

    public void OpenOptionsMenu()
    {
        mainMenuPanel.SetActive(false);
        optionsMenuPanel.SetActive(true);
    }

    public void Back()
    {
        optionsMenuPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}